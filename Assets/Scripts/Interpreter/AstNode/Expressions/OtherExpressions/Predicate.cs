using System.Collections.Generic;
using System;
public class Predicate : Expression
{
    public Expression Variable {get; private set;}
    public Expression Condition{get; private set;}
    public CodeLocation Location{get; private set;}
    public Scope Scope{get; private set;}
    public Predicate(Expression variable, Expression condition,CodeLocation location) : base(location)
    {
        this.variable = variable;
        this.condition = condition;
        this.location = location;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        Scope = scope.CreateChild();
        if(!(Variable is Variable))
        {
            errors.Add(new CompilingError(Variable.Location,ErrorCode.Invalid, $"The expression {variable} must be a variable.")); 
            return false;
        }
        if(!Scope.types.ContainsKey("unit")) Scope.types.Add("unit",ExpressionType.Card);
        else//revisar si deberia hacer esto, si pincha
        {
            errors.Add(new CompilingError(Location,ErrorCode.Invalid,$"The variable {variable} already exist in this context"));
            return false;
        }
        bool checkVariable = variable.CheckSemantic(context, this.scope,errors);
        bool checkCondition = condition.CheckSemantic(context, scope, errors);
        if(condition.ExpressionType == ExpressionType.Bool)
        {
            errors.Add(new CompilingError(condition.Location,ErrorCode.Invalid, $"The expression {condition} must be a boolean expression"));
            return false;
        }
        return checkCondition && checkVariable;
    }
    public override void Evaluate()
    {
        variable.Evaluate();
        condition.Evaluate();
        Value = condition.Value;
    }
    public override ExpressionType Type { get; set; }
    public override object? Value { get; set; }
    public override string ToString()
    {
        return String.Format("({0}) => {1}",variable,condition);
    }
}