/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    public GameObject Hand;
    public List<GameObject> CardsInHand; //= new List<GameObject>();
    public List<GameObject> CardsInDeck; //= new List<GameObject>();
    private bool Yasetoco = false;
    public void OnClick()
    {
        if(CardsInHand.Count == 0 && Yasetoco == false)
        {
            for(int i = 0; i < 10; i++)
            {
                Stole();
                Yasetoco = true;
            }
        }    
    }
    public void Stole()
    {
        int index = Random.Range(0, CardsInDeck.Count);
                GameObject drawCard = Instantiate(CardsInDeck[index], new Vector3(0,0,0), Quaternion.identity);
                CardsInDeck.RemoveAt(index);
                drawCard.transform.SetParent(Hand.transform, false);
                CardsInHand.Add(drawCard);
    }
}*/