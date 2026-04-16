# pySdp

SDPCLI Server 的 Python 客户端 + Web UI，用于驱动无头抓帧（Snapshot）和离线分析（Analysis）流程。

---

## 架构概览

```
┌─────────────────────────────────────────────────────┐
│  Browser  (Web UI)                                  │
│  ├─ Snapshot 面板：Connect → Launch → Capture       │
│  └─ Analysis 面板：选 .sdp → 选 targets → 分析      │
└──────────────┬──────────────────────────────────────┘
               │ HTTP (localhost:8000)
┌──────────────▼──────────────────────────────────────┐
│  webui/server.py  (FastAPI + uvicorn)               │
│  ├─ 静态文件服务  /static/                          │
│  ├─ 代理路由     /api/sdpcli/* → SDPCLI Server      │
│  └─ 文件路由     /api/files/* (浏览 .sdp / 结果目录)│
└──────────────┬──────────────────────────────────────┘
               │ HTTP (localhost:5000)
┌──────────────▼──────────────────────────────────────┐
│  SDPCLI Server  (dotnet, .\sdpcli.bat server)       │
│  ├─ /api/connect  /api/session/launch               │
│  ├─ /api/capture  /api/analysis                     │
│  └─ /api/jobs/*                                     │
└─────────────────────────────────────────────────────┘

pysdp/  ←  可独立使用的 Python 包（脚本直接调用）
```

**两层设计：**

- **`pysdp/`**：纯 Python 包，封装 SDPCLI HTTP API，提供同步阻塞式接口，可直接在脚本/自动化流程中 import 使用，不依赖 Web UI。
- **`webui/`**：FastAPI 应用，后端代理 SDPCLI Server，前端为单页 HTML（无构建步骤），用浏览器操作抓帧和分析。

---

## 前置条件

```powershell
# 启动 SDPCLI Server（snapdragon 仓库根目录）
dotnet build SDPCLI
.\sdpcli.bat server --port 5000
```

---

## 安装

```bash
# 激活 venv
.venv\Scripts\activate

pip install -r requirements.txt
```

**依赖：**

```
requests==2.33.1      # pysdp 核心 HTTP 客户端
fastapi               # WebUI 后端框架
uvicorn               # ASGI 服务器
```

---

## 使用 pysdp（脚本模式）

```python
from pysdp import SdpClient

client = SdpClient("http://localhost:5000")

# 完整流程（每步同步阻塞）
client.connect(device_id="192.168.1.100:5555")
client.launch("com.example.app/com.example.MainActivity")
result = client.capture()

analysis = client.analyze(
    sdp_path=result["sdpPath"],
    snapshot_id=result["captureId"],
    targets="label,metrics,status",
)
print(analysis["captureDir"])  # 结果目录
```

---

## 使用 WebUI（浏览器模式）

```bash
python webui/server.py          # 启动 WebUI，监听 localhost:8000
```

打开浏览器访问 `http://localhost:8000`。

---

## pysdp 包设计

### 模块结构

```
pysdp/
├── __init__.py       # 对外导出：SdpClient、异常类
├── client.py         # SdpClient 主类（同步阻塞）
├── _jobs.py          # JobPoller（内部）
├── _models.py        # 响应数据类（内部）
└── exceptions.py     # 异常定义
```

### SdpClient

```python
SdpClient(base_url="http://localhost:5000", poll_interval=2.0, timeout=600)
```

所有操作均为**同步阻塞**：内部轮询 Job 直到完成或超时。

#### 设备 & 会话

| 方法 | 说明 |
|---|---|
| `connect(device_id=None) → dict` | 连接设备，可指定设备 ID，省略则自动选择 |
| `launch(package_activity) → dict` | 启动 App，格式 `package/Activity` |
| `disconnect()` | 断开设备，重置为 Disconnected |
| `device_status() → dict` | 查询当前设备和会话状态 |

#### 抓帧

| 方法 | 说明 |
|---|---|
| `capture(output_dir=None, label=None) → dict` | 触发一帧 GPU Snapshot |

返回：`{"sdpPath": "...", "captureId": 2, "sessionId": "..."}`

一次 session 可多次调用 `capture()`，每次返回不同的 `captureId`。

#### 分析

| 方法 | 说明 |
|---|---|
| `analyze(sdp_path, snapshot_id, output_dir=None, targets=None) → dict` | 离线分析 .sdp 文件 |

返回：`{"sdpPath": "...", "captureId": 2, "sessionDir": "...", "captureDir": "...", "targets": "..."}`

