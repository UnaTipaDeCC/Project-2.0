using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassButtons : MonoBehaviour
{
    public GameObject Player;
    Player player;
    GameManager gameManager= GameManager.gameManager;
    GameContext gameContext = GameContext.Instance;

    public void OnClick()
    {
        if(gameContext.TriggerPlayer == player)
        {
            player.Passed = true;
            gameManager.ChangeTurn();
        }
        gameManager.EndRound();
        gameManager.EndGame();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = Player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
