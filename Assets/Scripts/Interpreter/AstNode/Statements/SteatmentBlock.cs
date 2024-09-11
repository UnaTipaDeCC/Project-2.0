using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class StatementBlock : Statement
{
    public List<Statement> Statements{get;}
    Scope Scope;
    public StatementBlock(List<Statement> statements, CodeLocation location) : base(location)
    {
        Statements = statements;
    }

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        Scope = scope;
        Debug.Log(scope.Contains("target"));
        foreach (var statement in Statements)
        {
            if(!statement.CheckSemantic(context,Scope,errors)) 
            {
                return false;
            }
        }
        return true;
    }
    public override void Execute()
    {
        foreach (var statement in Statements)
        {
            statement.Execute();
        }
    }
    public override string ToString()
    {
        string a = "";
        foreach(var statement in Statements)
        {
            a = "\n\t" + statement.ToString();
        }
        return String.Format(a);
    }
}