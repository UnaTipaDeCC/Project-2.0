using System.Collections.Generic;  
public class Grouping : Expression
{
    public override ExpressionType Type
    {
        get => expression.Type;
        set { }
    }
    public override object? Value {get; set;}
    public Expression expression{get; set;}
    public CodeLocation location{ get; set;}
    public Grouping(Expression expression, CodeLocation location) : base(location)
    {
        this.expression = expression;
        this.location = expression.Location; 
    }
    public override void Evaluate()
    {
        expression.Evaluate();
        Value = expression.Value;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    { return expression.CheckSemantic(context, scope, errors); }
}