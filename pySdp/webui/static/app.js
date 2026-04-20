/* ════════════════════════════════════════════════════════════════
   pySdp WebUI — app.js
   ════════════════════════════════════════════════════════════════ */

const API   = '/api/sdpcli';
const FILES = '/api/files';

// ── State ─────────────────────────────────────────────────────────────────────
const state = {
  serverAlive:      false,
  device:           'Disconnected',
  connectedDevice:  null,
  session:          null,
  captures:         [],   // [{sdpPath, captureId, sessionId}]
  lastAnalysisDir:      null, // captureDir from last completed analysis
  activeAnalysisJobId:  null, // job ID of running analysis (for cancel)
};

// One polling timer per operation section
const timers = { device: null, connect: null, launch: null, capture: null, analysis: null, logs: null };

// ── Log state ─────────────────────────────────────────────────────────────────
const logState = {
  filter:         'all',  // 'all' | 'info' | 'warning' | 'error'
  lastSeenId:     0,      // highest backend record ID the user has seen
  allRecords:     [],     // latest fetch (unfiltered, backend + frontend mixed)
  frontendUnread: 0,      // unseen frontend warn/error count
};

// ── Console capture ───────────────────────────────────────────────────────────

let _frontendLogId = -1;

function _interceptConsole() {
  const orig = { log: console.log, warn: console.warn, error: console.error };
  const lvl  = { log: 'info', warn: 'warning', error: 'error' };
  ['log', 'warn', 'error'].forEach(m => {
    console[m] = function (...args) {
      orig[m].apply(console, args);
      const msg = args.map(a =>
        (a instanceof Error) ? a.message : (typeof a === 'object' ? JSON.stringify(a) : String(a))
      ).join(' ');
      _pushFrontendLog(lvl[m], '[JS] ' + msg);
    };
  });
}

function _pushFrontendLog(level, message) {
  const rec = { id: _frontendLogId--, time: new Date().toISOString(), level, message };
  logState.allRecords.unshift(rec);
  if (logState.allRecords.length > 300) logState.allRecords.pop();

  const logsActive = document.getElementById('tab-logs')?.classList.contains('active');
  if (logsActive) {
    renderLogs();
  } else if (level === 'error' || level === 'warning') {
    logState.frontendUnread++;
    const backendUnread = logState.allRecords.filter(
      r => r.id > 0 && r.id > logState.lastSeenId && (r.level === 'error' || r.level === 'warning')
    ).length;
    updateLogBadge(backendUnread + logState.frontendUnread);
  }
}

// ── Low-level fetch helpers ───────────────────────────────────────────────────

async function apiPost(path, body = {}) {
  const res = await fetch(path, {
    method:  'POST',
    headers: { 'Content-Type': 'application/json' },
    body:    JSON.stringify(body),
  });
  return res.json();
}

async function apiGet(path) {
  const res = await fetch(path);
  return res.json();
}

async function apiDelete(path) {
  const res = await fetch(path, { method: 'DELETE' });
  return res.json();
}

// ── Progress UI ───────────────────────────────────────────────────────────────

function showProg(id, pct, phase) {
  const wrap  = document.getElementById(`${id}-prog`);
  const fill  = document.getElementById(`${id}-fill`);
  const label = document.getElementById(`${id}-plabel`);
  wrap.classList.remove('hidden');
  fill.style.width = `${Math.max(pct, 2)}%`;
  label.textContent = phase ? `${pct}% — ${phase}` : `${pct}%`;
}

function hideProg(id) {
  document.getElementById(`${id}-prog`).classList.add('hidden');
}

// ── Status messages ───────────────────────────────────────────────────────────

function setMsg(id, type, text) {
  const el = document.getElementById(`${id}-msg`);
  el.className = `status-msg s-${type}`;
  el.textContent = text;
}

// ── Job polling ───────────────────────────────────────────────────────────────

function pollJob(section, jobId, onTick, onDone, onError) {
  clearInterval(timers[section]);
  timers[section] = setInterval(async () => {
    let res;
    try {
      res = await apiGet(`${API}/jobs/${jobId}`);
    } catch (err) {
      clearInterval(timers[section]);
      onError('Network error: ' + err.message);
      return;
    }
    if (!res.ok) {
      clearInterval(timers[section]);
      onError(res.error || 'Job query failed');
      return;
    }
    const job = res.data;
    onTick(job);
    if (job.status === 'Completed') {
      clearInterval(timers[section]);
      onDone(job);
    } else if (job.status === 'Failed' || job.status === 'Cancelled') {
      clearInterval(timers[section]);
      onError(job.error || job.status);
    }
  }, 2000);
}

// ── Device status polling ─────────────────────────────────────────────────────

function startDevicePoll() {
  clearInterval(timers.device);
  timers.device = setInterval(syncDevice, 3000);
  syncDevice();   // immediate first fetch
}

