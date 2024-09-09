using System.Collections.Generic;
public class Or: BinaryExpression
{
    Expression? Right;
    Expression? Left;
    public Or(Expression? right, Expression? left, CodeLocation location) :base(location)
    {
        this.Right = right;
        this.Left = left;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool right = Right.CheckSemantic(context,scope,errors);
        bool left = Left.CheckSemantic(context,scope,errors);
        if (Right.Type != ExpressionType.Bool || Left.Type != ExpressionType.Bool)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Should be boolean expressions"));
            Type = ExpressionType.ErrorType;
            return false;
        }

        Type = ExpressionType.Bool;
        return right && left;
    }
    public override void Evaluate()
    {
        Right.Evaluate();
        Left.Evaluate();
        Value =  (bool)Left.Value || (bool)Right.Value;
    }
   public override ExpressionType Type
    {
        get
        {
            return ExpressionType.Bool;
        }
        set {}
    }
    public override object? Value { get; set; }
    public override string ToString()
    {
        if (Value == null)
        {
            return $"({Left} || {Right})";
        }
        return Value.ToString();
    }
}