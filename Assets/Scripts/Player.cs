using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int WonRounds = 0;
    public int Points = 0;
    public bool Passed;
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
        Debug.Log("hand asignado: " + Hand);
        //Deck = new List<CardGame>();
        Cementery = new List<CardGame>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
