using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialZones : MonoBehaviour
{
       private GameObject CARD;
    public List<GameObject> CartasEnZona;
    
    void OnCollisionEnter2D(Collision2D collision)
    {        
        CARD = collision.gameObject;
        CartasEnZona.Add(CARD);
    }
     void OnCollisionExit2D(Collision2D collision)
    {
        // Quita la carta si ya no esta colisionando 
        if (CartasEnZona.Contains(collision.gameObject))
        {
            CARD = collision.gameObject;
            CartasEnZona.Remove(CARD);
        }
    }
}
