﻿using System.Collections.ObjectModel;
using GenericModEngine.Core.Types.Error;

namespace GenericModEngine.Core.Types;

public class ModList
{
    private ObservableCollection<Mod> Mods { get; init; } = new();

    private ReadOnlyObservableCollection<Mod> _readonlyMods;
    public ReadOnlyObservableCollection<Mod> ReadOnlyList => _readonlyMods;

    public ModList()
    {
        _readonlyMods = new ReadOnlyObservableCollection<Mod>(Mods);
    }

    public ModList(ICollection<Mod> mods)
    {
        Mods = new ObservableCollection<Mod>(mods);
        _readonlyMods = new ReadOnlyObservableCollection<Mod>(Mods);
    }

    public void Add(Mod mod)
    {
        if (Mods.All(m => m.Manifest.ID != mod.Manifest.ID))
        {
            Mods.Add(mod);
        }
    }

    public void Add(Mod mod, int position)
    {
        if (Mods.All(m => m.Manifest.ID != mod.Manifest.ID))
        {
            Mods.Insert(position, mod);
        }
    }

    public void Remove(Mod mod)
    {
        Mods.Remove(mod);
    }

    public void Move(Mod mod, int position)
    {
        Mods.Move(Mods.IndexOf(mod), position);
    }

    public List<IModListError> Validate()
    {
        return Mods.SelectMany(ValidateMod).ToList();
    }

    public void AutoInsert(Mod mod)
    {
        int targetIndex = GetInsertionIndex(mod);
        if (targetIndex == -1)
        {
            this.Add(mod);
        }
        else
        {
            this.Add(mod, targetIndex);
        }
    }

    // If it needs to be inserted at the end, return -1
    // Otherwise return the index where it should be inserted
    public int GetInsertionIndex(Mod mod)
    {
        List<string> presentModsAfter = mod.Manifest.LoadAfter.Where(id => Mods.Any(m => m.Manifest.ID == id)).ToList();
        if (presentModsAfter.Count == 0)
        {
            return -1;
        }
        int targetIndex = presentModsAfter.Select(id => Mods.IndexOf(Mods.First(m => m.Manifest.ID == id))).Max() + 1;
        return targetIndex;
    }
    public void FixSimpleErrors()
    {
        List<IModListError> errors;
        do
        {
            errors = Validate().Where(e => e is NotLoadedAfterError).ToList();
            
            foreach (NotLoadedAfterError error in errors.OfType<NotLoadedAfterError>())
            {
                Mod? source = Mods.FirstOrDefault(m => m.Manifest.ID == error.Source);
                if (source is null)
                {
                    throw new KeyNotFoundException($"Mod {error.Source} not found");
                } 
                int targetIndex = GetInsertionIndex(source);
                if (targetIndex == -1)
                {
                    throw new InvalidOperationException($"No mod that should be loaded after {source} was found (should never happen !).");
                }
                this.Move(source, targetIndex);
            }
        } while (errors.Count > 0);
    }

    public void FixMissingDependencies(Mod mod, List<Mod> allMods)
    {
        List<IModListError> errors;
        do
        {
            errors = Validate().Where(e => e is MissingDependencyError).ToList();
            HashSet<string> dependenciesIds = errors.Select(e => (e as MissingDependencyError)!.Target!).ToHashSet();
            List<Mod> missingDependencies = allMods.Where(m => dependenciesIds.Contains(m.Manifest.ID)).ToList();
            HashSet<string> missingDependenciesIds = missingDependencies.Select(m => m.Manifest.ID).ToHashSet();
            HashSet<string> dependenciesNotFound = new HashSet<string>(dependenciesIds.Except(missingDependenciesIds));
            if (dependenciesNotFound.Count > 0)
            {
                string missingDependenciesString = string.Join("\n", dependenciesNotFound.Select(id => $"- {id}"));
                throw new InvalidOperationException("The following missing dependencies are not installed : \n {missingDependenciesString}");
            }

            foreach (Mod missingDependency in missingDependencies)
            {
                this.AutoInsert(missingDependency);
            }
        }
        while (errors.Count > 0);
    }

    private List<IModListError> ValidateMod(Mod mod)
    {
        if (!Mods.Contains(mod))
        {
            throw new ArgumentException($"Mod {mod} is not in the mod list");
        }
        List<IModListError> errors = new List<IModListError>();
        
        foreach (Mod otherMod in Mods)
        {
            if (otherMod == mod)
            {
                continue;
            }
            if (!mod.IsCompatibleWith(otherMod))
            {
                errors.Add(new IncompatibleModError(mod.Manifest.ID, otherMod.Manifest.ID));
            }

            if (Mods.IndexOf(otherMod) > Mods.IndexOf(mod) && mod.Manifest.LoadAfter.Contains(otherMod.Manifest.ID))
            {
                errors.Add(new NotLoadedAfterError(mod.Manifest.ID, otherMod.Manifest.ID));
            }
        }
        
        HashSet<string> modsIds = Mods.Select(mod => mod.Manifest.ID).ToHashSet();
        foreach (string dependency in mod.Manifest.Dependencies)
        {
            if (!modsIds.Contains(dependency))
            {
                errors.Add(new MissingDependencyError(mod.Manifest.ID, dependency));
            }
        }
        
        return errors;
    }

}