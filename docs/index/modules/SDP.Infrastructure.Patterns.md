# MODULE INDEX — SDP.Infrastructure.Patterns — AUTHORITATIVE ROUTING

## Routing Keywords
Systems: Infrastructure, Threading, Concurrency, Resource Management
Concepts: Single Consumer Queue, Action Queue, Cancellation, Resource Invalidation, Task Management
Common Logs: ResourcesInvalidator, m_errorLogger
Entry Symbols: ActionQueue, CancellableActionQueue, ResourcesInvalidator

## Role
Reusable infrastructure patterns providing thread-safe single-consumer action queues and cancellable resource invalidation framework for decoupled UI updates.

## Entry Points
| Symbol | Location |
|--------|----------|
| ActionQueue | [SdpClientFramework/DesignPatterns/SingleConsumer/ActionQueue.cs:9](../../../dll/project/SDPClientFramework/SdpClientFramework/DesignPatterns/SingleConsumer/ActionQueue.cs#L9) |
| CancellableActionQueue | [SdpClientFramework/DesignPatterns/SingleConsumer/CancellableActionQueue.cs:9](../../../dll/project/SDPClientFramework/SdpClientFramework/DesignPatterns/SingleConsumer/CancellableActionQueue.cs#L9) |
| IActionQueue | [SdpClientFramework/DesignPatterns/SingleConsumer/IActionQueue.cs:6](../../../dll/project/SDPClientFramework/SdpClientFramework/DesignPatterns/SingleConsumer/IActionQueue.cs#L6) |
| ResourcesInvalidator\<TRequest\> | [SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs:13](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L13) |
| IResourcesInvalidator\<TRequest\> | [SdpClientFramework/ResourcesInvalidator/IResourcesInvalidator.cs:7](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/IResourcesInvalidator.cs#L7) |
| IResourcePopulator\<TRequest\> | [SdpClientFramework/ResourcesInvalidator/IResourcePopulator.cs:7](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/IResourcePopulator.cs#L7) |

## Key Classes
| Class | Responsibility | Location |
|------|----------------|----------|
| ActionQueue | Single-consumer queue using BlockingCollection for sequential action execution | [ActionQueue.cs:9](../../../dll/project/SDPClientFramework/SdpClientFramework/DesignPatterns/SingleConsumer/ActionQueue.cs#L9) |
| CancellableActionQueue | Single-consumer queue with automatic cancellation of previous action on new queue | [CancellableActionQueue.cs:9](../../../dll/project/SDPClientFramework/SdpClientFramework/DesignPatterns/SingleConsumer/CancellableActionQueue.cs#L9) |
| ResourcesInvalidator\<TRequest\> | Abstract base for managing async resource invalidation with task cancellation | [ResourcesInvalidator.cs:13](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L13) |
| PerCategoryInvalidateInfo | Tracks per-category invalidation tasks and cancellation tokens | [ResourcesInvalidator.cs:411](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L411) |
| IActionQueue | Non-generic action queue contract | [IActionQueue.cs:6](../../../dll/project/SDPClientFramework/SdpClientFramework/DesignPatterns/SingleConsumer/IActionQueue.cs#L6) |
| IActionQueue\<T\> | Generic action queue contract with typed parameter | [IActionQueue.2.cs:6](../../../dll/project/SDPClientFramework/SdpClientFramework/DesignPatterns/SingleConsumer/IActionQueue.2.cs#L6) |
| IResourcesInvalidator\<TRequest\> | Contract for resource invalidation with lifecycle events | [IResourcesInvalidator.cs:7](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/IResourcesInvalidator.cs#L7) |
| IResourcePopulator\<TRequest\> | Contract for populating resource objects during invalidation | [IResourcePopulator.cs:7](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/IResourcePopulator.cs#L7) |
| IInvalidateRequest | Marker interface for invalidation requests with type and parallel flag | [IInvalidateRequest.cs:6](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/IInvalidateRequest.cs#L6) |
| InvalidateEventArgs\<TRequest\> | Event args containing invalidation request | [InvalidateEventArgs.cs:6](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/InvalidateEventArgs.cs#L6) |

## Key Methods
| Method | Purpose | Location | Triggered When |
|--------|---------|----------|----------------|
| ActionQueue.Queue | Add action to execution queue | [ActionQueue.cs:20](../../../dll/project/SDPClientFramework/SdpClientFramework/DesignPatterns/SingleConsumer/ActionQueue.cs#L20) | Client needs sequential execution |
| ActionQueue.ExecuteLoop | Infinite loop consuming and executing actions | [ActionQueue.cs:37](../../../dll/project/SDPClientFramework/SdpClientFramework/DesignPatterns/SingleConsumer/ActionQueue.cs#L37) | Background task started on init |
| ActionQueue.CreateExecuteTask | Start long-running consumer task | [ActionQueue.cs:30](../../../dll/project/SDPClientFramework/SdpClientFramework/DesignPatterns/SingleConsumer/ActionQueue.cs#L30) | First Queue() or constructor |
| CancellableActionQueue.Queue | Add action, cancel previous if running | [CancellableActionQueue.cs:13](../../../dll/project/SDPClientFramework/SdpClientFramework/DesignPatterns/SingleConsumer/CancellableActionQueue.cs#L13) | Client needs cancellable execution |
| CancellableActionQueue.CancelRunningAction | Cancel current action and wait for completion | [CancellableActionQueue.cs:44](../../../dll/project/SDPClientFramework/SdpClientFramework/DesignPatterns/SingleConsumer/CancellableActionQueue.cs#L44) | New action queued while one runs |
| CancellableActionQueue.ExecuteLoop | Dequeue, cancel previous, start new action | [CancellableActionQueue.cs:28](../../../dll/project/SDPClientFramework/SdpClientFramework/DesignPatterns/SingleConsumer/CancellableActionQueue.cs#L28) | Background task processes queue |
| ResourcesInvalidator.Invalidate | Entry point to request resource invalidation | [ResourcesInvalidator.cs:46](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L46) | UI/client needs resource refresh |
| ResourcesInvalidator.HandleRequest | Abstract method to handle specific request types | [ResourcesInvalidator.cs:120](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L120) | Subclass implements request logic |
| ResourcesInvalidator.QueueNewInvalidate | Create and spawn invalidation task | [ResourcesInvalidator.cs:124](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L124) | After HandleRequest processes request |
| ResourcesInvalidator.ExecuteInvalidate | Run invalidation with all populators | [ResourcesInvalidator.cs:323](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L323) | Spawned task executes |
| ResourcesInvalidator.StartRequestManagerTask | Create task to process invalidation queue | [ResourcesInvalidator.cs:217](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L217) | First Invalidate() call |
| ResourcesInvalidator.CheckForFinishedTasks | Poll task completion, handle exceptions | [ResourcesInvalidator.cs:239](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L239) | Request manager loop iteration |
| ResourcesInvalidator.CancelAllInvalidates | Cancel all pending tasks across categories | [ResourcesInvalidator.cs:159](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L159) | Cleanup or reset needed |
| ResourcesInvalidator.CancelInvalidateForCategory | Cancel tasks for specific category | [ResourcesInvalidator.cs:173](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L173) | Category-specific cancellation |
| ResourcesInvalidator.AddPopulator | Register resource populator | [ResourcesInvalidator.cs:200](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L200) | Subclass initialization |
| ResourcesInvalidator.DisableResourceView | Disable resource view via events | [ResourcesInvalidator.cs:64](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L64) | Before heavy operation |
| ResourcesInvalidator.EnableResourceView | Enable resource view via events | [ResourcesInvalidator.cs:75](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L75) | After operation completes |
| ResourcesInvalidator.ExecuteBegin | Raise InvalidateBegan event | [ResourcesInvalidator.cs:351](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L351) | Start of ExecuteInvalidate |
| ResourcesInvalidator.ExecuteEnd | Raise InvalidateEnded event | [ResourcesInvalidator.cs:364](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L364) | Successful invalidation completion |
| ResourcesInvalidator.InvalidateRequestExHandler | Handle task exceptions, raise events | [ResourcesInvalidator.cs:296](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L296) | Task throws exception |
| IResourcePopulator.PopulateResourceObjects | Populate resources for request | [IResourcePopulator.cs:10](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/IResourcePopulator.cs#L10) | ExecuteInvalidate iterates populators |

## Call Flow Skeleton
```
ActionQueue Pattern:
Client
 └── Queue(action)
      ├── Add to BlockingCollection
      └── CreateExecuteTask (if needed)
           └── ExecuteLoop (background)
                ├── Take() from collection (blocks)
                └── action() execution

CancellableActionQueue Pattern:
Client
 └── Queue(action)
      ├── Add to BlockingCollection
      └── ExecuteLoop (background)
           ├── Take() from collection
           ├── CancelRunningAction()
           │    ├── m_cancelSource.Cancel()
           │    ├── m_runningAction.Wait()
           │    └── Dispose old CancellationTokenSource
           └── Start new task with action(token)

ResourcesInvalidator Pattern:
Client
 └── Invalidate(request)
      ├── Add to m_invalidateQueue
      └── StartRequestManagerTask (first call only)
           └── Request Manager Loop
                ├── Take/TryTake from queue
                ├── HandleRequest(request) [abstract - subclass]
                └── QueueNewInvalidate(request)
                     ├── Create Task
                     └── ExecuteInvalidate(request, cancelToken)
                          ├── ExecuteBegin() → InvalidateBegan event
                          ├── For each IResourcePopulator:
                          │    └── PopulateResourceObjects(request, token)
                          ├── ExecuteEnd() → InvalidateEnded event
                          └── Exceptions → InvalidateRequestExHandler
                               ├── TaskCanceledException → InvalidateCanceled event
                               └── Other Exception → InvalidateFailed event
                ├── CheckForFinishedTasks()
                │    ├── Wait(timeout) on tasks
                │    └── Remove completed
                └── Loop continues
```

## Data Ownership Map
| Data | Created By | Used By | Destroyed By |
|------|------------|---------|--------------|
| BlockingCollection\<Action\> | ActionQueue constructor | ExecuteLoop (Take), Queue (Add) | ActionQueue disposal |
| Task m_executeTask | CreateExecuteTask | Queue (null check), ExecuteLoop | Task completion |
| BlockingCollection\<Action\<CancellationToken\>\> | CancellableActionQueue | ExecuteLoop, Queue | Queue disposal |
| CancellationTokenSource | CancelRunningAction | ExecuteLoop (Token), CancelRunningAction | CancelRunningAction (Dispose) |
| BlockingCollection\<TRequest\> | ResourcesInvalidator constructor | StartRequestManagerTask, Invalidate | ResourcesInvalidator disposal |
| Dictionary\<uint, PerCategoryInvalidateInfo\> | ResourcesInvalidator | QueueNewInvalidate, CheckForFinishedTasks, Cancel methods | CancelAllInvalidates |
| List\<IResourcePopulator\<TRequest\>\> | ResourcesInvalidator | AddPopulator, ExecuteInvalidate | ClearPopulators |
| Task m_requestManagerTask | StartRequestManagerTask | Invalidate (null check), Request manager | Task completion |
| PerCategoryInvalidateInfo | QueueNewInvalidate | CheckForFinishedTasks, Cancel methods | Dispose() on category cleanup |
| List\<Tuple\<uint, Task\>\> m_spawnedTasks | QueueNewInvalidate (Add) | CheckForFinishedTasks, CancelAllInvalidates | Clear on cancel all |
| IActionQueue m_clientCommandQueue | Constructor injection | InvalidateRequestExHandler (Queue events) | External ownership |

## Log → Code Map
| Log Keyword | Location | Meaning |
|-------------|----------|---------|
| ResourcesInvalidator | [ResourcesInvalidator.cs:380](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L380) | Logger instance name for error logging |
| m_errorLogger.LogException | [ResourcesInvalidator.cs:316](../../../dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs#L316) | Task exception logging in InvalidateRequestExHandler |

## Search Hints
```
Find ActionQueue entry:
search "class ActionQueue"

Find ResourcesInvalidator:
search "class ResourcesInvalidator<TRequest>"

Find cancellation logic:
search "CancelRunningAction|CancelAllInvalidates"

Find task spawning:
search "QueueNewInvalidate"

Find event handlers:
search "InvalidateBegan|InvalidateEnded|InvalidateFailed|InvalidateCanceled"

Jump to pattern roots:
open dll/project/SDPClientFramework/SdpClientFramework/DesignPatterns/SingleConsumer/ActionQueue.cs:9
open dll/project/SDPClientFramework/SdpClientFramework/ResourcesInvalidator/ResourcesInvalidator.cs:13
```
