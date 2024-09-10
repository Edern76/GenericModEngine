namespace GenericModEngine.Tests.Core.Utils.Data.Manifests;

public class MockManifestsJSON
{
    public static readonly string Manifest1 = @"{
        ""Name"": ""Mod1"",
        ""ID"": ""MockDev.Mods.Mod1""
    }";

    public static readonly string Manifest2 = @"{
        ""Name"": ""Mod2"",
        ""ID"": ""MockDev.Mods.Mod2"",
        ""Author"": ""MockDev"",
        ""Version"": ""1.0.0"",
        ""Description"": ""This is a mock mod"",
        ""Website"": ""https://www.mockdev.com"",
        ""Dependencies"": [""MockDev.Mods.Mod1""],
        ""Incompatibilities"" : [""OtherDev.Mods.IncompatibleMess""],
        ""LoadAfter"" : [""Core.Wrappers.Harmony""]
    }";

    public static readonly string Manifest3 = @"{
        ""Name"": ""Mod3"",
    }";
}