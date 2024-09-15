using System.Reflection;
using GenericModEngine.Core.Types.Assemblies;
using JSONPatcherCore.Operations.Base;
using Newtonsoft.Json.Linq;

namespace GenericModEngine.Core.Types;

public class LoadedModWrapper
{
    public Mod Mod { get; init; }
    public List<ModAssemblyWrapper> LoadedAssemblies { get; init; }
    public List<IPatchOperation> Patches { get; init; }
    public JObject RawModData { get; init; }
}
