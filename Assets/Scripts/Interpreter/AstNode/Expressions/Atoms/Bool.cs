using System.Collections.Generic;
public class Bool : AtomExpression
{
    public override ExpressionType Type { get {return ExpressionType.Bool;} set { } }

    public override void Evaluate()
    {
        
    }
    public override object? Value { get; set; }
    
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        return true;
    }
    public Bool(bool value, CodeLocation location) : base(location)
    {
        Value = value;
    }
     public override string ToString()
    {
        return $"{Value}";
    }
}