async function syncDevice() {
  // 1. Liveness check via /api/status
  let alive = false;
  try {
    const sr = await apiGet(`${API}/status`);
    alive = sr?.ok === true;
  } catch { /* connection refused */ }

  if (!alive) {
    if (state.serverAlive) {          // transition: online → offline
      state.serverAlive = false;
      state.device      = 'Disconnected';
      state.connectedDevice = null;
      state.session         = null;
      refreshHeader();
      refreshSteps();
    }
    setBadge('server-badge', 'err', '● SDPCLI: Offline');
    return;
  }

  // 2. Device state
  let res;
  try {
    res = await apiGet(`${API}/device`);
  } catch {
    setBadge('server-badge', 'warn', '● SDPCLI: OK');
    return;
  }

  const wasOffline = !state.serverAlive;
  const prevDevice = state.device;
  state.serverAlive = true;
  setBadge('server-badge', 'ok', '● SDPCLI: OK');

  if (res.ok && res.data) {
    state.device          = res.data.status;
    state.connectedDevice = res.data.connectedDevice || null;
    state.session         = res.data.session || null;
    refreshHeader();
    refreshSteps();
    if (wasOffline) console.log('SDPCLI server reconnected');
    if (prevDevice !== state.device) onDeviceStateChange(prevDevice, state.device);
  }
}

function onDeviceStateChange(from, to) {
  // App process killed: SessionActive → Connected
  if (from === 'SessionActive' && to === 'Connected') {
    setMsg('launch', 'warn', 'App process ended — session closed');
  }
  // Full disconnect
  if (to === 'Disconnected' && from !== 'Disconnected') {
    setMsg('connect', 'info', 'Disconnected');
    setMsg('launch',  '',    '');
  }
}

function setBadge(id, cls, text) {
  const el = document.getElementById(id);
  el.className = `badge badge-${cls}`;
  el.textContent = text;
}

// ── Header & step gating ──────────────────────────────────────────────────────

const STATUS_BADGE = {
  Disconnected:  ['gray',   'Disconnected'],
  Connecting:    ['warn',   'Connecting…'],
  Connected:     ['ok',     'Connected'],
  Launching:     ['warn',   'Launching…'],
  SessionActive: ['active', 'Session Active'],
  Capturing:     ['warn',   'Capturing…'],
};

function refreshHeader() {
  const [cls, label] = STATUS_BADGE[state.device] || ['gray', state.device];
  const suffix = state.connectedDevice ? ` · ${state.connectedDevice}` : '';
  setBadge('device-badge', cls, label + suffix);
}

function refreshSteps() {
  const alive = state.serverAlive;
  const s     = state.device;
  // All buttons off when server is offline
  setCardEnabled('card-connect', alive);
  setCardEnabled('card-launch',  alive && s === 'Connected');
  setCardEnabled('card-capture', alive && s === 'SessionActive');
  setBtn('btn-connect',    alive && s === 'Disconnected');
  setBtn('btn-disconnect', alive && s !== 'Disconnected');
  setBtn('btn-launch',     alive && s === 'Connected');
  setBtn('btn-capture',    alive && s === 'SessionActive');
}

function setCardEnabled(id, enabled) {
  document.getElementById(id).classList.toggle('disabled', !enabled);
}

function setBtn(id, enabled) {
  document.getElementById(id).disabled = !enabled;
}

// ── Connect ───────────────────────────────────────────────────────────────────

async function doConnect() {
  const deviceId = document.getElementById('device-id').value.trim();
  setBtn('btn-connect', false);
  setMsg('connect', 'info', 'Submitting…');

  let res;
  try {
    res = await apiPost(`${API}/connect`, deviceId ? { deviceId } : {});
  } catch (err) {
    setMsg('connect', 'error', err.message);
    setBtn('btn-connect', true);
    return;
  }
  if (!res.ok) {
    setMsg('connect', 'error', res.error);
    setBtn('btn-connect', true);
    return;
  }

  const jobId = res.data.jobId;
  showProg('connect', 0, 'initializing_sdk');
  setMsg('connect', 'info', `Job ${jobId}`);

  pollJob('connect', jobId,
    job => showProg('connect', job.progress, job.phase),
    job => {
      hideProg('connect');
      const devId = job.result?.deviceId || '';
      setMsg('connect', 'success', `Connected${devId ? ': ' + devId : ''}`);
      setBtn('btn-disconnect', true);
      setBtn('btn-connect', false);
    },
    err => {
      hideProg('connect');
      setMsg('connect', 'error', err);
      setBtn('btn-connect', true);
    }
  );
}

async function doDisconnect() {
  setBtn('btn-disconnect', false);
  let res;
  try {
    res = await apiPost(`${API}/disconnect`, {});
  } catch (err) {
    setMsg('connect', 'error', err.message);
    return;
  }
  if (res.ok) {
    setMsg('connect', 'info', 'Disconnected');
    state.device = 'Disconnected';
    refreshHeader();
    refreshSteps();
  } else {
    setMsg('connect', 'error', res.error);
    setBtn('btn-disconnect', true);
  }
}

// ── Launch ────────────────────────────────────────────────────────────────────

async function doLaunch() {
  const pkg = document.getElementById('pkg').value.trim();
  const act = document.getElementById('activity').value.trim();
  if (!pkg) { setMsg('launch', 'error', 'Package is required'); return; }

  const packageActivity = act ? `${pkg}/${act}` : pkg;
  setBtn('btn-launch', false);
  setMsg('launch', 'info', 'Submitting…');

  let res;
  try {
    res = await apiPost(`${API}/session/launch`, { packageActivity });
  } catch (err) {
    setMsg('launch', 'error', err.message);
    setBtn('btn-launch', state.device === 'Connected');
    return;
  }
  if (!res.ok) {
    setMsg('launch', 'error', res.error);
    setBtn('btn-launch', state.device === 'Connected');
    return;
  }

  const jobId = res.data.jobId;
  showProg('launch', 0, 'launching');
  setMsg('launch', 'info', `Job ${jobId}`);

  pollJob('launch', jobId,
    job => showProg('launch', job.progress, job.phase),
    _job => {
      hideProg('launch');
      setMsg('launch', 'success', 'Session active — ready to capture');
    },
    err => {
      hideProg('launch');
      setMsg('launch', 'error', err);
      setBtn('btn-launch', state.device === 'Connected');
    }
  );
}

