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
    setBtn('btn-capture', state.device === 'SessionActive');
    return;
  }
  if (!res.ok) {
    setMsg('capture', 'error', res.error);
    setBtn('btn-capture', state.device === 'SessionActive');
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
      setBtn('btn-capture', state.device === 'SessionActive');
    },
    err => {
      hideProg('capture');
      setMsg('capture', 'error', err);
      setBtn('btn-capture', state.device === 'SessionActive');
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
const DEFAULT_TARGETS = new Set(['dc','label','metrics','status']);

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
  const targets    = selectedTargets();

  if (!snapshotId || snapshotId < 2) {
    setMsg('analysis', 'error', 'Snapshot ID must be ≥ 2 — check Settings');
    document.getElementById('card-analysis-progress').style.display = '';
    return;
  }
  if (!targets) {
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

  let res;
  try {
    res = await apiPost(`${API}/analysis`, { sdpPath, snapshotId, targets });
  } catch (err) {
    _finishAnalysis(sdpPath, null, err.message);
    return;
  }
  if (!res.ok) {
    _finishAnalysis(sdpPath, null, res.error);
    return;
  }

  state.activeAnalysisJobId = res.data.jobId;

  pollJob('analysis', res.data.jobId,
    job => showProg('analysis', job.progress, job.phase),
    job => {
      const captureDir = normPath(job.result?.captureDir) || null;
      _finishAnalysis(sdpPath, captureDir, null);
    },
    err => _finishAnalysis(sdpPath, null, err)
  );
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
    // Enable Results button on the matching card
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

  // Auto-hide progress card after 3s
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

function openResults(captureDir) {
  if (!captureDir) return;
  state.lastAnalysisDir = captureDir;
  document.getElementById('results-dir').value = captureDir;
  switchTab('results');
  loadResults(captureDir);
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

async function loadResults(dir) {
  if (!dir) return;
  const container = document.getElementById('results-files');
  container.innerHTML = '<span class="muted">Loading…</span>';

  let res;
  try {
    res = await apiGet(`${FILES}/results?dir=${encodeURIComponent(dir)}`);
  } catch (err) {
    container.innerHTML = `<span class="s-error">${escHtml(err.message)}</span>`;
    return;
  }
  if (!res.ok) {
    container.innerHTML = `<span class="s-error">${escHtml(res.error)}</span>`;
    return;
  }

  const files = res.data || [];
  if (files.length === 0) {
    container.innerHTML = '<span class="muted">No files found.</span>';
    return;
  }

  const grid = document.createElement('div');
  grid.className = 'file-grid';
  files.forEach(f => {
    const btn = document.createElement('button');
    btn.className = `file-chip ext-${f.ext}`;
    btn.textContent = f.name;
    btn.onclick = () => viewFile(f.path, f.name, f.ext);
    grid.appendChild(btn);
  });
  container.innerHTML = '';
  container.appendChild(grid);
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

  // Build viewer card
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
    if (typeof marked !== 'undefined') {
      body.innerHTML = marked.parse(content);
    } else {
      body.innerHTML = `<pre class="code-pre">${escHtml(content)}</pre>`;
    }
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

  // Render mermaid after card is in DOM
  if (ext === 'md' && typeof mermaid !== 'undefined') {
    body.querySelectorAll('pre code.language-mermaid').forEach(code => {
      const div = document.createElement('div');
      div.className = 'mermaid';
      div.textContent = code.textContent;
      code.parentElement.replaceWith(div);
    });
    mermaid.run({ nodes: body.querySelectorAll('.mermaid') });
  }

  // Scroll viewer into view
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

  if (id === 'analysis') {
    const saved = localStorage.getItem('sdpDir');
    if (saved) {
      const input = document.getElementById('sdp-dir');
      if (!input.value) input.value = saved;
    }
    if (document.getElementById('sdp-dir').value) scanSdpFiles();
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
});
