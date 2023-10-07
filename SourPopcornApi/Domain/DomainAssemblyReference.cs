using System.Reflection;

namespace Domain;

public static class DomainAssemblyReference
{
    internal static readonly Assembly Assembly = typeof(DomainAssemblyReference).Assembly;
}
