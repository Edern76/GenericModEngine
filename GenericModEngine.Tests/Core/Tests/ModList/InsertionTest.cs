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
        Assert.Equal(3, modList.GetInsertionIndex(LoadAfterMods.AfterAfterBefore2));
        Assert.Equal(4, modList.GetInsertionIndex(LoadAfterMods.AfterBasic1AndBefore2));
        Assert.Equal(-1, modList.GetInsertionIndex(LoadAfterMods.AfterAfterAfterBefore3));
        Assert.Equal(-1, modList.GetInsertionIndex(LoadAfterMods.Basic2));
    }
    
    [Fact]
    public void Test_AutoInsert()
    {
        ModList modList = new ModList(new List<Mod>()
        {
            LoadAfterMods.Before1,
            LoadAfterMods.Before2,
            LoadAfterMods.AfterBefore2,
            LoadAfterMods.Basic1,
            LoadAfterMods.AfterBefore3,
        });
        modList.AutoInsert(LoadAfterMods.AfterAfterBefore2);
        modList.AutoInsert(LoadAfterMods.Basic2);
        modList.AutoInsert(LoadAfterMods.Before1);
        modList.AutoInsert(LoadAfterMods.AfterBasic2AndBefore1);
        Assert.Equal(8, modList.ReadOnlyList.Count);
        Assert.Equal(LoadAfterMods.Before1, modList.ReadOnlyList[0]);
        Assert.Equal(LoadAfterMods.Before2, modList.ReadOnlyList[1]);
        Assert.Equal(LoadAfterMods.AfterBefore2, modList.ReadOnlyList[2]);
        Assert.Equal(LoadAfterMods.AfterAfterBefore2, modList.ReadOnlyList[3]);
        Assert.Equal(LoadAfterMods.Basic1, modList.ReadOnlyList[4]);
        Assert.Equal(LoadAfterMods.AfterBefore3, modList.ReadOnlyList[5]);
        Assert.Equal(LoadAfterMods.Basic2, modList.ReadOnlyList[6]);
        Assert.Equal(LoadAfterMods.AfterBasic2AndBefore1, modList.ReadOnlyList[7]);
    }
}
