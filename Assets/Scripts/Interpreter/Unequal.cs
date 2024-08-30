
public class Unequal : BinaryExpression
{
    Expression? Right{get; set;}
    Expression? Left{get; set;}
    Token Operator{get; set;}
    //CodeLocation location{get; set;}
    public Unequal(Expression? Left,  Token Operator, Expression? Right,CodeLocation location ) : base(location)
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
        if((Right.Type == ExpressionType.Number && Left.Type == ExpressionType.Number) || (Right.Type == ExpressionType.Text && Left.Type == ExpressionType.Text))
        {
           Type = ExpressionType.Bool;
            return right && left;
        }
        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "No son del mismo tipo"));
        Type = ExpressionType.ErrorType;
        return false;
    }
    public override ExpressionType Type {get; set;}
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
            return String.Format("({0} != {1})", Left, Right);
        }
        return Value.ToString();
    }
}