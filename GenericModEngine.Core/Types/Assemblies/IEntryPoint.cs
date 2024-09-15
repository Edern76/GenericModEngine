using Newtonsoft.Json.Linq;

namespace GenericModEngine.Core.Types.Assemblies;

// Needs to be implemented by the mod's assembly
public interface IEntryPoint
{
    public void OnJsonMerged(JObject mergedJson); // When the final JSON is patched and merged, but before it is parsed into game usable objects
    public void OnJsonParsed(); // When the game is done parsing the final JSON into usable objects
}
