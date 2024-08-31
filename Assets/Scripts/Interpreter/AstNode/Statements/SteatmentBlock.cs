using System.Collections.Generic;
using System;

public class StatementBlock : Statement
{
    List<Statement> statements;
    Scope Scope;
    public StatementBlock(List<Statement> statements, CodeLocation location) : base(location)
    {
        this.statements = statements;
    }

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {

        foreach (var statement in statements)
        {
            Console.WriteLine("hola");
            if(!statement.CheckSemantic(context,scope,errors)) 
            {
                Console.WriteLine("algo fue mal en la evaluccion del statement: " + statement);
                return false;
            }
        }
        return true;
    }
    public override void Execute()
    {
        Console.WriteLine("ejecutanding");
        foreach (var statement in statements)
        {
            //Console.WriteLine("hola estoy examinando: " + statement);
            statement.Execute();
        }
    }
    public override string ToString()
    {
        string a = "";
        foreach(var statement in statements)
        {
            a = "\n\t" + statement.ToString();
        }
        return String.Format(a);
    }
}