using System.Reflection;

namespace Infrastructure;

public static class InfrastructureAssemblyReference
{
    internal static readonly Assembly Assembly = typeof(InfrastructureAssemblyReference).Assembly;
}