**`targets` 可选值**（逗号分隔，自动处理依赖）：

| Target | 输出文件 | 说明 |
|---|---|---|
| `dc` | `dc.json` | DrawCall 基础数据（其他 target 的依赖） |
| `shaders` | `shaders.json` + `.hlsl` | SPIR-V → HLSL 反编译 |
| `textures` | `textures.json` + `.png` | 贴图提取 |
| `buffers` | `buffers.json` + `.obj` | 顶点/索引缓冲 → OBJ |
| `label` | `label.json` | DrawCall 分类（规则 + LLM） |
| `metrics` | `metrics.json` | GPU 计数器 |
| `status` | `status.json` | 统计汇总 + 百分位数 |
| `topdc` | `topdc.json` | Top-5 DrawCall + 瓶颈归因 |
| `analysis` | `analysis.md` | LLM 瓶颈分析报告 |
| `dashboard` | `dashboard.md` | 规则驱动图表 Markdown |

#### Job 底层 API（精细控制）

```python
job_id = client.submit_capture()           # 仅提交，不等待
job    = client.wait_for_job(job_id)       # 阻塞等待完成
job    = client.get_job(job_id)            # 单次查询
jobs   = client.list_jobs()                # 列出所有 Job
client.cancel_job(job_id)
client.delete_job(job_id)                  # 仅对终态 Job 有效
```

### _jobs.py — JobPoller

```python
class JobPoller:
    def wait(self, job_id: str, timeout: float, on_progress=None) -> dict
```

- 每 `poll_interval` 秒调用一次 `GET /api/jobs/{id}`
- `on_progress(phase, progress)` 回调可用于打印进度
- 超时后抛出 `SdpTimeoutError`
- Job 以 `Failed` 状态结束时抛出 `SdpJobError`

### _models.py — 数据类

```python
@dataclass
class JobStatus:
    id: str
    type: str                    # Connect / Launch / Capture / Analysis
    status: str                  # Pending / Running / Cancelling / Completed / Failed / Cancelled
    phase: str | None            # 当前阶段名（Running 时有值）
    progress: int                # 0–100
    result: dict | None
    error: str | None

@dataclass
class DeviceInfo:
    status: str                  # Disconnected / Connecting / Connected / ...
    connected_device: str | None
    session: dict | None
```

### exceptions.py — 异常体系

| 异常 | 触发条件 |
|---|---|
| `SdpError` | 基类 |
| `SdpStateError` | 设备状态不满足操作前提（HTTP 409） |
| `SdpValidationError` | 参数不合法（HTTP 400） |
| `SdpJobError` | Job 以 Failed 状态结束 |
| `SdpTimeoutError` | 超过 timeout 仍未完成 |
| `SdpConnectionError` | 无法连接到 SDPCLI Server |

---

## WebUI 设计

### 模块结构

```
webui/
├── server.py              # FastAPI 应用入口 + uvicorn 启动
├── routes/
│   ├── proxy.py           # /api/sdpcli/* 代理到 SDPCLI Server
│   └── files.py           # /api/files/* 文件系统（浏览 .sdp、结果目录）
└── static/
    ├── index.html         # 单页 HTML（无构建步骤）
    ├── app.js             # 前端逻辑（原生 fetch + 轮询）
    └── style.css          # 样式
```

### 页面布局

```
┌──────────────────────────────────────────────────────┐
│  Header: pySdp  [●] Server: OK  设备状态: SessionActive │
├──────────────┬───────────────────────────────────────┤
│              │                                       │
│  [Snapshot]  │  Step 1 — Connect                     │
│  [Analysis]  │  Device ID: [____________] [Connect]  │
│  [Results]   │  ✓ Connected: 192.168.1.100:5555       │
│              │                                       │
│              │  Step 2 — Launch App                  │
│              │  Package: [___________________]        │
│              │  Activity: [__________________]        │
│              │  [Launch]                             │
│              │  ✓ Session Active  PID: 12345          │
│              │                                       │
│              │  Step 3 — Capture                     │
│              │  Label: [__________] [Capture]        │
│              │  ████████░░ 80%  importing            │
│              │  ✓ sdpPath: D:/captures/...sdp        │
│              │    captureId: 2                       │
└──────────────┴───────────────────────────────────────┘
```

