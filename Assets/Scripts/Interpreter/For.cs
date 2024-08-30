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
        forScope.variables.Add(item.Value,null); 
        forScope.variables.Add(collection.Value,null);
        body.Execute();
        /*for(int i = 0; i < collection.Count;i++)
        {
            forScope.Set(item,collection[i]);
            itemExpression.Evaluate();
            body.Execute();
        }*/
        
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
        if(forScope.Contains(collection.Value))
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, "The variable " + item.Value + "already exist"));
            return false;
        }
        else forScope.types.Add(collection.Value,ExpressionType.List); 

        //bool checkColection = collection.CheckSemantic(context, scope, errors);

        bool checkBody = body.CheckSemantic(context, scope,errors);
        forScope.types.Add(collection.ToString(),ExpressionType.List);//en realidad deberia valorar anteriormente que ese nombre sea uno ya definido anteriormente como lista
        /*if(collection.Type != ExpressionType.List)
        {
            errors.Add(new CompilingError(collection.Location, ErrorCode.Invalid, "The " + collection.Value +  "must be a collection"));
            return false;
        }*/
        return checkBody;
    }
}