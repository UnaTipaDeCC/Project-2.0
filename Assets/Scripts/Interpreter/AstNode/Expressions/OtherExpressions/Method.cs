//using System.Reflection.Metadata;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
class Method : Expression
{
    Expression caller;
    Expression? argument;
    Token name;
    CodeLocation location;
    Scope Scope;
    public Method(Token name, Expression caller, CodeLocation location, Expression argument = null) : base(location)
    {
        this.caller = caller;
        this.argument = argument;
        this.name = name;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool checkArgument = true;
        bool checkCaller = caller.CheckSemantic(context, scope, errors);
        
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
        if(argument != null)
        {
            checkArgument = argument.CheckSemantic(context, scope, errors);
            Debug.Log(argument.Type);
            if(!context.ParamsOfMethodsTypes.ContainsKey(name.Value))
            {
                Debug.Log("EN EL PRIMERO");
                errors.Add(new CompilingError(location,ErrorCode.Invalid,"The method " + name.Value + "doesnt have a param"));
                Type = ExpressionType.ErrorType;
                return false;
            }
            if(argument.Type != context.ParamsOfMethodsTypes[name.Value])
            {
                Debug.Log("EN EL SEGUNDO");
                errors.Add(new CompilingError(location,ErrorCode.Invalid,"The type of the param of this method is invalid"));
                Type = ExpressionType.ErrorType;
                return false;
            }
        }
        Debug.Log(checkCaller);
        Debug.Log(checkArgument);
        Debug.Log("abajo");
        Debug.Log(checkArgument && checkCaller);
        Type = context.GetType(name.Value);
        return checkArgument && checkCaller;
    }
    public override void Evaluate()
    {
        caller.Evaluate();
        if(argument != null) argument.Evaluate();
        if(caller.Value is GameContext)//no se si me pueden poner eso asi
        {
            Player player = GameContext.Instance.ReturnPlayer((int)argument.Value);
            switch (name.Value)
            {
                case "HandOfPlayer":
                Value = GameContext.Instance.HandOfPlayer(player);
                break;
                case "FieldOfPlayer":
                Value = GameContext.Instance.FieldOfPlayer(player);
                break;
                case "DeckOfPlayer":
                Value = GameContext.Instance.DeckOfPlayer(player);
                break;
                case "GraveryardOfPlayer":
                Value = GameContext.Instance.GraveryardOfPlayer(player);
                break;

            }
        }
        else if(caller.Value is List<CardGame> list) 
        {
            switch(name.Value)
            {
                case "Push":
                GameContext.Instance.Push((CardGame)argument.Value,list);
                break;
                case "SendBottom":
                GameContext.Instance.SendBottom((CardGame)argument.Value,list);
                break;
                case "Pop":
                Value = GameContext.Instance.Pop(list);
                break;
                case "Suffle":
                GameContext.Instance.Shuffle(list); 
                break;
                case "Find":
                //Filtrar las cartas segun el valor del predicate
                List<CardGame> filteredCards = new List<CardGame>();
                Predicate predicate = (Predicate)argument;
                Variable var = (Variable)predicate.Variable;
                foreach(CardGame card in (List<CardGame>)caller.Value)
                {
                    Scope.Set(var.Name, card);
                    predicate.Evaluate();
                    Debug.Log("valor del predicate " + predicate.Value);
                    if((bool)predicate.Value)
                    {
                        filteredCards.Add(card);   
                    }
                }
                Debug.Log(filteredCards.Count);
                Value = filteredCards;
                break;
                case "Remove":
                GameContext.Instance.Remove(list,(CardGame)argument.Value);
                break;
                
            }
           
        }
    }
    public override ExpressionType Type { get; set;}
    public override object? Value { get; set;}
    public override string ToString()
    {
        return $"{caller.Value}.{name.Value}({argument.Value})";
    }
}