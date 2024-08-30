//using System.Reflection.Metadata;
using System.Collections.Generic;
class Method : Expression
{
    Expression caller;
    Expression? argument;
    Token name;
    CodeLocation location;
    public Method(Token name, Expression caller, CodeLocation location, Expression argument = null) : base(location)
    {
        this.caller = caller;
        this.argument = argument;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool checkArgument = true;
        bool checkCaller = CheckSemantic(context, scope, errors);
        if(argument != null)
        {
            checkArgument = argument.CheckSemantic(context, scope, errors);
        }
        if(!context.Contains(name.Value))
        {
            errors.Add(new CompilingError(location,ErrorCode.Invalid,"The method " + name.Value + "is not valid"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        if(caller.Type != context.GetCallerType(name.Value))
        {
            errors.Add(new CompilingError(location,ErrorCode.Invalid,"The method " + name.Value + "cant be called by an expression of that type"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        Type = context.GetType(name.Value);
        return checkArgument && checkCaller;
    }
    public override void Evaluate()
    {
        caller.Evaluate();
        //Console.WriteLine("evalue el metodo: " + name.Value);
    }
    public override ExpressionType Type { get; set;}
    public override object? Value { get; set;}
    public override string ToString()
    {
        return $"{caller.Value}.{name.Value}({argument.Value})";
    }
}