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
        this.Variable = variable;
        this.Condition = condition;
        this.Location = location;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        Scope = scope.CreateChild();
        if(!(Variable is Variable))
        {
            errors.Add(new CompilingError(Variable.Location,ErrorCode.Invalid, $"The expression {Variable} must be a variable.")); 
            Type = ExpressionType.ErrorType;
            return false;
        }
        if(!Scope.types.ContainsKey("unit")) Scope.types.Add("unit",ExpressionType.Card);
        else//revisar si deberia hacer esto, si pincha
        {
            errors.Add(new CompilingError(Location,ErrorCode.Invalid,$"The variable {Variable} already exist in this context"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        bool checkVariable = Variable.CheckSemantic(context, Scope,errors);
        bool checkCondition = Condition.CheckSemantic(context, Scope, errors);
        /*if(Condition.ExpressionType != ExpressionType.Bool)//revisar esto
        {
            errors.Add(new CompilingError(Condition.Location,ErrorCode.Invalid, $"The expression {Condition} must be a boolean expression"));
            Type = ExpressionType.ErrorType;
            return false;
        }*/
        Type = ExpressionType.Bool;
        return checkCondition && checkVariable;
    }
    public override void Evaluate()
    {
        Variable.Evaluate();
        Condition.Evaluate();
        Value = Condition.Value;
    }
    public override ExpressionType Type { get; set; }
    public override object? Value { get; set; }
    public override string ToString()
    {
        return String.Format("({0}) => {1}",Variable,Condition);
    }
}