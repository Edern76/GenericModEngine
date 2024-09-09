using System.Reflection;
using GenericModEngine.Core.Types;
using GenericModEngine.Core.Types.Error;
using GenericModEngine.Tests.Core.Utils.Data;

namespace GenericModEngine.Tests;

public class ValidationTest
{
    [Fact]
    public void Test_Validate_Correct()
    {
        ModList modList = new ModList(new List<Mod>()
        {
            LoadAfterMods.Before1,
            LoadAfterMods.Before2,
            LoadAfterMods.AfterBefore2,
            DependencyMods.Dependency1,
            DependencyMods.Dependency2,
            DependencyMods.Dependency3,
            LoadAfterMods.Basic1,
            LoadAfterMods.AfterBefore3,
            DependencyMods.Dependent2,
            CompatibilityMods.Incompatible1,
            DependencyMods.Dependent1,
        });
        
        List<IModListError> errors = modList.Validate();
        Assert.Empty(errors);
    }

    [Fact]
    public void Test_Validate_WrongOrder()
    {
        ModList modList = new ModList(new List<Mod>()
        {
            LoadAfterMods.AfterBefore2,
            LoadAfterMods.Before2,
            LoadAfterMods.Before3,
            LoadAfterMods.AfterBefore3,
            LoadAfterMods.AfterBasic1AndBefore2,
            LoadAfterMods.Basic1,
            LoadAfterMods.AfterAfterBefore2,
            LoadAfterMods.AfterAfterAfterBefore3,
        });
        
        List<IModListError> errors = modList.Validate();
        Assert.Equal(2, errors.Count);
        Assert.True(errors.All(e => e is NotLoadedAfterError));
        Assert.Contains(errors, e => e.Source == LoadAfterMods.AfterBefore2.Manifest.ID && e.Target == LoadAfterMods.Before2.Manifest.ID);
        Assert.Contains(errors, e => e.Source == LoadAfterMods.AfterBasic1AndBefore2.Manifest.ID && e.Target == LoadAfterMods.Basic1.Manifest.ID);
    }

    [Fact]
    public void Test_Validate_Circular()
    {
        ModList modList = new ModList(new List<Mod>()
        {
            DependencyMods.CircularDependency1, // Should not count as an error
            LoadAfterMods.Circular1, // Should count as an error
            LoadAfterMods.Circular2,
            DependencyMods.CircularDependency2,
        });
        
        List<IModListError> errors = modList.Validate();
        Assert.Equal(2, errors.Count);
        Assert.Contains(errors, e => e is NotLoadedAfterError);
        Assert.Contains(errors, e => e is CircularLoadAfterError && (e.Source == LoadAfterMods.Circular1.Manifest.ID && e.Target == LoadAfterMods.Circular2.Manifest.ID || e.Source == LoadAfterMods.Circular2.Manifest.ID && e.Target == LoadAfterMods.Circular1.Manifest.ID));
    }

    [Fact]
    public void Test_Validate_MissingDependency()
    {
        ModList modList = new ModList(new List<Mod>()
        {
            DependencyMods.Dependency1,
            DependencyMods.Dependency3,
            DependencyMods.Dependent2, // Depends on 2 and 3
            DependencyMods.Dependent1, // Depends on 1
            DependencyMods.Dependent3, // Depends on 4
        });
        
        List<IModListError> errors = modList.Validate();
        Assert.Equal(2, errors.Count);
        Assert.True(errors.All(e => e is MissingDependencyError));
        Assert.Contains(errors, e => e.Source == DependencyMods.Dependent2.Manifest.ID && e.Target == DependencyMods.Dependency2.Manifest.ID);
        Assert.Contains(errors, e => e.Source == DependencyMods.Dependent3.Manifest.ID && e.Target == DependencyMods.Dependency4.Manifest.ID);
    }

    [Fact]
    public void Test_Validate_Incompatible()
    {
        ModList modList = new ModList(new List<Mod>()
        {
            CompatibilityMods.Compatible1,
            CompatibilityMods.Incompatible1,
            CompatibilityMods.Incompatible2,
        });
        
        List<IModListError> errors = modList.Validate();
        Assert.Single(errors);
        Assert.True(errors[0] is IncompatibleModError);
        Assert.True(errors[0] is IModListError e && e.Source == CompatibilityMods.Incompatible1.Manifest.ID && e.Target == CompatibilityMods.Compatible1.Manifest.ID);
    }

    [Fact]
    public void Test_Validate_Single_NotInList()
    {
        ModList modList = new ModList(new List<Mod>()
        {
            DependencyMods.Dependency1,
            DependencyMods.Dependency2,
            DependencyMods.Dependency3,
            DependencyMods.Dependent1,
            DependencyMods.Dependent2,
            DependencyMods.Dependent3,
        });
        
        MethodInfo method = typeof(ModList).GetMethod("ValidateMod", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.Throws<ArgumentException>(() =>
        {
            try
            {
                return method!.Invoke(modList, new object[] { LoadAfterMods.Basic1 });
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException!;
            }
        });
    }
}