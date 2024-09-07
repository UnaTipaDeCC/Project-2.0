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
        //Scope = scope.CreateChild();
        //se chequea que la expresion sea una variable
        if(!(Variable is Variable))
        {
            errors.Add(new CompilingError(Variable.Location,ErrorCode.Invalid, $"The expression {Variable} must be a variable.")); 
            Type = ExpressionType.ErrorType;
            return false;
        }
        //se chequea que la variable no haya sido previamente definida
        Variable var = (Variable)Variable;
        if(scope.Contains(var.Name))
        {
            errors.Add(new CompilingError(Location,ErrorCode.Invalid,$"The variable {var} already exist in this context"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        else scope.SetType(var.Name,ExpressionType.Card);
        //se chequea la condicion
        bool checkCondition = Condition.CheckSemantic(context, scope, errors);
        if(Condition.Type != ExpressionType.Bool)
        {
            errors.Add(new CompilingError(Condition.Location,ErrorCode.Invalid, $"The expression {Condition} must be a boolean expression"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        Type = ExpressionType.Bool;
        return checkCondition; //&& checkVariable;
    }
    public override void Evaluate()
    {
       // Variable.Evaluate();
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