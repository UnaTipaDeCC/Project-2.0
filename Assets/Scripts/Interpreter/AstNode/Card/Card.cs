using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System;
public class Card : ASTNode
{
    public Expression Power {get; set;}
    public Expression Type {get;set;}
    public Expression Name {get; set;}
    public Expression Faction {get; set;}
    public List<Expression> Range {get; set;}
    public List<EffectAction> Effects {get; set;}
    //public List<string> cardElements;

    public Card(Expression power, Expression type, Expression name,Expression faction, List<Expression> range,List<EffectAction> effects, CodeLocation location) : base (location)
    {
        Power = power;
        Type = type;
        Name = name;
        Faction = faction;
        Range = range;
        Effects = effects;
    }

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        Scope Scope = scope.CreateChild();
        //se chequean las propiedades y se verifica que el tipo sea valido
        bool checkPower = Power.CheckSemantic(context, Scope, errors);
        bool checkType = Type.CheckSemantic(context,Scope,errors);
        bool checkName = Name.CheckSemantic(context,Scope, errors);
        bool checkFaction = Faction.CheckSemantic(context,Scope,errors);
        bool checkEffects = true;
        bool checkRange = true;
        //Range
        foreach(Expression range in Range)
        {
            checkRange = checkRange && range.CheckSemantic(context,Scope,errors);
            range.Evaluate();
            if(range.Type != ExpressionType.Text)
            {
                errors.Add(new CompilingError(range.Location, ErrorCode.Invalid, "All Range  must be a text"));
                return false;
            }
            if((string)range.Value != "Melee" && (string)range.Value != "Siege" && (string)range.Value != "Ranged")
            {
                errors.Add(new CompilingError(range.Location, ErrorCode.Invalid, "Invalid range, zones must be: Melee, Ranged or Siege"));
                return false;
            }
        }
        //power
        if (Power.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(Power.Location, ErrorCode.Invalid, "The Power must be numerical"));
            return false;
        }
        //type
        if(Type.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Type.Location, ErrorCode.Invalid, "The Type must be a text"));
            return false;
        }
        Type.Evaluate();
        if(!context.PossiblesTypes.Contains((string)Type.Value))
        {
            errors.Add(new CompilingError(Type.Location, ErrorCode.Invalid, "The Type isnt valid"));
            return false;
        }
        //Faction
        if(Faction.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Faction.Location, ErrorCode.Invalid, "The Faction must be a text"));
            return false;
        }
        Faction.Evaluate();
        if((string)Faction.Value != "Hormigas Bravas" && (string)Faction.Value != "Hormigas Locas")
        {
            errors.Add(new CompilingError(Faction.Location, ErrorCode.Invalid, "The Faction must be Hormigas Bravas or Hormigas Locas "));
            return false;
        }
        //Nombre
        if(Name.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Name.Location, ErrorCode.Invalid, "The Name must be a text"));
            return false;
        }
        //Effects
        foreach(var effect in Effects)
        {
            checkEffects = checkEffects && effect.CheckSemantic(context,scope,errors);
        }
        
        return checkPower && checkFaction && checkName && checkType && checkRange && checkEffects;
    }
    
    public void Build()
    {
        //evaluar las propiedades de la carta
        foreach(var a in Range)
        {
            a.Evaluate();
        }
        Power.Evaluate();
        Type.Evaluate();
        Name.Evaluate();
        Faction.Evaluate();
        string name = (string)Name.Value;
        string type = (string)Type.Value;
        string faction = (string)Faction.Value;
        double power = (double)Power.Value;
        string range = (string)Range[0].Value;
       
        //se crea una nueva instancia de cardGame
        CardGame card = ScriptableObject.CreateInstance<CardGame>();
        
        //Se le asignan las propiedades
        card.Type = (CardGame.type)Enum.Parse(typeof(CardGame.type), (string)Type.Value);
        card.Faction = faction == "Hormigas Bravas" ? CardGame.faction.HormigasBravas : CardGame.faction.HormigasLocas;
        card.Damage = Convert.ToInt32(power);
        card.OriginalDamage = card.Damage;
        card.Range = range;
        card.Name = name;
        card.Description = "Cartica creada por el usuario :)";
        card.Artwork = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Pictures/default.jpg");
        //card.EffectsList = Effects;
        card.Effect = CardGame.effects.Especial;
        if(card.EffectsList == null)
        {
            card.EffectsList = new List<EffectAction>();
        }
        /*foreach(var effect in Effects)
        {
            card.EffectsList.Add(effect);
        }*/
        card.EffectsList.AddRange(Effects);
        Debug.Log(card.EffectsList.Count + " estos son los efectos de " + card.Name);
        
        //se verifica la faccion y el tipo de la carta para guardarla y posteriormente instanciarla
        if(faction == "Hormigas Locas") 
        {
            if(type == "Lider")
            {
                CreatedCards.LocasLider = card;
            }
            else CreatedCards.LocasCards.Add(card);
        }
        else 
        {
            if(type == "Lider")
            {
                CreatedCards.BravasLider = card;
            }
            CreatedCards.BravasCards.Add(card);
        }
        //AssetDatabase.CreateAsset(card, scriptableObjectPath);
        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();

       /* GameObject cardPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Card.prefab");
        GameObject cardCopy = GameObject.Instantiate(cardPrefab);
        cardCopy.GetComponent<CardDisplay>().Card = card;
         if(faction == "Hormigas Locas") CreatedCards.LocasCards.Add(cardCopy);
        else {Debug.Log(card.Faction); CreatedCards.BravasCards.Add(cardCopy);}
        
        //CardDisplay cardDisplay = cardCopy.GetComponent<CardDisplay>();
        Debug.Log(card.Owner);
        //PrefabUtility.SaveAsPrefabAsset(cardCopy, cardPath);
        GameObject.Destroy(cardCopy);//si no eso destroy*/
    }

    public override string ToString()
    {
        return $"Card: \n\t Name: {Name} \n\t Power: {Power} \n\t Type: {Type} \n\t Faction: {Faction} \n\t Range.Count = {Range.Count}";
    }
}