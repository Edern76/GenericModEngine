using GenericModEngine.Core.Types;
using GenericModEngine.Tests.Core.Utils.Data;

namespace GenericModEngine.Tests;

public class InsertionTest
{
    [Fact]
    public void Test_InsertionIndex()
    {
        ModList modList = new ModList(new List<Mod>()
        {
            LoadAfterMods.Before1,
            LoadAfterMods.Before2,
            LoadAfterMods.AfterBefore2,
            LoadAfterMods.Basic1,
            LoadAfterMods.AfterBefore3,
        });
        Assert.Equal(2, modList.GetInsertionIndex(LoadAfterMods.AfterAfterBefore2));
        Assert.Equal(3, modList.GetInsertionIndex(LoadAfterMods.AfterBasic1AndBefore2));
        Assert.Equal(-1, modList.GetInsertionIndex(LoadAfterMods.AfterAfterAfterBefore3));
        Assert.Equal(-1, modList.GetInsertionIndex(LoadAfterMods.Basic2));
    }
}
