using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using System;
public class Unary : Expression
{
    public override ExpressionType Type {get; set;}
    public override object? Value {get; set;}
    public Token Operator;
    public Expression Right;
    public Scope Scope;
    public CodeLocation Location;
    public Unary(Token Operator, Expression right, CodeLocation location) : base(location)
    {
        this.Right = right;
        this.Operator = Operator;
        this.Location = Operator.Location;
    }
    public override void Evaluate()
    {
        if(Operator.Value == TokenValues.Sub || Operator.Value == TokenValues.Negation)
        {
            Right.Evaluate();
            Value = Operator.Value == TokenValues.Sub ? -(double)Right.Value : !(bool)Right.Value;
        }
        else
        {
            UpdateVariable();
        }
    }
    private void UpdateVariable()
    {
        Variable var = (Variable)Right;
        double currentValue = (double)Scope.Get(var.Name);
        switch (Operator.Value)
        {
            case TokenValues.Decrement:
                Scope.Set(var.Name, currentValue - 1);
                break;
            case TokenValues.Increment:
                Scope.Set(var.Name, currentValue + 1);
                break;
            default:
                throw new InvalidOperationException("Unsupported operator for variable update.");
        }
        Value = Scope.Get(var.Name);
        Debug.Log($"The value of variable {var.Name} after increment/decrement is {Value}");
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        Scope = scope;
        if(Operator.Value == TokenValues.Increment ||Operator.Value == TokenValues.Decrement)
        {
            //si es una variable fue previamente definida
            if(!(Right is Variable))
            {
                errors.Add(new CompilingError(Location,ErrorCode.Invalid, $"The expression before the '{Operator.Value}' must be a variable"));
                Type = ExpressionType.ErrorType;
                return false;
            }
            else
            {
                Variable var = (Variable)Right;
                if(scope.GetType(var.Name) != ExpressionType.Number)
                {
                    errors.Add(new CompilingError(Location,ErrorCode.Invalid, $"The expression before the '{Operator.Value} ' must be a number variable"));
                    Type = ExpressionType.ErrorType;
                    return false;
                }
                else Type = ExpressionType.Number;
            }

            
        }
        bool right = Right.CheckSemantic(context, scope,errors);
        if(Operator.Value == TokenValues.Negation)
        {
            if(Right.Type != ExpressionType.Bool)
            {
                errors.Add(new CompilingError(Location,ErrorCode.Invalid,"The expression must be a boolean")); 
                Type = ExpressionType.ErrorType; 
                return false;
            }
            else Type = ExpressionType.Bool;
        }
        else 
        {    
            if(Right.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(Location,ErrorCode.Invalid,"The expression must be a number")); 
                Type = ExpressionType.ErrorType; 
                return false;
            } 
            else Type = ExpressionType.Number;
        }
        return right;
    }
}