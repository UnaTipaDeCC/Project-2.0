class VariableModifier : Expression // quiza no sea necesario
{
    Token operatorToken;
    Expression variable;
    public CodeLocation location;
    public VariableModifier(Expression expression, Token operatorToken, CodeLocation location): base(location)
    {
        this.operatorToken = operatorToken;
        this.variable = expression;
        this.location = location;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        if(!(variable is Variable v))
        {
            errors.Add(new CompilingError(location,ErrorCode.Invalid, "The expression before the '" +operatorToken.Value + "' must be a variable"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        bool checkVariable = variable.CheckSemantic(context, scope, errors);
        if(variable.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(location,ErrorCode.Invalid, "The variable must be a number for this operation"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        if(checkVariable) 
        {
            Type = ExpressionType.Number;
            return true;
        }
        else
        {
            Type = ExpressionType.ErrorType;
            return false;
        }
    }
    public override void Evaluate()
    {
        variable.Evaluate();
        if(operatorToken.Value == "++") Value = (double)variable.Value + 1;
        else Value = (double)variable.Value - 1;
    }
    public override ExpressionType Type { get; set ; }
    public override object? Value { get; set; }
    public override string ToString()
    {
        if(Value == null) return String.Format("{0} {1}",variable.Value,operatorToken.Value);
        else return Value.ToString();
    }
}