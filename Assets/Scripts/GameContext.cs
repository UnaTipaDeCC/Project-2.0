//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<GameObject> Board {get {return BoardCardas();}}
    private List<GameObject> BoardCardas()//necesitara ser cartas tal vez? 
    {
        List<GameObject> board = new List<GameObject>();
        board.AddRange(BravasPlayer.GetComponent<Player>().Field);
        board.AddRange(LocasPlayer.GetComponent<Player>().Field);
        board.AddRange(WheatherZone.GetComponent<Zones>().CardsInZone);
        return board;
    }
    public List<GameObject> HandOfPlayer(Player player) => player.Hand.GetComponent<Zones>().CardsInZone;
    public List<GameObject> DeckOfPlayer(Player player) => player.Deck;
    public List<GameObject> FieldOfPlayer(Player player) => player.Field;
    public List<GameObject> GraveryardOfPlayer(Player player) => player.Cementery;
    public List<GameObject> Field => FieldOfPlayer(TriggerPlayer);
    public List<GameObject> Graveryard => GraveryardOfPlayer(TriggerPlayer);
    public List<GameObject> Hand => HandOfPlayer(TriggerPlayer);
    public List<GameObject> Deck => DeckOfPlayer(TriggerPlayer);
    
    public void Shuffle(List<GameObject> gameObjects)
    {
        for (int i = gameObjects.Count - 1; i > 0; i--)
        {
            // Selecciona un índice aleatorio entre 0 y i
            int j = Random.Range(0, i + 1);
            // Intercambia gameObjects[i] con el elemento en el índice aleatorio
            GameObject temp = gameObjects[i];
            gameObjects[i] = gameObjects[j];
            gameObjects[j] = temp;
        }
    }
    public void Remove(List<GameObject> list, GameObject card) => list.Remove(card);
    public GameObject Pop(List<GameObject> gameObjects)
    {
        if (gameObjects.Count == 0)
        {
            Debug.LogWarning("La lista está vacía. No se puede hacer pop.");
            return null; // O lanzar una excepción, según tu preferencia
        }
        GameObject topObject = gameObjects[gameObjects.Count - 1];
        gameObjects.RemoveAt(gameObjects.Count - 1);
        return topObject;
    }
    public void Push(GameObject obj, List<GameObject> gameObjects) => gameObjects.Add(obj);
    public void SendBottom(GameObject obj, List<GameObject> gameObjects)  => gameObjects.Insert(0, obj);
    
}
