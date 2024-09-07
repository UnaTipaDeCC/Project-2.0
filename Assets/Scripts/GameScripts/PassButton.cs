using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassButtons : MonoBehaviour
{
    GameManager gameManager;
    GameContext gameContext;
    MessageDisplay message;

    public void OnClick()
    {
        if(gameManager.CurrentPlayer)
        {
            gameContext.BravasPlayer.GetComponent<Player>().Passed = true;
            message.ShowMessage("El jugador 1 se pasa");
              
        }
        else 
        {
            gameContext.LocasPlayer.GetComponent<Player>().Passed = true;
            message.ShowMessage("El jugador 2 se pasa");
        }
        gameManager.ChangeTurn();
        gameManager.EndRound();
        gameManager.EndGame();
    }
    // Start is called before the first frame update
    void Start()
    {
        gameManager= GameManager.gameManager;
        gameContext = GameContext.Instance;
        message = MessageDisplay.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
