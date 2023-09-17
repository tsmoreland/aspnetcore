using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable once CheckNamespace -- namespace needs to match System.Runtime.CompilerServices, see reference in remarks for details
namespace System.Runtime.CompilerServices;

/// <summary>
/// The IsExternalInit type is only included in the net5.0 (and future) target frameworks. When compiling against older target frameworks you will need to manually define this type.
/// (per Jared Parsons MSFT)
/// </summary>
/// <remarks>
/// For Reference:
/// <see href="https://developercommunity.visualstudio.com/t/error-cs0518-predefined-type-systemruntimecompiler/1244809">Error CS0518 Predefined type 'System.Runtime.CompilerServices.IsExternalInit' is not defined or imported</see>
/// </remarks>
[ExcludeFromCodeCoverage, DebuggerNonUserCode]
internal static class IsExternalInit
{
}
