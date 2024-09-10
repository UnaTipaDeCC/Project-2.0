using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class Player : MonoBehaviour
{
    public int WonRounds = 0;
    public int Points = 0;
    public bool Passed = false;
    public CardGame LiderCard;
    public List<CardGame> Cementery;
    public List<CardGame> Deck;
    public int Id{get; private set;}
    public bool CanChange = true; //indica que no se ha jugado ningana carta 
    public int ChandedCards = 0;
   
    //referencia a las zonas del tablero
    public GameObject Melee;
    public GameObject Siege;
    public GameObject Ranged;
    public GameObject MeleeIncrement;
    public GameObject SiegeIncrement;
    public GameObject RangedIncrement;
    public GameObject LiderZone;
    public GameObject Hand;
    public TMP_Text Counter;   //contador de la escena
    public TMP_Text RoundsCounter;  //contador de rondas ganadas de la escena
    public List<CardGame> Field{get {return GetField();}}
    private List<CardGame> GetField()
    {
        List<CardGame> list = new List<CardGame>();
        //list.AddRange(Hand.GetComponent<Zones>().CardsInZone); 
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
            CardGame card = Deck[0];
            Hand.GetComponent<Zones>().CardsInZone.Add(card);
            Hand.GetComponent<Zones>().RefreshZone();
            Deck.RemoveAt(0); // Eliminar la primera carta del mazo
        }
    }
    public void LiderCardInstance()
    {
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
    } 
    public void UpdatePoints() 
    {
        Points = Melee.GetComponent<Zones>().GetPoints() + Siege.GetComponent<Zones>().GetPoints() + Ranged.GetComponent<Zones>().GetPoints();
    }

    public void ChangeCard(CardGame card)
    {
        //verificar que pueda cambiar carta
        if(CanChange && ChandedCards < 2)
        {
            Hand.GetComponent<Zones>().CardsInZone.Remove(card);
            Cementery.Add(card);
            Stole(1);
            ChandedCards ++;
        }
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        Cementery = new List<CardGame>();
        //establecer las zonas como hijos del player
        Melee.transform.SetParent(this.transform, false);
        Ranged.transform.SetParent(this.transform,false);
        Siege.transform.SetParent(this.transform,false);
    }

    // Update is called once per frame
    void Update()
    {
        Counter.text = Points.ToString();
        RoundsCounter.text = WonRounds.ToString();
    }
}
