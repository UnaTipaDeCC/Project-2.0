/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class EscogerCartas : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject cardZoom;
    public GameObject clickedCard;
    private GameObject deck;
    private GameObject contador;
    private GameObject hand;
    private List<GameObject> cardsInHand;
    private List<GameObject> cardsInDeck;

    // Start is called before the first frame update
    void Start()
    {
       gameManager = GameObject.FindObjectOfType<GameManager>();
       cardZoom = GameObject.Find("ZoomCard");
    }   
    public void Cambiardoscartas()
    { 
        //Comprueba a que faccion pertenece la carta y en funcion de eso asigna los valores necesarios y verifica que el jugador no haya jugado, ni pasado, ni activado la carta lider(basicamente que no haya empezado el juego)
        if((clickedCard.CompareTag("Card") && clickedCard.GetComponent<cardDisplay>().card.faccion == Card.Faccion.Hormigas_Bravas) || (clickedCard.CompareTag("SpecialCard") && clickedCard.GetComponent<SpecialCardsDisplay>().specialcard.faccion == SpecialCards.Faccion.Hormigas_Bravas))
        {
            deck = GameObject.Find("Deck");
            contador = GameObject.Find("Contador");
            hand = GameObject.Find("Hand");
            cardsInHand = deck.GetComponent<Draw>().CardsInHand;
            cardsInDeck = deck.GetComponent<Draw>().CardsInDeck;
        if(gameManager.RondasGanadas1 == 0 && gameManager.PlayerOnePassed == false && gameManager.LeaderCardBActivated == false && gameManager.Cambieb  < 2 && Input.GetMouseButtonUp(1) && contador.GetComponent<Contador>().puntos == 0 && deck.GetComponent<Draw>().CardsInHand.Count == 10)
        {
            EscogeCartas(cardsInHand , cardsInDeck);
            gameManager.Cambieb++;
            Debug.Log(gameManager.Cambieb);
        }
        else Debug.Log("Ya cambiaste las dos cartas o ya jugaste y no  puedes cambiar");
        }
        else if((clickedCard.CompareTag("Card") && clickedCard.GetComponent<cardDisplay>().card.faccion == Card.Faccion.Hormigas_Locas) || (clickedCard.CompareTag("SpecialCard") && clickedCard.GetComponent<SpecialCardsDisplay>().specialcard.faccion == SpecialCards.Faccion.Hormigas_Locas))
        {
            deck = GameObject.Find("Deck (1)");
            contador = GameObject.Find("Contador (1)");
            hand = GameObject.Find("Hand1");
            cardsInHand = deck.GetComponent<Draw>().CardsInHand;
            cardsInDeck = deck.GetComponent<Draw>().CardsInDeck;
            if(gameManager.RondasGanadas2 == 0 && gameManager.PlayerTwoPassed == false && gameManager.LeaderCardLActivated == false && gameManager.CambieL < 2 && Input.GetMouseButtonUp(1) && contador.GetComponent<Contador>().puntos == 0 && deck.GetComponent<Draw>().CardsInHand.Count == 10)
            {
                EscogeCartas(cardsInHand , cardsInDeck);
                gameManager.CambieL++;
                Debug.Log(gameManager.CambieL);
            }
        else Debug.Log("Ya cambiaste las dos cartas o ya jugaste y no  puedes cambiar");
        }
        void EscogeCartas(List<GameObject> cardsInHand, List<GameObject> cardsInDeck)
        {
            cardsInHand.Remove(clickedCard);
            cardsInDeck.Add(clickedCard);
            clickedCard.SetActive(false);
            int index = UnityEngine.Random.Range(0, cardsInDeck.Count);
            GameObject drawCard = Instantiate(cardsInDeck[index], new Vector3(0,0,0), Quaternion.identity);
            cardsInDeck.RemoveAt(index);
            drawCard.transform.SetParent(hand.transform, false);
            cardsInHand.Add(drawCard);
            Debug.Log("cambie una carta");
            Transform hijoADestruir = cardZoom.transform.GetChild(0);
            if (hijoADestruir != null) { Destroy(hijoADestruir.gameObject); }   
        }       
    }
}*/
  
