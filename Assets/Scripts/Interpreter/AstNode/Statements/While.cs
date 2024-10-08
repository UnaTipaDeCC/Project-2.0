using System.Collections.Generic;
using System;
using Unity.VisualScripting;
public class While : Statement
{
    Expression condition;
    Statement body;
    CodeLocation codeLocation;
    Scope scope;
    public While(Expression condition, Statement body, CodeLocation codeLocation) : base(codeLocation)
    {
        this.condition = condition;
        this.body = body;
        this.codeLocation = codeLocation;
    }

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.scope = scope.CreateChild();
        //chequear que la condicion del while sea una expresion booleana
        bool checkCondition = condition.CheckSemantic(context,scope,errors);
        if(condition.Type != ExpressionType.Bool)
        {
            errors.Add(new CompilingError(condition.Location,ErrorCode.Invalid, "In a while statement, the expression within the parentheses must be a bool."));
            return false;
        }
        bool checkBody = body.CheckSemantic(context,this.scope,errors);
        return checkCondition && checkBody;
    }
    public override void Execute()
    {
        condition.Evaluate();
        while((bool)condition.Value)
        {  
            body.Execute();
            condition.Evaluate();
        }
    }
    public override string ToString()
    {
        return String.Format("While({0}) {{1}}",condition,body);
    }
}