using System.Collections.Immutable;
using System.Collections.ObjectModel;
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
        try
        {
            Mods.Move(Mods.IndexOf(mod), position);
        }
        catch (ArgumentOutOfRangeException)
        {
            throw new ArgumentException(
                $"Cannot move mod {mod} to position {position} as it is not in the list"
            );
        }
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
        List<string> presentModsAfter = mod
            .Manifest.LoadAfter.Where(id => Mods.Any(m => m.Manifest.ID == id))
            .ToList();
        List<string> modsNeededToLoadBefore = Mods.Where(m =>
                m.Manifest.LoadAfter.Contains(mod.Manifest.ID)
            )
            .Select(m => m.Manifest.ID)
            .ToList();
        if (presentModsAfter.Count == 0 && modsNeededToLoadBefore.Count == 0)
        {
            return -1;
        }
        int minIndex = GetMinInsertionIndex(mod, presentModsAfter);
        int maxIndex = GetMaxInsertionIndex(mod, modsNeededToLoadBefore);
        if (maxIndex != -1 && minIndex > maxIndex)
        {
            throw new InvalidOperationException(
                $"Cannot determine insertion index for mod {mod.Manifest.ID} as it would create conflicts."
            );
        }
        int targetIndex = maxIndex == -1 ? minIndex : maxIndex;
        return targetIndex;
    }

    private int GetMinInsertionIndex(Mod mod, List<string> presentModsAfter)
    {
        if (presentModsAfter.Count == 0)
        {
            return 0;
        }
        List<Mod> modsWithoutCurrent = Mods.Where(m => m != mod).ToList();
        int targetIndex =
            presentModsAfter
                .Select(id =>
                    modsWithoutCurrent.IndexOf(modsWithoutCurrent.First(mm => mm.Manifest.ID == id))
                )
                .Max() + 1;
        return targetIndex;
    }

    private int GetMaxInsertionIndex(Mod mod, List<string> modsNeededToLoadBefore)
    {
        if (modsNeededToLoadBefore.Count == 0)
        {
            return -1;
        }
        int targetIndex = modsNeededToLoadBefore
            .Select(id => Mods.IndexOf(Mods.First(mm => mm.Manifest.ID == id)))
            .Min();
        if (targetIndex > 0)
        {
            targetIndex--;
        }
        return targetIndex;
    }

    public void FixSimpleErrors()
    {
        List<IModListError> errors;
        do
        {
            errors = Validate()
                .Where(e => e is NotLoadedAfterError || e is CircularLoadAfterError)
                .OrderBy(e => Mods.IndexOf(Mods.First(m => m.Manifest.ID == e.Source)))
                .ToList();
            if (errors.Any(e => e is CircularLoadAfterError))
            {
                throw new InvalidOperationException(
                    "Cannot automatically fix errors because there is at least one circular load order requirement."
                );
            }

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
                    throw new InvalidOperationException(
                        $"No mod that should be loaded after {source} was found (should never happen !)."
                    );
                }
                this.Move(source, targetIndex);
            }
        } while (errors.Count > 0);
    }

    public void FixMissingDependencies(List<Mod> allMods)
    {
        List<IModListError> errors;
        do
        {
            errors = Validate().Where(e => e is MissingDependencyError).ToList();
            HashSet<string> dependenciesIds = errors
                .Select(e => (e as MissingDependencyError)!.Target!)
                .ToHashSet();
            List<Mod> missingDependencies = allMods
                .Where(m => dependenciesIds.Contains(m.Manifest.ID))
                .ToList();
            HashSet<string> missingDependenciesIds = missingDependencies
                .Select(m => m.Manifest.ID)
                .ToHashSet();
            HashSet<string> dependenciesNotFound = new HashSet<string>(
                dependenciesIds.Except(missingDependenciesIds)
            );
            if (dependenciesNotFound.Count > 0)
            {
                string missingDependenciesString = string.Join(
                    "\n",
                    dependenciesNotFound.Select(id => $"- {id}")
                );
                throw new InvalidOperationException(
                    "The following missing dependencies are not installed : \n {missingDependenciesString}"
                );
            }

            foreach (Mod missingDependency in missingDependencies)
            {
                this.AutoInsert(missingDependency);
            }
        } while (errors.Count > 0);
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

            if (
                Mods.IndexOf(otherMod) > Mods.IndexOf(mod)
                && mod.Manifest.LoadAfter.Contains(otherMod.Manifest.ID)
            )
            {
                errors.Add(new NotLoadedAfterError(mod.Manifest.ID, otherMod.Manifest.ID));
                if (otherMod.Manifest.LoadAfter.Contains(mod.Manifest.ID))
                {
                    errors.Add(new CircularLoadAfterError(mod.Manifest.ID, otherMod.Manifest.ID));
                }
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
