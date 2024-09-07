using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassButtons : MonoBehaviour
{
    GameManager gameManager;
    GameContext gameContext;

    public void OnClick()
    {
        if(gameManager.CurrentPlayer)
        {
            gameContext.BravasPlayer.GetComponent<Player>().Passed = true;
            
        }
        else gameContext.LocasPlayer.GetComponent<Player>().Passed = true;
        gameManager.ChangeTurn();
        gameManager.EndRound();
        gameManager.EndGame();
    }
    // Start is called before the first frame update
    void Start()
    {
        gameManager= GameManager.gameManager;
        gameContext = GameContext.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
