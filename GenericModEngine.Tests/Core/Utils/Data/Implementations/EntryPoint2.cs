using GenericModEngine.Core.Types.Assemblies;
using Newtonsoft.Json.Linq;

namespace GenericModEngine.Tests.Core.Utils.Data.Implementations;

public class EntryPoint2 : IEntryPoint
{
    public void OnJsonMerged(JObject mergedJson)
    {
        Console.WriteLine("EntryPoint2 OnJsonMerged");
    }

    public void OnJsonParsed()
    {
        Console.WriteLine("EntryPoint2 OnJsonParsed");
    }
}
