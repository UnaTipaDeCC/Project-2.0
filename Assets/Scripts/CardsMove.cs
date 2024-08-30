/*using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public class CardsMove : MonoBehaviour
{
    public GameObject Zone;
     public GameObject Card;
     public Card CardInfo;
     public GameManager gameManager;
     private GameObject deck;
     private GameObject deck1;
     private List<GameObject> cardsinhand;
     private List<GameObject> cardsinhand1;
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        deck = GameObject.FindGameObjectWithTag("Deck");//todas las cartas que estan en este deck pertenecen a la faccion hormigas bravas 
        deck1 = GameObject.FindGameObjectWithTag("Deck(1)");//todas las cartas que estan en este deck pertenecen a la faccion hormigas locas
        cardsinhand = deck.GetComponent<Draw>().CardsInHand;
        cardsinhand1 = deck1.GetComponent<Draw>().CardsInHand;
    }
    public void OnClick()
    {
        if(gameManager.PlayerOnePassed == false  && CardInfo.faccion == global::Card.Faccion.Hormigas_Bravas)//comprueba que el jugador uno no se haya pasado y que sea una de sus cartas
        {
            //la zona a la que debe moverse
            if(CardInfo.tipoDeAtaque == global::Card.TipoDeAtaque.Melee)
            {
                if(gameManager.IsPlayerOneTurn && CardInfo.jugada == false)//comprueba que sea el turno del jugador uno y que la carta no haya sido jugada
                {
                    Mover("MeleeZone",cardsinhand);
                    if(gameManager.PlayerTwoPassed == false) //comprueba que el otro jugador no se haya pasado y en ese caso cambia el turno
                    {
                    gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
                    Debug.Log("cambie el turno");
                    }
                }
                else 
                {   
                    Debug.Log("no es tu turno o ya estas en la mesa");
                }  
            }
            else if(CardInfo.tipoDeAtaque == global::Card.TipoDeAtaque.Ranged)
            {
                if(gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
                {
                    Mover("RangedZone",cardsinhand);
                    if(gameManager.PlayerTwoPassed == false)
                    {
                        gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
                        Debug.Log("cambie el turno");
                    }
                }
                else
                {
                    Debug.Log("no es tu turno o ya esta en la mesa");
                }
            }
            else if(CardInfo.tipoDeAtaque == global::Card.TipoDeAtaque.Siege)
            {
                if(gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
                {   
                    Mover("SiegeZone",cardsinhand);
                    if(gameManager.PlayerTwoPassed == false)
                    {
                        gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
                        Debug.Log("cambie el turno");
                    }
                }
                else
                {
                    Debug.Log("no es tu turno o ya esta en la mesa");
                }
            }
        }
        if(gameManager.PlayerTwoPassed == false && CardInfo.faccion == global::Card.Faccion.Hormigas_Locas)//comprueba que el jugador no se haya pasado y que sea una de sus cartas
        {
            if(CardInfo.tipoDeAtaque == global::Card.TipoDeAtaque.Melee)
            {
                if(!gameManager.IsPlayerOneTurn && CardInfo.jugada == false)//comprueba que sea el turno del jugador 2
                {
                    Mover("MeleeZone (1)",cardsinhand1);
                    Debug.Log(CardInfo.jugada);
                    if(gameManager.PlayerOnePassed == false)
                    {
                        gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
                        Debug.Log("cambie el turno"); 
                    }
                }
                else
                {
                    Debug.Log("no es tu turno o ya esta en la mesa");
                }                 
            }
            else if( CardInfo.tipoDeAtaque == global::Card.TipoDeAtaque.Ranged)// && CardInfo.faccion == global::Card.Faccion.Hormigas_Locas)
            {
                if(!gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
                {
                    Mover("RangedZone (1)",cardsinhand1);
                    if(gameManager.PlayerOnePassed == false) 
                    {
                        gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
                        Debug.Log("cambie el turno");   
                    }     
                }
                else
                {
                    Debug.Log("no es tu turno o ya esta en la mesa");
                } 
            }
            else if(CardInfo.tipoDeAtaque == global::Card.TipoDeAtaque.Siege)// && CardInfo.faccion == global::Card.Faccion.Hormigas_Locas)
            {
                if(!gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
                {
                    Mover("SiegeZone (1)",cardsinhand1);
                    if(gameManager.PlayerOnePassed == false) 
                    {
                        gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
                        Debug.Log("cambie el turno");
                    }
                }
                else
                {
                    Debug.Log("no es tu turno o ya esta en la mesa");
                }    
            }
        }      
    }
    private void Mover(string tagZone, List<GameObject> cardsinhand)
    {
        Zone = GameObject.Find(tagZone);
        Card.transform.SetParent(Zone.transform, false);//mover el gameObject a la zona que le  corresponde
        Card.transform.position = Zone.transform.position; 
        cardsinhand.Remove(Card);//remover de la mano
        CardInfo.jugada = true;//ya se jugo la carta
    }
}*/
