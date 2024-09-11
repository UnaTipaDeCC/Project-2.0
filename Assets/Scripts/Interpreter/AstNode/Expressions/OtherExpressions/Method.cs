//using System.Reflection.Metadata;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Unity.VisualScripting;
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
        Scope = scope;
        bool checkArgument = true;
        bool checkCaller = caller.CheckSemantic(context, scope, errors);
        Debug.Log(name.Value);
        if(!context.Contains(name.Value))
        {
            errors.Add(new CompilingError(location,ErrorCode.Invalid,"The method " + name.Value + " is not valid"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        if(caller.Type != context.GetCallerType(name.Value))
        {
            errors.Add(new CompilingError(location,ErrorCode.Invalid,"The method " + name.Value + " cant be called by an expression of that type"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        if(argument != null)
        {
            checkArgument = argument.CheckSemantic(context, scope, errors);
            Debug.Log(argument.Type);
            if(!context.ParamsOfMethodsTypes.ContainsKey(name.Value))
            {
                errors.Add(new CompilingError(location,ErrorCode.Invalid,"The method " + name.Value + "doesnt have a param"));
                Type = ExpressionType.ErrorType;
                return false;
            }
            if(argument.Type != context.ParamsOfMethodsTypes[name.Value])
            {
                errors.Add(new CompilingError(location,ErrorCode.Invalid,"The type of the param of this method is invalid"));
                Type = ExpressionType.ErrorType;
                return false;
            }
        }
        Type = context.GetType(name.Value);
        return checkArgument && checkCaller;
    }
    public override void Evaluate()
    {
        caller.Evaluate();
        if(argument != null && name.Value != "Find") argument.Evaluate();
        
        if(caller.Value is GameContext)
        {
            
            Debug.Log(argument.Value);
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
            Debug.Log("es una lista lo que llama al metodo +  " + name.Value);
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
                case "Shuffle":
                GameContext.Instance.Shuffle(list); 
                break;
                case "Find":
                Debug.Log("era un find");
                List<CardGame> cardGames = (List<CardGame>)caller.Value;
                Debug.Log("antes de entrar al foreach el count de la lista es: " + cardGames.Count);
                //Filtrar las cartas segun el valor del predicate
                List<CardGame> filteredCards = new List<CardGame>();
                Predicate predicate = (Predicate)argument;
                Variable var = (Variable)predicate.Variable;
                foreach(CardGame card in (List<CardGame>)caller.Value)
                {
                    Debug.Log("la variable es: " + var.Name);
                    Scope.Set(var.Name, card);
                    CardGame c = (CardGame)Scope.Get(var.Name);
                    Debug.Log(c.Name);
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
                GameContext.Instance.RemoveCard(list,(CardGame)argument.Value);
                break;
                
            }
           
        }
        Debug.Log("el resultado de " + name.Value + " es " + Value);
    }
    public override ExpressionType Type { get; set;}
    public override object? Value { get; set;}
    public override string ToString()
    {
        return $"{caller.Value}.{name.Value}({argument.Value})";
    }
}