// ── Capture ───────────────────────────────────────────────────────────────────

async function doCapture() {
  const label = document.getElementById('cap-label').value.trim();
  setBtn('btn-capture', false);
  setMsg('capture', 'info', 'Submitting…');

  const body = label ? { label } : {};
  let res;
  try {
    res = await apiPost(`${API}/capture`, body);
  } catch (err) {
    setMsg('capture', 'error', err.message);
    syncDevice();
    return;
  }
  if (!res.ok) {
    setMsg('capture', 'error', res.error);
    syncDevice();
    return;
  }

  const jobId = res.data.jobId;
  showProg('capture', 0, 'starting_capture');
  setMsg('capture', 'info', `Job ${jobId}`);

  pollJob('capture', jobId,
    job => showProg('capture', job.progress, job.phase),
    job => {
      hideProg('capture');
      const r = job.result || {};
      setMsg('capture', 'success', `Done  captureId: ${r.captureId ?? '—'}`);
      if (r.captureId != null) {
        addCaptureRow(r);
        state.captures.push(r);
      }
      syncDevice();
    },
    err => {
      hideProg('capture');
      setMsg('capture', 'error', err);
      syncDevice();
    }
  );
}

function normPath(p) {
  return p ? p.replace(/\\/g, '/') : p;
}

function addCaptureRow(capture) {
  const list = document.getElementById('captures-list');
  const row  = document.createElement('div');
  row.className = 'capture-item';
  const path = normPath(capture.sdpPath) || '—';

  const tag = document.createElement('span');
  tag.className   = 'capture-tag';
  tag.textContent = `# ${capture.captureId}`;

  const pathSpan = document.createElement('span');
  pathSpan.className   = 'capture-path';
  pathSpan.title       = path;
  pathSpan.textContent = path;

  const btn = document.createElement('button');
  btn.className   = 'btn-secondary btn-sm';
  btn.textContent = 'Analyze →';
  btn.addEventListener('click', () => goAnalyze(path, capture.captureId));

  row.appendChild(tag);
  row.appendChild(pathSpan);
  row.appendChild(btn);
  list.prepend(row);
}

// ── Analysis ──────────────────────────────────────────────────────────────────

// sdpPath → captureDir, persists across file switches
const sdpAnalysisCache = {};

const ALL_TARGETS     = ['dc','shaders','textures','buffers','label','metrics','status','topdc','analysis','dashboard'];
const DEFAULT_TARGETS = new Set(ALL_TARGETS);

// Targets handled by C# (SDK P/Invoke — must run on SDPCLI server)
const CS_TARGETS = new Set(['dc','shaders','textures','buffers','metrics']);

// Targets handled by Python, in dependency order
const PY_STEPS = [
  { key: 'label',     endpoint: 'label',       phase: 'label_drawcalls' },
  { key: 'status',    endpoint: 'status',       phase: 'generate_stats'  },
  { key: 'topdc',     endpoint: 'topdc',        phase: 'generate_topdc'  },
  { key: 'analysis',  endpoint: 'analysis_md',  phase: 'report_analysis' },
  { key: 'dashboard', endpoint: 'dashboard',    phase: 'dashboard'       },
];

function initTargetChips() {
  const grid = document.getElementById('targets-grid');
  ALL_TARGETS.forEach(t => {
    const lbl = document.createElement('label');
    lbl.className = 'target-chip';
    lbl.innerHTML = `<input type="checkbox" id="tgt-${t}"${DEFAULT_TARGETS.has(t) ? ' checked' : ''}> ${t}`;
    grid.appendChild(lbl);
  });
}

function selectedTargets() {
  return ALL_TARGETS.filter(t => document.getElementById(`tgt-${t}`)?.checked).join(',');
}

// ── SDP file browser ──────────────────────────────────────────────────────────

