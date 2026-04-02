# Configuration File Guide

The `config.ini` file allows you to automate testing by pre-configuring app launch and capture settings.

## Configuration Options

### App Settings

**PackageName**: Android package name of the app to profile
```ini
PackageName=com.ea.gp.fcmnova
```
- Leave empty for interactive mode (prompt for input)

**ActivityName**: Full activity name (without package prefix)
```ini
ActivityName=com.ea.frostbite.engine.runtime.main.FrostbiteActivity
```
- Leave empty to show activity selection menu
- Takes priority over ActivityIndex

**ActivityIndex**: Activity selection by index (0-N)
```ini
ActivityIndex=2
```
- 0 = package-only launch (no specific activity)
- 1, 2, 3... = select activity by index from discovered list
- Leave empty or set to -1 for interactive selection

### Rendering API

**RenderingAPI**: Graphics API to use for profiling
```ini
RenderingAPI=16
```

Values:
- `0` = None (auto-detect)
- `1` = DirectX11
- `2` = DirectX12  
- `4` = OpenCL
- `8` = OpenGL
- `16` = Vulkan (recommended for most Android apps)

### Capture Settings

**CaptureType**: Type of profiling capture
```ini
CaptureType=2
```

Values:
- `1` = Realtime
- `2` = Trace (recommended)
- `4` = Snapshot
- `8` = Sampling

**AutoStartCapture**: Automatically capture frame after app launches
```ini
AutoStartCapture=false
```
- `true` = Capture frame immediately after app launches
- `false` = Wait for user to press ENTER before capturing

**Note**: Capture is a single-frame snapshot operation. It automatically completes and exports data - no manual stop required.

## Usage Examples

### Fully Automated Testing
```ini
PackageName=com.example.app
ActivityName=com.example.MainActivity
RenderingAPI=16
CaptureType=2
AutoStartCapture=true
```
Result: Automatically launch app, capture a frame, then export data.

### Semi-Automated with Manual Capture Trigger
```ini
PackageName=com.example.app
ActivityIndex=1
RenderingAPI=16
CaptureType=2
AutoStartCapture=false
```
Result: Auto-launch app with first discovered activity, wait for user to press ENTER to capture frame.

### Interactive Mode
Leave config.ini empty or delete it - program will prompt for all inputs.

## Notes

- Config file must be named `config.ini` and placed in the same directory as the executable
- Lines starting with `#` or `;` are comments
- Empty values fall back to interactive mode for that specific setting
- Invalid values will use defaults or prompt for input
