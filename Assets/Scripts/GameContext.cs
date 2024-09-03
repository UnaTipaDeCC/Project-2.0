//using System.Collections;
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
    public GameObject WheatherZone; // revisar despues esta parte
    public Player TriggerPlayer
    {
        get{
        if(GameManager.gameManager.CurrentPlayer) return BravasPlayer.GetComponent<Player>() ;
        else return LocasPlayer.GetComponent<Player>();
        }
    }
    public Player ReturnPlayer(int id)
    {
        if(id == 1) return BravasPlayer.GetComponent<Player>();
        else if(id == 2) return LocasPlayer.GetComponent<Player>();
        else throw new Exception("Invalid id of player");

    }
    public List<CardDisplay> Board {get {return BoardCardas();}}
    private List<CardDisplay> BoardCardas()//necesitara ser cartas tal vez? 
    {
        List<CardDisplay> board = new List<CardDisplay>();
        board.AddRange(BravasPlayer.GetComponent<Player>().Field);
        board.AddRange(LocasPlayer.GetComponent<Player>().Field);
        board.AddRange(WheatherZone.GetComponent<Zones>().CardsInZone);
        return board;
    }
    public List<CardDisplay> HandOfPlayer(Player player) => player.Hand.GetComponent<Zones>().CardsInZone;
    public List<CardDisplay> DeckOfPlayer(Player player) => player.Deck;
    public List<CardDisplay> FieldOfPlayer(Player player) => player.Field;
    public List<CardDisplay> GraveryardOfPlayer(Player player) => player.Cementery;
    public List<CardDisplay> Field => FieldOfPlayer(TriggerPlayer);
    public List<CardDisplay> Graveryard => GraveryardOfPlayer(TriggerPlayer);
    public List<CardDisplay> Hand => HandOfPlayer(TriggerPlayer);
    public List<CardDisplay> Deck => DeckOfPlayer(TriggerPlayer);
    
    public void Shuffle(List<CardDisplay> gameObjects)
    {
        for (int i = gameObjects.Count - 1; i > 0; i--)
        {
            // Selecciona un índice aleatorio entre 0 y i
            int j = UnityEngine.Random.Range(0, i + 1);
            // Intercambia gameObjects[i] con el elemento en el índice aleatorio
            CardDisplay temp = gameObjects[i];
            gameObjects[i] = gameObjects[j];
            gameObjects[j] = temp;
        }
    }
    public void Remove(List<CardDisplay> list, CardDisplay card) => list.Remove(card);
    public CardDisplay Pop(List<CardDisplay> gameObjects)
    {
        if (gameObjects.Count == 0)
        {
            Debug.LogWarning("La lista está vacía. No se puede hacer pop.");
            return null; // O lanzar una excepción, según tu preferencia
        }
        CardDisplay topObject = gameObjects[gameObjects.Count - 1];
        gameObjects.RemoveAt(gameObjects.Count - 1);
        return topObject;
    }
    public void Push(CardDisplay obj, List<CardDisplay> gameObjects) => gameObjects.Add(obj);
    public void SendBottom(CardDisplay obj, List<CardDisplay> gameObjects)  => gameObjects.Insert(0, obj);
    
}
