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
    private Player bravasPlayer;
    private Player locasPlayer;
    private MessageDisplay message;
    public void ChangeTurn()
    {
        //se verifica que el otro jugador no se haya pasado para cambiar el turno
        if(!GameContext.Instance.OtherPlayer.Passed)
            CurrentPlayer = !CurrentPlayer;
    }
   
    public bool CurrentPlayer = true; // true: player 1 (Hormigas Bravas) and false: player 2 (Hormigas Locas)
    private void Start()
    {
        message = MessageDisplay.Instance;
        bravasPlayer = GameContext.Instance.BravasPlayer.GetComponent<Player>();
        locasPlayer = GameContext.Instance.LocasPlayer.GetComponent<Player>();
        if(CreatedCards.LocasLider != null) locasPlayer.LiderCard = CreatedCards.LocasLider;
        if(CreatedCards.BravasLider != null) bravasPlayer.LiderCard = CreatedCards.BravasLider;
        bravasPlayer.LiderCardInstance();
        locasPlayer.LiderCardInstance();
        CreatedCards.AddToDeck();
        StartActions();
    }
    private void RefreshCards(Player player)
    {
        foreach (CardGame card in player.Deck)
        {
            card.Played = false;
            card.Damage = card.OriginalDamage;
            card.AfectedByWeather = false; 
        }
    }
    private void StartActions()
    {
        if (bravasPlayer == null || locasPlayer == null)
        {
            Debug.LogError("Uno de los jugadores no tiene el componente Player asignado o es null.");
            return; // Salir del método para evitar el NullReferenceException
        }
        RefreshCards(GameContext.Instance.BravasPlayer.GetComponent<Player>());
        RefreshCards(GameContext.Instance.LocasPlayer.GetComponent<Player>());
        bravasPlayer.WonRounds = 0;
        locasPlayer.WonRounds = 0;
        bravasPlayer.Points = 0;
        locasPlayer.Points = 0;
        bravasPlayer.Passed = false;
        locasPlayer.Passed = false;
        bravasPlayer.Stole(10);
        locasPlayer.Stole(10);
    }
    public void EndRound()
    {
        //Comprueba que ambos jugadores se hayan pasado
        if(locasPlayer.Passed && bravasPlayer.Passed)
        {
            message.ShowMessage("Fin de la ronda");
            //se verifica el ganador
            if(locasPlayer.Points < bravasPlayer.Points)
            {
                message.ShowMessage("Jugador 1 gana esta ronda");
                bravasPlayer.WonRounds ++;
                CurrentPlayer = true;
            }
            else if(locasPlayer.Points > bravasPlayer.Points)
            {
                message.ShowMessage("Jugador 2 gana esta ronda");
                locasPlayer.WonRounds ++;
                CurrentPlayer = false;
            }
            else 
            {
                message.ShowMessage("Empate!!");
                locasPlayer.WonRounds ++;
                bravasPlayer.WonRounds ++;
            }
            //actualozar los valores y el tablero
            ClearField();
            locasPlayer.Points = 0;
            bravasPlayer.Points = 0;
            locasPlayer.Passed = false;
            bravasPlayer.Passed = false;
            //robar las cartas de inicio de una nueva ronda
            bravasPlayer.Stole(CheckHowManyCardsToDraw(bravasPlayer));
            locasPlayer.Stole(CheckHowManyCardsToDraw(locasPlayer));
        }
    }
    public void EndGame()
    {
        //comprobar que se haya acabado el juego
        if((bravasPlayer.WonRounds >= 2 || locasPlayer.WonRounds >= 2) && bravasPlayer.WonRounds != locasPlayer.WonRounds)
        {
            //comprobar el ganador
            if(bravasPlayer.WonRounds > locasPlayer.WonRounds)
            {
                SceneManager.LoadScene("BravasWins");
            }
            else if(bravasPlayer.WonRounds < locasPlayer.WonRounds)
            {
                SceneManager.LoadScene("LocasWins");
            }
        }
    }
    void ClearField()
    {
        //se limpia cada una de las filas de de los jugadores
        ClearList(bravasPlayer, bravasPlayer.Melee);
        ClearList(bravasPlayer, bravasPlayer.Ranged);
        ClearList(bravasPlayer, bravasPlayer.Siege);
        ClearList(bravasPlayer, bravasPlayer.MeleeIncrement);
        ClearList(bravasPlayer, bravasPlayer.SiegeIncrement);
        ClearList(bravasPlayer, bravasPlayer.RangedIncrement);
        ClearList(locasPlayer, locasPlayer.Melee);
        ClearList(locasPlayer, locasPlayer.Ranged);
        ClearList(locasPlayer, locasPlayer.Siege);
        ClearList(locasPlayer, locasPlayer.MeleeIncrement);
        ClearList(locasPlayer, locasPlayer.SiegeIncrement);
        ClearList(locasPlayer, locasPlayer.RangedIncrement);
        ClearList(locasPlayer, GameContext.Instance.WeatherZone, true);
    }
    void ClearList(Player player, GameObject zone, bool isWeatherZone = false) // optimizar despues
    {
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
