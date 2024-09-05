using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Player : MonoBehaviour
{
    public int WonRounds = 0;
    public int Points = 0;
    public bool Passed = false;
    public CardGame LiderCard;
    public GameObject LiderZone;
    public GameObject Hand;
    public List<CardGame> Deck;
    public GameObject Melee;
    public GameObject Siege;
    public GameObject Ranged;
    public GameObject MeleeIncrement;
    public GameObject SiegeIncrement;
    public GameObject RangedIncrement;
    public List<CardGame> Cementery;
    public int Id{get; private set;}
    public List<CardGame> Field{get {return GetField();}}
    private List<CardGame> GetField()
    {
        List<CardGame> list = new List<CardGame>();
        list.AddRange(Hand.GetComponent<Zones>().CardsInZone); 
        list.AddRange(Melee.GetComponent<Zones>().CardsInZone);
        list.AddRange(Siege.GetComponent<Zones>().CardsInZone);
        list.AddRange(MeleeIncrement.GetComponent<Zones>().CardsInZone);
        list.AddRange(SiegeIncrement.GetComponent<Zones>().CardsInZone);
        list.AddRange(RangedIncrement.GetComponent<Zones>().CardsInZone);
        return list;
    }
    public void Stole(int n) 
    {
        Debug.Log(Deck.Count + "lo que hay en el mazo");
        if (Deck.Count < n)
        {
            Debug.Log("No se pueden robar esa cantidad de cartas");
            return;
        }
        GameContext.Instance.Shuffle(Deck);

        for (int i = 0; i < n; i++)
        {   
            /*if (Deck.Count == 0)
            {
                Debug.LogWarning("No hay más cartas en el mazo.");
                return;
            }

            GameObject card = Deck[0]; // Obtener la primera carta del mazo

            if (Hand == null)
            {
                Debug.LogError("Hand no está asignado.");
                return;
            }

            Zones handZones = Hand.GetComponent<Zones>();
            if (handZones == null)
            {
                Debug.LogError("El objeto Hand no tiene un componente Zones.");
                return;
            }
            Debug.Log(CardsMove.Instance == null);*/
            CardGame card = Deck[0];
            Hand.GetComponent<Zones>().CardsInZone.Add(card);
            Hand.GetComponent<Zones>().RefreshZone();
            Deck.RemoveAt(0); // Eliminar la primera carta del mazo
        }
    }
    

    // Start is called before the first frame update
    void Start()
    {
        #region LiderCardInstance
        // Ruta del prefab de la carta
        string cardPath = "Assets/Prefabs/Card.prefab";
        
        // Cargar el prefab de la carta
        GameObject cardPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(cardPath);
        if (cardPrefab != null)
        {
            // Instanciar la carta
            GameObject cardInstance = GameObject.Instantiate(cardPrefab);
            
            // Asignar la carta al componente CardDisplay
            cardInstance.GetComponent<CardDisplay>().Card = LiderCard; 
            
            // Establecer el padre a la zona especificada
            cardInstance.transform.SetParent(LiderZone.transform, false); 
            
            // Opcional: Ajustar la posición de la carta si es necesario
            // cardInstance.transform.localPosition = Vector3.zero; // Por ejemplo, centrar en la zona
            
            Debug.Log("Carta instanciada en la zona: " + LiderZone.name + ", Carta: " + LiderCard.Name);
        }
        else
        {
            Debug.LogError("No se pudo cargar el prefab de la carta.");
        }
        #endregion
        Cementery = new List<CardGame>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