async function scanSdpFiles() {
  const dir = document.getElementById('sdp-dir').value.trim();
  if (!dir) { setMsg('sdp-scan', 'error', 'Enter a directory first'); return; }
  localStorage.setItem('sdpDir', dir);

  const grid = document.getElementById('sdp-file-grid');
  grid.innerHTML = '<span class="muted">Scanning…</span>';
  document.getElementById('sdp-scan-msg').textContent = '';

  let res;
  try {
    res = await apiGet(`${FILES}/sdp?dir=${encodeURIComponent(dir)}`);
  } catch (err) {
    grid.innerHTML = '';
    setMsg('sdp-scan', 'error', err.message);
    return;
  }
  if (!res.ok) {
    grid.innerHTML = '';
    setMsg('sdp-scan', 'error', res.error);
    return;
  }

  const files = res.data || [];
  grid.innerHTML = '';
  if (files.length === 0) {
    grid.innerHTML = '<span class="muted">No .sdp files found.</span>';
    return;
  }

  files.forEach(f => {
    const fpath = normPath(f.path);
    const card  = document.createElement('div');
    card.className       = 'sdp-card';
    card.dataset.sdpPath = fpath;

    const sizeMb = (f.size / 1048576).toFixed(1);
    const date   = new Date(f.modified * 1000).toLocaleString();

    const icon = document.createElement('div');
    icon.className   = 'sdp-card-icon';
    icon.textContent = '📦';

    const name = document.createElement('div');
    name.className   = 'sdp-card-name';
    name.textContent = f.name;
    name.title       = fpath;

    const meta = document.createElement('div');
    meta.className        = 'sdp-card-meta';
    meta.textContent      = `${sizeMb} MB\n${date}`;
    meta.style.whiteSpace = 'pre';

    // Analyze button
    const analyzeBtn = document.createElement('button');
    analyzeBtn.className   = 'btn-secondary btn-sm sdp-analyze-btn';
    analyzeBtn.textContent = 'Analyze';
    analyzeBtn.addEventListener('click', () => doAnalyze(fpath));

    // Results button — enabled only if already analyzed
    const resultsBtn = document.createElement('button');
    resultsBtn.className   = 'btn-secondary btn-sm sdp-results-btn';
    resultsBtn.textContent = 'Results';
    resultsBtn.disabled    = !sdpAnalysisCache[fpath];
    resultsBtn.addEventListener('click', () => openResults(sdpAnalysisCache[fpath]));

    const btns = document.createElement('div');
    btns.className = 'sdp-card-btns';
    btns.appendChild(analyzeBtn);
    btns.appendChild(resultsBtn);

    card.appendChild(icon);
    card.appendChild(name);
    card.appendChild(meta);
    card.appendChild(btns);
    grid.appendChild(card);
  });

  // Also scan the analysis subdirectory and enable Results buttons for matching sdp names
  const analysisDir = dir.replace(/\\/g, '/').replace(/\/$/, '') + '/analysis';
  scanAnalyses(analysisDir, null, null, /*silent=*/true).then(() => {
    // Match runs to sdp cards by name stem (e.g. "2026-04-20T18-11-00" in run name)
    resultsState.runs.forEach(run => {
      grid.querySelectorAll('.sdp-card').forEach(card => {
        const sdpPath = card.dataset.sdpPath;
        const sdpStem = sdpPath.replace(/\\/g, '/').split('/').pop().replace(/\.sdp$/, '');
        if (run.name === sdpStem && run.snapshots.length > 0) {
          // Use the first snapshot dir as captureDir representative
          const captureDir = run.snapshots[0].path;
          sdpAnalysisCache[sdpPath] = captureDir;
          const rb = card.querySelector('.sdp-results-btn');
          if (rb) { rb.disabled = false; rb.onclick = () => openResults(captureDir); }
        }
      });
    });
  });
}

// ── Analysis ──────────────────────────────────────────────────────────────────

function goAnalyze(sdpPath, captureId) {
  const dirInput  = document.getElementById('sdp-dir');
  const parentDir = sdpPath.substring(0, sdpPath.lastIndexOf('/'));
  if (parentDir && !dirInput.value) {
    dirInput.value = parentDir;
    localStorage.setItem('sdpDir', parentDir);
  }
  document.getElementById('snapshot-id').value = captureId ?? 2;
  switchTab('analysis');  // triggers scanSdpFiles if dir is set
}

async function doAnalyze(sdpPath) {
  if (state.activeAnalysisJobId) {
    console.warn('Analysis already running');
    return;
  }

  const snapshotId = parseInt(document.getElementById('snapshot-id').value, 10);
  const allSelected = new Set(selectedTargets().split(',').filter(Boolean));

  if (!snapshotId || snapshotId < 2) {
    setMsg('analysis', 'error', 'Snapshot ID must be ≥ 2 — check Settings');
    document.getElementById('card-analysis-progress').style.display = '';
    return;
  }
  if (allSelected.size === 0) {
    setMsg('analysis', 'error', 'Select at least one target in Settings');
    document.getElementById('card-analysis-progress').style.display = '';
    return;
  }

  // Show progress card
  document.getElementById('analysis-progress-name').textContent = sdpPath.split('/').pop();
  document.getElementById('card-analysis-progress').style.display = '';
  setMsg('analysis', 'info', 'Submitting…');
  showProg('analysis', 0, 'starting');
  document.querySelectorAll('.sdp-analyze-btn').forEach(b => b.disabled = true);

  // C# targets: intersection of selected + CS_TARGETS
  const csTargets = [...allSelected].filter(t => CS_TARGETS.has(t)).join(',');

  let res;
  try {
    res = await apiPost(`${API}/analysis`, { sdpPath, snapshotId, targets: csTargets || 'dc' });
  } catch (err) {
    _finishAnalysis(sdpPath, null, err.message);
    return;
  }
  if (!res.ok) {
    _finishAnalysis(sdpPath, null, res.error);
    return;
  }

  state.activeAnalysisJobId = res.data.jobId;

  // C# job occupies 0-70% of progress
  pollJob('analysis', res.data.jobId,
    job => showProg('analysis', Math.round(job.progress * 0.70), job.phase),
    job => {
      const captureDir = normPath(job.result?.captureDir) || null;
      if (!captureDir) { _finishAnalysis(sdpPath, null, 'No captureDir in job result'); return; }
      _runPySteps(sdpPath, captureDir, allSelected);
    },
    err => _finishAnalysis(sdpPath, null, err)
  );
}

