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
    /*private Player BravasPlayer = GameContext.Instance.BravasPlayer.GetComponent<Player>();
    private Player LocasPlayer = GameContext.Instance.LocasPlayer.GetComponent<Player>();  
   */ public bool CurrentPlayer = true; // true: player 1 (Hormigas Bravas) and false: player 2 (Hormigas Locas)
    private void Start()
    {
        Debug.Log(GameContext.Instance); // Verifica si GameContext.Instance es null
Debug.Log(GameContext.Instance.BravasPlayer); // Verifica si BravasPlayer es null
Debug.Log(GameContext.Instance.LocasPlayer); // Verifica si LocasPlayer es null
        CreatedCards.AddToDeck();
        StartActions();
    }
    private void RefreshCards(Player player)
    {
        foreach (CardGame card in player.Deck)
        {
            card.Played = false;
            card.AffectedByClimate = false;
            card.Damage = card.OriginalDamage;
            card.Increased = false;
        }
    }
    private void StartActions()
    {
        
    Player bravasPlayer = GameContext.Instance.BravasPlayer.GetComponent<Player>();
    Player locasPlayer = GameContext.Instance.LocasPlayer.GetComponent<Player>();

    if (bravasPlayer == null || locasPlayer == null)
    {
        Debug.LogError("Uno de los jugadores no tiene el componente Player asignado o es null.");
        return; // Salir del método para evitar el NullReferenceException
    }
        RefreshCards(GameContext.Instance.BravasPlayer.GetComponent<Player>());
        RefreshCards(GameContext.Instance.LocasPlayer.GetComponent<Player>());
        GameContext.Instance.BravasPlayer.GetComponent<Player>().WonRounds = 0;
        GameContext.Instance.LocasPlayer.GetComponent<Player>().WonRounds = 0;
        GameContext.Instance.BravasPlayer.GetComponent<Player>().Points = 0;
        GameContext.Instance.LocasPlayer.GetComponent<Player>().Points = 0;
        GameContext.Instance.BravasPlayer.GetComponent<Player>().Passed = false;
        GameContext.Instance.LocasPlayer.GetComponent<Player>().Passed = false;
        GameContext.Instance.BravasPlayer.GetComponent<Player>().Stole(10) ;
        GameContext.Instance.LocasPlayer.GetComponent<Player>().Stole(10);
    }
    public void EndRound()
    {
        //Comprueba que ambos jugadores se hayan pasado
        if(GameContext.Instance.LocasPlayer.GetComponent<Player>().Passed && GameContext.Instance.BravasPlayer.GetComponent<Player>().Passed)
        {
            //revisar lo de las cartas lideres
        }
    }
}
