using System.Reflection;

namespace GenericModEngine.Core.Types.Assemblies;

public class UnloadedAssemblyWrapper
{
    public string AssemblyPath { get; init; }
    public AssemblyName AssemblyName { get; init; }

    public UnloadedAssemblyWrapper(string assemblyPath)
        : this(assemblyPath, false) { }

    internal UnloadedAssemblyWrapper(string assemblyPath, bool mock)
    {
        this.AssemblyPath = assemblyPath;
        if (!this.AssemblyPath.EndsWith(".dll"))
        {
            throw new InvalidOperationException($"File at {assemblyPath} is not a valid DLL.");
        }
        if (mock)
        {
            this.AssemblyName = new AssemblyName("MockAssembly");
        }
        else
        {
            this.AssemblyName = AssemblyName.GetAssemblyName(assemblyPath);
        }
    }
}