// Python generation steps run sequentially after C# job completes (70-100% progress)
async function _runPySteps(sdpPath, captureDir, selected) {
  const steps = PY_STEPS.filter(s => selected.has(s.key));
  const total  = steps.length;

  for (let i = 0; i < total; i++) {
    const step = steps[i];
    const pct  = 70 + Math.round(((i) / total) * 30);
    showProg('analysis', pct, step.phase);
    try {
      const res = await apiPost(`${FILES}/${step.endpoint}?snapshot_dir=${encodeURIComponent(captureDir)}`, {});
      if (!res.ok) {
        console.warn(`Python step ${step.key} failed: ${res.error}`);
        // non-fatal — continue with remaining steps
      }
    } catch (err) {
      console.warn(`Python step ${step.key} error: ${err.message}`);
    }
  }

  _finishAnalysis(sdpPath, captureDir, null);
}

function _finishAnalysis(sdpPath, captureDir, error) {
  state.activeAnalysisJobId = null;
  document.querySelectorAll('.sdp-analyze-btn').forEach(b => b.disabled = false);

  if (error) {
    setMsg('analysis', 'error', error);
    return;
  }

  setMsg('analysis', 'success', `Done: ${captureDir || '—'}`);
  showProg('analysis', 100, 'complete');

  if (captureDir) {
    state.lastAnalysisDir = captureDir;
    sdpAnalysisCache[sdpPath] = captureDir;
    const card = [...document.querySelectorAll('.sdp-card')]
                   .find(c => c.dataset.sdpPath === sdpPath);
    if (card) {
      const rb = card.querySelector('.sdp-results-btn');
      if (rb) {
        rb.disabled = false;
        rb.onclick  = () => openResults(captureDir);
      }
    }
  }

  setTimeout(() => {
    document.getElementById('card-analysis-progress').style.display = 'none';
    setMsg('analysis', '', '');
  }, 3000);
}

async function cancelAnalysis() {
  if (!state.activeAnalysisJobId) return;
  try {
    await apiPost(`${API}/jobs/${state.activeAnalysisJobId}/cancel`, {});
    setMsg('analysis', 'warn', 'Cancelling…');
  } catch (err) {
    setMsg('analysis', 'error', 'Cancel failed: ' + err.message);
  }
}

// captureDir is the snapshot_N subdir under an analysis run (e.g. .../analysis/2026-04-20T18-11-00/snapshot_2)
// We navigate up to the analysis root and scan all runs, then auto-select the right one.
function openResults(captureDir) {
  if (!captureDir) return;
  state.lastAnalysisDir = captureDir;
  switchTab('results');
  // Derive the analysis root (two levels up: snapshot_N → run → analysis root)
  const normDir = normPath(captureDir);
  const parts   = normDir.replace(/\\/g, '/').split('/');
  // parts[-1] = snapshot_N, parts[-2] = run timestamp, parts[-3] = analysis root
  const analysisRoot = parts.slice(0, -2).join('/');
  const runName      = parts[parts.length - 2];
  const snapId       = parts[parts.length - 1];
  const rootInput    = document.getElementById('results-root');
  if (rootInput) rootInput.value = analysisRoot;
  scanAnalyses(analysisRoot, runName, snapId);
}

function goToResults() {
  openResults(state.lastAnalysisDir);
}

// ── Analysis settings ─────────────────────────────────────────────────────────

function toggleAnalysisSettings() {
  const body    = document.getElementById('analysis-settings-body');
  const chevron = document.getElementById('settings-chevron');
  const open    = body.style.display === 'none';
  body.style.display    = open ? '' : 'none';
  chevron.textContent   = open ? '▼' : '▶';
}

function saveAnalysisSettings() {
  const settings = {
    snapshotId: document.getElementById('snapshot-id').value,
    targets:    selectedTargets(),
  };
  localStorage.setItem('analysisSettings', JSON.stringify(settings));
  setMsg('settings-save', 'success', 'Saved');
  setTimeout(() => { const el = document.getElementById('settings-save-msg'); if (el) el.textContent = ''; }, 2000);
}

function loadAnalysisSettings() {
  const raw = localStorage.getItem('analysisSettings');
  if (!raw) return;
  try {
    const s = JSON.parse(raw);
    if (s.snapshotId) document.getElementById('snapshot-id').value = s.snapshotId;
    if (s.targets) {
      const tgtSet = new Set(s.targets.split(',').map(t => t.trim()));
      ALL_TARGETS.forEach(t => {
        const el = document.getElementById(`tgt-${t}`);
        if (el) el.checked = tgtSet.has(t);
      });
    }
  } catch { /* ignore corrupt settings */ }
}

// ── Results ───────────────────────────────────────────────────────────────────

// Current results state
const resultsState = {
  runs:        [],   // parsed from /api/files/analyses
  activeRun:   null, // run name string
  activeSnap:  null, // snapshot id string e.g. "snapshot_2"
};

async function scanAnalyses(root, autoRunName, autoSnapId, silent = false) {
  const rootInput = document.getElementById('results-root');
  const dir = root || (rootInput && rootInput.value.trim());
  if (!dir) return;
  if (!silent) {
    if (rootInput && rootInput.value !== dir) rootInput.value = dir;
    localStorage.setItem('analysisRoot', dir);
    setMsg('results-scan', 'info', 'Scanning…');
  }

  let res;
  try {
    res = await apiGet(`${FILES}/analyses?root=${encodeURIComponent(dir)}`);
  } catch (err) {
    if (!silent) setMsg('results-scan', 'error', err.message);
    return;
  }
  if (!res.ok) {
    if (!silent) setMsg('results-scan', 'error', res.error);
    return;
  }

  const runs = res.data || [];
  if (!silent) {
    localStorage.setItem('analysisRoot', dir);
    if (rootInput && rootInput.value !== dir) rootInput.value = dir;
    setMsg('results-scan', '', '');
    resultsState.runs = runs;
    renderRunSelector(autoRunName || null, autoSnapId || null);
  } else {
    // silent: just return runs for caller to use
    resultsState.runs = runs;
  }
}

