using GenericModEngine.Core.Types;
using GenericModEngine.Tests.Core.Utils.Data;

namespace GenericModEngine.Tests;

public class BaseOperationsTest
{
    [Fact]
    public void Test_Add()
    {
        ModList modList = new ModList();
        modList.Add(LoadAfterMods.Basic1);
        Assert.Single(modList.ReadOnlyList);
        Assert.Equal(LoadAfterMods.Basic1, modList.ReadOnlyList[0]);
    }

    [Fact]
    public void Test_Add_Existing()
    {
        ModList modList = new ModList(new List<Mod>(){
            LoadAfterMods.Basic1,
            LoadAfterMods.Basic2,
            LoadAfterMods.Before1
        });
        modList.Add(LoadAfterMods.Basic1);
        Assert.Equal(3, modList.ReadOnlyList.Count);
        Assert.Equal(LoadAfterMods.Basic1, modList.ReadOnlyList[0]);
        Assert.Equal(LoadAfterMods.Basic2, modList.ReadOnlyList[1]);
        Assert.Equal(LoadAfterMods.Before1, modList.ReadOnlyList[2]);
    }
    
    [Fact]
    public void Test_Add_AtPosition()
    {

        ModList modList = new ModList(new List<Mod>(){
            LoadAfterMods.Basic1,
            LoadAfterMods.Basic2,
            LoadAfterMods.Before1
        });
        modList.Add(LoadAfterMods.Before2, 1);
        Assert.Equal(4, modList.ReadOnlyList.Count);
        Assert.Equal(LoadAfterMods.Basic1, modList.ReadOnlyList[0]);
        Assert.Equal(LoadAfterMods.Before2, modList.ReadOnlyList[1]);
        Assert.Equal(LoadAfterMods.Basic2, modList.ReadOnlyList[2]);
        Assert.Equal(LoadAfterMods.Before1, modList.ReadOnlyList[3]);
    }
    
    [Fact]
    public void Test_Add_AtPosition_Existing()
    {
        ModList modList = new ModList(new List<Mod>(){
            LoadAfterMods.Basic1,
            LoadAfterMods.Basic2,
            LoadAfterMods.Before1
        });
        modList.Add(LoadAfterMods.Basic1, 1);
        Assert.Equal(3, modList.ReadOnlyList.Count);
        Assert.Equal(LoadAfterMods.Basic1, modList.ReadOnlyList[0]);
        Assert.Equal(LoadAfterMods.Basic2, modList.ReadOnlyList[1]);
        Assert.Equal(LoadAfterMods.Before1, modList.ReadOnlyList[2]);
    }

    [Fact]
    public void Test_Remove()
    {
        ModList modList = new ModList(new List<Mod>(){
            LoadAfterMods.Basic1,
            LoadAfterMods.Basic2,
            LoadAfterMods.Before1
        });
        modList.Remove(LoadAfterMods.Basic1);
        Assert.Equal(2, modList.ReadOnlyList.Count);
        Assert.Equal(LoadAfterMods.Basic2, modList.ReadOnlyList[0]);
        Assert.Equal(LoadAfterMods.Before1, modList.ReadOnlyList[1]);
    }

    [Fact]
    public void Test_Remove_NotInList()
    {
        ModList modList = new ModList(new List<Mod>(){
            LoadAfterMods.Basic1,
            LoadAfterMods.Basic2,
            LoadAfterMods.Before1
        });
        modList.Remove(LoadAfterMods.Before2);
        Assert.Equal(3, modList.ReadOnlyList.Count);
        Assert.Equal(LoadAfterMods.Basic1, modList.ReadOnlyList[0]);
        Assert.Equal(LoadAfterMods.Basic2, modList.ReadOnlyList[1]);
        Assert.Equal(LoadAfterMods.Before1, modList.ReadOnlyList[2]);
    }
    
    [Fact]
    public void Test_Move()
    {
        ModList modList = new ModList(new List<Mod>(){
            LoadAfterMods.Basic1,
            LoadAfterMods.Basic2,
            LoadAfterMods.Before1
        });
        modList.Move(LoadAfterMods.Basic1, 1);
        Assert.Equal(3, modList.ReadOnlyList.Count);
        Assert.Equal(LoadAfterMods.Basic2, modList.ReadOnlyList[0]);
        Assert.Equal(LoadAfterMods.Basic1, modList.ReadOnlyList[1]);
        Assert.Equal(LoadAfterMods.Before1, modList.ReadOnlyList[2]);
    }
    
    [Fact]
    public void Test_Move_NotInList()
    {
        ModList modList = new ModList(new List<Mod>(){
            LoadAfterMods.Basic1,
            LoadAfterMods.Basic2,
            LoadAfterMods.Before1
        });
        Assert.Throws<ArgumentException>(() => modList.Move(LoadAfterMods.Before2, 1));

    }
}