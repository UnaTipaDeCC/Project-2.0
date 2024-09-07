using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameContext : MonoBehaviour
{
    public static GameContext Instance { get; private set; }
    private void Awake()
    {
        // Verifica si ya existe una instancia de GameContext
        if (Instance == null)
        {
            Instance = this; // Asigna la instancia
            DontDestroyOnLoad(gameObject); // No destruir este objeto al cargar nuevas escenas
        }
        else
        {
            Destroy(gameObject); // Destruye este objeto si ya existe una instancia
        }
    }
    public GameObject BravasPlayer;
    public GameObject LocasPlayer;
    public GameObject WeatherZone;
    public Player TriggerPlayer
    {
        get
        {
            if(GameManager.gameManager.CurrentPlayer) return BravasPlayer.GetComponent<Player>() ;
            else return LocasPlayer.GetComponent<Player>();
        }
    }
    public Player OtherPlayer
    {
        get
        {
            if(GameManager.gameManager.CurrentPlayer) return LocasPlayer.GetComponent<Player>() ;
            else return BravasPlayer.GetComponent<Player>();
        }
    }
    public Player ReturnPlayer(int id)
    {
        if(id == 1) return BravasPlayer.GetComponent<Player>();
        else if(id == 2) return LocasPlayer.GetComponent<Player>();
        else throw new Exception("Invalid id of player");

    }
    public List<CardGame> Board {get {return BoardCardas();}}
    private List<CardGame> BoardCardas()//necesitara ser cartas tal vez? 
    {

        List<CardGame> board = new List<CardGame>();
        board.AddRange(BravasPlayer.GetComponent<Player>().Field);
        board.AddRange(LocasPlayer.GetComponent<Player>().Field);
        board.AddRange(WeatherZone.GetComponent<Zones>().CardsInZone);
        return board; 
    }
    public List<CardGame> HandOfPlayer(Player player) => player.Hand.GetComponent<Zones>().CardsInZone;
    public List<CardGame> DeckOfPlayer(Player player) => player.Deck;
    public List<CardGame> FieldOfPlayer(Player player) => player.Field;
    public List<CardGame> GraveryardOfPlayer(Player player) => player.Cementery;
    public List<CardGame> Field => FieldOfPlayer(TriggerPlayer);
    public List<CardGame> Graveryard => GraveryardOfPlayer(TriggerPlayer);
    public List<CardGame> Hand => HandOfPlayer(TriggerPlayer); 
    public List<CardGame> Deck => DeckOfPlayer(TriggerPlayer);
    
    public void Shuffle(List<CardGame> gameObjects)
    {
        for (int i = gameObjects.Count - 1; i > 0; i--)
        {
            // Selecciona un índice aleatorio entre 0 y i
            int j = UnityEngine.Random.Range(0, i + 1);
            // Intercambia gameObjects[i] con el elemento en el índice aleatorio
            CardGame temp = gameObjects[i];
            gameObjects[i] = gameObjects[j];
            gameObjects[j] = temp;
        } 
    }
    public void Remove(List<CardGame> list, CardGame card) => list.Remove(card);
    public CardGame Pop(List<CardGame> gameObjects)
    {
        if (gameObjects.Count == 0)
        {
            Debug.LogWarning("La lista está vacía. No se puede hacer pop.");
            return null; // O lanzar una excepción, según tu preferencia
        }
        CardGame topObject = gameObjects[gameObjects.Count - 1];
        gameObjects.RemoveAt(gameObjects.Count - 1);
        return topObject;
    }
    public void Push(CardGame obj, List<CardGame> gameObjects) => gameObjects.Add(obj);
    public void SendBottom(CardGame obj, List<CardGame> gameObjects)  => gameObjects.Insert(0, obj);
    public void CleanField()
    {
        CleanZone(BravasPlayer.GetComponent<Player>().Cementery,BravasPlayer.GetComponent<Player>().Melee);
        CleanZone(BravasPlayer.GetComponent<Player>().Cementery,BravasPlayer.GetComponent<Player>().Siege);
        CleanZone(BravasPlayer.GetComponent<Player>().Cementery,BravasPlayer.GetComponent<Player>().Ranged);
        CleanZone(BravasPlayer.GetComponent<Player>().Cementery,BravasPlayer.GetComponent<Player>().SiegeIncrement);
        CleanZone(BravasPlayer.GetComponent<Player>().Cementery,BravasPlayer.GetComponent<Player>().MeleeIncrement);
        CleanZone(BravasPlayer.GetComponent<Player>().Cementery,BravasPlayer.GetComponent<Player>().RangedIncrement);
        CleanWeatherZone();
        CleanZone(LocasPlayer.GetComponent<Player>().Cementery,LocasPlayer.GetComponent<Player>().Melee);
        CleanZone(LocasPlayer.GetComponent<Player>().Cementery,LocasPlayer.GetComponent<Player>().Siege);
        CleanZone(LocasPlayer.GetComponent<Player>().Cementery,LocasPlayer.GetComponent<Player>().Ranged);
        CleanZone(LocasPlayer.GetComponent<Player>().Cementery,LocasPlayer.GetComponent<Player>().SiegeIncrement);
        CleanZone(LocasPlayer.GetComponent<Player>().Cementery,LocasPlayer.GetComponent<Player>().MeleeIncrement);
        CleanZone(LocasPlayer.GetComponent<Player>().Cementery,LocasPlayer.GetComponent<Player>().RangedIncrement);
    }
    private void CleanZone(List<CardGame> cementery, GameObject zone )
    {
        List<CardGame> cards = zone.GetComponent<Zones>().CardsInZone;
        foreach (CardGame game in cards)
        {
            cementery.Add(game);
            zone.GetComponent<Zones>().CardsInZone.Remove(game);
        }
        zone.GetComponent<Zones>().RefreshZone();
    }
    public void CleanWeatherZone()
    {
        foreach (CardGame game in WeatherZone.GetComponent<Zones>().CardsInZone)
        {
            ReturnPlayer(game.Owner).Cementery.Add(game);
        }
        WeatherZone.GetComponent<Zones>().RefreshZone();
    }
}