function renderRunSelector(autoRunName, autoSnapId) {
  const container = document.getElementById('run-selector');
  const runs = resultsState.runs;
  if (runs.length === 0) {
    container.innerHTML = '<span class="muted">No analysis runs found.</span>';
    document.getElementById('snapshot-viewer').style.display = 'none';
    return;
  }

  container.innerHTML = '';
  const list = document.createElement('div');
  list.className = 'run-list';

  const selectRun = autoRunName || runs[0].name;

  runs.forEach(run => {
    const row = document.createElement('button');
    row.className = 'run-item' + (run.name === selectRun ? ' active' : '');
    row.textContent = run.name;
    row.onclick = () => {
      list.querySelectorAll('.run-item').forEach(b => b.classList.remove('active'));
      row.classList.add('active');
      resultsState.activeRun = run.name;
      renderSnapshotTabs(run, null, /*openDashboard=*/true);
    };
    list.appendChild(row);
  });

  container.appendChild(list);

  // Auto-select run
  const selectedRun = runs.find(r => r.name === selectRun) || runs[0];
  resultsState.activeRun = selectedRun.name;
  renderSnapshotTabs(selectedRun, autoSnapId, /*openDashboard=*/true);
}

function renderSnapshotTabs(run, autoSnapId, openDashboard = false) {
  const tabsEl   = document.getElementById('snap-tabs');
  const panelsEl = document.getElementById('snap-panels');
  const viewer   = document.getElementById('snapshot-viewer');

  tabsEl.innerHTML   = '';
  panelsEl.innerHTML = '';
  viewer.style.display = 'block';

  if (!run.snapshots || run.snapshots.length === 0) {
    panelsEl.innerHTML = '<span class="muted">No snapshots in this run.</span>';
    return;
  }

  const activeSnap = autoSnapId || run.snapshots[0].id;
  resultsState.activeSnap = activeSnap;

  run.snapshots.forEach(snap => {
    // Tab button
    const tab = document.createElement('button');
    tab.className = 'snap-tab-btn' + (snap.id === activeSnap ? ' active' : '');
    tab.textContent = snap.id;
    tab.onclick = () => {
      tabsEl.querySelectorAll('.snap-tab-btn').forEach(b => b.classList.remove('active'));
      tab.classList.add('active');
      resultsState.activeSnap = snap.id;
      panelsEl.querySelectorAll('.snap-panel').forEach(p => p.style.display = 'none');
      const panel = document.getElementById(`panel-${snap.id}`);
      if (panel) panel.style.display = '';
    };
    tabsEl.appendChild(tab);

    // Panel
    const panel = document.createElement('div');
    panel.className = 'snap-panel';
    panel.id = `panel-${snap.id}`;
    panel.style.display = snap.id === activeSnap ? '' : 'none';
    panel.appendChild(buildSnapPanel(snap));
    panelsEl.appendChild(panel);
  });

  // Auto-open dashboard in the active snapshot (only when explicitly requested)
  if (openDashboard) {
    const activeSn = run.snapshots.find(s => s.id === activeSnap) || run.snapshots[0];
    const dashboard = activeSn.analysis.find(f => f.name.includes('dashboard'));
    if (dashboard) viewFile(dashboard.path, dashboard.name, dashboard.ext);
  }
}

function buildSnapPanel(snap) {
  const wrap = document.createElement('div');

  // ── Analysis section (default open) ─────────────────────────────
  wrap.appendChild(buildSection('Analysis', snap.analysis, snap.per_dc, true));
  // ── Statistics section ──────────────────────────────────────────
  wrap.appendChild(buildSection('Statistics', snap.statistics, null, false));
  // ── Raw section ─────────────────────────────────────────────────
  wrap.appendChild(buildSection('Raw', snap.raw, null, false));

  return wrap;
}

