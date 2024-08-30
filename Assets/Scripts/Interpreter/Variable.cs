public class Variable : AtomExpression
{
    public string Name;
    Scope VariableScope{get ;set;}
    public CodeLocation Location;
    public Variable(string name, CodeLocation location) : base(location)
    {
        this.Name = name;
        //this.scope = scope;
        this.Location = location;
    }
    public override string ToString()
    {
        return String.Format(Name);
    }
    public override object? Value { get; set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        VariableScope = scope;
        if(VariableScope.GetType(Name) == ExpressionType.ErrorType)
        {
            Console.WriteLine(Name);
            errors.Add(new CompilingError(Location,ErrorCode.Invalid,"la variable: " + Name + " no esta definida (estoy en el check de la variable)"));
            Type = ExpressionType.Identifier;
            return false;
        }
        else
        {
            Type = scope.GetType(Name);
            Console.WriteLine("en la variable despues de ponerle un tipo, todo ok: + " + Name);
            return true;
        }
        
    }
    public override void Evaluate()
    {
        this.Value = this.VariableScope.Get(Name);
    } 
    public override ExpressionType Type { get; set; }


}