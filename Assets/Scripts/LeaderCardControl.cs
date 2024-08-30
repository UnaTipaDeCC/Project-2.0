/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderCardControl : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject card;
    public SpecialCards cardInfo;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    public void OnClick()
    {
        if(gameManager.PlayerOnePassed == false && gameManager.IsPlayerOneTurn && cardInfo.faccion == SpecialCards.Faccion.Hormigas_Bravas)//comprueba que sea el turno correspondiente
        {
            Debug.Log("todo bien hasta aqui");
            if(gameManager.LeaderCardBActivated == false)//comprueba que no se haya activado la carta lider 
            {
            Debug.Log("active esta historia");
            if(gameManager.PlayerTwoPassed == false)
            {
                gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
            }
            gameManager.LeaderCardBActivated = true;
            }
            else Debug.Log("ya la activaste");
        }
        else if(gameManager.PlayerTwoPassed == false  && cardInfo.faccion == SpecialCards.Faccion.Hormigas_Locas)
        {
            if(gameManager.IsPlayerOneTurn == false)
            {
            Debug.Log("soy de esta faccion");
            if(gameManager.LeaderCardLActivated == false)
            {
                Debug.Log("active esta historia");
                if(gameManager.PlayerOnePassed == false)  
                {
                    gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
                }
                gameManager.LeaderCardLActivated = true;
            }
            else Debug.Log("ya la activaste");    
            }
            else Debug.Log("NO ES MI TURNO");
        }  
    }
}*/
