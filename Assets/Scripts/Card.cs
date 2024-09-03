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
    public int Damage;
    public int OriginalDamage;
    public bool AffectedByClimate = false;
    public bool Increased = false;
    public string Range;
    //public int EffectsCount = Effects.Count;
    public List<EffectAction> Effects;
    public Faction Faction;
    enum Faction
    {
        Hormigas Locas,
        Hormigas Bravas,
    }
    public Type Type;
    enum Type
    {
        Oro,
        Plata,
        Lider,
        Clima,
        Aumento
    }
    public int Owner
    {
        get
        {
            if(Faction == "Hormigas Bravas") return 1;
            else return 2;
        }
    }
}
