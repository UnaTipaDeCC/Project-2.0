using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

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
    public void MoveCard(CardDisplay cardDisplay)
    {
        CardGame card = cardDisplay.Card;
        Player owner = GameContext.Instance.ReturnPlayer(card.Owner);
        switch (card.Range)
        {
            case "Melee":
            Move(owner.Melee, cardDisplay);
            break;
            case "Ranged":
            Move(owner.Ranged, cardDisplay);
            break;
            case "Siege":
            Move(owner.Siege, cardDisplay);
            break;
            //LAS DE AUMENTO Y LAS CLIMAS 
        }
        Debug.Log("Moviendo la carta: " + card.Name);
    }
    private void Move(GameObject zone, CardDisplay cardGame)
    {
        cardGame.transform.SetParent(zone.transform, false);//mover el gameObject a la zona que le  corresponde
        cardGame.transform.position = zone.transform.position;
        cardGame.Card.Played = true; 
    }

}
