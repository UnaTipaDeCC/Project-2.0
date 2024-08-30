public class Unary : Expression
{
    public override ExpressionType Type {get; set;}
    public override object? Value {get; set;}
    public Token Operator;
    public Expression Right;
    public CodeLocation Location;
    public Unary(Token Operator, Expression right, CodeLocation location) : base(location)
    {
        this.Right = right;
        this.Operator = Operator;
        this.Location = Operator.Location;
    }
    public override void Evaluate()
    {
        Right.Evaluate() ;
        switch(Operator.Value)
        {
            case TokenValues.Sub:
            Value = -(double)Right.Value;
            break;
            case TokenValues.Decrement:
            Value = (double)Right.Value -1;
            break;
            case TokenValues.Increment:
            Value = (double)Right.Value + 1;
            break;
            case TokenValues.Negation:
            Value = Value = !(bool)Right.Value;
            break;
        }
        /*if(Operator.Value == TokenValues.Sub) Value = -(double)Right.Value;
        else if (Operator.Value == TokenValues.Negation) Value = !(bool)Right.Value; */
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        if((Operator.Value == TokenValues.Increment ||Operator.Value == TokenValues.Decrement) && !(Right is Variable))
        {
            errors.Add(new CompilingError(Location,ErrorCode.Invalid, "The expression before the '" + Operator.Value + "' must be a variable"));
            Type = ExpressionType.ErrorType;
            return false;
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