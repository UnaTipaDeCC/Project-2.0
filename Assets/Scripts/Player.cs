using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Hand;
    public List<CardGame> Deck;
    public GameObject Melee;
    public GameObject Siege;
    public GameObject Ranged;
    public List<CardGame > Cementery;
    public int Id{get; private set;}
    public List<CardGame> Field{get {return GetField();}}
    private List<CardGame> GetField()
    {
        List<CardGame> list = new List<CardGame>();
        list.AddRange(Hand.GetComponent<Zones>().CardsInZone); 
        list.AddRange(Melee.GetComponent<Zones>().CardsInZone);
        list.AddRange(Siege.GetComponent<Zones>().CardsInZone);
        return list;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Deck = new List<CardGame>();
        Cementery = new List<CardGame>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
