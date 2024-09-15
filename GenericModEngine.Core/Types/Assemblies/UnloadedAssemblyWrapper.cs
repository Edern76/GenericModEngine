using System.Reflection;

namespace GenericModEngine.Core.Types.Assemblies;

public class UnloadedAssemblyWrapper
{
    public string AssemblyPath { get; init; }
    public AssemblyName AssemblyName { get; init; }

    public UnloadedAssemblyWrapper(string assemblyPath)
    {
        this.AssemblyPath = assemblyPath;
        this.AssemblyName = AssemblyName.GetAssemblyName(assemblyPath);
    }
}
