using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;

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
    private Player BravasPlayer;
    private Player LocasPlayer;
    public void ChangeTurn()
    {
        //se verifica que el otro jugador no se haya pasado para cambiar el turno
        if(!GameContext.Instance.OtherPlayer.Passed)
            CurrentPlayer = !CurrentPlayer;
    }
   
    public bool CurrentPlayer = true; // true: player 1 (Hormigas Bravas) and false: player 2 (Hormigas Locas)
    private void Start()
    {
        BravasPlayer = GameContext.Instance.BravasPlayer.GetComponent<Player>();
        LocasPlayer = GameContext.Instance.LocasPlayer.GetComponent<Player>();
        Debug.Log(GameContext.Instance); // Verifica si GameContext.Instance es null
        Debug.Log(GameContext.Instance.BravasPlayer); // Verifica si BravasPlayer es null
        Debug.Log(GameContext.Instance.LocasPlayer); // Verifica si LocasPlayer es null
        if(CreatedCards.LocasLider != null) LocasPlayer.LiderCard = CreatedCards.LocasLider;
        if(CreatedCards.BravasLider != null) BravasPlayer.LiderCard = CreatedCards.BravasLider;
        BravasPlayer.LiderCardInstance();
        LocasPlayer.LiderCardInstance();
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
        if (BravasPlayer == null || LocasPlayer == null)
        {
            Debug.LogError("Uno de los jugadores no tiene el componente Player asignado o es null.");
            return; // Salir del método para evitar el NullReferenceException
        }
        RefreshCards(GameContext.Instance.BravasPlayer.GetComponent<Player>());
        RefreshCards(GameContext.Instance.LocasPlayer.GetComponent<Player>());
        BravasPlayer.WonRounds = 0;
        LocasPlayer.WonRounds = 0;
        BravasPlayer.Points = 0;
        LocasPlayer.Points = 0;
        BravasPlayer.Passed = false;
        LocasPlayer.Passed = false;
        BravasPlayer.Stole(10);
        LocasPlayer.Stole(10);
    }
    public void EndRound()
    {
        //Comprueba que ambos jugadores se hayan pasado
        if(LocasPlayer.Passed && BravasPlayer.Passed)
        {
            //se verifica el ganador
            if(LocasPlayer.Points < BravasPlayer.Points)
            {
                BravasPlayer.WonRounds ++;
                CurrentPlayer = true;
            }
            else if(LocasPlayer.Points > BravasPlayer.Points)
            {
                LocasPlayer.WonRounds ++;
                CurrentPlayer = false;
            }
            else 
            {
                LocasPlayer.WonRounds ++;
                BravasPlayer.WonRounds ++;
            }
            //actualozar los valores y el tablero
            ClearField();
            LocasPlayer.Points = 0;
            BravasPlayer.Points = 0;
            LocasPlayer.Passed = false;
            BravasPlayer.Passed = false;
            //robar las cartas de inicio de una nueva ronda
            BravasPlayer.Stole(CheckHowManyCardsToDraw(BravasPlayer));
            LocasPlayer.Stole(CheckHowManyCardsToDraw(LocasPlayer));
        }
    }
    public void EndGame()
    {
        //comprobar que se haya acabado el juego
        if((BravasPlayer.WonRounds >= 2 || LocasPlayer.WonRounds >= 2) && BravasPlayer.WonRounds != LocasPlayer.WonRounds)
        {
            if(BravasPlayer.WonRounds > LocasPlayer.WonRounds)
            {
                SceneManager.LoadScene("BravasWins");
            }
            else if(BravasPlayer.WonRounds < LocasPlayer.WonRounds)
            {
                SceneManager.LoadScene("LocasWins");
            }
        }
    }
    void ClearField()
    {
        //se limpia cada una de las filas de de los jugadores
        ClearList(BravasPlayer, BravasPlayer.Melee);
        ClearList(BravasPlayer, BravasPlayer.Ranged);
        ClearList(BravasPlayer, BravasPlayer.Siege);
        ClearList(BravasPlayer, BravasPlayer.MeleeIncrement);
        ClearList(BravasPlayer, BravasPlayer.SiegeIncrement);
        ClearList(BravasPlayer, BravasPlayer.RangedIncrement);
        ClearList(LocasPlayer, LocasPlayer.Melee);
        ClearList(LocasPlayer, LocasPlayer.Ranged);
        ClearList(LocasPlayer, LocasPlayer.Siege);
        ClearList(LocasPlayer, LocasPlayer.MeleeIncrement);
        ClearList(LocasPlayer, LocasPlayer.SiegeIncrement);
        ClearList(LocasPlayer, LocasPlayer.RangedIncrement);
        ClearList(LocasPlayer, GameContext.Instance.WeatherZone, true);
    }
    void ClearList(Player player, GameObject zone, bool isWeatherZone = false) // optimizar despues
    {
        // Obtener el jugador
        //Player player = GameContext.Instance.BravasPlayer.GetComponent<Player>();
        // Obtener la zona con menos cartas
        //GameObject zone = GetZoneWithLeastCards(player, player.Melee.GetComponent<Zones>().CardsInZone, player.Siege.GetComponent<Zones>().CardsInZone, player.Ranged.GetComponent<Zones>().CardsInZone);
        // Obtener la lista de cartas en la zona seleccionada
        List<CardGame> cardsInZone = zone.GetComponent<Zones>().CardsInZone;
         
        // Iterar a través de las cartas en la zona
        for (int i = cardsInZone.Count - 1; i >= 0; i--) // Iterar hacia atrás para evitar modificar la lista mientras se itera
        {
            CardGame card = cardsInZone[i];
            //si es una lista normal
            if(!isWeatherZone)
            {
                // Agregar la carta al cementerio del jugador que se pasa como parametro
                player.Cementery.Add(card);
            }
            else
            {
                //se accede al jugador al que pertenece la carta y se agrega al cementerio del mismo
                GameContext.Instance.ReturnPlayer(card.Owner).Cementery.Add(card);
            }
            // Eliminar la carta de la zona actual
            cardsInZone.RemoveAt(i);  
        }
        // Actualizar la zona después de las modificaciones
        zone.GetComponent<Zones>().RefreshZone();
    }
    int CheckHowManyCardsToDraw(Player player)
    {
        List<CardGame> cardsInHand = player.Hand.GetComponent<Zones>().CardsInZone;

        if(cardsInHand.Count == 10) return 0;
        else if(cardsInHand.Count == 9) return 1;
        else return 2;
    } 
}
