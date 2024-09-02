using System.Collections.Generic;
using System;
class Property : Expression
{
    Expression caller;
    Token name;
    CodeLocation location;
    public Property(Token name, Expression caller, CodeLocation location) : base(location)
    {
        this.caller = caller;
        this.name = name;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool checkCaller = caller.CheckSemantic(context, scope,errors);
        if(!context.Contains(name.Value))
        {
            errors.Add(new CompilingError(location,ErrorCode.Invalid,"The property " + name.Value + " is not valid"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        if(caller.Type != context.GetCallerType(name.Value))
        {
            errors.Add(new CompilingError(location,ErrorCode.Invalid,"The property " + name.Value + " cant be called by an expression of that type"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        Type = context.GetType(name.Value);
        return checkCaller;
    }
    public override void Evaluate()
    {
        caller.Evaluate();   
        Console.WriteLine("evaluando: " + name.Value);  
        if (caller.Value is CardGame card)
        {
            //CardGame card = (CardGame)caller.Value;
            switch(name.Value)
            {
                case "Power":
                Value = card.Damage;
                break;
                case "Name":
                Value = card.Name;
                break;
                case "Faction":
                Value = card.Faction;
                break;
                case "Range":
                Value = card.Range;
                break;
                case "Type":
                Value = card.Type;
                break;
                case "Owner":
                Value = card.Owner;
                break;
                default: throw new Exception("Invalid property: " + name.Value);
            }  
        }
        else if (caller.Value is GameContext context){
            switch(name.Value)
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
                Value = context.Graveryard;
                break;
                default: throw new Exception("Invalid property: " + name.Value);
            }  
            
        }
    }
    public override ExpressionType Type { get; set; }
    public override object? Value { get; set;}
    public override string ToString()
    {
        return String.Format("{0}.{1}",caller,name);
    }
}