function buildSection(title, files, perDcFiles, defaultOpen) {
  const section = document.createElement('div');
  section.className = 'result-section';

  const hdr = document.createElement('div');
  hdr.className = 'result-section-hdr';
  hdr.innerHTML = `<span class="result-section-chevron">${defaultOpen ? '▼' : '▶'}</span> ${escHtml(title)}`;
  hdr.style.cursor = 'pointer';

  const body = document.createElement('div');
  body.className = 'result-section-body';
  body.style.display = defaultOpen ? '' : 'none';

  hdr.onclick = () => {
    const open = body.style.display !== 'none';
    body.style.display = open ? 'none' : '';
    hdr.querySelector('.result-section-chevron').textContent = open ? '▶' : '▼';
  };

  // File chips
  const grid = document.createElement('div');
  grid.className = 'file-grid';
  (files || []).forEach(f => {
    const btn = document.createElement('button');
    btn.className = `file-chip ext-${f.ext}`;
    btn.textContent = f.name;
    btn.onclick = () => viewFile(f.path, f.name, f.ext);
    grid.appendChild(btn);
  });
  body.appendChild(grid);

  // per_dc_content folder (only in Analysis section)
  if (perDcFiles && perDcFiles.length > 0) {
    const folder = document.createElement('div');
    folder.className = 'per-dc-folder';

    const folderHdr = document.createElement('div');
    folderHdr.className = 'per-dc-hdr';
    folderHdr.innerHTML = `<span class="per-dc-chevron">▶</span> per_dc_content/ <span class="muted" style="font-size:11px">${perDcFiles.length} files</span>`;
    folderHdr.style.cursor = 'pointer';

    const folderBody = document.createElement('div');
    folderBody.className = 'per-dc-body';
    folderBody.style.display = 'none';

    folderHdr.onclick = () => {
      const open = folderBody.style.display !== 'none';
      folderBody.style.display = open ? 'none' : '';
      folderHdr.querySelector('.per-dc-chevron').textContent = open ? '▼' : '▶';
    };

    const dcGrid = document.createElement('div');
    dcGrid.className = 'file-grid';
    perDcFiles.forEach(f => {
      const btn = document.createElement('button');
      btn.className = `file-chip ext-${f.ext}`;
      btn.textContent = f.name;
      btn.onclick = () => viewFile(f.path, f.name, f.ext);
      dcGrid.appendChild(btn);
    });
    folderBody.appendChild(dcGrid);

    folder.appendChild(folderHdr);
    folder.appendChild(folderBody);
    body.appendChild(folder);
  } else if (perDcFiles && perDcFiles.length === 0) {
    const folder = document.createElement('div');
    folder.className = 'per-dc-folder';
    folder.innerHTML = `<span class="muted" style="font-size:12px">per_dc_content/ (empty)</span>`;
    body.appendChild(folder);
  }

  if ((files || []).length === 0 && (!perDcFiles || perDcFiles.length === 0)) {
    grid.innerHTML = '<span class="muted" style="font-size:12px">No files.</span>';
  }

  section.appendChild(hdr);
  section.appendChild(body);
  return section;
}

async function viewFile(path, name, ext) {
  const viewer = document.getElementById('file-viewer');
  viewer.innerHTML = '<span class="muted" style="padding:8px 0;display:block">Loading…</span>';

  let res;
  try {
    res = await apiGet(`${FILES}/read?path=${encodeURIComponent(path)}`);
  } catch (err) {
    viewer.innerHTML = `<span class="s-error">${escHtml(err.message)}</span>`;
    return;
  }
  if (!res.ok) {
    viewer.innerHTML = `<div class="s-error" style="padding:8px">${escHtml(res.error)}</div>`;
    return;
  }

  const content = res.data.content || '';

  const card = document.createElement('div');
  card.className = 'viewer-card';

  const hdr = document.createElement('div');
  hdr.className = 'viewer-header';
  hdr.innerHTML = `
    <span class="viewer-title">${escHtml(name)}</span>
    <button class="viewer-close" onclick="document.getElementById('file-viewer').innerHTML=''" title="Close">✕</button>
  `;

  const body = document.createElement('div');
  body.className = 'viewer-body';

  if (ext === 'md') {
    body.classList.add('md');
    body.innerHTML = typeof marked !== 'undefined'
      ? marked.parse(content)
      : `<pre class="code-pre">${escHtml(content)}</pre>`;
  } else if (ext === 'json') {
    let pretty = content;
    try { pretty = JSON.stringify(JSON.parse(content), null, 2); } catch { /* keep raw */ }
    body.innerHTML = `<pre class="code-pre">${escHtml(pretty)}</pre>`;
  } else {
    body.innerHTML = `<pre class="code-pre">${escHtml(content)}</pre>`;
  }

  card.appendChild(hdr);
  card.appendChild(body);
  viewer.innerHTML = '';
  viewer.appendChild(card);

  if (ext === 'md' && typeof mermaid !== 'undefined') {
    body.querySelectorAll('pre code.language-mermaid').forEach(code => {
      const div = document.createElement('div');
      div.className = 'mermaid';
      div.textContent = code.textContent;
      code.parentElement.replaceWith(div);
    });
    mermaid.run({ nodes: body.querySelectorAll('.mermaid') });
  }

  viewer.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
}

// ── Log Viewer ────────────────────────────────────────────────────────────────

function startLogPoll() {
  clearInterval(timers.logs);
  timers.logs = setInterval(fetchLogs, 5000);
  fetchLogs();
}

async function fetchLogs() {
  let res;
  try {
    res = await apiGet('/api/logs?limit=200');
  } catch { return; }
  if (!res.ok) return;

  const records = res.data || [];
  logState.allRecords = records;

  const logsActive = document.getElementById('tab-logs').classList.contains('active');
  if (logsActive) {
    renderLogs();
    if (records.length > 0) logState.lastSeenId = Math.max(...records.map(r => r.id));
    updateLogBadge(0);
  } else {
    const unread = records.filter(
      r => r.id > 0 && r.id > logState.lastSeenId && (r.level === 'error' || r.level === 'warning')
    ).length;
    updateLogBadge(unread + logState.frontendUnread);
  }
}

function renderLogs() {
  const container = document.getElementById('log-list');
  let records = logState.allRecords;
  if (logState.filter !== 'all') records = records.filter(r => r.level === logState.filter);

  if (records.length === 0) {
    container.innerHTML = `<span class="muted log-empty">No ${logState.filter === 'all' ? '' : logState.filter + ' '}entries.</span>`;
    return;
  }
  container.innerHTML = '';
  records.forEach(rec => container.appendChild(buildLogEntry(rec)));
}

