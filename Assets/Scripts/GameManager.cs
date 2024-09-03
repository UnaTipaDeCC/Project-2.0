using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    private void Awake()
    {
        if (gameManager == null)
        {
            gameManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool CurrentPlayer = true; // true: player 1 (Hormigas Bravas) and false: player 2 (Hormigas Locas)
    private void Start()
    {
        CreatedCards.AddToDeck();
        GameContext.Instance.BravasPlayer.GetComponent<Player>().Stole(1);
        GameContext.Instance.LocasPlayer.GetComponent<Player>().Stole(1);

    }
}
