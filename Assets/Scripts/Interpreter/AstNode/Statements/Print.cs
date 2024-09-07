using System.Collections.Generic;
using System;
public class Print : Statement
{
    public Expression Value { get; private set; }
    public Scope PrintScope;


    public Print(Expression value, CodeLocation location) : base(location)
    {
        Value = value;
    }
    public override void Execute()
    {
        Value.Evaluate();
        //Console.WriteLine(Value.Value);
        //Console.WriteLine(Value.Value);
    }

    public override bool CheckSemantic(Context context,Scope scope, List<CompilingError> errors)
    {
        PrintScope = scope;
        bool a = Value.CheckSemantic(context,PrintScope,errors);
        return a;
    }
    public override string ToString()
    {
        return String.Format("print : {0}",Value);
    }
}