```
Analysis 面板:
  .sdp Path:    [D:/captures/xxx.sdp] [Browse]
  Snapshot ID:  [2]
  Targets:      [✓dc] [✓shaders] [✓label] [✓metrics] [✓status] [ topdc] [ analysis] [ dashboard]
  [Analyze]
  ████████████ 65%  label_drawcalls
  ✓ 完成 → captureDir: D:/analysis-out/.../snapshot_2/

Results 面板:
  [dc.json ↗] [label.json ↗] [metrics.json ↗] [status.json ↗] [topdc.json ↗]
  [analysis.md ↗] [dashboard.md ↗]
  ┌── dc.json 预览 (前 5 条) ──┐
  │ ...                        │
  └────────────────────────────┘
```

### 前端交互逻辑

- 页面加载时轮询 `GET /api/sdpcli/device`（每 3 秒）同步顶部设备状态徽章
- 提交操作后轮询 `GET /api/sdpcli/jobs/{id}`（每 2 秒）更新进度条和阶段名
- Job 完成后自动填充后续步骤的输入（如 Capture 完成后自动填充 Analysis 的 sdpPath / snapshotId）
- 所有错误在对应面板内 inline 展示，不弹窗

### proxy.py 路由

| WebUI 路由 | 代理到 | 说明 |
|---|---|---|
| `POST /api/sdpcli/connect` | `POST :5000/api/connect` | 透传 |
| `POST /api/sdpcli/disconnect` | `POST :5000/api/disconnect` | 透传 |
| `POST /api/sdpcli/session/launch` | `POST :5000/api/session/launch` | 透传 |
| `POST /api/sdpcli/capture` | `POST :5000/api/capture` | 透传 |
| `POST /api/sdpcli/analysis` | `POST :5000/api/analysis` | 透传 |
| `GET /api/sdpcli/device` | `GET :5000/api/device` | 透传 |
| `GET /api/sdpcli/jobs` | `GET :5000/api/jobs` | 透传 |
| `GET /api/sdpcli/jobs/{id}` | `GET :5000/api/jobs/{id}` | 透传 |
| `POST /api/sdpcli/jobs/{id}/cancel` | `POST :5000/api/jobs/{id}/cancel` | 透传 |

### files.py 路由

| 路由 | 说明 |
|---|---|
| `GET /api/files/sdp?dir=D:/captures` | 列出目录下所有 .sdp 文件（含大小、修改时间） |
| `GET /api/files/results?dir=D:/analysis-out/xxx/snapshot_2` | 列出分析结果文件 |
| `GET /api/files/read?path=...&lines=50` | 读取 JSON 文件前 N 行（用于预览） |

---

## 目录结构（最终）

```
pySdp/
├── pysdp/
│   ├── __init__.py
│   ├── client.py
│   ├── _jobs.py
│   ├── _models.py
│   └── exceptions.py
├── webui/
│   ├── server.py
│   ├── routes/
│   │   ├── proxy.py
│   │   └── files.py
│   └── static/
│       ├── index.html
│       ├── app.js
│       └── style.css
├── examples/
│   ├── snapshot.py
│   └── batch_analysis.py
├── .venv/
├── requirements.txt
└── README.md
```

---

## 错误处理

```python
from pysdp import SdpClient, SdpStateError, SdpJobError, SdpTimeoutError

try:
    client.capture()
except SdpStateError as e:
    print(f"设备状态不对: {e}")       # e.g. "Cannot capture from state 'Connected'"
except SdpJobError as e:
    print(f"抓帧失败: {e.error}")
except SdpTimeoutError:
    print("超时，可手动查询 Job 状态")
```

---

## 与 SDPCLI Server 的端点对照

| pysdp 方法 | HTTP 端点 | Job 前缀 | 典型耗时 |
|---|---|---|---|
| `connect()` | `POST /api/connect` | `con-` | 90–120 秒 |
| `launch()` | `POST /api/session/launch` | `lnc-` | 30–60 秒 |
| `capture()` | `POST /api/capture` | `cap-` | 3–5 分钟 |
| `analyze()` | `POST /api/analysis` | `ana-` | 2–10 分钟 |
| `disconnect()` | `POST /api/disconnect` | — | 即时 |
| `device_status()` | `GET /api/device` | — | 即时 |
| `get_job()` | `GET /api/jobs/{id}` | — | 即时 |
| `list_jobs()` | `GET /api/jobs` | — | 即时 |
| `cancel_job()` | `POST /api/jobs/{id}/cancel` | — | 即时 |
| `delete_job()` | `DELETE /api/jobs/{id}` | — | 即时 |
