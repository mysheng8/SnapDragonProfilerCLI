# MODULE INDEX — SDP.Testing.AutomatedWorkflow — AUTHORITATIVE ROUTING

## Routing Keywords
Systems: Test Automation, Headless Testing, UI Automation, Integration Testing
Concepts: Workflow Plugin, Widget Abstraction, Test Runner, Automated Testing
Common Logs: AutomatedWorkflowExecutor, Workflow Executed Successfully, AutomatedWorkflowError.txt
Entry Symbols: AutomatedWorkflowExecutor, ISDPAutomatedWorkflowPlugin, ITestableWidget

## Role
Automated workflow execution framework enabling headless/scripted testing of Snapdragon Profiler desktop client through widget abstraction layer and plugin-based test automation.

## Entry Points
| Symbol | Location |
|--------|----------|
| AutomatedWorkflowExecutor | [AutomatedWorkflow/AutomatedWorkflowExecutor.cs:12](../../../dll/project/SDPClientFramework/AutomatedWorkflow/AutomatedWorkflowExecutor.cs#L12) |
| AutomatedWorkflowExecutor (constructor) | [AutomatedWorkflow/AutomatedWorkflowExecutor.cs:15](../../../dll/project/SDPClientFramework/AutomatedWorkflow/AutomatedWorkflowExecutor.cs#L15) |
| ISDPAutomatedWorkflowPlugin | [AutomatedWorkflow/ISDPAutomatedWorkflowPlugin.cs:10](../../../dll/project/SDPClientFramework/AutomatedWorkflow/ISDPAutomatedWorkflowPlugin.cs#L10) |
| ITestableWidget | [AutomatedWorkflow/TestableWidgets/ITestableWidget.cs:7](../../../dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/ITestableWidget.cs#L7) |
| WidgetNames | [AutomatedWorkflow/WidgetNames.cs:7](../../../dll/project/SDPClientFramework/AutomatedWorkflow/WidgetNames.cs#L7) |

## Key Classes
| Class | Responsibility | Location |
|------|----------------|----------|
| AutomatedWorkflowExecutor | Test runner executing plugin workflow in long-running task | [AutomatedWorkflowExecutor.cs:12](../../../dll/project/SDPClientFramework/AutomatedWorkflow/AutomatedWorkflowExecutor.cs#L12) |
| ISDPAutomatedWorkflowPlugin | Plugin contract with Init/Execute lifecycle and widget registry | [ISDPAutomatedWorkflowPlugin.cs:10](../../../dll/project/SDPClientFramework/AutomatedWorkflow/ISDPAutomatedWorkflowPlugin.cs#L10) |
| WidgetNames | Static factory for standardized widget name generation | [WidgetNames.cs:7](../../../dll/project/SDPClientFramework/AutomatedWorkflow/WidgetNames.cs#L7) |
| ITestableWidget | Base widget interface with Enabled/Visible properties | [TestableWidgets/ITestableWidget.cs:7](../../../dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/ITestableWidget.cs#L7) |
| IClickableWidget | Clickable UI elements (buttons) with Click/Press/Release | [TestableWidgets/IClickableWidget.cs:6](../../../dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/IClickableWidget.cs#L6) |
| ICheckButtonWidget | Checkboxes/toggle buttons with SetChecked | [TestableWidgets/ICheckButtonWidget.cs:6](../../../dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/ICheckButtonWidget.cs#L6) |
| IMenuItemWidget | Menu items with Select action | [TestableWidgets/IMenuItemWidget.cs:6](../../../dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/IMenuItemWidget.cs#L6) |
| ITreeViewWidget | Tree view navigation with path-based selection/expansion | [TestableWidgets/ITreeViewWidget.cs:8](../../../dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/ITreeViewWidget.cs#L8) |
| ISaveFileDialog | Save file dialog abstraction with file path setting | [TestableWidgets/ISaveFileDialog.cs:8](../../../dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/ISaveFileDialog.cs#L8) |
| TreeViewPath | Helper class for tree view path representation | [TestableWidgets/TreeViewPath.cs:7](../../../dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/TreeViewPath.cs#L7) |

## Key Methods
| Method | Purpose | Location | Triggered When |
|--------|---------|----------|----------------|
| AutomatedWorkflowExecutor (constructor) | Start long-running workflow task | [AutomatedWorkflowExecutor.cs:15](../../../dll/project/SDPClientFramework/AutomatedWorkflow/AutomatedWorkflowExecutor.cs#L15) | Test runner initialization |
| AutomatedWorkflowExecutor.AddTestableWidget | Register widget with plugin | [AutomatedWorkflowExecutor.cs:67](../../../dll/project/SDPClientFramework/AutomatedWorkflow/AutomatedWorkflowExecutor.cs#L67) | UI creates testable widget |
| AutomatedWorkflowExecutor.RemoveTestableWidget | Unregister widget from plugin | [AutomatedWorkflowExecutor.cs:72](../../../dll/project/SDPClientFramework/AutomatedWorkflow/AutomatedWorkflowExecutor.cs#L72) | Widget destroyed |
| AutomatedWorkflowExecutor.OnWorkflowError | Log error, write to file, exit app | [AutomatedWorkflowExecutor.cs:60](../../../dll/project/SDPClientFramework/AutomatedWorkflow/AutomatedWorkflowExecutor.cs#L60) | Workflow failure |
| ISDPAutomatedWorkflowPlugin.Init | Initialize plugin with command-line args | [ISDPAutomatedWorkflowPlugin.cs:30](../../../dll/project/SDPClientFramework/AutomatedWorkflow/ISDPAutomatedWorkflowPlugin.cs#L30) | Workflow task starts |
| ISDPAutomatedWorkflowPlugin.Execute | Run test workflow logic | [ISDPAutomatedWorkflowPlugin.cs:33](../../../dll/project/SDPClientFramework/AutomatedWorkflow/ISDPAutomatedWorkflowPlugin.cs#L33) | After Init succeeds |
| ISDPAutomatedWorkflowPlugin.OnCoreInitialized | Hook after ConnectionManager.InitCore | [ISDPAutomatedWorkflowPlugin.cs:27](../../../dll/project/SDPClientFramework/AutomatedWorkflow/ISDPAutomatedWorkflowPlugin.cs#L27) | Core initialization complete |
| ISDPAutomatedWorkflowPlugin.AddTestableWidget | Store widget reference by name | [ISDPAutomatedWorkflowPlugin.cs:21](../../../dll/project/SDPClientFramework/AutomatedWorkflow/ISDPAutomatedWorkflowPlugin.cs#L21) | AutomatedWorkflowExecutor forwards |
| ISDPAutomatedWorkflowPlugin.RemoveTestableWidget | Remove widget reference by name | [ISDPAutomatedWorkflowPlugin.cs:24](../../../dll/project/SDPClientFramework/AutomatedWorkflow/ISDPAutomatedWorkflowPlugin.cs#L24) | AutomatedWorkflowExecutor forwards |
| IClickableWidget.Click | Simulate click on widget | [TestableWidgets/IClickableWidget.cs:13](../../../dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/IClickableWidget.cs#L13) | Test interacts with button |
| IClickableWidget.Press | Simulate press down | [TestableWidgets/IClickableWidget.cs:16](../../../dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/IClickableWidget.cs#L16) | Test simulates press |
| IClickableWidget.Release | Simulate press release | [TestableWidgets/IClickableWidget.cs:19](../../../dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/IClickableWidget.cs#L19) | Test completes press/release cycle |
| ICheckButtonWidget.SetChecked | Set checkbox state | [TestableWidgets/ICheckButtonWidget.cs:9](../../../dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/ICheckButtonWidget.cs#L9) | Test toggles checkbox |
| IMenuItemWidget.Select | Select menu item | [TestableWidgets/IMenuItemWidget.cs:9](../../../dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/IMenuItemWidget.cs#L9) | Test navigates menu |
| ITreeViewWidget.TrySelectRootTextNode | Select root level tree node | [TestableWidgets/ITreeViewWidget.cs:11](../../../dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/ITreeViewWidget.cs#L11) | Test selects root item |
| ITreeViewWidget.TrySelectTextNodeThatStartsWith | Navigate and select nested node | [TestableWidgets/ITreeViewWidget.cs:14](../../../dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/ITreeViewWidget.cs#L14) | Test navigates tree path |
| ITreeViewWidget.TryExpandNodeThatStartsWith | Expand tree node | [TestableWidgets/ITreeViewWidget.cs:20](../../../dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/ITreeViewWidget.cs#L20) | Test expands parent node |
| ITreeViewWidget.TryDoubleClickNodeThatStartsWithSubstring | Double-click tree node | [TestableWidgets/ITreeViewWidget.cs:23](../../../dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/ITreeViewWidget.cs#L23) | Test activates tree item |
| ISaveFileDialog.SetFilePath | Set file path in save dialog | [TestableWidgets/ISaveFileDialog.cs:10](../../../dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/ISaveFileDialog.cs#L10) | Test saves file |
| WidgetNames.LAUNCH_APPLICATION_BUTTON | Generate launch button widget name | [WidgetNames.cs:40](../../../dll/project/SDPClientFramework/AutomatedWorkflow/WidgetNames.cs#L40) | Test needs button name |
| WidgetNames.PROCESS_LIST | Generate process list widget name | [WidgetNames.cs:46](../../../dll/project/SDPClientFramework/AutomatedWorkflow/WidgetNames.cs#L46) | Test needs process list |
| WidgetNames.METRICS_LIST | Generate metrics list widget name | [WidgetNames.cs:52](../../../dll/project/SDPClientFramework/AutomatedWorkflow/WidgetNames.cs#L52) | Test needs metrics list |

## Call Flow Skeleton
```
Automated Test Execution:
Application startup with plugin args
 └── UIManager.AddAutomatedWorkflowExecutor(executor)
      └── new AutomatedWorkflowExecutor(plugin, args)
           └── Task.Factory.StartNew(..., LongRunning)
                ├── plugin.Init(args) → Result<string>
                ├── .Bind(() => plugin.Execute())
                └── .Match(
                     success: () → m_logger.LogInformation("Workflow Executed Successfully")
                     failure: (error) → OnWorkflowError(error)
                )

Widget Registration Flow:
UI creates widget
 └── UIManager.CacheTestableWidget(name, widget)
      └── testRunner.AddTestableWidget(name, widget)
           └── AutomatedWorkflowExecutor.AddTestableWidget(name, widget)
                └── m_workflowPlugin.AddTestableWidget(name, widget)
                     └── Plugin stores widget in internal dictionary

Widget Usage Flow:
Plugin test logic
 └── Retrieve widget by name from internal dictionary
      ├── Cast to specific interface (IClickableWidget, ITreeViewWidget, etc.)
      └── Call widget methods
           ├── await widget.Enabled / widget.Visible
           ├── widget.Click() / widget.SetChecked(true)
           └── await widget.TrySelectTextNode(path)

Error Handling Flow:
Plugin.Execute() returns Result.Error(message)
 └── AutomatedWorkflowExecutor catches in Match
      └── OnWorkflowError(error)
           ├── m_logger.LogError(error)
           ├── File.WriteAllText("AutomatedWorkflowError.txt", error)
           └── new ExitAppCommand().Execute()

Widget Name Generation:
Test needs to reference widget
 └── WidgetNames.LAUNCH_APPLICATION_BUTTON(CaptureType.Snapshot, 1)
      └── Returns: "Snapshot 1 Launch Application Button"
 └── WidgetNames.PROCESS_LIST(CaptureType.Trace, 2)
      └── Returns: "Trace 2 Process List"
 └── WidgetNames.NewSnapshotWelcomeScreenButton
      └── Returns: "Snapshot Button"

Tree View Navigation:
Plugin navigates tree
 └── ITreeViewWidget widget = GetWidget(WidgetNames.PROCESS_LIST(...))
      ├── TreeViewPath path = new string[] { "Parent", "Child", "Leaf" }
      ├── await widget.TryExpandNodeThatStartsWith(path[0..1], column)
      └── await widget.TrySelectTextNodeThatStartsWith(path, column)
           └── Returns Result<string> (success or error message)

Lifecycle Integration:
SdpApp.InitCore completes
 └── plugin.OnCoreInitialized() called
      └── Plugin knows core is ready
           └── Can now interact with ConnectionManager, etc.
```

## Data Ownership Map
| Data | Created By | Used By | Destroyed By |
|------|------------|---------|--------------|
| ISDPAutomatedWorkflowPlugin instance | External (passed to constructor) | AutomatedWorkflowExecutor, workflow task | External ownership |
| m_automatedWorkflow Task | AutomatedWorkflowExecutor constructor | Workflow execution | Task completion |
| m_logger (AutomatedWorkflowExecutor) | AutomatedWorkflowExecutor field init | OnWorkflowError, workflow task | Executor disposal |
| Widget references | UI components via AddTestableWidget | Plugin via internal dictionary | RemoveTestableWidget |
| AutomatedWorkflowError.txt | OnWorkflowError | External (test harness) | Not managed |
| TreeViewPath.Path array | Test code | Tree view widget methods | Caller manages |
| Result\<string\> | Plugin Init/Execute | AutomatedWorkflowExecutor (Match) | Scope-based |
| m_workflowPlugin reference | Constructor param | AddTestableWidget, RemoveTestableWidget, workflow task | Executor disposal |

## Log → Code Map
| Log Keyword | Location | Meaning |
|-------------|----------|---------|
| m_logger.LogInformation | [AutomatedWorkflowExecutor.cs:38](../../../dll/project/SDPClientFramework/AutomatedWorkflow/AutomatedWorkflowExecutor.cs#L38) | Workflow executed successfully |
| m_logger.LogError | [AutomatedWorkflowExecutor.cs:61](../../../dll/project/SDPClientFramework/AutomatedWorkflow/AutomatedWorkflowExecutor.cs#L61) | Workflow error occurred |
| AutomatedWorkflowExecutor | [AutomatedWorkflowExecutor.cs:85](../../../dll/project/SDPClientFramework/AutomatedWorkflow/AutomatedWorkflowExecutor.cs#L85) | Logger instance name |
| Workflow Executed Successfully | [AutomatedWorkflowExecutor.cs:38](../../../dll/project/SDPClientFramework/AutomatedWorkflow/AutomatedWorkflowExecutor.cs#L38) | Success log message text |
| AutomatedWorkflowError.txt | [AutomatedWorkflowExecutor.cs:88](../../../dll/project/SDPClientFramework/AutomatedWorkflow/AutomatedWorkflowExecutor.cs#L88) | Error output file on failure |

## Search Hints
```
Find test runner entry:
search "class AutomatedWorkflowExecutor"
search "new AutomatedWorkflowExecutor"

Find plugin interface:
search "interface ISDPAutomatedWorkflowPlugin"
search "Init|Execute.*Result<string>"

Find widget abstractions:
search "interface I.*Widget"
search "IClickableWidget|ITreeViewWidget|ICheckButtonWidget"

Find widget registration:
search "AddTestableWidget|RemoveTestableWidget"
search "CacheTestableWidget"

Find widget naming:
search "class WidgetNames"
search "LAUNCH_APPLICATION_BUTTON|PROCESS_LIST|METRICS_LIST"

Find workflow execution:
search "Task.Factory.StartNew.*LongRunning"
search "plugin.Init|plugin.Execute"

Find error handling:
search "OnWorkflowError|AutomatedWorkflowError.txt"
search "Result<string>.*Match"

Jump to key components:
open dll/project/SDPClientFramework/AutomatedWorkflow/AutomatedWorkflowExecutor.cs:12
open dll/project/SDPClientFramework/AutomatedWorkflow/ISDPAutomatedWorkflowPlugin.cs:10
open dll/project/SDPClientFramework/AutomatedWorkflow/WidgetNames.cs:7
open dll/project/SDPClientFramework/AutomatedWorkflow/TestableWidgets/ITestableWidget.cs:7
```
