using System;
using System.Collections.Generic;
using UnityEngine;
using Unity;
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
        SelectorScope = scope.CreateChild();
        bool checkSingle = true;
        if(Single is null)
        {
            Single.Type = ExpressionType.Bool;
            Single.Value = false; // por defecto es false
        }
        else
        {
            checkSingle = Single.CheckSemantic(context, SelectorScope, errors);
            if(ExpressionType.Bool != Single.Type)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "The 'Single' must be boolean expression"));
                return false;
            }
        }
        if(!(Predicate is Predicate))
        {
            errors.Add(new CompilingError(Predicate.Location, ErrorCode.Invalid, "The 'Predicate' must recive a lamnda expression"));
            return false;
        } 
        bool checkPredicate = Predicate.CheckSemantic(context,SelectorScope,errors);
        if(ExpressionType.Bool != Predicate.Type)
        {
            errors.Add(new CompilingError(Predicate.Location, ErrorCode.Invalid, "The 'Predicate' must be a boolean expression")); 
            return false;
        } 
        bool checkSource = Source.CheckSemantic(context, SelectorScope, errors);
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
        //obtener la lista de cartas con las que se va a trabajar el efecto
        List<CardGame> source = new List<CardGame>();
        switch ((string)Source.Value)
        {
            case "hand":  
            source = GameContext.Instance.TriggerPlayer.Hand.GetComponent<Zones>().CardsInZone;
            break;
            case "otherHand":
            source = GameContext.Instance.OtherPlayer.Hand.GetComponent<Zones>().CardsInZone;
            break;
            case "deck":
            source = GameContext.Instance.TriggerPlayer.Deck;
            break;
            case "OtherDeck":
            source = GameContext.Instance.OtherPlayer.Deck;
            break;
            case "field":
            source = GameContext.Instance.TriggerPlayer.Field;
            break;
            case "otherFiled":
            source = GameContext.Instance.OtherPlayer.Field;
            break;
            case "parent":
            //manejar esta posibilidad mas adelante
            break;
            default: throw new ArgumentException("Invalid source");
        }
        List<CardGame> resultList = new List<CardGame>();
        Single.Evaluate();
        bool single = (bool)Single.Value;
        Predicate predicate = (Predicate)Predicate; // revisar si hay otra forma para acceder a su scope
        /*foreach(var card in source)
        {
            predicate.Scope.Set("unit", card);
            predicate.Evaluate();
            if((bool)predicate.Value)
            {
                resultList.Add(card);
                if(single) break;
            }
        }
        //PENSAR como en el effect ACTION ESTA LISTA SE CONVIERTE EN EL TARGET
    */}
}