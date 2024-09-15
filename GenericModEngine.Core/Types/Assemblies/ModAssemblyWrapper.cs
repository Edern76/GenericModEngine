using System.Reflection;

namespace GenericModEngine.Core.Types.Assemblies;

public class ModAssemblyWrapper
{
    public Assembly Assembly { get; init; }
    public IEntryPoint? EntryPoint { get; init; }

    public ModAssemblyWrapper(Assembly assembly)
    {
        this.Assembly = assembly;

        List<Type> entryPointTypes = assembly
            .GetTypes()
            .Where(t => typeof(IEntryPoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToList();

        if (entryPointTypes.Count > 1)
        {
            throw new InvalidOperationException(
                $"Assembly {assembly.FullName} has more than one entry point."
            );
        }

        if (entryPointTypes.Count == 1)
        {
            this.EntryPoint = (IEntryPoint)Activator.CreateInstance(entryPointTypes[0]!)!;
        }
    }
}
