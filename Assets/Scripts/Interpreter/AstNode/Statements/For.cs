using System.Collections.Generic;
//using System.Diagnostics; 
using UnityEngine;
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
        forScope = scope.CreateChild();
        //se chequea que el item no haya sido declarado previamente
        if(forScope.Contains(item.Value))
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, "The variable " + item.Value + "already exist"));
            return false;
        }
        //se define en el scope
        else 
        {
            forScope.SetType(item.Value,ExpressionType.Card);
            Debug.Log(forScope.GetType(item.Value));
            Debug.Log("en el for el item es " + item.Value);
        }
        
        //se chequea que la lista este definida 
        if(!forScope.Contains(collection.Value))
        {
            errors.Add(new CompilingError(location, ErrorCode.Invalid, "The collection " + item.Value + " must be already declared"));
            return false;
        }
        /*else if(forScope.GetType(collection.Value) != ExpressionType.List)
        {
            errors.Add(new CompilingError(location,ErrorCode.Invalid, $"The collection '{collection.Value}' must be a list"));
            return false;
        }*/
        //se chequea el cuerpo de for semanticamente
        bool checkBody = body.CheckSemantic(context, forScope,errors);
        Debug.Log(checkBody + "en el for");
        return checkBody;
    }
}