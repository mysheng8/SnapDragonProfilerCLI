# Snapshot Capture Metrics Export Limitation

## 现状说明

Snapshot capture的metrics_export.csv为空（只有表头）是**正常行为**。

## 技术原因

### 1. ExportAllMetricData的数据源
- `Client.ExportAllMetricData()` 从数据库的 `tblDataLog` 表读取
- SQL查询示例：
```sql
SELECT Process.name, Metric.name, tblDataLog.timestamp, tblMetricDouble.value
FROM tblDataLog
INNER JOIN tblMetricDouble ON tblMetricDouble.ROWID = tblDataLog.valueID
INNER JOIN Metric ON Metric.id = tblDataLog.metric
INNER JOIN Process ON Process.pid = tblDataLog.process
```

### 2. Snapshot vs Realtime 的metrics记录差异

| Capture类型 | tblDataLog记录 | Metrics数据特点 |
|------------|---------------|----------------|
| **Realtime** | ✅ 持续记录 | 时间序列数据，按帧/时间间隔采样 |
| **Trace** | ✅ 持续记录 | 完整trace期间的metrics时间序列 |
| **Snapshot** | ❌ 不记录 | 仅采集瞬时状态快照 |

### 3. Snapshot capture的metrics存储位置

Snapshot的metrics值存储在**snapshot专用表**中（不是tblDataLog）：

```
数据库表结构：
- VulkanSnapshotMetricValue  - Vulkan snapshot的metrics瞬时值
- DX12SnapshotMetricValue    - DX12 snapshot的metrics瞬时值  
- GLESSnapshotShaderStatsData - OpenGL shader统计数据
- VulkanSnapshotScreenshots   - 截图数据
- ... 其他snapshot buffers
```

这些数据需要通过**GUI的Inspector**查看，或通过`GetBufferData()`读取buffers。

### 4. 为什么数据库有255KB但export是空的

数据库包含：
- ✅ Metric/MetricCategory定义表
- ✅ Process信息表
- ✅ Capture记录表
- ✅ Snapshot buffers (screenshot等)
- ✅ 元数据
- ❌ **没有tblDataLog时间序列数据**

因此ExportAllMetricData返回空CSV是正常的。

## GUI的处理方式

GUI在Snapshot capture后：
1. **不使用ExportAllMetricData导出metrics**
2. 通过Inspector UI显示snapshot buffers
3. 读取VulkanSnapshot*表查看瞬时metric值
4. 通过`GetBufferData()`读取screenshot和shader数据

## CLI的限制

当前CLI实现：
- ✅ 可以capture snapshot
- ✅ 可以导出screenshot (`GetBufferData()`)
- ✅ Metrics被正确激活
- ❌ **但ExportAllMetricData不适用于Snapshot**

## 解决方案

### 选项1：需要metrics时间序列数据
使用**Realtime capture**代替Snapshot：

```csharp
// 修改 config.ini
CaptureType=Realtime
CaptureDuration=10  // 采集10秒

// Realtime capture会持续记录metrics到tblDataLog
// ExportAllMetricData可以导出完整的时间序列CSV
```

### 选项2：读取Snapshot的metrics瞬时值
需要直接查询snapshot表（未实现）：

```sql
-- 查询Vulkan snapshot的metrics值（示例）
SELECT * FROM VulkanSnapshotMetricValue WHERE captureID = 3;

-- 查询shader统计
SELECT * FROM GLESSnapshotShaderStatsData WHERE captureID = 3;
```

### 选项3：仅使用Snapshot的screenshot功能
当前CLI已支持：
- ✅ Screenshot export (`snapshot_screenshot.dat`)
- ✅ Shader data (存储在database中)
- ✅ GPU state snapshot

## 推荐做法

**对于性能分析（需要metrics）**:
- 使用 **Realtime capture** (CaptureType=1)
- 可以获得完整的GPU/CPU metrics时间序列
- `metrics_export.csv` 会包含详细数据

**对于状态调试（需要screenshot/shader）**:
- 使用 **Snapshot capture** (CaptureType=4) 
- 通过GUI打开sdp.db查看snapshot数据
- 或使用CLI导出的screenshot文件

## 参考代码

GUI的Realtime metrics导出（有数据）：
```csharp
// OpenSessionDialogController.GetRealTimeValues()
SELECT Process.name, Metric.name, 
       tblDataLog.timestamp, tblMetricDouble.value
FROM tblDataLog
INNER JOIN tblMetricDouble ON tblMetricDouble.ROWID = tblDataLog.valueID
...
```

GUI的Snapshot处理（不导出metrics）：
```csharp
// SnapshotModel.RecordMetrics() 
// 只记录激活的metrics列表到CaptureMetrics表
// 实际metrics值在snapshot buffers中
```
