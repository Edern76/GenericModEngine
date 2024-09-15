using Newtonsoft.Json.Linq;

namespace GenericModEngine.Tests.Core.Utils.Data.Implementations;

public class GenericClass2
{
    public void OnJsonMerged(JObject mergedJson)
    {
        Console.WriteLine("GenericClass2 OnJsonMerged");
        Console.WriteLine("Oops not implemented the interface");
    }
}
