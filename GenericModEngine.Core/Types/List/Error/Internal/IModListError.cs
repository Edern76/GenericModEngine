﻿namespace GenericModEngine.Core.Types.Error;

public interface IModListError
{
    public string Source { get; init; }
    public string? Target { get; init; }
    public ModListErrorSeverity Severity { get; }
    
    public string ToString();
}