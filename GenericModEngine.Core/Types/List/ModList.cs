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

    public void Add(Mod mod)
    {
        Mods.Add(mod);
    }

    public void Add(Mod mod, int position)
    {
        Mods.Insert(position, mod);
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

    public void FixSimpleErrors()
    {
        List<IModListError> errors;
        do
        {
            errors = Validate();
            if (errors.Any(e => e is not NotLoadedAfterError))
            {
                throw new InvalidOperationException("Cannot automatically fix non load order related errors.");
            }

            foreach (NotLoadedAfterError error in errors.OfType<NotLoadedAfterError>())
            {
                Mod? source = Mods.FirstOrDefault(m => m.Manifest.ID == error.Source);
                if (source is null)
                {
                    throw new KeyNotFoundException($"Mod {error.Source} not found");
                }
                List<string> presentModsAfter = source.Manifest.LoadAfter.Where(id => Mods.Any(m => m.Manifest.ID == id)).ToList();
                if (presentModsAfter.Count == 0)
                {
                    throw new InvalidOperationException($"No mod that should be loaded after {source} was found (should never happen !).");
                }
                int targetIndex = presentModsAfter.Select(id => Mods.IndexOf(Mods.First(m => m.Manifest.ID == id))).Max();
                this.Move(source, targetIndex);
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