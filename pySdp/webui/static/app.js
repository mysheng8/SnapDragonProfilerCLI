/* ════════════════════════════════════════════════════════════════
   pySdp WebUI — app.js
   ════════════════════════════════════════════════════════════════ */

const API   = '/api/sdpcli';
const FILES = '/api/files';
const DATA  = '/api/data';

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

// ── Device / package / activity dropdowns ─────────────────────────────────────

async function refreshDeviceList() {
  const sel = document.getElementById('device-id');
  try {
    const res = await apiGet(`${API}/devices`);
    if (!res.ok) return;
    const current = sel.value;
    sel.innerHTML = '<option value="">— auto-select —</option>';
    (res.data || []).forEach(d => {
      const opt = document.createElement('option');
      opt.value = d.serial;
      opt.textContent = `${d.serial}  (${d.state})`;
      if (d.state !== 'device') opt.disabled = true;
      sel.appendChild(opt);
    });
    if (current) sel.value = current;
  } catch (_) {}
}

async function refreshPackageList() {
  const serial = document.getElementById('device-id').value.trim();
  const sel = document.getElementById('pkg');
  try {
    const url = serial ? `${API}/app/packages?serial=${encodeURIComponent(serial)}` : `${API}/app/packages`;
    const res = await apiGet(url);
    if (!res.ok) return;
    const current = sel.value;
    sel.innerHTML = '<option value="">— select package —</option>';
    (res.data || []).forEach(pkg => {
      const opt = document.createElement('option');
      opt.value = pkg;
      opt.textContent = pkg;
      sel.appendChild(opt);
    });
    if (current) sel.value = current;
    document.getElementById('activity').innerHTML = '<option value="">— default launch —</option>';
  } catch (_) {}
}

async function onPkgChange() {
  const serial  = document.getElementById('device-id').value.trim();
  const pkg     = document.getElementById('pkg').value.trim();
  const actSel  = document.getElementById('activity');
  actSel.innerHTML = '<option value="">— default launch —</option>';
  if (!pkg) return;
  try {
    const url = `${API}/app/activities?package=${encodeURIComponent(pkg)}` +
                (serial ? `&serial=${encodeURIComponent(serial)}` : '');
    const res = await apiGet(url);
    if (!res.ok) return;
    (res.data || []).forEach(act => {
      const opt = document.createElement('option');
      opt.value = act;
      opt.textContent = act;
      actSel.appendChild(opt);
    });
  } catch (_) {}
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

const ALL_TARGETS     = ['screenshot','ingest','dc','shaders','textures','buffers','label','metrics','status','topdc','analysis'];
const DEFAULT_TARGETS = new Set(ALL_TARGETS);

// Targets handled by C# (SDK P/Invoke — must run on SDPCLI server)
const CS_TARGETS = new Set(['dc','shaders','textures','buffers','metrics']);


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

    // Questions button — enabled only if already analyzed
    const resultsBtn = document.createElement('button');
    resultsBtn.className   = 'btn-secondary btn-sm sdp-results-btn';
    resultsBtn.textContent = 'Questions';
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

  // Enable Questions buttons only for sdp files that have been ingested into DuckDB
  apiGet(`${DATA}/snapshots`).then(res => {
    if (!res.ok) return;
    const snapshots = res.data || [];
    grid.querySelectorAll('.sdp-card').forEach(card => {
      const sdpPath = card.dataset.sdpPath;
      const sdpName = sdpPath.replace(/\\/g, '/').split('/').pop();
      const sdpStem = sdpName.replace(/\.sdp$/i, '');
      const match = snapshots.find(s =>
        s.sdp_name === sdpName ||
        s.sdp_name === sdpStem ||
        s.run_name  === sdpStem
      );
      if (match) {
        sdpAnalysisCache[sdpPath] = match.snapshot_dir;
        const rb = card.querySelector('.sdp-results-btn');
        if (rb) { rb.disabled = false; rb.onclick = () => openResults(match.snapshot_dir); }
      }
    });
  }).catch(() => {});
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

  if (!snapshotId || snapshotId < 1) {
    setMsg('analysis', 'error', 'Snapshot ID must be ≥ 1 (use 1 for all snapshots) — check Settings');
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
  try { localStorage.setItem('activeCsJob', JSON.stringify({ jobId: res.data.jobId, sdpPath, allSelected: [...allSelected] })); } catch {}

  // C# job occupies 0-70% of progress
  pollJob('analysis', res.data.jobId,
    job => showProg('analysis', Math.round(job.progress * 0.70), job.phase),
    job => {
      const r = job.result || {};
      // Single-snapshot result: { captureDir, sessionDir, ... }
      // Multi-snapshot result:  { captureIds: [...], sessionDir, ... }
      if (r.captureDir) {
        const captureDir = normPath(r.captureDir);
        _runPySteps(sdpPath, captureDir, allSelected);
      } else if (r.captureIds && r.captureIds.length > 0 && r.sessionDir) {
        const sessionDir = normPath(r.sessionDir);
        const captureDirs = r.captureIds.map(id => sessionDir + '/snapshot_' + id);
        _runPyStepsAll(sdpPath, captureDirs, allSelected);
      } else {
        _finishAnalysis(sdpPath, null, 'No captureDir in job result');
      }
    },
    err => _finishAnalysis(sdpPath, null, err)
  );
}

// Run Python pipeline for multiple capture dirs sequentially.
async function _runPyStepsAll(sdpPath, captureDirs, selected) {
  const total = captureDirs.length;
  for (let i = 0; i < total; i++) {
    const dir = captureDirs[i];
    showProg('analysis', 70 + Math.round((i / total) * 30), `python [${i + 1}/${total}] snapshot`);
    await new Promise(resolve => {
      // Patch _finishAnalysis for this one snapshot so we can intercept completion/error
      // and continue the loop rather than tearing down the whole progress UI.
      const targets = ['screenshot', 'ingest', 'label', 'status', 'topdc', 'analysis']
        .filter(k => selected.has(k)).join(',');
      if (!targets) { resolve(); return; }
      apiPost(`${DATA}/pipeline?snapshot_dir=${encodeURIComponent(dir)}&targets=${encodeURIComponent(targets)}`, {})
        .then(res => {
          if (!res.ok) {
            console.warn('Pipeline submit failed for', dir, res.error);
            resolve(); return;
          }
          const jobId = res.job_id;
          // Persist so a page refresh can resume the current snapshot
          try { localStorage.setItem('activePipelineJob', JSON.stringify({ jobId, sdpPath, captureDir: dir })); } catch {}
          const poll = setInterval(async () => {
            let pr;
            try { pr = await apiGet(`${DATA}/pipeline/${jobId}`); } catch { return; }
            if (!pr.ok) {
              clearInterval(poll);
              try { localStorage.removeItem('activePipelineJob'); } catch {}
              console.warn('Pipeline poll failed for', dir, pr.error);
              resolve(); return;
            }
            const pct = 70 + Math.round(((i + pr.data.progress / 100) / total) * 30);
            showProg('analysis', pct, `[${i + 1}/${total}] ${pr.data.phase || pr.data.status}`);
            if (pr.data.status === 'completed') {
              clearInterval(poll);
              try { localStorage.removeItem('activePipelineJob'); } catch {}
              resolve();
            } else if (pr.data.status === 'failed' || pr.data.status === 'cancelled') {
              clearInterval(poll);
              try { localStorage.removeItem('activePipelineJob'); } catch {}
              console.warn('Pipeline failed for', dir, pr.data.error || pr.data.status);
              resolve(); // non-fatal — continue to next snapshot
            }
          }, 2000);
        })
        .catch(err => { console.warn('Pipeline error for', dir, err.message); resolve(); });
    });
  }
  const lastDir = captureDirs[total - 1] || null;
  _finishAnalysis(sdpPath, lastDir, null);
}

// Python pipeline: submit to server-side job manager, then poll.
// The pipeline runs in a background thread on the server — browser refresh does not interrupt it.
async function _runPySteps(sdpPath, captureDir, selected) {
  // Build ordered targets from the user's selection (ingest first if selected)
  const targets = ['screenshot', 'ingest', 'label', 'status', 'topdc', 'analysis']
    .filter(k => selected.has(k)).join(',');

  if (!targets) {
    // Nothing to do on the Python side
    _finishAnalysis(sdpPath, captureDir, null);
    return;
  }

  // Submit pipeline job
  let res;
  try {
    res = await apiPost(`${DATA}/pipeline?snapshot_dir=${encodeURIComponent(captureDir)}&targets=${encodeURIComponent(targets)}`, {});
  } catch (err) {
    _finishAnalysis(sdpPath, null, 'Pipeline submit error: ' + err.message);
    return;
  }
  if (!res.ok) {
    _finishAnalysis(sdpPath, null, res.error || 'Pipeline submit failed');
    return;
  }

  const jobId = res.job_id;
  // Persist job_id so a page refresh can resume polling
  try { localStorage.setItem('activePipelineJob', JSON.stringify({ jobId, sdpPath, captureDir })); } catch {}

  _pollPipelineJob(jobId, sdpPath, captureDir);
}

function _pollPipelineJob(jobId, sdpPath, captureDir) {
  clearInterval(timers.analysis);
  timers.analysis = setInterval(async () => {
    let res;
    try {
      res = await apiGet(`${DATA}/pipeline/${jobId}`);
    } catch (err) {
      // Network blip — keep polling
      return;
    }
    if (!res.ok) {
      clearInterval(timers.analysis);
      try { localStorage.removeItem('activePipelineJob'); } catch {}
      _finishAnalysis(sdpPath, null, res.error || 'Pipeline poll failed');
      return;
    }

    const job = res.data;
    // Map pipeline 0-100 to the overall 70-100% progress band
    const pct = 70 + Math.round(job.progress * 0.30);
    showProg('analysis', pct, job.phase || job.status);

    if (job.status === 'completed') {
      clearInterval(timers.analysis);
      try { localStorage.removeItem('activePipelineJob'); } catch {}
      _finishAnalysis(sdpPath, captureDir, null);
    } else if (job.status === 'failed' || job.status === 'cancelled') {
      clearInterval(timers.analysis);
      try { localStorage.removeItem('activePipelineJob'); } catch {}
      _finishAnalysis(sdpPath, null, job.error || job.status);
    }
    // else: pending/running — keep polling
  }, 2000);
}

function _finishAnalysis(sdpPath, captureDir, error) {
  state.activeAnalysisJobId = null;
  try { localStorage.removeItem('activeCsJob'); } catch {}
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

    // Refresh shared snapshot picker so Questions/Explorer see the new data.
    // Auto-select the snapshot that matches captureDir if picker already open.
    sharedSnapScan().then(() => {
      if (!sharedSnapState.snapshotId) return;
      // Try to find and select the snapshot whose dir matches captureDir
      const norm = normPath(captureDir);
      for (const run of sharedSnapState.runs) {
        const s = run.snapshots.find(x => normPath(x.snapshot_dir) === norm);
        if (s) { _onSharedSnapSelect(s); break; }
      }
    }).catch(() => {});
  }

  setTimeout(() => {
    document.getElementById('card-analysis-progress').style.display = 'none';
    setMsg('analysis', '', '');
  }, 3000);
}

async function cancelAnalysis() {
  // Cancel C# job if active
  if (state.activeAnalysisJobId) {
    try {
      await apiPost(`${API}/jobs/${state.activeAnalysisJobId}/cancel`, {});
      setMsg('analysis', 'warn', 'Cancelling…');
    } catch (err) {
      setMsg('analysis', 'error', 'Cancel failed: ' + err.message);
    }
  }
  // Cancel Python pipeline job if active
  const saved = _getActivePipelineJob();
  if (saved) {
    try {
      await apiPost(`${DATA}/pipeline/${saved.jobId}/cancel`, {});
    } catch { /* ignore */ }
    try { localStorage.removeItem('activePipelineJob'); } catch {}
    clearInterval(timers.analysis);
  }
}

