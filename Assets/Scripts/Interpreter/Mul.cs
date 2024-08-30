public class Mul : BinaryExpression
{
    Expression? Right{get; set;}
    Expression? Left{get; set;}
    Token Operator{get; set;}
    public override ExpressionType Type {get; set;}
    public override object? Value {get; set;}

    public Mul(Expression? left,Token Operator,Expression? right,CodeLocation location) : base(location)
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
        
        Value = (double)Right.Value * (double)Left.Value;
    }

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool right = Right.CheckSemantic(context, scope, errors);
        bool left = Left.CheckSemantic(context, scope, errors);
        if (Right.Type != ExpressionType.Number || Left.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "No son dos numeros"));
            Type = ExpressionType.ErrorType;
            return false;
        }

        Type = ExpressionType.Number;
        return right && left;
    }

    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("({0} * {1})", Left, Right);
        }
        return Value.ToString();
    }
}