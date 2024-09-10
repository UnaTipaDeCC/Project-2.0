using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEditor;
using UnityEngine.XR;

public class CardsMove : MonoBehaviour
{
    
    public static CardsMove Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para mover la carta
    public void MoveCard(CardGame card)
    {
        Player owner = GameContext.Instance.ReturnPlayer(card.Owner);
        GameObject zone = new GameObject();
        switch (card.Type)
        {
            case CardGame.type.Aumento:
            if(card.Range == "Melee") zone = owner.MeleeIncrement;
            else if (card.Range == "Siege") zone = owner.SiegeIncrement;
            else if(card.Range == "Ranged") zone = owner.RangedIncrement;
            break;
            case CardGame.type.Clima:
            zone = GameContext.Instance.WeatherZone;
            break;
            case CardGame.type.Despeje:
            zone = GameContext.Instance.WeatherZone;
            break;
            default: 
            if(card.Range == "Melee") zone = owner.Melee;
            else if (card.Range == "Siege") zone = owner.Siege;
            else if(card.Range == "Ranged") zone = owner.Ranged;
            break;
        }
        Move(zone,card,owner);
        card.Played = true;
        
        Debug.Log("Moviendo la carta: " + card.Name);
    }
    public void Move(GameObject zone, CardGame cardGame, Player owner)
    {
        zone.GetComponent<Zones>().CardsInZone.Add(cardGame);
        zone.GetComponent<Zones>().RefreshZone();
        owner.Hand.GetComponent<Zones>().CardsInZone.Remove(cardGame);
        owner.Hand.GetComponent<Zones>().RefreshZone();
    }
   
}
