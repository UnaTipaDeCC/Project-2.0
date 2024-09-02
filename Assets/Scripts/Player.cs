using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject Hand;
    public List<GameObject> Deck;
    public GameObject Melee;
    public GameObject Siege;
    public GameObject Ranged;
    public List<GameObject> Cementery;
    public int Id{get; private set;}
    public List<GameObject> Field{get {return GetField();}}
    private List<GameObject> GetField()
    {
        List<GameObject> list = new List<GameObject>();
        list.AddRange(Hand.GetComponent<Zones>().CardsInZone);
        list.AddRange(Melee.GetComponent<Zones>().CardsInZone);
        list.AddRange(Siege.GetComponent<Zones>().CardsInZone);
        return list;
    }

    // Start is called before the first frame update
    void Start()
    {
        Deck = new List<GameObject>();
        Cementery = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
