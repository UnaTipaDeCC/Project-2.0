using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System;
public class Card : ASTNode
{
    //public string Id {get; set;}
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
        bool checkPower = Power.CheckSemantic(context, scope, errors);
        bool checkType = Type.CheckSemantic(context,scope,errors);
        bool checkName = Name.CheckSemantic(context,scope, errors);
        bool checkFaction = Faction.CheckSemantic(context,scope,errors);
        bool checkEffects = true;
        bool checkRange = true;
        foreach(Expression range in Range)
        {
            checkRange = checkRange && range.CheckSemantic(context,scope,errors);
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
        if (Power.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(Power.Location, ErrorCode.Invalid, "The Power must be numerical"));
            return false;
        }
        if(Type.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Type.Location, ErrorCode.Invalid, "The Type must be a text"));
            return false;
        }
        if(Faction.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Faction.Location, ErrorCode.Invalid, "The Faction must be a text"));
            return false;
        }
        if(Name.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Name.Location, ErrorCode.Invalid, "The Name must be a text"));
            return false;
        }
        foreach(var effect in Effects)
        {
            checkEffects = checkEffects && effect.CheckSemantic(context,scope,errors);
        }
        Type.Evaluate();
        if(!context.PossiblesTypes.Contains((string)Type.Value))
        {
            errors.Add(new CompilingError(Type.Location, ErrorCode.Invalid, "The Type isnt valid"));
            return false;
        }
        return checkPower && checkFaction && checkName && checkType && checkRange && checkEffects;
    }
    
    public void Build()
    {
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
        Debug.Log(name);
        Debug.Log(type);
        Debug.Log(faction);
        Debug.Log(range);
        Debug.Log(power);

        /*string scriptableObjectPath = "Assets/ScriptableObjects/UserCardSO.asset";
        string cardPath = "Assets/Prefabs/UserCard.prefab";*/

        CardGame card = ScriptableObject.CreateInstance<CardGame>();
        card.Name = name;
        card.Type = type;
        card.Faction = faction;
        card.Damage = Convert.ToInt32(power);
        card.OriginalDamage = Convert.ToInt32(power);
        card.Range = range;
        card.Description = "Cartica creada por el usuario :)";
        card.Artwork = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Pictures/default.jpg");
        
        if(faction == "Hormigas Locas") CreatedCards.LocasCards.Add(card);
        else {Debug.Log(card.Faction); CreatedCards.BravasCards.Add(card);}
        

        
        /*AssetDatabase.CreateAsset(card, scriptableObjectPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        GameObject cardPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Card.prefab");
        GameObject cardCopy = GameObject.Instantiate(cardPrefab);
        cardCopy.GetComponent<CardDisplay>().Card = (CardGame)AssetDatabase.LoadAssetAtPath<ScriptableObject>(scriptableObjectPath);
        CardDisplay cardDisplay = cardCopy.GetComponent<CardDisplay>();
        Debug.Log(card.Owner);
        //GameContext.Instance.ReturnPlayer(card.Owner).Deck.Add(cardDisplay);
        PrefabUtility.SaveAsPrefabAsset(cardCopy, cardPath);
        GameObject.DestroyImmediate(cardCopy);*/
    }

    public override string ToString()
    {
        return $"Card: \n\t Name: {Name} \n\t Power: {Power} \n\t Type: {Type} \n\t Faction: {Faction} \n\t Range.Count = {Range.Count}";
    }
}