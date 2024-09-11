using System.Collections.Generic;
using UnityEngine;
public class Equal : BinaryExpression
{
    Expression? Right{get; set;}
    Expression? Left{get; set;}
    Token Operator{get; set;}
    //CodeLocation location{get; set;}
    public Equal(Expression? Left,  Token Operator, Expression? Right,CodeLocation location ) : base(location)
    {
        this.Left = Left;
        this.Right = Right;
        this.Operator = Operator;
        location = Operator.Location;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool right = Right.CheckSemantic(context, scope, errors);
        bool left = Left.CheckSemantic(context,scope, errors);
        //comprobar que sean del mismo tipo
        if((Right.Type == ExpressionType.Number && Left.Type == ExpressionType.Number) || (Right.Type == ExpressionType.Text && Left.Type == ExpressionType.Text) || (Right.Type == ExpressionType.Bool && Left.Type == ExpressionType.Bool))
        {
            Type = ExpressionType.Bool;
            return right && left;
        }
        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Must be the same type"));
        Type = ExpressionType.ErrorType;
        return false;
    }
    public override ExpressionType Type {get => ExpressionType.Bool; set{}}
    public override object? Value {get; set;}
    public override void Evaluate()
    {
        Right.Evaluate();
        Left.Evaluate();

        if(Right.Type == ExpressionType.Number && Left.Type == ExpressionType.Number)
        {
            Value = (double)Left.Value == (double)Right.Value;
        }
        else if(Right.Type == ExpressionType.Text && Left.Type == ExpressionType.Text)
        {
            Value = (string)Left.Value == (string)Right.Value;
        }
        else if(Left.Type == ExpressionType.Bool && Right.Type == ExpressionType.Bool)
        {
            Value = (bool)Right.Value == (bool)Left.Value;
        }
    }
    public override string ToString()
    {
        if (Value == null)
        {
                return $"({Left} == {Right})";
        }
        return Value.ToString();
    }
}
