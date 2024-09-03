using System.Collections.Generic;
class For : Statement
{
    Expression itemExpression;
    Expression collectionExpression;
    Token item;
    Token collection;
    Scope forScope;
    Statement body;
    CodeLocation location;
    public For(Token item, Token collection, Statement body, CodeLocation location) : base(location)
    {
        this.item = item;
        this.collection = collection;
        this.body = body;
        this.location = location;
        itemExpression = new Variable(item.Value,item.Location);
        collectionExpression = new Variable(collection.Value,collection.Location);
    }
    public override void Execute()
    {
        List<CardGame> list = (List<CardGame>)forScope.Get(collection.Value);
        foreach(CardGame card in list)
        {
            forScope.Set(item.Value,card);
            //itemExpression.Evaluate();
            body.Execute();
        }
        
    }
    public override bool CheckSemantic(Context context,Scope scope, List<CompilingError> errors)
    {
        //collection.Evaluate();
        forScope = scope.CreateChild();
        if(forScope.Contains(item.Value))
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, "The variable " + item.Value + "already exist"));
            return false;
        }
        else forScope.types.Add(item.Value,ExpressionType.Card); 
        if(!forScope.Contains(collection.Value))
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, "The collection " + item.Value + "must be already declared"));
            return false;
        }
        else if(forScope.GetType(collection.Value) != ExpressionType.List)
        {
            errors.Add(new CompilingError(location,ErrorCode.Invalid, $"The collection '{collection.Value}' must be a list"));
            return false;
        }
        bool checkBody = body.CheckSemantic(context, scope,errors);
        return checkBody;
    }
}