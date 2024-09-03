using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zones : MonoBehaviour
{
    public List<CardGame> CardsInZone;
//
    private void OnTriggerEnter(Collider collider)
    {
        GameObject card = collider.gameObject; 
        
    //if (other.CompareTag("Carta"))
    //{
        CardsInZone.Add(card.GetComponent<CardDisplay>().Card);
    //}
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
