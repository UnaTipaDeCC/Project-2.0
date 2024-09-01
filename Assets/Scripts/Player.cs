using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<GameObject> Hand;
    public List<GameObject> Deck;
    public List<GameObject> Melee;
    public List<GameObject> Siege;
    public List<GameObject> Ranged;
    public List<GameObject> Cementery;
    public int Id{get; private set;}

    // Start is called before the first frame update
    void Start()
    {
        Hand = new List<GameObject>();
        Deck = new List<GameObject>();
        Melee = new List<GameObject>();
        Siege = new List<GameObject>();
        Ranged = new List<GameObject>();
        Cementery = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
