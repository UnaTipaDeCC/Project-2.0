/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablero : MonoBehaviour
{
    private GameObject CARD;
    public List<GameObject> CartasEnZona = new List<GameObject>();
    public int Suma = 0; 
    private GameManager gameManager;
    void Start()
    {
       gameManager = GameObject.FindObjectOfType<GameManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //agrega a la lista la carta cuando colisiona y agrega al contador el poder de la misma
        CARD = collision.gameObject;
        CartasEnZona.Add(CARD);
        if(CARD.CompareTag("Card"))
        {
        if(CARD.GetComponent<cardDisplay>().card.tipoDeCarta == Card.TipoDeCarta.silver && gameManager.MeleeClimaOn == true && CARD.GetComponent<cardDisplay>().card.tipoDeAtaque == Card.TipoDeAtaque.Melee)
        {
            CARD.GetComponent<cardDisplay>().card.Damage -= 1; 
            CARD.GetComponent<cardDisplay>().card.AfectadaPorUnClima = true;
        }
        if(CARD.GetComponent<cardDisplay>().card.tipoDeCarta == Card.TipoDeCarta.silver && gameManager.SiegeClimaOn == true && CARD.GetComponent<cardDisplay>().card.tipoDeAtaque == Card.TipoDeAtaque.Siege)
        {
            CARD.GetComponent<cardDisplay>().card.Damage -= 1; 
            CARD.GetComponent<cardDisplay>().card.AfectadaPorUnClima = true;
        }
        if(CARD.GetComponent<cardDisplay>().card.tipoDeCarta == Card.TipoDeCarta.silver && gameManager.RangedClimaOn == true && CARD.GetComponent<cardDisplay>().card.tipoDeAtaque == Card.TipoDeAtaque.Ranged)
        {
            CARD.GetComponent<cardDisplay>().card.Damage -= 1; 
            CARD.GetComponent<cardDisplay>().card.AfectadaPorUnClima = true;
        }
        Suma += CARD.GetComponent<cardDisplay>().card.Damage;
        } 
        
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Quita la carta si ya no esta colisionando 
        if (CartasEnZona.Contains(collision.gameObject))
        {
            CARD = collision.gameObject;
            CartasEnZona.Remove(CARD);
            if(CARD.CompareTag("Card"))
            {
                Suma -= CARD.GetComponent<cardDisplay>().card.Damage;
            }           
        }
    }

    
}*/