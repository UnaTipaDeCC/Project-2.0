//using System.Reflection.Metadata.Ecma335;
using System.Collections.Generic;
public class Concatenation: BinaryExpression
{
    public override ExpressionType Type {get; set;}
    public override object? Value {get; set;}
    Expression? Right{get; set;}
    Expression? Left{get; set;}
    Token Operator{get; set;}
    //CodeLocation location{get; set;}
    public Concatenation(Expression? left,Token Operator,Expression? right,CodeLocation location) : base(location)
    {
        this.Left = left;
        this.Right = right;
        this.Operator = Operator;
        location = Operator.Location;
    }

    public override void Evaluate()
    {
        Right.Evaluate();
        Left.Evaluate();
        if(Operator.Value == TokenValues.ConcatenationWithoutSpace) Value = (string)Left.Value + (string)Right.Value;
        else Value = (string)Left.Value + " " +  (string)Right.Value;
    }

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool right = Right.CheckSemantic(context, scope, errors);
        bool left = Left.CheckSemantic(context, scope, errors);
        if (Right.Type != ExpressionType.Text || Left.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "No se pueden concatenar tipos que no sean strings"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        Type = ExpressionType.Text;
        return right && left;
    }

    public override string ToString()
    {
        if (Value == null)
        {
            if(Operator.Value == TokenValues.ConcatenationWithoutSpace)
            return $"({Left} @ {Right})";
            else return $"({Left} @@ {Right})";
        }
        return Value.ToString();
    }
}