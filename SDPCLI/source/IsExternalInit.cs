// C# 9+ record types require IsExternalInit, which is only defined in .NET 5+.
// This shim makes record and init-only properties compile on .NET Framework targets.
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}