function buildLogEntry(rec) {
  const el = document.createElement('div');
  el.className = `log-entry log-${rec.level}`;

  const timeStr = rec.time.includes('T')
    ? rec.time.split('T')[1].replace(/\.\d+/, '').replace(/[+-]\d{2}:\d{2}$/, '')
    : rec.time;

  let ctxHtml = '';
  if (rec.context && Object.keys(rec.context).length > 0) {
    const parts = Object.entries(rec.context).map(([k, v]) => `${escHtml(k)}: ${escHtml(String(v))}`).join(' · ');
    ctxHtml = `<div class="log-ctx">${parts}</div>`;
  }

  const tbHtml = rec.traceback ? `
    <button class="log-tb-toggle" onclick="toggleTb(this)">▶ traceback</button>
    <pre class="log-tb hidden">${escHtml(rec.traceback.trim())}</pre>` : '';

  el.innerHTML = `
    <div class="log-header">
      <span class="log-lvl lvl-${rec.level}">${rec.level.toUpperCase()}</span>
      <span class="log-time">${timeStr}</span>
      <span class="log-msg">${escHtml(rec.message)}</span>
    </div>
    ${ctxHtml}${tbHtml}
  `;
  return el;
}

function toggleTb(btn) {
  const pre = btn.nextElementSibling;
  const isHidden = pre.classList.toggle('hidden');
  btn.textContent = isHidden ? '▶ traceback' : '▼ traceback';
}

function setLogFilter(level) {
  logState.filter = level;
  document.querySelectorAll('.log-filter-btn').forEach(b => {
    b.classList.toggle('active', b.dataset.filter === level);
  });
  renderLogs();
}

async function clearLogs() {
  await apiDelete('/api/logs');
  logState.allRecords = [];
  logState.lastSeenId = 0;
  updateLogBadge(0);
  renderLogs();
}

function updateLogBadge(count) {
  const badge = document.getElementById('log-tab-badge');
  if (count > 0) {
    badge.textContent = count > 99 ? '99+' : String(count);
    badge.classList.remove('hidden');
  } else {
    badge.classList.add('hidden');
  }
}

// ── Tab navigation ────────────────────────────────────────────────────────────

function switchTab(id) {
  document.querySelectorAll('.tab-content').forEach(el => el.classList.remove('active'));
  document.querySelectorAll('.tab-btn').forEach(el => el.classList.remove('active'));
  document.getElementById(`tab-${id}`).classList.add('active');
  document.querySelector(`.tab-btn[data-tab="${id}"]`).classList.add('active');

  if (id === 'snapshot') {
    syncDevice();
  }

  if (id === 'analysis') {
    const saved = localStorage.getItem('sdpDir');
    if (saved) {
      const input = document.getElementById('sdp-dir');
      if (!input.value) input.value = saved;
    }
    if (document.getElementById('sdp-dir').value) scanSdpFiles();
  }

  if (id === 'results') {
    const rootInput = document.getElementById('results-root');
    if (!rootInput.value) {
      const saved = localStorage.getItem('analysisRoot')
        || (() => { const d = localStorage.getItem('sdpDir'); return d ? d.replace(/\\/g, '/').replace(/\/$/, '') + '/analysis' : null; })();
      if (saved) rootInput.value = saved;
    }
    if (rootInput.value && resultsState.runs.length === 0) scanAnalyses();
  }

  if (id === 'logs') {
    logState.frontendUnread = 0;
    renderLogs();
    const backendIds = logState.allRecords.filter(r => r.id > 0).map(r => r.id);
    if (backendIds.length > 0) logState.lastSeenId = Math.max(...backendIds);
    updateLogBadge(0);
  }
}

// ── Utilities ─────────────────────────────────────────────────────────────────

function escHtml(str) {
  return String(str)
    .replace(/&/g, '&amp;')
    .replace(/</g, '&lt;')
    .replace(/>/g, '&gt;')
    .replace(/"/g, '&quot;');
}

function escAttr(str) {
  return String(str).replace(/'/g, "\\'");
}

// ── Init ──────────────────────────────────────────────────────────────────────

document.addEventListener('DOMContentLoaded', () => {
  // Intercept console output → log panel
  _interceptConsole();

  // Mermaid config (startOnLoad:false — we call run() manually after DOM insertion)
  if (typeof mermaid !== 'undefined') {
    mermaid.initialize({ startOnLoad: false, theme: 'default' });
  }

  // Wire up tab buttons
  document.querySelectorAll('.tab-btn').forEach(btn => {
    btn.addEventListener('click', () => switchTab(btn.dataset.tab));
  });

  // Build target checkboxes, then restore saved settings
  initTargetChips();
  loadAnalysisSettings();

  // Initial step state
  refreshSteps();

  // Start polling device status
  startDevicePoll();

  // Start polling logs (badge updates when tab is not active)
  startLogPoll();

  // Auto-load results: prefer explicit analysisRoot, fall back to sdpDir/analysis
  const savedRoot = localStorage.getItem('analysisRoot')
    || (() => { const d = localStorage.getItem('sdpDir'); return d ? d.replace(/\\/g, '/').replace(/\/$/, '') + '/analysis' : null; })();
  if (savedRoot) {
    const rootInput = document.getElementById('results-root');
    if (rootInput) rootInput.value = savedRoot;
    scanAnalyses(savedRoot);
  }
});
