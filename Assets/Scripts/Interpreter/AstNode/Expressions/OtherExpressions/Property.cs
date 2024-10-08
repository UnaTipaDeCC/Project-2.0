using System.Collections.Generic;
using System;
//using System.Diagnostics;
using UnityEngine;
class Property : Expression
{
    public Expression Caller{get; private set;}
    public Token Name{get; private set;}
    CodeLocation location;
    public Property(Token name, Expression caller, CodeLocation location) : base(location)
    {
        this.Caller = caller;
        this.Name = name;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool checkCaller = Caller.CheckSemantic(context, scope,errors);
        if(!context.Contains(Name.Value))
        {
            errors.Add(new CompilingError(location,ErrorCode.Invalid,"The property " + Name.Value + " is not valid"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        if(Caller.Type != context.GetCallerType(Name.Value))
        {
            errors.Add(new CompilingError(location,ErrorCode.Invalid,"The property " + Name.Value + " cant be called by an expression of that type"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        Type = context.GetType(Name.Value);
        return checkCaller;
    }
    public override void Evaluate()
    {
        //evaluar la expresion que llama a la propiedad y llamar a  la propiedad del juego correspondiente
        Caller.Evaluate(); 
        if (Caller.Value is CardGame card)
        {
            switch(Name.Value)
            {
                case "Power":
                Value = card.Damage;
                Debug.Log("el poder es: " + Value);
                break;
                case "Name":
                Value = card.Name;
                Debug.Log("el nombre es: " + Value);
                break;
                case "Faction":
                Value = card.GetFaction;
                Debug.Log("la faccion es: " + Value);
                break;
                case "Range":
                Value = card.Range;
                Debug.Log("el range es: " + Value);
                break;
                case "Type":
                Value = card.Type.ToString();
                Debug.Log("el type es: " + Value);
                break;
                case "Owner":
                Value = card.Owner;
                Debug.Log("el owner es: " + Value);
                break;
                default: throw new Exception("Invalid property: " + Name.Value);
            }  
        }
        else if (Caller.Value is GameContext context){
            switch(Name.Value)
            {
                case "TriggerPlayer":
                Value = context.TriggerPlayer.Id;
                break;
                case "Board":
                Value = context.Board;
                break;
                case "Hand":
                Value = context.Hand;
                break;
                case "Deck":
                Value = context.Deck;
                break;
                case "Graveryard":
                Value = context.Graveryard;
                break;
                case "Field":
                Value = context.Field;
                break;
                default: throw new Exception("Invalid property: " + Name.Value);
            }
        }
    }
    public override ExpressionType Type { get; set; }
    public override object? Value { get; set;}
    public override string ToString()
    {
        return String.Format("{0}.{1}",Caller,Name);
    }
}