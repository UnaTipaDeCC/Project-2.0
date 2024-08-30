/*using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PasarTurno : MonoBehaviour
{
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }
    public void OnClick()
    {
        if(gameManager.IsPlayerOneTurn)//comprueba que sea el turno del jugador
        {
            gameManager.PlayerOnePassed = true;
            Debug.Log("se paso el jugador uno");
            gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn; 
        }
        else 
        {
            gameManager.PlayerTwoPassed = true;
            Debug.Log("se paso el jugador dos");
            gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
        }
        gameManager.EndRound();
        gameManager.EndGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/