// Resume a C# analysis job that was running before a page refresh
function _resumeCsJobIfAny() {
  let saved;
  try { saved = JSON.parse(localStorage.getItem('activeCsJob') || 'null'); } catch { saved = null; }
  if (!saved) return;
  const { jobId, sdpPath, allSelected: allSelectedArr } = saved;
  const allSelected = new Set(allSelectedArr || []);

  apiGet(`${API}/jobs/${jobId}`).then(res => {
    if (!res.ok) { try { localStorage.removeItem('activeCsJob'); } catch {} return; }
    const job = res.data;
    if (job.status === 'Completed') {
      // C# done — jump straight to Python phase
      try { localStorage.removeItem('activeCsJob'); } catch {}
      const r = job.result || {};
      if (r.captureDir) {
        _runPySteps(sdpPath, normPath(r.captureDir), allSelected);
      } else if (r.captureIds && r.captureIds.length > 0 && r.sessionDir) {
        const captureDirs = r.captureIds.map(id => normPath(r.sessionDir) + '/snapshot_' + id);
        _runPyStepsAll(sdpPath, captureDirs, allSelected);
      } else {
        try { localStorage.removeItem('activeCsJob'); } catch {}
      }
      return;
    }
    if (job.status === 'Failed' || job.status === 'Cancelled') {
      try { localStorage.removeItem('activeCsJob'); } catch {}
      return;
    }
    // Still running — restore progress UI and resume polling
    state.activeAnalysisJobId = jobId;
    document.getElementById('analysis-progress-name').textContent = sdpPath.split('/').pop();
    document.getElementById('card-analysis-progress').style.display = '';
    setMsg('analysis', 'info', `Resuming analysis job ${jobId}…`);
    showProg('analysis', Math.round(job.progress * 0.70), job.phase || 'running');
    document.querySelectorAll('.sdp-analyze-btn').forEach(b => b.disabled = true);
    pollJob('analysis', jobId,
      j => showProg('analysis', Math.round(j.progress * 0.70), j.phase),
      j => {
        try { localStorage.removeItem('activeCsJob'); } catch {}
        const r = j.result || {};
        if (r.captureDir) {
          _runPySteps(sdpPath, normPath(r.captureDir), allSelected);
        } else if (r.captureIds && r.captureIds.length > 0 && r.sessionDir) {
          const captureDirs = r.captureIds.map(id => normPath(r.sessionDir) + '/snapshot_' + id);
          _runPyStepsAll(sdpPath, captureDirs, allSelected);
        } else {
          _finishAnalysis(sdpPath, null, 'No captureDir in job result');
        }
      },
      err => { try { localStorage.removeItem('activeCsJob'); } catch {} _finishAnalysis(sdpPath, null, err); }
    );
  }).catch(() => { try { localStorage.removeItem('activeCsJob'); } catch {} });
}

function _getActivePipelineJob() {
  try {
    const raw = localStorage.getItem('activePipelineJob');
    return raw ? JSON.parse(raw) : null;
  } catch { return null; }
}

// Resume polling a pipeline job that was running before a page refresh
function _resumePipelineJobIfAny() {
  const saved = _getActivePipelineJob();
  if (!saved) return;
  apiGet(`${DATA}/pipeline/${saved.jobId}`).then(res => {
    if (!res.ok || res.data.status === 'completed' || res.data.status === 'failed' || res.data.status === 'cancelled') {
      try { localStorage.removeItem('activePipelineJob'); } catch {}
      return;
    }
    // Job still running — restore progress UI and resume polling
    const { jobId, sdpPath, captureDir } = saved;
    document.getElementById('analysis-progress-name').textContent = sdpPath.split('/').pop();
    document.getElementById('card-analysis-progress').style.display = '';
    setMsg('analysis', 'info', `Resuming pipeline job ${jobId}…`);
    showProg('analysis', 70 + Math.round(res.data.progress * 0.30), res.data.phase || 'running');
    document.querySelectorAll('.sdp-analyze-btn').forEach(b => b.disabled = true);
    _pollPipelineJob(jobId, sdpPath, captureDir);
  }).catch(() => {
    try { localStorage.removeItem('activePipelineJob'); } catch {}
  });
}

// captureDir is the snapshot_N subdir under an analysis run (e.g. .../analysis/2026-04-20T18-11-00/snapshot_2)
// Navigate to Questions tab for the run/snapshot.
function openResults(captureDir) {
  if (!captureDir) return;
  state.lastAnalysisDir = captureDir;
  switchTab('questions');
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
  const dir = root || localStorage.getItem('analysisRoot');
  if (!dir) return;
  if (!silent) {
    localStorage.setItem('analysisRoot', dir);
  }

  let res;
  try {
    res = await apiGet(`${FILES}/analyses?root=${encodeURIComponent(dir)}`);
  } catch (err) {
    return;
  }
  if (!res.ok) return;

  const runs = res.data || [];
  resultsState.runs = runs;
  if (!silent) {
    renderRunSelector(autoRunName || null, autoSnapId || null);
  }
}

function renderRunSelector(autoRunName, autoSnapId) {
  const runs = resultsState.runs;
  if (runs.length === 0) {
    document.getElementById('snapshot-viewer').style.display = 'none';
    document.getElementById('file-viewer').innerHTML = '';
    return;
  }

  const selectRun = autoRunName || runs[0].name;

  // Auto-select run
  const selectedRun = runs.find(r => r.name === selectRun) || runs[0];
  resultsState.activeRun = selectedRun.name;
  renderSnapshotTabs(selectedRun, autoSnapId);
}

