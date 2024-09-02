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
        if(!(variable is Variable))
        {
            errors.Add(new CompilingError(variable.Location,ErrorCode.Invalid, $"The expression {variable} must be a variable.")); 
            return false;
        }
        if(scope.GetType(variable.ToString()) != ExpressionType.ErrorType)//eso quiere decir que ya esta declarada
        {
            errors.Add(new CompilingError(variable.Location,ErrorCode.Invalid, $"The variable {variable} already exist.")); 
            return false;
        }
        this.scope.types.Add("unit",ExpressionType.Card);
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