using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardGame : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Artwork;
    public bool Played = false;
    public double Damage; 
    public double OriginalDamage;
    public bool AfectedByWeather = false;
    public string Range;
    public effects Effect;
    public enum effects
    {
        None,
        Especial,
        Stole,
        RemoveLowestPowerCardFromOpponent,
        RemoveGreatestPowerCardFromOpponent,
        EqualizeCardPowerToAverageOfOwnFieldCards,
        WeatherEffect,
        ClearListWithLeastCards,
        MultiplyCardPowerByCount,
        IncreasEffect,
        BravasLiderEffect,
        LocasLiderEffect, 
        SetWeatherCard,
        SetEncreasCard,
        Clear
    }
    public List<EffectAction> EffectsList;
    public string GetFaction
    {
        get
        { 
            return Faction == faction.HormigasLocas? "Hormigas Locas" : "Hormigas Bravas";
        }
    }
    
    public faction Faction;
    public enum faction
    {
        HormigasLocas,
        HormigasBravas,
    }
    public type Type;
    public enum type
    {
        Oro,
        Plata,
        Lider,
        Clima,
        Aumento,
        Despeje
    }
    public int Owner
    {
        get
        {
            if(Faction == faction.HormigasBravas) return 1;
            else return 2;
        }
    }
    public void ActivateEffect()
    {
        Effects.Instance.ExecuteEffect(this);
    }
}
