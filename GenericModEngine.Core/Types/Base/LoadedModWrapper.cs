using System.Reflection;
using JSONPatcherCore.Operations.Base;
using Newtonsoft.Json.Linq;

namespace GenericModEngine.Core.Types;

public class LoadedModWrapper
{
    public Mod Mod { get; init; }
    public List<Assembly> LoadedAssemblies { get; init; }
    public List<IPatchOperation> Patches { get; init; }
    public JObject RawModData { get; init; }
}