using System;
using System.Collections.Generic;
using UnityEngine;
using Unity;
public class Selector : Statement
{
    public Expression Source { get; private set; }
    public Expression Single { get; private set; }
    public Expression Predicate { get; private set; }
    public List<CardGame> FiltredCards{ get; private set; }
    public CodeLocation Location;
    public bool IsPost {get; set;} // saber si es un action o un post action
    public Scope SelectorScope{ get; private set;}
    Scope Scope; //referencia al scope global
 
    public Selector(Expression source, Expression single, Expression predicate, CodeLocation location) : base(location)
    {
        this.Source = source;
        this.Single = single;
        this.Predicate = predicate;
        this.Location = location;
    }

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        Scope = scope;
        SelectorScope = scope.CreateChild();
        //chequear el Single y ponerlo en false en caso de que sea null
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
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "The 'Single' must be a boolean expression"));
                return false;
            }
        }
        //chequear que sea un predicate y en ese caso chequearlo semanticamente
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
        //chequear semanticamente el source, verificar que sea valido
        bool checkSource = Source.CheckSemantic(context, SelectorScope, errors);
        if(Source.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Single.Location,ErrorCode.Invalid,"The 'Source' must be a text"));
            return false;
        }
        Source.Evaluate();
        if(!IsPost)
        {
            //revisar que sea una fuente valido
            if(!context.ContainsSource((string)Source.Value))
            {
                errors.Add(new CompilingError(Source.Location,ErrorCode.Invalid,"The 'Source' isnt a possible expression"));
                return false;
            }
            else if((string)Source.Value == "parent")
            {
                errors.Add(new CompilingError(Source.Location,ErrorCode.Invalid,"The 'Source' cant be 'parent' if the effect isnt a postAction"));
                return false;
            }
        }
        else
        {
            if(!context.ContainsSource((string)Source.Value))
            {
                errors.Add(new CompilingError(Source.Location,ErrorCode.Invalid,"The postAction 'Source' isnt a possible expression"));
                return false;
            }
        }
        return checkSource && checkSingle && checkPredicate; 
    }

    public override void Execute()
    {
        Source.Evaluate();
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
            case "otherField":
            source = GameContext.Instance.OtherPlayer.Field;
            break;
            case "board":
            source = GameContext.Instance.Board;
            break;
            case "parent":
            //se accede al scope del efecto, al par(action,postAction) y se le asigna la lista filtrada del action
            source = Scope.EffectPair.Item1.Selector.FiltredCards;
            break;
            default: throw new ArgumentException("Invalid source");
        }
        List<CardGame> resultList = new List<CardGame>();
        Single.Evaluate();
        bool single = (bool)Single.Value;
        Predicate predicate = (Predicate)Predicate;
        Debug.Log("estoy en el selector y lo del source es:" + source.Count);
        foreach(var card in source)
        {
            Variable a = (Variable)predicate.Variable;
         
            Debug.Log(a.Name);
            Debug.Log(card.Name);
          
            //actualizar el valor de la variable del predicate
            SelectorScope.Set(a.Name, card); 
            predicate.Evaluate();
            Debug.Log("el valor del predicate " + predicate.Value);
            if((bool)predicate.Value) 
            {
                resultList.Add(card); 
                if(single) break; //una vez se encuentra la primera salir
            }
        }
        Debug.Log("despues de filtrar las cartas en el source: " + resultList.Count);
        FiltredCards = resultList;
    }
}