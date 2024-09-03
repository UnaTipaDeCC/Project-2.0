using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Hand;
    public List<CardDisplay> Deck;
    public GameObject Melee;
    public GameObject Siege;
    public GameObject Ranged;
    public List<CardDisplay> Cementery;
    public int Id{get; private set;}
    public List<CardDisplay> Field{get {return GetField();}}
    private List<CardDisplay> GetField()
    {
        List<CardDisplay> list = new List<CardDisplay>();
        list.AddRange(Hand.GetComponent<Zones>().CardsInZone);
        list.AddRange(Melee.GetComponent<Zones>().CardsInZone);
        list.AddRange(Siege.GetComponent<Zones>().CardsInZone);
        return list;
    }

    // Start is called before the first frame update
    void Start()
    {
        Deck = new List<CardDisplay>();
        Cementery = new List<CardDisplay>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
