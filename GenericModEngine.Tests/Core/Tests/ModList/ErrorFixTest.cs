using GenericModEngine.Core.Types;
using GenericModEngine.Tests.Core.Utils.Data;

namespace GenericModEngine.Tests;

public class ErrorFixTest
{
    [Fact]
    public void Test_FixSimpleErrors()
    {
        ModList modList = new ModList(new List<Mod>()
        {
            LoadAfterMods.AfterBefore2,
            LoadAfterMods.AfterBasic2AndBefore2,
            LoadAfterMods.Before2,
            LoadAfterMods.Before3,
            LoadAfterMods.AfterBefore3,
            LoadAfterMods.AfterBasic1AndBefore2,
            LoadAfterMods.Basic1,
            LoadAfterMods.AfterAfterBefore2,
            LoadAfterMods.Basic2,
            LoadAfterMods.AfterAfterAfterBefore3,
        });
        modList.FixSimpleErrors();
        Assert.Equal(10, modList.ReadOnlyList.Count);
        Assert.Empty(modList.Validate());
        Assert.Equal(LoadAfterMods.Before2, modList.ReadOnlyList[0]);
        Assert.Equal(LoadAfterMods.AfterBefore2, modList.ReadOnlyList[1]);
        Assert.Equal(LoadAfterMods.Before3, modList.ReadOnlyList[2]);
        Assert.Equal(LoadAfterMods.AfterBefore3, modList.ReadOnlyList[3]);
        Assert.Equal(LoadAfterMods.Basic1, modList.ReadOnlyList[4]);
        Assert.Equal(LoadAfterMods.AfterBasic1AndBefore2, modList.ReadOnlyList[5]);
        Assert.Equal(LoadAfterMods.AfterAfterBefore2, modList.ReadOnlyList[6]);
        Assert.Equal(LoadAfterMods.Basic2, modList.ReadOnlyList[7]);
        Assert.Equal(LoadAfterMods.AfterBasic2AndBefore2, modList.ReadOnlyList[8]);
        Assert.Equal(LoadAfterMods.AfterAfterAfterBefore3, modList.ReadOnlyList[9]);
    }
}