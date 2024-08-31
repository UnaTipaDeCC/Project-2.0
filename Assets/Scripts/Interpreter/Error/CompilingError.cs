using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class CompilingError: Exception
{
    public ErrorCode Code { get; private set; }
    public string Argument { get; private set; }
    public CodeLocation Location {get; private set;}
    public CompilingError(CodeLocation location, ErrorCode code, string argument)
    {
        this.Code = code;
        this.Argument = argument;
        Location = location;
    }
    public override string ToString()
    {
        return $"{Code} error: {Argument} in {Location}";
    }
}

    public enum ErrorCode
    {
        None,
        Expected,
        Invalid,
        Unknown,
    }