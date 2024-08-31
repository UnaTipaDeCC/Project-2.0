using System.Collections.Generic;
using System;
public class Predicate : Expression
{
    Expression variable;
    Expression condition;
    CodeLocation location;
    Scope scope;
    public Predicate(Expression variable, Expression condition,CodeLocation location) : base(location)
    {
        this.variable = variable;
        this.condition = condition;
        this.location = location;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.scope = scope;
        this.scope.types.Add("unit",ExpressionType.Card);
        bool checkVariable = variable.CheckSemantic(context, this.scope,errors);
        bool checkCondition = condition.CheckSemantic(context, scope, errors);
        return checkCondition && checkVariable;
    }
    public override void Evaluate()
    {
        condition.Evaluate();
        //this.scope.Set("unit",null);
    }
    public override ExpressionType Type { get; set; }
    public override object? Value { get; set; }
    public override string ToString()
    {
        return String.Format("({0}) => {1}",variable.Value,condition);
    }
}