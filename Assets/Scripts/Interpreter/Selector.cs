public class Selector : Statement
{
    public Expression Source { get; private set; }
    public Expression Single { get; private set; }
    public Expression Predicate { get; private set; }
    public Selector? Parent { get; private set; }
    public CodeLocation Location;
    public Scope SelectorScope{ get; private set;}

    public Selector(Expression source, Expression single, Expression predicate, CodeLocation location, Selector parent = null) : base(location)
    {
        this.Source = source;
        this.Single = single;
        this.Predicate = predicate;
        this.Location = location;
        this.Parent = parent;
    }

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.SelectorScope = scope;
        bool checkSource = Source.CheckSemantic(context, scope, errors);
        bool checkSingle = Single.CheckSemantic(context, scope, errors);
        bool checkPredicate = Predicate.CheckSemantic(context,scope,errors);
        if(Single.Type != ExpressionType.Bool)
        {
            errors.Add(new CompilingError(Single.Location,ErrorCode.Invalid,"The 'Single' must be boolean expression"));
            return false;
        }
        if(Source.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Single.Location,ErrorCode.Invalid,"The 'Source' must be boolean expression"));
            return false;
        }
        Source.Evaluate();
        if(!context.ContainsSource((string)Source.Value))
        {
            errors.Add(new CompilingError(Single.Location,ErrorCode.Invalid,"The 'Source' isnt a possible expression"));
            return false;
        }
        return checkSource && checkSingle && checkPredicate; 
    }

    public override void Execute()
    {
        Source.Evaluate();
        Predicate.Evaluate();
        Single.Evaluate();
    }
}