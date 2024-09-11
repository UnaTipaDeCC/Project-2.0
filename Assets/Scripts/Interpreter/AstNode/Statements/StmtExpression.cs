using System.Collections.Generic;

public class StmtExpression : Statement
{
    //maneja el caso en el que se llamen a metodos
    Expression expression;
    public Scope Scope{ get; set; }
    public StmtExpression(Expression expression, CodeLocation location) : base(location)
    {
        this.expression = expression;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        Scope= scope;
        bool checkExp = expression.CheckSemantic(context, scope, errors);
        if(!(expression is Method))
        {
            errors.Add(new CompilingError(expression.Location,ErrorCode.Invalid,"should be a method"));
            return false;
        }
        return checkExp;
    }
    public override void Execute()
    {
        expression.Evaluate();
    }
    public override string ToString()
    {
        return expression.ToString();
    }
}