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
        //verificar en el context que sea un metodo valido y que la expresion que lo llama sea de un tipo valido
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
        //en caso de que tenga un parametro el metodo, chequearlo semanticamente y verificar que sea un parametro valido para este metodo
        if(argument != null)
        {
            checkArgument = argument.CheckSemantic(context, scope, errors);
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
        //acceder al context y asociar el tipo
        Type = context.GetType(name.Value);
        return checkArgument && checkCaller;
    }
    public override void Evaluate()
    {
        caller.Evaluate();
        //se evalua el parametro de existir a menos que sea un find, en ese caso se evalua segun sea necesario
        if(argument != null && name.Value != "Find") argument.Evaluate();
        
        //determinar de que tipo es la expresion que llama al metodo y en funcion de eso, asignar el valor o llamar al metodo correspondiente
        if(caller.Value is GameContext)
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
                List<CardGame> cardGames = (List<CardGame>)caller.Value;
                Debug.Log("antes de entrar al foreach el count de la lista es: " + cardGames.Count);
                //Filtrar las cartas segun el valor del predicate
                List<CardGame> filteredCards = new List<CardGame>();
                Predicate predicate = (Predicate)argument;
                Variable var = (Variable)predicate.Variable;
                foreach(CardGame card in (List<CardGame>)caller.Value)
                {
                    Debug.Log("la variable es: " + var.Name);
                    //actualizar el valor de la variable del predicate
                    Scope.Set(var.Name, card);
                    
                    CardGame c = (CardGame)Scope.Get(var.Name);
                    Debug.Log(c.Name);
                    //evaluar el predicate y en funcion de eso se agrega a la lista
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
    }
    public override ExpressionType Type { get; set;}
    public override object? Value { get; set;}
    public override string ToString()
    {
        if(argument is null) return $"{caller.Value}.{name.Value}()";
        else return $"{caller.Value}.{name.Value}({argument.Value})";
    }
}