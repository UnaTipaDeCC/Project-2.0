using System.Collections.Generic;
public class Indexer : Expression
{
    Expression exp;
    Expression index;
    CodeLocation location;
    public Indexer(Expression expression, Expression index, CodeLocation location) : base(location)
    {
        this.exp = expression;
        this.index = index;
        this.location = location;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool checkExp = exp.CheckSemantic(context,scope,errors);
        bool checkIndex = index.CheckSemantic(context,scope,errors);
        if(exp.Type != ExpressionType.List )
        {
            errors.Add(new CompilingError(exp.Location,ErrorCode.Invalid, "Can't index something that is not a list"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        if(index.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(exp.Location,ErrorCode.Invalid, "Index must be a number"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        return checkExp && checkIndex;
    }
    public override void Evaluate()
    {
        index.Evaluate();
        exp.Evaluate();
       // Value = ((List<T>)exp.Value)[(int)index.Value];;
    }
    public override ExpressionType Type { get; set; }
    public override object? Value { get; set; }
}