function renderSnapshotTabs(run, autoSnapId) {
  const panelsEl = document.getElementById('snap-panels');
  const viewer   = document.getElementById('snapshot-viewer');

  panelsEl.innerHTML = '';
  viewer.style.display = 'block';

  if (!run.snapshots || run.snapshots.length === 0) {
    panelsEl.innerHTML = '<span class="muted">No snapshots in this run.</span>';
    return;
  }

  // Show only the selected snapshot — the shared picker pills serve as the tab selector
  const activeSnap = autoSnapId || run.snapshots[0].id;
  resultsState.activeSnap = activeSnap;
  const snap = run.snapshots.find(s => s.id === activeSnap) || run.snapshots[0];

  const panel = document.createElement('div');
  panel.className = 'snap-panel';
  panel.appendChild(buildSnapPanel(snap));
  panelsEl.appendChild(panel);
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

async function viewFile(path, name, ext, scroll = true) {
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

  if (scroll) viewer.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
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

// ── Shared Snapshot Picker (Questions + Explorer) ────────────────────────────

const sharedSnapState = {
  snapshotId:   null,   // currently selected DuckDB snapshot_id
  snapshotDir:  null,   // snapshot_dir for the selected snapshot
  screenshot:   null,   // screenshot path
  runs:         [],     // [{name, snapshots:[{snapshot_id, sdp_name, run_name, snapshot_dir, screenshot}]}]
  activeRun:    null,   // run name string
};

// Called when the shared picker selects a new snapshot
function _onSharedSnapSelect(snapRow) {
  sharedSnapState.snapshotId  = snapRow.snapshot_id;
  sharedSnapState.snapshotDir = snapRow.snapshot_dir;
  sharedSnapState.screenshot  = snapRow.screenshot || null;

  _renderSharedScreenshot();

  // Update pill active states
  document.querySelectorAll('.shared-snap-pill').forEach(b => {
    b.classList.toggle('active', parseInt(b.dataset.snapId, 10) === snapRow.snapshot_id);
  });

  // Notify Explorer
  explorerState.snapshotId  = snapRow.snapshot_id;
  explorerState.selectedApiId = null;
  _catFilterSel.clear();
  const cols = document.getElementById('explorer-columns');
  if (cols) cols.style.display = 'none';
  const detail = document.getElementById('explorer-detail-panel');
  if (detail) detail.innerHTML = '';

  // Notify Questions
  questionsState.snapshotId = snapRow.snapshot_id;
  // Reset drill-down
  questionsCtrl.corrCategory = null;
  const corrTitle = document.getElementById('q-corr-title');
  if (corrTitle) corrTitle.textContent = 'Clock Correlation (Pearson r) — All DCs';
  const corrReset = document.getElementById('q-corr-reset');
  if (corrReset) corrReset.style.display = 'none';

  const activeTab = document.querySelector('.tab-btn.active')?.dataset.tab;
  if (activeTab === 'explorer') {
    loadExplorerDCs();
  } else if (activeTab === 'questions') {
    _fetchClockCorrelation(snapRow.snapshot_id);
    _fetchQuestionsData();
  } else if (activeTab === 'results') {
    if (snapRow.snapshot_dir) {
      const normDir = normPath(snapRow.snapshot_dir);
      const parts   = normDir.replace(/\\/g, '/').split('/');
      scanAnalyses(parts.slice(0, -2).join('/'), parts[parts.length - 2], parts[parts.length - 1]);
    }
  }
}

function _renderSharedScreenshot() {
  const wrap = document.getElementById('shared-snap-screenshot');
  if (!wrap) return;
  wrap.innerHTML = '';
  const shot = sharedSnapState.screenshot;
  if (!shot) return;
  const canvas = document.createElement('canvas');
  canvas.className = 'snap-screenshot-img';
  canvas.style.cssText = 'display:block;width:100%;border:1px solid var(--border);border-radius:6px;';
  const img = new Image();
  img.onload = () => {
    canvas.width  = img.naturalHeight;
    canvas.height = img.naturalWidth;
    const ctx = canvas.getContext('2d');
    ctx.translate(canvas.width / 2, canvas.height / 2);
    ctx.rotate(-Math.PI / 2);
    ctx.drawImage(img, -img.naturalWidth / 2, -img.naturalHeight / 2);
  };
  img.onerror = () => { wrap.innerHTML = ''; };
  img.src = `${FILES}/image?path=${encodeURIComponent(shot)}`;
  wrap.appendChild(canvas);
}

function _renderSharedRunList(runs) {
  const list = document.getElementById('shared-snap-run-list');
  if (!list) return;
  list.innerHTML = '';
  if (!runs.length) {
    list.innerHTML = '<span class="muted" style="font-size:12px">No runs found.</span>';
    return;
  }
  const wrap = document.createElement('div');
  wrap.style.cssText = 'display:flex;flex-wrap:wrap;gap:4px;';
  runs.forEach(run => {
    const btn = document.createElement('button');
    btn.className = 'run-item' + (run.name === sharedSnapState.activeRun ? ' active' : '');
    btn.textContent = run.name;
    btn.onclick = () => {
      sharedSnapState.activeRun = run.name;
      wrap.querySelectorAll('.run-item').forEach(b => b.classList.remove('active'));
      btn.classList.add('active');
      _renderSharedSnapPills(run.snapshots);
    };
    wrap.appendChild(btn);
  });
  list.appendChild(wrap);
}

function _renderSharedSnapPills(snapshots) {
  const pills = document.getElementById('shared-snap-pills');
  if (!pills) return;
  pills.innerHTML = '';
  const wrap = document.createElement('div');
  wrap.style.cssText = 'display:flex;flex-wrap:wrap;gap:4px;';
  snapshots.forEach(s => {
    const btn = document.createElement('button');
    btn.className = 'shared-snap-pill' + (s.snapshot_id === sharedSnapState.snapshotId ? ' active' : '');
    btn.dataset.snapId = s.snapshot_id;
    btn.textContent = `#${s.snapshot_id} ${s.sdp_name || s.run_name || ''}`;
    btn.title = s.snapshot_dir || '';
    btn.onclick = () => _onSharedSnapSelect(s);
    wrap.appendChild(btn);
  });
  pills.appendChild(wrap);
}

async function sharedSnapScan() {
  const dir = document.getElementById('shared-snap-dir')?.value.trim();
  const msg = document.getElementById('shared-snap-msg');
  if (dir) localStorage.setItem('analysisRoot', dir);
  if (msg) { msg.className = 'status-msg'; msg.textContent = 'Scanning…'; }

  // Load from DuckDB snapshots (no need to scan filesystem runs separately)
  try {
    const res = await apiGet(`${DATA}/snapshots`);
    if (!res.ok) {
      if (msg) { msg.className = 'status-msg s-error'; msg.textContent = res.error; }
      return;
    }
    msg.textContent = '';

    // Group by run_name
    const runMap = new Map();
    (res.data || []).forEach(s => {
      if (!runMap.has(s.run_name)) runMap.set(s.run_name, []);
      runMap.get(s.run_name).push(s);
    });
    sharedSnapState.runs = [...runMap.entries()].map(([name, snaps]) => ({ name, snapshots: snaps }));

    if (!sharedSnapState.activeRun && sharedSnapState.runs.length) {
      sharedSnapState.activeRun = sharedSnapState.runs[0].name;
    }
    _renderSharedRunList(sharedSnapState.runs);
    const activeRun = sharedSnapState.runs.find(r => r.name === sharedSnapState.activeRun) || sharedSnapState.runs[0];
    if (activeRun) _renderSharedSnapPills(activeRun.snapshots);

    // Auto-select the first snapshot if none selected yet
    if (!sharedSnapState.snapshotId && activeRun?.snapshots.length) {
      _onSharedSnapSelect(activeRun.snapshots[0]);
    } else if (sharedSnapState.snapshotId) {
      // Re-render screenshot for current selection
      _renderSharedScreenshot();
    }
  } catch (err) {
    if (msg) { msg.className = 'status-msg s-error'; msg.textContent = err.message; }
  }
}

async function sharedManualIngest() {
  const dir = document.getElementById('shared-ingest-dir')?.value.trim();
  const msg = document.getElementById('shared-snap-msg');
  if (!dir) { if (msg) { msg.className = 'status-msg s-error'; msg.textContent = 'Enter a snapshot_dir path first.'; } return; }
  if (msg) { msg.className = 'status-msg'; msg.textContent = 'Ingesting…'; }
  try {
    const res = await apiPost(`${DATA}/ingest?snapshot_dir=${encodeURIComponent(dir)}`, {});
    if (!res.ok) { if (msg) { msg.className = 'status-msg s-error'; msg.textContent = res.error || 'Ingest failed'; } return; }
    if (msg) { msg.className = 'status-msg s-success'; msg.textContent = `Ingested snapshot_id=${res.snapshot_id}`; }
    await sharedSnapScan();
    // Select the newly ingested snapshot
    for (const run of sharedSnapState.runs) {
      const s = run.snapshots.find(x => x.snapshot_id === res.snapshot_id);
      if (s) { _onSharedSnapSelect(s); break; }
    }
  } catch (err) {
    if (msg) { msg.className = 'status-msg s-error'; msg.textContent = err.message; }
  }
}

async function sharedReIngest() {
  const msg = document.getElementById('shared-snap-msg');
  const snapDir = sharedSnapState.snapshotDir;
  if (!snapDir) { if (msg) { msg.className = 'status-msg s-error'; msg.textContent = 'Select a snapshot first.'; } return; }
  if (msg) { msg.className = 'status-msg'; msg.textContent = 'Re-ingesting…'; }
  try {
    const res = await apiPost(`${DATA}/ingest?snapshot_dir=${encodeURIComponent(snapDir)}`, {});
    if (!res.ok) { if (msg) { msg.className = 'status-msg s-error'; msg.textContent = res.error || 'Re-ingest failed'; } return; }
    if (msg) { msg.className = 'status-msg s-success'; msg.textContent = `Re-ingested snapshot_id=${res.snapshot_id}`; }
    const activeTab = document.querySelector('.tab-btn.active')?.dataset.tab;
    if (activeTab === 'explorer') loadExplorerDCs();
    else if (activeTab === 'questions') { _fetchClockCorrelation(res.snapshot_id); _fetchQuestionsData(); }
  } catch (err) {
    if (msg) { msg.className = 'status-msg s-error'; msg.textContent = err.message; }
  }
}

// ── Explorer Tab ─────────────────────────────────────────────────────────────

const explorerState = {
  snapshotId:     null,
  dcs:            [],
  selectedApiId:  null,
};

const questionsState = { snapshotId: null };

// Legacy stub — kept so old call-sites don't break; shared picker is now authoritative
async function loadExplorerSnapshots() { await sharedSnapScan(); }
async function explorerManualIngest()  { await sharedManualIngest(); }
async function explorerReIngest()      { await sharedReIngest(); }
async function _loadSnapshotsIntoSelect() { /* replaced by shared picker */ }

// Category filter state — Set of selected categories; empty = show all
const _catFilterSel = new Set();
let   _catFilterAll = [];  // all known categories from last load

function _catFilterActive() {
  return _catFilterSel.size > 0 && _catFilterSel.size < _catFilterAll.length;
}

function _buildCatDropdown(categories) {
  _catFilterAll = categories;
  const dd  = document.getElementById('cat-filter-dropdown');
  if (!dd) return;
  dd.innerHTML = '';

  // "All" toggle
  const allLbl = document.createElement('label');
  allLbl.className = 'cat-dd-all';
  const allChk = document.createElement('input');
  allChk.type = 'checkbox';
  allChk.checked = _catFilterSel.size === 0;
  allChk.onchange = () => {
    _catFilterSel.clear();
    dd.querySelectorAll('.cat-item-chk').forEach(c => { c.checked = false; });
    allChk.checked = true;
    _applyCatFilter();
  };
  allLbl.appendChild(allChk);
  allLbl.appendChild(document.createTextNode('All'));
  dd.appendChild(allLbl);

  const sep = document.createElement('div');
  sep.className = 'cat-dd-sep';
  dd.appendChild(sep);

  categories.forEach(cat => {
    const lbl = document.createElement('label');
    const chk = document.createElement('input');
    chk.type = 'checkbox';
    chk.className = 'cat-item-chk';
    chk.checked = _catFilterSel.has(cat);
    chk.dataset.cat = cat;
    chk.onchange = () => {
      if (chk.checked) _catFilterSel.add(cat);
      else             _catFilterSel.delete(cat);
      // If none checked → treat as all
      if (_catFilterSel.size === 0) allChk.checked = true;
      else allChk.checked = false;
      _applyCatFilter();
    };
    const dot = document.createElement('span');
    dot.style.cssText = `display:inline-block;width:10px;height:10px;border-radius:2px;background:${_catColor(cat)};flex-shrink:0`;
    lbl.appendChild(chk);
    lbl.appendChild(dot);
    lbl.appendChild(document.createTextNode(' ' + cat));
    dd.appendChild(lbl);
  });
}

function _updateCatFilterBtn() {
  const btn = document.getElementById('cat-filter-btn');
  if (!btn) return;
  if (_catFilterSel.size === 0 || _catFilterSel.size === _catFilterAll.length) {
    btn.textContent = 'All categories ▾';
  } else if (_catFilterSel.size === 1) {
    btn.textContent = [..._catFilterSel][0] + ' ▾';
  } else {
    btn.textContent = `${_catFilterSel.size} categories ▾`;
  }
}

function _applyCatFilter() {
  _updateCatFilterBtn();
  const filtered = _catFilterActive()
    ? explorerState.dcs.filter(dc => _catFilterSel.has(dc.category || ''))
    : explorerState.dcs;
  const cnt = document.getElementById('explorer-dc-count');
  if (cnt) cnt.textContent = `(${filtered.length} DCs)`;
  renderExplorerDCTable(filtered);
  renderClockChart(filtered);
}

function toggleCatDropdown(e) {
  e.stopPropagation();
  const dd = document.getElementById('cat-filter-dropdown');
  if (!dd) return;
  dd.style.display = dd.style.display === 'none' ? '' : 'none';
}

// Close dropdown when clicking outside
document.addEventListener('click', e => {
  const wrap = document.getElementById('cat-filter-wrap');
  if (wrap && !wrap.contains(e.target)) {
    const dd = document.getElementById('cat-filter-dropdown');
    if (dd) dd.style.display = 'none';
  }
});

async function loadExplorerDCs() {
  const snapId = explorerState.snapshotId;
  if (!snapId) return;
  // Always fetch all DCs; filter client-side so dropdown stays populated
  const url = `${DATA}/draw_calls?snapshot_id=${snapId}`;

  const tbl = document.getElementById('explorer-dc-table');
  tbl.innerHTML = '<span class="muted" style="font-size:12px">Loading…</span>';
  document.getElementById('explorer-columns').style.display = '';

  try {
    const res = await apiGet(url);
    if (!res.ok) {
      tbl.innerHTML = `<span class="s-error">${escHtml(res.error)}</span>`;
      return;
    }
    explorerState.dcs = res.data || [];

    // Collect unique categories in appearance order
    const catSeen = new Set();
    const cats = [];
    for (const dc of explorerState.dcs) {
      const c = dc.category || '';
      if (!catSeen.has(c)) { catSeen.add(c); cats.push(c); }
    }
    // Remove stale selections that no longer exist
    for (const c of [..._catFilterSel]) {
      if (!catSeen.has(c)) _catFilterSel.delete(c);
    }
    _buildCatDropdown(cats);
    _updateCatFilterBtn();
    _applyCatFilter();
  } catch (err) {
    tbl.innerHTML = `<span class="s-error">${escHtml(err.message)}</span>`;
  }
}

// ── DC list column-tab state ─────────────────────────────────────────────────
if (!explorerState.colTab)  explorerState.colTab  = 'params';
if (!explorerState.sortCol) explorerState.sortCol = null;  // null = original order
if (!explorerState.sortDir) explorerState.sortDir = 1;     // 1=asc -1=desc

function setDcColTab(tab) {
  explorerState.colTab = tab;
  document.querySelectorAll('.dc-col-tab').forEach(b =>
    b.classList.toggle('active', b.dataset.colTab === tab));
  _applyCatFilter();
}

// Column definitions per tab.
// Each col: { key, label, val(dc, seqNo) }
function _dcColDefs(tab) {
  const SEQ   = { key: '_seq',     label: '#',        val: (dc, i) => i + 1 };
  const APID  = { key: 'api_id',   label: 'API ID',   val: dc => dc.api_id };
  const NAME  = { key: 'api_name', label: 'API Name', val: dc => dc.api_name || '—' };

  if (tab === 'params') return [
    SEQ, APID, NAME,
    { key: 'vertex_count',   label: 'Verts',    val: dc => dc.vertex_count   ?? '—' },
    { key: 'index_count',    label: 'Indices',  val: dc => dc.index_count    ?? '—' },
    { key: 'instance_count', label: 'Inst',     val: dc => dc.instance_count ?? '—' },
    { key: 'first_vertex',   label: 'fVtx',     val: dc => dc.first_vertex   ?? '—' },
    { key: 'first_index',    label: 'fIdx',     val: dc => dc.first_index    ?? '—' },
    { key: 'vertex_offset',  label: 'vOff',     val: dc => dc.vertex_offset  ?? '—' },
    { key: 'first_instance', label: 'fInst',    val: dc => dc.first_instance ?? '—' },
    { key: 'draw_count',     label: 'drawCnt',  val: dc => dc.draw_count     ?? '—' },
  ];

  if (tab === 'metrics') return [
    SEQ, APID, NAME,
    { key: 'clocks',                 label: 'Clocks',        val: dc => dc.clocks               ?? '—' },
    { key: 'fragments_shaded',       label: 'Frags',         val: dc => dc.fragments_shaded      ?? '—' },
    { key: 'vertices_shaded',        label: 'Verts',         val: dc => dc.vertices_shaded       ?? '—' },
    { key: 'read_total_bytes',       label: 'Read(B)',       val: dc => dc.read_total_bytes      ?? '—' },
    { key: 'write_total_bytes',      label: 'Write(B)',      val: dc => dc.write_total_bytes     ?? '—' },
    { key: 'shaders_busy_pct',       label: 'ShBusy%',      val: dc => _fmt1(dc.shaders_busy_pct) },
    { key: 'shaders_stalled_pct',    label: 'ShStall%',     val: dc => _fmt1(dc.shaders_stalled_pct) },
    { key: 'time_alus_working_pct',  label: 'ALU%',         val: dc => _fmt1(dc.time_alus_working_pct) },
    { key: 'tex_fetch_stall_pct',    label: 'TexStall%',    val: dc => _fmt1(dc.tex_fetch_stall_pct) },
    { key: 'tex_l1_miss_pct',        label: 'TexL1Miss%',   val: dc => _fmt1(dc.tex_l1_miss_pct) },
    { key: 'tex_pipes_busy_pct',     label: 'TexPipes%',    val: dc => _fmt1(dc.tex_pipes_busy_pct) },
    { key: 'lrz_pixels_killed',      label: 'LRZ',          val: dc => dc.lrz_pixels_killed     ?? '—' },
  ];

  // label tab
  return [
    SEQ, APID, NAME,
    { key: 'category',     label: 'Category',   val: dc => dc.category     || '—' },
    { key: 'subcategory',  label: 'Subcategory',val: dc => dc.subcategory  || '—' },
    { key: 'detail',       label: 'Detail',     val: dc => dc.detail       || '—' },
    { key: 'confidence',   label: 'Conf',       val: dc => dc.confidence != null ? dc.confidence.toFixed(2) : '—' },
    { key: 'label_source', label: 'Source',     val: dc => dc.label_source || '—' },
  ];
}

function _fmt1(v) { return v != null ? (+v).toFixed(1) : '—'; }

function renderExplorerDCTable(dcs) {
  const container = document.getElementById('explorer-dc-table');
  if (!dcs || dcs.length === 0) {
    container.innerHTML = '<span class="muted" style="font-size:12px">No draw calls found.</span>';
    return;
  }

  const cols    = _dcColDefs(explorerState.colTab);
  const sortCol = explorerState.sortCol;
  const sortDir = explorerState.sortDir;

  // Build indexed array preserving original order as seq number
  let rows = dcs.map((dc, i) => ({ dc, seq: i + 1 }));

  // Sort — null sortCol = original order (seq)
  if (sortCol && sortCol !== '_seq') {
    // Find col def in current tab first; fall back to searching all tabs so
    // switching tabs never resets an active sort (row order stays the same).
    let colDef = cols.find(c => c.key === sortCol);
    if (!colDef) {
      for (const t of ['params', 'metrics', 'label']) {
        colDef = _dcColDefs(t).find(c => c.key === sortCol);
        if (colDef) break;
      }
    }
    if (colDef) {
      rows.sort((a, b) => {
        const va = colDef.val(a.dc, a.seq - 1);
        const vb = colDef.val(b.dc, b.seq - 1);
        const aNone = va === '—' || va == null || va === '';
        const bNone = vb === '—' || vb == null || vb === '';
        if (aNone && bNone) return 0;
        if (aNone) return 1;
        if (bNone) return -1;
        if (!isNaN(+va) && !isNaN(+vb))
          return sortDir * (+va - +vb);
        return sortDir * String(va).localeCompare(String(vb));
      });
    }
  } else if (sortCol === '_seq') {
    rows.sort((a, b) => sortDir * (a.seq - b.seq));
  }

  const table = document.createElement('table');
  table.className = 'explorer-dc-table';

  // Header
  const thead = table.createTHead();
  const hrow  = thead.insertRow();
  cols.forEach(col => {
    const th = document.createElement('th');
    const isSorted = col.key === sortCol;
    th.innerHTML = escHtml(col.label)
      + (isSorted ? (sortDir === 1 ? ' <span class="sort-arrow">▲</span>' : ' <span class="sort-arrow">▼</span>') : '');
    if (col.key !== '_seq' && col.key !== '_groups') {
      th.classList.add('sortable');
      th.onclick = () => {
        if (explorerState.sortCol === col.key) {
          explorerState.sortDir *= -1;
        } else {
          explorerState.sortCol = col.key;
          explorerState.sortDir = 1;
        }
        renderExplorerDCTable(explorerState.dcs);
      };
    }
    hrow.appendChild(th);
  });

  // Body
  const tbody = table.createTBody();
  rows.forEach(({ dc, seq }) => {
    const tr = tbody.insertRow();
    tr.className = 'dc-row' + (dc.api_id === explorerState.selectedApiId ? ' active' : '');
    tr.onclick = () => loadExplorerDCDetail(dc.api_id);

    cols.forEach((col, ci) => {
      const td = tr.insertCell();
      const val = col.val(dc, seq - 1);
      td.textContent = val;
      if (ci === 0) td.className = 'dc-seq-cell';  // seq column styling
    });
  });

  container.innerHTML = '';
  container.appendChild(table);
}

// ── DC clock bar chart ────────────────────────────────────────────────────────

// Category → bar colour (mirrors label tab colour cues)
const _CAT_COLORS = {
  Scene:      '#3b82f6',
  Shadow:     '#64748b',
  UI:         '#8b5cf6',
  PostFX:     '#ec4899',
  Character:  '#f59e0b',
  Terrain:    '#22c55e',
  Particles:  '#f97316',
  Compute:    '#06b6d4',
  Unknown:    '#94a3b8',
};
function _catColor(cat) {
  if (!cat) return '#94a3b8';
  for (const [k, v] of Object.entries(_CAT_COLORS)) {
    if (cat.toLowerCase().startsWith(k.toLowerCase())) return v;
  }
  return '#94a3b8';
}

function renderClockChart(dcs) {
  const wrap = document.getElementById('dc-clock-chart-wrap');
  const canvas = document.getElementById('dc-clock-chart');
  const tooltip = document.getElementById('dc-clock-tooltip');
  if (!wrap || !canvas) return;

  // Only show if at least some DCs have clock data
  const hasClock = dcs.some(dc => dc.clocks != null && dc.clocks > 0);
  if (!hasClock) { wrap.style.display = 'none'; return; }
  wrap.style.display = '';

  const PADDING = { top: 12, right: 8, bottom: 20, left: 44 };
  const W = canvas.offsetWidth || canvas.parentElement.offsetWidth || 600;
  const H = 220;
  canvas.width  = W;
  canvas.height = H;

  const ctx = canvas.getContext('2d');
  ctx.clearRect(0, 0, W, H);

  const chartW = W - PADDING.left - PADDING.right;
  const chartH = H - PADDING.top  - PADDING.bottom;

  const maxClock = Math.max(...dcs.map(dc => dc.clocks || 0));
  if (maxClock === 0) { wrap.style.display = 'none'; return; }

  const barW   = Math.max(1, chartW / dcs.length);
  const gap    = barW > 4 ? Math.max(1, barW * 0.15) : 0;
  const fillW  = barW - gap;

  // Y-axis labels
  ctx.fillStyle = '#94a3b8';
  ctx.font = '10px system-ui, sans-serif';
  ctx.textAlign = 'right';
  ctx.textBaseline = 'middle';
  const yTicks = 3;
  for (let i = 0; i <= yTicks; i++) {
    const v = (maxClock * i) / yTicks;
    const y = PADDING.top + chartH - (chartH * i / yTicks);
    ctx.fillText(_fmtK(v), PADDING.left - 4, y);
    ctx.strokeStyle = '#e5e7eb';
    ctx.lineWidth = 0.5;
    ctx.beginPath();
    ctx.moveTo(PADDING.left, y);
    ctx.lineTo(PADDING.left + chartW, y);
    ctx.stroke();
  }

  // Bars
  dcs.forEach((dc, i) => {
    const clk = dc.clocks || 0;
    const barH = (clk / maxClock) * chartH;
    const x = PADDING.left + i * barW + gap / 2;
    const y = PADDING.top + chartH - barH;
    ctx.fillStyle = dc.api_id === explorerState.selectedApiId
      ? '#f59e0b'
      : _catColor(dc.category);
    ctx.fillRect(x, y, fillW, barH);
  });

  // X-axis baseline
  ctx.strokeStyle = '#cbd5e1';
  ctx.lineWidth = 1;
  ctx.beginPath();
  ctx.moveTo(PADDING.left, PADDING.top + chartH);
  ctx.lineTo(PADDING.left + chartW, PADDING.top + chartH);
  ctx.stroke();

  // Hover / click handling — attach only once, replace by re-creating handlers
  canvas._chartMeta = { dcs, barW, gap, PADDING, chartH, maxClock, fillW };

  canvas.onmousemove = (e) => {
    const rect = canvas.getBoundingClientRect();
    const mx = (e.clientX - rect.left) * (canvas.width / rect.width);
    const meta = canvas._chartMeta;
    const idx = Math.floor((mx - meta.PADDING.left) / meta.barW);
    if (idx < 0 || idx >= meta.dcs.length) { tooltip.style.display = 'none'; return; }
    const dc = meta.dcs[idx];
    tooltip.style.display = '';
    tooltip.textContent = `#${dc.api_id} ${dc.api_name || ''} | clocks: ${(dc.clocks||0).toLocaleString()} | ${dc.category || '—'}`;
    // Position tooltip: follow mouse, clamp to canvas bounds
    const tx = Math.min(e.offsetX + 12, wrap.offsetWidth - tooltip.offsetWidth - 4);
    const ty = Math.max(0, e.offsetY - 28);
    tooltip.style.left = tx + 'px';
    tooltip.style.top  = ty + 'px';
  };
  canvas.onmouseleave = () => { tooltip.style.display = 'none'; };
  canvas.onclick = (e) => {
    const rect = canvas.getBoundingClientRect();
    const mx = (e.clientX - rect.left) * (canvas.width / rect.width);
    const meta = canvas._chartMeta;
    const idx = Math.floor((mx - meta.PADDING.left) / meta.barW);
    if (idx >= 0 && idx < meta.dcs.length) {
      loadExplorerDCDetail(meta.dcs[idx].api_id);
    }
  };
}

function _fmtK(v) {
  if (v >= 1_000_000) return (v / 1_000_000).toFixed(1) + 'M';
  if (v >= 1_000)     return (v / 1_000).toFixed(0) + 'K';
  return String(Math.round(v));
}

async function loadExplorerDCDetail(apiId) {
  explorerState.selectedApiId = apiId;
  // Re-render table + chart to update active highlight
  _applyCatFilter();

  const panel = document.getElementById('explorer-detail-panel');
  panel.innerHTML = '<div class="card"><span class="muted">Loading…</span></div>';

  try {
    const res = await apiGet(`${DATA}/dc/${apiId}?snapshot_id=${explorerState.snapshotId}`);
    if (!res.ok) {
      panel.innerHTML = `<div class="card"><span class="s-error">${escHtml(res.error)}</span></div>`;
      return;
    }
    renderExplorerDCDetail(panel, res.data);
  } catch (err) {
    panel.innerHTML = `<div class="card"><span class="s-error">${escHtml(err.message)}</span></div>`;
  }
}

// Return a compact param string based on api_name
function _dcParamSummary(dc) {
  const n = dc.api_name || '';
  if (n === 'vkCmdDraw')
    return `vtx=${dc.vertex_count ?? 0} inst=${dc.instance_count ?? 0} fv=${dc.first_vertex ?? 0}`;
  if (n === 'vkCmdDrawIndexed')
    return `idx=${dc.index_count ?? 0} inst=${dc.instance_count ?? 0} fi=${dc.first_index ?? 0} vo=${dc.vertex_offset ?? 0}`;
  if (n === 'vkCmdDrawIndirect' || n === 'vkCmdDrawIndexedIndirect')
    return `drawCount=${dc.draw_count ?? 0}`;
  if (n === 'vkCmdDispatch')
    return `${dc.group_count_x ?? 0}×${dc.group_count_y ?? 0}×${dc.group_count_z ?? 0}`;
  // fallback
  if ((dc.vertex_count ?? 0) > 0) return `vtx=${dc.vertex_count}`;
  if ((dc.index_count  ?? 0) > 0) return `idx=${dc.index_count}`;
  return '—';
}

// Return full params kv pairs for the detail panel
function _dcParamRows(dc) {
  const n = dc.api_name || '';
  if (n === 'vkCmdDraw') return [
    ['vertexCount',   dc.vertex_count   ?? 0],
    ['instanceCount', dc.instance_count ?? 0],
    ['firstVertex',   dc.first_vertex   ?? 0],
    ['firstInstance', dc.first_instance ?? 0],
  ];
  if (n === 'vkCmdDrawIndexed') return [
    ['indexCount',    dc.index_count    ?? 0],
    ['instanceCount', dc.instance_count ?? 0],
    ['firstIndex',    dc.first_index    ?? 0],
    ['vertexOffset',  dc.vertex_offset  ?? 0],
    ['firstInstance', dc.first_instance ?? 0],
  ];
  if (n === 'vkCmdDrawIndirect' || n === 'vkCmdDrawIndexedIndirect') return [
    ['drawCount', dc.draw_count ?? 0],
  ];
  if (n === 'vkCmdDispatch') return [
    ['groupCountX', dc.group_count_x ?? 0],
    ['groupCountY', dc.group_count_y ?? 0],
    ['groupCountZ', dc.group_count_z ?? 0],
  ];
  // fallback — show whatever is non-zero
  const rows = [];
  if ((dc.vertex_count   ?? 0) !== 0) rows.push(['vertexCount',   dc.vertex_count]);
  if ((dc.index_count    ?? 0) !== 0) rows.push(['indexCount',    dc.index_count]);
  if ((dc.instance_count ?? 0) !== 0) rows.push(['instanceCount', dc.instance_count]);
  if ((dc.draw_count     ?? 0) !== 0) rows.push(['drawCount',     dc.draw_count]);
  if ((dc.group_count_x  ?? 0) !== 0) rows.push(['groupCountX',   dc.group_count_x]);
  return rows.length ? rows : [['(no params)', '—']];
}

function renderExplorerDCDetail(container, dc) {
  container.innerHTML = '';

  const card = document.createElement('div');
  card.className = 'card';

  // ── Compact top: DC params + label + metrics in one card ──────────
  const titleRow = document.createElement('div');
  titleRow.className = 'dc-detail-title';
  titleRow.innerHTML = `<span>Draw Call #${dc.api_id}</span>`;
  card.appendChild(titleRow);

  const topGrid = document.createElement('div');
  topGrid.className = 'dc-detail-grid';

  // Params column
  const paramsCol = document.createElement('div');
  paramsCol.className = 'dc-detail-col';
  const lbl = dc.label;
  const paramRows = [
    ['API Name',    dc.api_name    || '—'],
    ['Pipeline',    dc.pipeline_id ?? '—'],
    ..._dcParamRows(dc),
    ['Category',    lbl?.category    || '—'],
    ['Subcategory', lbl?.subcategory || '—'],
    ['Detail',      lbl?.detail      || '—'],
    ['Confidence',  lbl?.confidence != null ? Number(lbl.confidence).toFixed(2) : '—'],
    ['Label src',   lbl?.label_source || '—'],
  ];
  paramRows.forEach(([k, v]) => {
    const row = document.createElement('div');
    row.className = 'dc-kv-row';
    row.innerHTML = `<span class="dc-kv-key">${escHtml(k)}</span><span class="dc-kv-val">${escHtml(String(v))}</span>`;
    paramsCol.appendChild(row);
  });
  topGrid.appendChild(paramsCol);

  // Metrics column
  const metricsCol = document.createElement('div');
  metricsCol.className = 'dc-detail-col';
  const stats = dc.metric_stats || {};
  if (dc.metrics && Object.keys(dc.metrics).length > 0) {
    const metricEntries = Object.entries(dc.metrics).filter(([k]) =>
      !['snapshot_id', 'api_id'].includes(k));
    metricEntries.forEach(([k, v]) => {
      const row = document.createElement('div');
      row.className = 'dc-kv-row dc-metric-row';

      const s = stats[k];
      let heatBg = '';
      let medianHtml = '';
      if (s && s.min != null && s.max != null && s.median != null && typeof v === 'number') {
        const val = v, mn = s.min, mx = s.max, med = s.median;
        // normalise 0→green, 0.5→yellow, 1→red relative to [min, max]
        const range = mx - mn;
        const t = range > 0 ? Math.max(0, Math.min(1, (val - mn) / range)) : 0;
        const r = Math.round(t < 0.5 ? t * 2 * 255 : 255);
        const g = Math.round(t < 0.5 ? 255 : (1 - t) * 2 * 255);
        heatBg = `background:rgba(${r},${g},0,0.18);border-radius:3px;`;
        const fmtMed = med >= 1e6 ? (med/1e6).toFixed(1)+'M'
                     : med >= 1e3 ? (med/1e3).toFixed(1)+'K'
                     : med.toFixed(1);
        medianHtml = `<span class="dc-metric-median" title="median / min / max">`
          + `med:${fmtMed}</span>`;
      }

      const fmtVal = typeof v === 'number'
        ? (v >= 1e6 ? (v/1e6).toFixed(2)+'M' : v >= 1e3 ? (v/1e3).toFixed(1)+'K' : String(v))
        : (v != null ? String(v) : '—');

      row.style.cssText = heatBg;
      row.innerHTML = `<span class="dc-kv-key">${escHtml(k)}</span>`
        + `<span class="dc-kv-val">${escHtml(fmtVal)}</span>`
        + medianHtml;
      metricsCol.appendChild(row);
    });
  } else {
    metricsCol.innerHTML = '<span class="muted" style="font-size:12px">No metrics.</span>';
  }
  topGrid.appendChild(metricsCol);
  card.appendChild(topGrid);

  // ── Sub-lists ─────────────────────────────────────────────────────

  // Shaders sub-list
  const shadersSection = _buildDetailSection(`Shaders (${(dc.shader_stages||[]).length})`, true);
  if (dc.shader_stages && dc.shader_stages.length > 0) {
    const list = document.createElement('div');
    list.className = 'dc-sublist';
    dc.shader_stages.forEach(s => {
      const item = _buildSublistItem(
        `${s.stage || '?'} · ${s.entry_point || s.module_id || '—'}`,
        null
      );
      if (s.file_path) {
        const fp   = s.file_path;
        const name = fp.split(/[\\/]/).pop();
        const ext  = name.split('.').pop().toLowerCase();
        // Download button
        const dlBtn = document.createElement('a');
        dlBtn.href = `${FILES}/raw?path=${encodeURIComponent(fp)}&download=1`;
        dlBtn.download = name;
        dlBtn.textContent = '⬇';
        dlBtn.title = 'Download ' + name;
        dlBtn.style.cssText = 'margin-left:6px;font-size:12px;text-decoration:none;opacity:.7;cursor:pointer';
        dlBtn.onclick = e => e.stopPropagation();
        item.el.querySelector('.dc-sublist-label')?.appendChild(dlBtn);
        item.setExpand(() => {
          const wrap = document.createElement('div');
          wrap.className = 'dc-sublist-preview';
          viewFileInto(wrap, fp, name, ext);
          return wrap;
        });
      }
      list.appendChild(item.el);
    });
    shadersSection.body.appendChild(list);
  } else {
    shadersSection.body.innerHTML = '<span class="muted" style="font-size:12px">No shaders.</span>';
  }
  // Textures sub-list
  const texSection = _buildDetailSection(`Textures (${(dc.textures||[]).length})`, true);
  if (dc.textures && dc.textures.length > 0) {
    const list = document.createElement('div');
    list.className = 'dc-sublist';
    dc.textures.forEach(t => {
      const dims = (t.width && t.height) ? `${t.width}x${t.height}${t.depth > 1 ? 'x'+t.depth : ''}` : '';
      const label = `#${t.texture_id} · ${dims || '—'} · ${t.format || '—'}`;
      const item = _buildSublistItem(label, null);
      if (t.file_path) {
        const fp   = t.file_path;
        const name = fp.split(/[\\/]/).pop();
        const ext  = name.split('.').pop().toLowerCase();
        item.setExpand(() => {
          const wrap = document.createElement('div');
          wrap.className = 'dc-sublist-preview';
          if (ext === 'png' || ext === 'jpg' || ext === 'jpeg' || ext === 'bmp') {
            const img = document.createElement('img');
            img.className = 'dc-preview-img';
            img.src = `${FILES}/image?path=${encodeURIComponent(fp)}`;
            img.alt = name;
            wrap.appendChild(img);
          } else {
            viewFileInto(wrap, fp, name, ext);
          }
          return wrap;
        });
      }
      list.appendChild(item.el);
    });
    texSection.body.appendChild(list);
  } else {
    texSection.body.innerHTML = '<span class="muted" style="font-size:12px">No textures.</span>';
  }
  // Render Targets sub-list
  const rtList = dc.render_targets || [];
  const rtSection = _buildDetailSection(`Render Targets (${rtList.length})`, true);
  if (rtList.length > 0) {
    const list = document.createElement('div');
    list.className = 'dc-sublist';
    rtList.forEach(rt => {
      const typeStr = rt.attachment_type || '—';
      const dims    = (rt.width && rt.height) ? `${rt.width}×${rt.height}` : '—';
      const fmt     = rt.format || '—';
      const label   = `[${rt.attachment_index ?? '?'}] ${typeStr} · ${dims} · ${fmt}`;
      const item    = _buildSublistItem(label, null);
      // Show extra fields on expand
      item.setExpand(() => {
        const wrap = document.createElement('div');
        wrap.style.cssText = 'padding:6px 10px;font-size:12px;display:grid;grid-template-columns:140px 1fr;gap:2px 8px';
        const rows = [
          ['resource_id',    rt.resource_id],
          ['renderpass_id',  rt.renderpass_id],
          ['framebuffer_id', rt.framebuffer_id],
          ['width',          rt.width],
          ['height',         rt.height],
          ['format',         rt.format],
          ['attachment_type',rt.attachment_type],
        ];
        rows.forEach(([k, v]) => {
          if (v == null) return;
          wrap.innerHTML += `<span style="color:var(--text-muted);font-weight:600">${escHtml(k)}</span>`
            + `<span style="font-family:monospace">${escHtml(String(v))}</span>`;
        });
        return wrap;
      });
      list.appendChild(item.el);
    });
    rtSection.body.appendChild(list);
  } else {
    rtSection.body.innerHTML = '<span class="muted" style="font-size:12px">No render targets.</span>';
  }
  // Buffers/Mesh sub-list
  const meshSection = _buildDetailSection('Buffers / Mesh', true);
  if (dc.mesh_file) {
    const list = document.createElement('div');
    list.className = 'dc-sublist';
    const fp   = dc.mesh_file;
    const name = fp.split(/[\\/]/).pop();
    const item = _buildSublistItem(name, null);
    item.setExpand(() => {
      const wrap = document.createElement('div');
      wrap.className = 'dc-sublist-preview';
      _buildMeshViewer(wrap, fp);
      return wrap;
    });
    list.appendChild(item.el);
    meshSection.body.appendChild(list);
  } else {
    meshSection.body.innerHTML = '<span class="muted" style="font-size:12px">No mesh file.</span>';
  }
  // Order: Textures → Mesh → Render Targets → Shaders
  card.appendChild(texSection.el);
  card.appendChild(meshSection.el);
  card.appendChild(rtSection.el);
  card.appendChild(shadersSection.el);

  container.appendChild(card);
  container.scrollTop = 0;
}

// Build a clickable sub-list item that expands inline on click.
// Returns {el, setExpand(fn)} — setExpand registers the expand factory function.
function _buildSublistItem(label, badge) {
  const el = document.createElement('div');
  el.className = 'dc-sublist-item';

  const hdr = document.createElement('div');
  hdr.className = 'dc-sublist-hdr';
  hdr.innerHTML = `<span class="dc-sublist-chevron">▶</span><span class="dc-sublist-label">${escHtml(label)}</span>`;
  if (badge) {
    const b = document.createElement('span');
    b.className = 'dc-sublist-badge';
    b.textContent = badge;
    hdr.appendChild(b);
  }
  el.appendChild(hdr);

  const expandEl = document.createElement('div');
  expandEl.className = 'dc-sublist-expand';
  expandEl.style.display = 'none';
  el.appendChild(expandEl);

  let expandFn = null;
  let loaded   = false;

  hdr.style.cursor = 'pointer';
  hdr.onclick = () => {
    const open = expandEl.style.display !== 'none';
    if (!open) {
      if (!loaded && expandFn) {
        expandEl.appendChild(expandFn());
        loaded = true;
      }
      expandEl.style.display = '';
      hdr.querySelector('.dc-sublist-chevron').textContent = '▼';
    } else {
      expandEl.style.display = 'none';
      hdr.querySelector('.dc-sublist-chevron').textContent = '▶';
    }
  };

  return {
    el,
    setExpand(fn) { expandFn = fn; hdr.classList.add('dc-sublist-hdr--expandable'); },
  };
}

// Load a file's content into a DOM element (no external viewer card).
async function viewFileInto(container, fp, name, ext) {
  container.innerHTML = '<span class="muted" style="font-size:12px">Loading…</span>';
  try {
    const res = await apiGet(`${FILES}/read?path=${encodeURIComponent(fp)}`);
    if (!res.ok) {
      container.innerHTML = `<span class="s-error" style="font-size:12px">${escHtml(res.error)}</span>`;
      return;
    }
    const content = res.data.content || '';
    if (ext === 'md') {
      const div = document.createElement('div');
      div.className = 'md viewer-body';
      div.style.padding = '10px 0';
      div.innerHTML = typeof marked !== 'undefined' ? marked.parse(content) : `<pre class="code-pre">${escHtml(content)}</pre>`;
      container.innerHTML = '';
      container.appendChild(div);
      if (typeof mermaid !== 'undefined') {
        div.querySelectorAll('pre code.language-mermaid').forEach(code => {
          const d = document.createElement('div');
          d.className = 'mermaid';
          d.textContent = code.textContent;
          code.parentElement.replaceWith(d);
        });
        mermaid.run({ nodes: div.querySelectorAll('.mermaid') });
      }
    } else {
      container.innerHTML = `<pre class="code-pre" style="max-height:400px;overflow-y:auto">${escHtml(content)}</pre>`;
    }
  } catch (err) {
    container.innerHTML = `<span class="s-error" style="font-size:12px">${escHtml(err.message)}</span>`;
  }
}

// ── 3D OBJ mesh viewer (Three.js) ────────────────────────────────────────────

function _buildMeshViewer(container, fp) {
  if (typeof THREE === 'undefined' || typeof THREE.OBJLoader === 'undefined') {
    container.innerHTML = '<span class="muted" style="font-size:12px">Three.js not loaded — cannot preview mesh.</span>';
    return;
  }

  const wrap = document.createElement('div');
  wrap.className = 'mesh-viewer-wrap';

  // Toolbar: wireframe toggle + stats
  const toolbar = document.createElement('div');
  toolbar.className = 'mesh-viewer-toolbar';
  const btnWire = document.createElement('button');
  btnWire.className = 'btn-secondary btn-sm';
  btnWire.textContent = 'Wireframe';
  btnWire.style.cssText = 'padding:2px 8px;font-size:11px';
  const statsSpan = document.createElement('span');
  statsSpan.className = 'mesh-viewer-stats';
  toolbar.appendChild(btnWire);
  toolbar.appendChild(statsSpan);

  const hint = document.createElement('div');
  hint.className = 'mesh-viewer-hint';
  hint.textContent = 'Drag to rotate · Scroll to zoom · Right-drag to pan';

  const loading = document.createElement('div');
  loading.className = 'mesh-viewer-loading';
  loading.textContent = 'Loading mesh…';

  wrap.appendChild(toolbar);
  wrap.appendChild(loading);
  wrap.appendChild(hint);
  container.appendChild(wrap);

  // Defer init until after the element is in the DOM and laid out
  setTimeout(() => {
    const W = wrap.offsetWidth  || 560;
    const H = wrap.offsetHeight || 340;

    // Scene
    const scene = new THREE.Scene();
    scene.background = new THREE.Color(0x1e293b);

    const camera = new THREE.PerspectiveCamera(45, W / H, 0.01, 10000);
    camera.position.set(0, 0, 5);

    const renderer = new THREE.WebGLRenderer({ antialias: true });
    renderer.setPixelRatio(Math.min(window.devicePixelRatio, 2));
    renderer.setSize(W, H);
    renderer.domElement.className = 'mesh-viewer-canvas';
    wrap.insertBefore(renderer.domElement, hint);

    // Lights
    scene.add(new THREE.AmbientLight(0xffffff, 0.55));
    const sun = new THREE.DirectionalLight(0xffffff, 0.9);
    sun.position.set(2, 3, 4);
    scene.add(sun);
    const fill = new THREE.DirectionalLight(0x8090cc, 0.35);
    fill.position.set(-3, -1, -2);
    scene.add(fill);

    // OrbitControls
    const controls = new THREE.OrbitControls(camera, renderer.domElement);
    controls.enableDamping = true;
    controls.dampingFactor = 0.08;

    // Wireframe state
    let wireframe = false;
    const meshes = [];
    btnWire.onclick = () => {
      wireframe = !wireframe;
      btnWire.classList.toggle('active', wireframe);
      meshes.forEach(m => { m.material.wireframe = wireframe; });
    };

    // Load OBJ
    const loader = new THREE.OBJLoader();
    loader.load(
      `${FILES}/raw?path=${encodeURIComponent(fp)}`,
      obj => {
        loading.remove();

        const mat = new THREE.MeshPhongMaterial({
          color: 0x5b9bd5,
          specular: 0x333344,
          shininess: 50,
          side: THREE.DoubleSide,
        });

        let totalVerts = 0, totalTris = 0;
        obj.traverse(child => {
          if (!child.isMesh) return;
          child.material = mat.clone();
          meshes.push(child);
          const geo = child.geometry;
          const pos = geo.attributes.position;
          if (pos) totalVerts += pos.count;
          if (geo.index) totalTris += geo.index.count / 3;
          else if (pos)  totalTris += pos.count / 3;
        });

        statsSpan.textContent =
          `Verts: ${totalVerts.toLocaleString()} · Tris: ${Math.round(totalTris).toLocaleString()}`;

        // Center mesh at origin
        const box    = new THREE.Box3().setFromObject(obj);
        const center = new THREE.Vector3();
        box.getCenter(center);
        obj.position.sub(center);
        scene.add(obj);

        // Fit camera to bounding sphere
        const sphere = new THREE.Sphere();
        box.getBoundingSphere(sphere);
        const r      = sphere.radius || 1;
        const fovRad = THREE.MathUtils.degToRad(camera.fov);
        const dist   = (r / Math.sin(fovRad / 2)) * 1.25;
        camera.position.set(0, r * 0.4, dist);
        camera.near = dist / 200;
        camera.far  = dist * 20;
        camera.updateProjectionMatrix();
        controls.target.set(0, 0, 0);
        controls.update();
      },
      null,
      err => {
        loading.textContent = 'OBJ load failed';
        console.error('OBJ load error', err);
      }
    );

    // Animation loop — auto-stop when detached from DOM
    let animId;
    const tick = () => {
      animId = requestAnimationFrame(tick);
      controls.update();
      renderer.render(scene, camera);
      if (!document.body.contains(renderer.domElement)) {
        cancelAnimationFrame(animId);
        renderer.dispose();
      }
    };
    tick();

    // Resize observer
    new ResizeObserver(() => {
      const nW = wrap.offsetWidth;
      const nH = wrap.offsetHeight;
      if (nW > 0 && nH > 0) {
        camera.aspect = nW / nH;
        camera.updateProjectionMatrix();
        renderer.setSize(nW, nH);
      }
    }).observe(wrap);
  }, 50);
}

// Build a collapsible detail section (returns {el, body})
function _buildDetailSection(title, defaultOpen) {
  const section = document.createElement('div');
  section.className = 'result-section';

  const hdr = document.createElement('div');
  hdr.className = 'result-section-hdr';
  hdr.style.cursor = 'pointer';
  hdr.innerHTML = `<span class="result-section-chevron">${defaultOpen ? '▼' : '▶'}</span> ${escHtml(title)}`;

  const body = document.createElement('div');
  body.className = 'result-section-body';
  body.style.display = defaultOpen ? '' : 'none';

  hdr.onclick = () => {
    const open = body.style.display !== 'none';
    body.style.display = open ? 'none' : '';
    hdr.querySelector('.result-section-chevron').textContent = open ? '▶' : '▼';
  };

  section.appendChild(hdr);
  section.appendChild(body);
  return { el: section, body };
}

// ── Questions Tab ─────────────────────────────────────────────────────────────

const questionsCtrl = {
  chartType:      'bar',
  allData:        [],   // [{category, dc_count, metricName:{sum,median,min,max,variance}}]
  columns:        [],   // available metric column names
  selectedMetric: '',   // metric shown in right chart
  labelCorrs:     {},   // {category: {r, n}} for selectedMetric
  corrCategory:   null, // null = all DCs; string = drill-down to that label
};

const _Q_AGGS = ['sum', 'median', 'min', 'max', 'variance'];

async function loadQuestionsSnapshots() { /* replaced by shared picker */ }
async function _loadAvailableMetrics()  { /* no-op */ }

function setQChartType(type) {
  questionsCtrl.chartType = type;
  document.querySelectorAll('.q-switch-opt[data-chart]').forEach(b =>
    b.classList.toggle('active', b.dataset.chart === type));
  if (questionsCtrl.allData.length) _renderQuestionsResult();
}

function setQCorrCategory(cat) {
  questionsCtrl.corrCategory = cat;
  const title  = document.getElementById('q-corr-title');
  const reset  = document.getElementById('q-corr-reset');
  if (title) title.textContent = cat
    ? `Clock Correlation (Pearson r) — ${cat}`
    : 'Clock Correlation (Pearson r) — All DCs';
  if (reset) reset.style.display = cat ? '' : 'none';

  // Keep label dropdown in sync
  const sel = document.getElementById('q-label-select');
  if (sel) sel.value = cat || '';

  // Highlight selected row in the table
  document.querySelectorAll('#questions-table-wrap tr[data-cat]').forEach(tr => {
    tr.classList.toggle('q-row-selected', tr.dataset.cat === cat);
  });

  const snapId = sharedSnapState.snapshotId;
  if (snapId) _fetchClockCorrelation(snapId);
}

function _buildMetricButtons(columns) {
  const wrap = document.getElementById('q-metric-btns');
  if (!wrap) return;
  wrap.innerHTML = '';
  const nonClock = columns.filter(c => c !== 'clocks');
  nonClock.forEach(col => {
    const btn = document.createElement('button');
    btn.className = 'q-metric-btn' + (col === questionsCtrl.selectedMetric ? ' active' : '');
    btn.textContent = col;
    btn.onclick = () => {
      questionsCtrl.selectedMetric = col;
      wrap.querySelectorAll('.q-metric-btn').forEach(b => b.classList.toggle('active', b === btn));
      const snapId = sharedSnapState.snapshotId;
      if (snapId) _fetchLabelCorrelations(snapId);
      _renderQuestionsResult();
    };
    wrap.appendChild(btn);
  });
  // If selectedMetric no longer in columns, pick first
  if (!nonClock.includes(questionsCtrl.selectedMetric) && nonClock.length) {
    questionsCtrl.selectedMetric = nonClock[0];
    wrap.querySelector('.q-metric-btn')?.classList.add('active');
  }
}

async function _fetchQuestionsData() {
  const snapId = sharedSnapState.snapshotId;
  const msg    = document.getElementById('questions-msg');
  if (!snapId) return;

  questionsState.snapshotId = snapId;
  // Reset drill-down when switching snapshots
  questionsCtrl.corrCategory = null;
  const _corrTitle = document.getElementById('q-corr-title');
  if (_corrTitle) _corrTitle.textContent = 'Clock Correlation (Pearson r) — All DCs';
  const _corrReset = document.getElementById('q-corr-reset');
  if (_corrReset) _corrReset.style.display = 'none';
  const _labelSel = document.getElementById('q-label-select');
  if (_labelSel) _labelSel.value = '';
  msg.textContent = 'Loading…';
  msg.className   = 'status-msg s-info';

  try {
    const res = await apiGet(`${DATA}/label_agg_multi?snapshot_id=${snapId}`);
    if (!res.ok) {
      msg.textContent = res.error;
      msg.className   = 'status-msg s-error';
      document.getElementById('questions-result-card').style.display = 'none';
      return;
    }
    msg.textContent = '';
    msg.className   = 'status-msg';
    questionsCtrl.allData = res.data    || [];
    questionsCtrl.columns = res.columns || [];

    _buildMetricButtons(questionsCtrl.columns);
    _renderQuestionsResult();
    _fetchClockCorrelation(snapId);
    _fetchLabelCorrelations(snapId);
  } catch (err) {
    msg.textContent = err.message;
    msg.className   = 'status-msg s-error';
  }
}

async function loadQuestionsData() { await _fetchQuestionsData(); }

async function _fetchClockCorrelation(snapId) {
  const card = document.getElementById('q-corr-card');
  const body = document.getElementById('q-corr-body');
  if (!card || !body) return;
  body.innerHTML = '<span class="muted" style="font-size:12px">Computing…</span>';
  card.style.display = '';
  const cat = questionsCtrl.corrCategory;
  const url = `${DATA}/clock_correlation?snapshot_id=${snapId}` +
              (cat ? `&category=${encodeURIComponent(cat)}` : '');
  try {
    const res = await apiGet(url);
    if (!res.ok) { body.innerHTML = `<span class="s-error">${escHtml(res.error)}</span>`; return; }
    _renderCorrTable(res.data || []);
  } catch (err) {
    body.innerHTML = `<span class="s-error">${escHtml(err.message)}</span>`;
  }
}

function _renderCorrTable(rows) {
  const body = document.getElementById('q-corr-body');
  if (!rows.length) {
    body.innerHTML = '<span class="muted" style="font-size:12px">Not enough paired data (need ≥10 DCs with both clocks and metric values).</span>';
    return;
  }

  const frag = document.createDocumentFragment();
  rows.forEach(row => {
    const r   = row.r;
    const abs = Math.abs(r);
    const pos = r >= 0;
    const color = pos ? '#3b82f6' : '#22c55e';

    const div = document.createElement('div');
    div.className = 'q-corr-row';

    const name = document.createElement('div');
    name.className   = 'q-corr-name';
    name.textContent = row.metric;
    name.title       = row.metric;

    const barWrap = document.createElement('div');
    barWrap.className = 'q-corr-bar-wrap';

    const halfNeg = document.createElement('div');
    halfNeg.className = 'q-corr-half q-corr-half-neg';
    const halfPos = document.createElement('div');
    halfPos.className = 'q-corr-half q-corr-half-pos';

    const bar = document.createElement('div');
    bar.className = pos ? 'q-corr-bar q-corr-bar-pos' : 'q-corr-bar q-corr-bar-neg';
    bar.style.width      = (abs * 100).toFixed(1) + '%';
    bar.style.background = color;

    if (pos) { halfPos.appendChild(bar); }
    else      { halfNeg.appendChild(bar); }

    barWrap.appendChild(halfNeg);
    barWrap.appendChild(halfPos);

    const rVal = document.createElement('div');
    rVal.className   = 'q-corr-r';
    rVal.textContent = (r >= 0 ? '+' : '') + r.toFixed(3);
    rVal.style.color = color;

    const nVal = document.createElement('div');
    nVal.className   = 'q-corr-n';
    nVal.textContent = 'n=' + row.n;

    div.appendChild(name);
    div.appendChild(barWrap);
    div.appendChild(rVal);
    div.appendChild(nVal);
    frag.appendChild(div);
  });

  body.innerHTML = '';
  body.appendChild(frag);
}

function _renderQLabelDropdown(rows) {
  const sel = document.getElementById('q-label-select');
  if (!sel) return;
  const prev = sel.value;
  sel.innerHTML = '<option value="">All labels</option>';
  (rows || []).forEach(r => {
    const opt = document.createElement('option');
    opt.value = r.category;
    opt.textContent = r.category + (r.dc_count != null ? ` (${r.dc_count})` : '');
    sel.appendChild(opt);
  });
  // Restore selection if still valid
  if (prev && [...sel.options].some(o => o.value === prev)) sel.value = prev;
}

function onQLabelSelect(cat) {
  // Mirror into corrCategory drill-down
  setQCorrCategory(cat || null);
}

function goExplorerForLabel() {
  const sel = document.getElementById('q-label-select');
  const cat = sel ? sel.value : '';
  switchTab('explorer');
  // After tab switch, apply the category filter
  if (cat) {
    _catFilterSel.clear();
    _catFilterSel.add(cat);
  } else {
    _catFilterSel.clear();
  }
  // Load DCs if not yet loaded, then apply filter
  if (explorerState.snapshotId) {
    if (explorerState.dcs.length > 0) {
      _buildCatDropdown(_catFilterAll);
      _applyCatFilter();
    } else {
      loadExplorerDCs();  // _applyCatFilter called inside after load
    }
  }
}

function _renderQuestionsResult() {
  const card = document.getElementById('questions-result-card');
  const rows = questionsCtrl.allData;
  if (!rows.length) {
    document.getElementById('questions-table-wrap').innerHTML =
      '<span class="muted" style="font-size:12px">No data. Run analysis with ingest first.</span>';
    card.style.display = '';
    _renderQLabelDropdown([]);
    return;
  }
  card.style.display = '';
  _renderQLabelDropdown(rows);

  const metric = questionsCtrl.selectedMetric;
  const type   = questionsCtrl.chartType;

  const lbl2 = document.getElementById('q-chart2-label');
  if (lbl2) lbl2.textContent = metric || '—';

  // Charts use sum for display
  const clockRows = rows.map(r => ({ category: r.category, value: r.clocks?.sum ?? 0, dc_count: r.dc_count }));
  const selRows   = rows.map(r => ({ category: r.category, value: r[metric]?.sum ?? 0, dc_count: r.dc_count }));

  if (type === 'pie') {
    _drawQPie('q-chart-clock', 'q-chart-clock-tip', clockRows, 'clocks', 'sum');
    _drawQPie('q-chart-sel',   'q-chart-sel-tip',   selRows,   metric,   'sum');
  } else {
    _drawQBar('q-chart-clock', 'q-chart-clock-tip', clockRows, 'clocks', 'sum');
    _drawQBar('q-chart-sel',   'q-chart-sel-tip',   selRows,   metric,   'sum');
  }

  _renderQTable(rows);
}

// ── Shared bar renderer ───────────────────────────────────────────────────────
function _drawQBar(canvasId, tipId, rows, metric, agg) {
  const canvas  = document.getElementById(canvasId);
  const tooltip = document.getElementById(tipId);
  if (!canvas) return;
  const W = canvas.offsetWidth || canvas.parentElement?.offsetWidth || 400;
  const H = 320;
  canvas.width  = W;
  canvas.height = H;

  const PAD = { top: 20, right: 8, bottom: 60, left: 56 };
  const cW  = W - PAD.left - PAD.right;
  const cH  = H - PAD.top  - PAD.bottom;
  const ctx = canvas.getContext('2d');
  ctx.clearRect(0, 0, W, H);

  const maxVal = Math.max(...rows.map(r => r.value || 0));
  if (!maxVal) {
    ctx.fillStyle = '#94a3b8'; ctx.font = '12px system-ui';
    ctx.textAlign = 'center';
    ctx.fillText('No data', W / 2, H / 2);
    return;
  }

  const barW  = cW / rows.length;
  const fillW = Math.max(1, barW * 0.72);
  const gap   = (barW - fillW) / 2;

  // Grid + Y labels
  ctx.font = '10px system-ui'; ctx.fillStyle = '#94a3b8'; ctx.textAlign = 'right';
  for (let i = 0; i <= 4; i++) {
    const y = PAD.top + cH - (cH * i / 4);
    ctx.fillText(_fmtK(maxVal * i / 4), PAD.left - 4, y + 3);
    ctx.strokeStyle = '#e5e7eb'; ctx.lineWidth = 0.5;
    ctx.beginPath(); ctx.moveTo(PAD.left, y); ctx.lineTo(PAD.left + cW, y); ctx.stroke();
  }

  // Bars + X labels
  rows.forEach((row, i) => {
    const barH = ((row.value || 0) / maxVal) * cH;
    const x = PAD.left + i * barW + gap;
    ctx.fillStyle = _catColor(row.category);
    ctx.fillRect(x, PAD.top + cH - barH, fillW, barH);
    ctx.save();
    ctx.translate(x + fillW / 2, PAD.top + cH + 6);
    ctx.rotate(Math.PI / 4);
    ctx.fillStyle = '#475569'; ctx.font = '10px system-ui'; ctx.textAlign = 'left';
    ctx.fillText(row.category, 0, 0);
    ctx.restore();
  });

  // Baseline
  ctx.strokeStyle = '#cbd5e1'; ctx.lineWidth = 1;
  ctx.beginPath(); ctx.moveTo(PAD.left, PAD.top + cH); ctx.lineTo(PAD.left + cW, PAD.top + cH); ctx.stroke();

  // Hover + click
  canvas._meta = { rows, barW, gap, fillW, PAD, cH, maxVal };
  canvas.style.cursor = 'pointer';
  canvas.onmousemove = (e) => {
    const rect = canvas.getBoundingClientRect();
    const mx   = (e.clientX - rect.left) * (canvas.width / rect.width);
    const meta = canvas._meta;
    const idx  = Math.floor((mx - meta.PAD.left) / meta.barW);
    if (idx < 0 || idx >= meta.rows.length) { tooltip.style.display = 'none'; return; }
    const r = meta.rows[idx];
    tooltip.style.display = '';
    tooltip.textContent = `${r.category}: ${_fmtK(r.value)} (${r.dc_count} DCs)`;
    tooltip.style.left = Math.min(e.offsetX + 12, canvas.offsetWidth - 160) + 'px';
    tooltip.style.top  = Math.max(0, e.offsetY - 28) + 'px';
  };
  canvas.onmouseleave = () => { tooltip.style.display = 'none'; };
  canvas.onclick = (e) => {
    const rect = canvas.getBoundingClientRect();
    const mx   = (e.clientX - rect.left) * (canvas.width / rect.width);
    const meta = canvas._meta;
    const idx  = Math.floor((mx - meta.PAD.left) / meta.barW);
    if (idx < 0 || idx >= meta.rows.length) return;
    const cat = meta.rows[idx].category;
    setQCorrCategory(questionsCtrl.corrCategory === cat ? null : cat);
  };
}

// ── Shared pie renderer ───────────────────────────────────────────────────────
function _drawQPie(canvasId, tipId, rows, metric, agg) {
  const canvas  = document.getElementById(canvasId);
  const tooltip = document.getElementById(tipId);
  if (!canvas) return;
  const W = canvas.offsetWidth || canvas.parentElement?.offsetWidth || 400;
  const H = 320;
  canvas.width  = W;
  canvas.height = H;

  const ctx   = canvas.getContext('2d');
  ctx.clearRect(0, 0, W, H);
  const total = rows.reduce((s, r) => s + (r.value || 0), 0);
  if (!total) {
    ctx.fillStyle = '#94a3b8'; ctx.font = '12px system-ui'; ctx.textAlign = 'center';
    ctx.fillText('No data', W / 2, H / 2); return;
  }

  const cx = W * 0.42;
  const cy = H / 2;
  const R  = Math.min(cx - 16, cy - 16);

  let angle = -Math.PI / 2;
  const slices = rows.filter(r => (r.value || 0) > 0).map(r => {
    const sweep = (r.value / total) * 2 * Math.PI;
    const s = { ...r, startAngle: angle, sweep };
    angle += sweep;
    return s;
  });

  slices.forEach(s => {
    ctx.beginPath();
    ctx.moveTo(cx, cy);
    ctx.arc(cx, cy, R, s.startAngle, s.startAngle + s.sweep);
    ctx.closePath();
    ctx.fillStyle = _catColor(s.category);
    ctx.fill();
    ctx.strokeStyle = '#fff'; ctx.lineWidth = 1.5; ctx.stroke();
  });

  // Legend
  const legX = cx + R + 16;
  let legY = Math.max(20, cy - slices.length * 10);
  ctx.font = '11px system-ui'; ctx.textAlign = 'left';
  slices.forEach(s => {
    ctx.fillStyle = _catColor(s.category);
    ctx.fillRect(legX, legY - 9, 10, 10);
    ctx.fillStyle = '#334155';
    ctx.fillText(`${s.category} ${((s.value / total) * 100).toFixed(1)}%`, legX + 14, legY);
    legY += 18;
  });

  // Hover + click
  canvas._pieMeta = { slices, cx, cy, R, total, W, H };
  canvas.style.cursor = 'pointer';
  const _pieHitSlice = (e) => {
    const rect = canvas.getBoundingClientRect();
    const mx   = (e.clientX - rect.left) * (W / rect.width) - canvas._pieMeta.cx;
    const my   = (e.clientY - rect.top)  * (H / rect.height) - canvas._pieMeta.cy;
    if (Math.sqrt(mx*mx + my*my) > canvas._pieMeta.R) return null;
    let ang = Math.atan2(my, mx);
    if (ang < -Math.PI / 2) ang += 2 * Math.PI;
    return canvas._pieMeta.slices.find(sl => ang >= sl.startAngle && ang < sl.startAngle + sl.sweep) || null;
  };
  canvas.onmousemove = (e) => {
    const s = _pieHitSlice(e);
    if (!s) { tooltip.style.display = 'none'; return; }
    tooltip.style.display = '';
    tooltip.textContent = `${s.category}: ${_fmtK(s.value)} (${((s.value/canvas._pieMeta.total)*100).toFixed(1)}%, ${s.dc_count} DCs)`;
    tooltip.style.left = (e.offsetX + 12) + 'px';
    tooltip.style.top  = Math.max(0, e.offsetY - 28) + 'px';
  };
  canvas.onmouseleave = () => { tooltip.style.display = 'none'; };
  canvas.onclick = (e) => {
    const s = _pieHitSlice(e);
    if (!s) return;
    setQCorrCategory(questionsCtrl.corrCategory === s.category ? null : s.category);
  };
}

async function _fetchLabelCorrelations(snapId) {
  const metric = questionsCtrl.selectedMetric;
  if (!metric || metric === 'clocks') { questionsCtrl.labelCorrs = {}; _reRenderQTable(); return; }
  try {
    const res = await apiGet(`${DATA}/label_correlations?snapshot_id=${snapId}&metric=${encodeURIComponent(metric)}`);
    if (!res.ok) return;
    questionsCtrl.labelCorrs = {};
    (res.data || []).forEach(d => { questionsCtrl.labelCorrs[d.category] = d; });
    _reRenderQTable();
  } catch { /* ignore */ }
}

function _reRenderQTable() {
  if (questionsCtrl.allData.length) _renderQTable(questionsCtrl.allData);
}

// ── Table: Category + DCs + clocks×5aggs + selectedMetric×5aggs + r ──────────
function _renderQTable(rows) {
  const wrap   = document.getElementById('questions-table-wrap');
  if (!rows.length) { wrap.innerHTML = ''; return; }

  const metric   = questionsCtrl.selectedMetric;
  const showSel  = metric && metric !== 'clocks';
  const showCorr = showSel && Object.keys(questionsCtrl.labelCorrs).length > 0;

  const AGG_LABELS = { sum: 'Sum', median: 'Med', min: 'Min', max: 'Max', variance: 'Var' };

  // Build header: Category | DCs | clocks_sum | clocks_med | clocks_min | clocks_max | clocks_var
  //                        [if showSel] | metric_sum … metric_var
  //                        [if showCorr] | r
  const headers = ['Category', 'DCs'];
  _Q_AGGS.forEach(a => headers.push(`clocks ${AGG_LABELS[a]}`));
  if (showSel) _Q_AGGS.forEach(a => headers.push(`${metric} ${AGG_LABELS[a]}`));
  if (showCorr) headers.push('r (clocks↔metric)');

  const table = document.createElement('table');
  table.className = 'questions-table';

  const thead = table.createTHead();
  const hrow  = thead.insertRow();
  headers.forEach(h => {
    const th = document.createElement('th');
    th.textContent = h;
    hrow.appendChild(th);
  });

  const tbody = table.createTBody();
  rows.forEach(row => {
    const tr = tbody.insertRow();
    tr.dataset.cat = row.category;
    tr.style.cursor = 'pointer';
    if (row.category === questionsCtrl.corrCategory) tr.classList.add('q-row-selected');
    tr.onclick = () => setQCorrCategory(questionsCtrl.corrCategory === row.category ? null : row.category);

    // Category
    const catTd = tr.insertCell();
    catTd.className = 'questions-category-cell';
    const dot = document.createElement('span');
    dot.style.cssText = `display:inline-block;width:9px;height:9px;border-radius:2px;background:${_catColor(row.category)};margin-right:6px;vertical-align:middle`;
    catTd.appendChild(dot);
    catTd.appendChild(document.createTextNode(row.category));

    // DCs
    const dcTd = tr.insertCell();
    dcTd.textContent = row.dc_count ?? '—';
    dcTd.style.textAlign = 'right';

    // Clocks × 5 aggs
    const clockAggs = row.clocks || {};
    _Q_AGGS.forEach(a => {
      const td = tr.insertCell();
      const v  = clockAggs[a];
      td.textContent = v != null ? _fmtK(v) : '—';
      td.style.textAlign = 'right';
    });

    // Selected metric × 5 aggs
    if (showSel) {
      const metAggs = row[metric] || {};
      _Q_AGGS.forEach(a => {
        const td = tr.insertCell();
        const v  = metAggs[a];
        td.textContent = v != null ? _fmtK(v) : '—';
        td.style.textAlign = 'right';
      });
    }

    // Correlation r
    if (showCorr) {
      const corrInfo = questionsCtrl.labelCorrs[row.category];
      const rTd = tr.insertCell();
      rTd.style.textAlign = 'right';
      rTd.style.fontFamily = 'ui-monospace, monospace';
      if (!corrInfo || corrInfo.r == null) {
        rTd.textContent = corrInfo ? `n=${corrInfo.n} (too few)` : '—';
        rTd.style.color = 'var(--text-muted)';
      } else {
        const r = corrInfo.r;
        rTd.textContent = (r >= 0 ? '+' : '') + r.toFixed(3) + `  n=${corrInfo.n}`;
        rTd.style.color = r >= 0 ? '#3b82f6' : '#22c55e';
        rTd.style.fontWeight = '600';
      }
    }
  });

  wrap.innerHTML = '';
  wrap.appendChild(table);
}

// ── Tab navigation ────────────────────────────────────────────────────────────

function switchTab(id) {
  document.querySelectorAll('.tab-content').forEach(el => el.classList.remove('active'));
  document.querySelectorAll('.tab-btn').forEach(el => el.classList.remove('active'));
  document.getElementById(`tab-${id}`).classList.add('active');
  document.querySelector(`.tab-btn[data-tab="${id}"]`).classList.add('active');

  // Show shared picker on questions + explorer + results
  const picker = document.getElementById('shared-snap-card');
  if (picker) picker.style.display = (id === 'questions' || id === 'explorer' || id === 'results') ? '' : 'none';

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
    // Results tab is driven by the shared snapshot picker
    const snapDir = sharedSnapState.snapshotDir;
    if (snapDir) {
      const normDir = normPath(snapDir);
      const parts   = normDir.replace(/\\/g, '/').split('/');
      const analysisRoot = parts.slice(0, -2).join('/');
      const runName      = parts[parts.length - 2];
      const snapId       = parts[parts.length - 1];
      if (resultsState.runs.length === 0) {
        scanAnalyses(analysisRoot, runName, snapId);
      } else {
        // Runs already loaded — just re-render the selector to the right snapshot
        renderRunSelector(runName, snapId);
      }
    }
  }

  if (id === 'explorer') {
    // Seed directory input from localStorage if not already set
    const dirInput = document.getElementById('shared-snap-dir');
    if (dirInput && !dirInput.value) {
      const saved = localStorage.getItem('analysisRoot')
        || (() => { const d = localStorage.getItem('sdpDir'); return d ? d.replace(/\\/g, '/').replace(/\/$/, '') + '/analysis' : null; })();
      if (saved) dirInput.value = saved;
    }
    if (sharedSnapState.runs.length === 0) sharedSnapScan();
    else if (sharedSnapState.snapshotId) loadExplorerDCs();
  }

  if (id === 'questions') {
    const dirInput = document.getElementById('shared-snap-dir');
    if (dirInput && !dirInput.value) {
      const saved = localStorage.getItem('analysisRoot')
        || (() => { const d = localStorage.getItem('sdpDir'); return d ? d.replace(/\\/g, '/').replace(/\/$/, '') + '/analysis' : null; })();
      if (saved) dirInput.value = saved;
    }
    if (sharedSnapState.runs.length === 0) {
      sharedSnapScan();  // will call _fetchQuestionsData via _onSharedSnapSelect
    } else {
      const snapId = sharedSnapState.snapshotId;
      if (snapId) { _fetchClockCorrelation(snapId); _fetchQuestionsData(); }
    }
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
    mermaid.initialize({ startOnLoad: false, theme: 'default', maxTextSize: 2000000 });
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

  // Pre-populate device dropdown
  refreshDeviceList();

  // Start polling logs (badge updates when tab is not active)
  startLogPoll();

  // Resume C# analysis job if it was running before a page refresh
  _resumeCsJobIfAny();

  // Resume pipeline job if it was running before a page refresh
  _resumePipelineJobIfAny();

  // Pre-load shared picker so Questions/Explorer are ready immediately
  const savedRoot = localStorage.getItem('analysisRoot')
    || (() => { const d = localStorage.getItem('sdpDir'); return d ? d.replace(/\\/g, '/').replace(/\/$/, '') + '/analysis' : null; })();
  if (savedRoot) {
    const snapDirInput = document.getElementById('shared-snap-dir');
    if (snapDirInput) snapDirInput.value = savedRoot;
    sharedSnapScan();
  }
});
