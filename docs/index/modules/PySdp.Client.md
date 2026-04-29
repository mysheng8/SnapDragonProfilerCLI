# MODULE INDEX — PySdp.Client — AUTHORITATIVE ROUTING

## Routing Keywords
Systems: pysdp, SdpClient, JobPoller, Python SDK, SDPCLI HTTP API
Concepts: synchronous blocking client, job polling, connect/capture/analyze, Python scripting, CI integration
Common Logs: SdpConnectionError, SdpStateError, SdpValidationError, SdpError, Cannot connect to SDPCLI Server
Entry Symbols: SdpClient, SdpClient.connect, SdpClient.capture, SdpClient.analyze, SdpClient.launch, JobPoller, SdpConnectionError, SdpStateError

## Role

Synchronous Python client package for the SDPCLI Server HTTP API: wraps connect/launch/capture/analyze operations as blocking calls with job polling, progress callbacks, and typed exception hierarchy. Intended for scripting and CI use.

## Entry Points

| Symbol | Location |
|--------|----------|
| `SdpClient.__init__(base_url, poll_interval, timeout, verbose)` | [pySdp/pysdp/client.py](../../../pySdp/pysdp/client.py#L9) |
| `SdpClient.connect(device_id)` | [pySdp/pysdp/client.py](../../../pySdp/pysdp/client.py#L36) |
| `SdpClient.capture(output_dir, label)` | [pySdp/pysdp/client.py](../../../pySdp/pysdp/client.py#L82) |
| `SdpClient.analyze(sdp_path, snapshot_id)` | [pySdp/pysdp/client.py](../../../pySdp/pysdp/client.py#L114) |

## Key Classes

| Class | Responsibility | Location |
|-------|----------------|----------|
| `SdpClient` | Blocking client: POST jobs, poll until terminal, return result dict | [pySdp/pysdp/client.py](../../../pySdp/pysdp/client.py#L9) |
| `JobPoller` | Polling loop: GET /api/jobs/{id} until status in (completed/failed/cancelled) | [pySdp/pysdp/_jobs.py](../../../pySdp/pysdp/_jobs.py) |
| `SdpConnectionError` | Raised when requests.ConnectionError occurs (server not running) | [pySdp/pysdp/exceptions.py](../../../pySdp/pysdp/exceptions.py) |
| `SdpStateError` | Raised on HTTP 409 (conflicting server state) | [pySdp/pysdp/exceptions.py](../../../pySdp/pysdp/exceptions.py) |
| `SdpValidationError` | Raised on HTTP 400 (bad request body) | [pySdp/pysdp/exceptions.py](../../../pySdp/pysdp/exceptions.py) |
| `SdpError` | Base exception for all non-connection errors | [pySdp/pysdp/exceptions.py](../../../pySdp/pysdp/exceptions.py) |

## Key Methods

| Method | Purpose | Location | Triggered When |
|--------|---------|----------|----------------|
| `SdpClient.connect(device_id)` | POST /api/connect → poll job → return {deviceId, status} | [client.py:36](../../../pySdp/pysdp/client.py#L36) | Script: begin profiling session |
| `SdpClient.launch(package_activity)` | POST /api/session/launch → poll job | [client.py:52](../../../pySdp/pysdp/client.py#L52) | Script: launch app after connect |
| `SdpClient.capture(output_dir, label)` | POST /api/capture → poll job → return {sdpPath, captureId} | [client.py:82](../../../pySdp/pysdp/client.py#L82) | Script: trigger GPU snapshot |
| `SdpClient.submit_capture(...)` | POST /api/capture → return job_id without waiting | [client.py:99](../../../pySdp/pysdp/client.py#L99) | Non-blocking capture submission |
| `SdpClient.analyze(sdp_path, snapshot_id)` | POST /api/analysis → poll job → return {sdpPath, captureId, sessionDir} | [client.py:114](../../../pySdp/pysdp/client.py#L114) | Script: offline analysis |
| `SdpClient.wait_for_job(job_id, on_progress)` | Block until job terminal; optional progress callback | [client.py:144](../../../pySdp/pysdp/client.py#L144) | After submit_capture |
| `SdpClient.device_status()` | GET /api/device → {status, connectedDevice, session} | [client.py:72](../../../pySdp/pysdp/client.py#L72) | Status polling |
| `SdpClient.list_jobs()` | GET /api/jobs → list of job dicts | [client.py:164](../../../pySdp/pysdp/client.py#L164) | Job inspection |
| `SdpClient.cancel_job(job_id)` | POST /api/jobs/{id}/cancel | [client.py:169](../../../pySdp/pysdp/client.py#L169) | Abort running job |
| `SdpClient._check_response(resp)` | Map HTTP status codes to typed exceptions | [client.py:229](../../../pySdp/pysdp/client.py#L229) | Every HTTP response |

## Call Flow Skeleton

```
SdpClient("http://localhost:5000")
├── connect(device_id)
│   └── POST /api/connect → job_id → JobPoller.wait(job_id) → result
├── launch(package_activity)
│   └── POST /api/session/launch → job_id → wait
├── capture(output_dir, label)
│   └── POST /api/capture → job_id → wait → {sdpPath, captureId}
└── analyze(sdp_path, snapshot_id, targets)
    └── POST /api/analysis → job_id → wait → {sdpPath, captureId, sessionDir}
```

## Exception Hierarchy

```
SdpError
├── SdpConnectionError   — requests.ConnectionError (server unreachable)
├── SdpStateError        — HTTP 409 (wrong device state)
└── SdpValidationError   — HTTP 400 (bad parameters)
```

## Log → Code Map

| Log Keyword | Location | Meaning |
|-------------|----------|---------|
| `"Cannot connect to SDPCLI Server at"` | [client.py:202](../../../pySdp/pysdp/client.py#L202) | Server not running |
| `"HTTP {status_code}: {error}"` | [client.py:247](../../../pySdp/pysdp/client.py#L247) | Non-409/400 HTTP error |

## Search Hints

```
Find client usage examples:
open pySdp/examples/snapshot.py
open pySdp/examples/batch_analysis.py

Find job polling logic:
open pySdp/pysdp/_jobs.py

Find exception types:
open pySdp/pysdp/exceptions.py

Jump:
open pySdp/pysdp/client.py:9     # SdpClient class
open pySdp/pysdp/client.py:36    # connect()
open pySdp/pysdp/client.py:82    # capture()
open pySdp/pysdp/client.py:114   # analyze()
```
