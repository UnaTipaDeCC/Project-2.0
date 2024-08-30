/*using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class SpecialCardsControl : MonoBehaviour
{
    private GameObject Zone;
    public GameObject Card;
    public SpecialCards CardInfo;
    private GameManager gameManager;
    private GameObject zone;
    private GameObject zone1;
    private GameObject deck;
    private GameObject deck1;
    private List<GameObject> cardsinhand;
    private List<GameObject> cardsinhand1;
    private GameObject Cementery;
    private GameObject Cementery1;
    private GameObject cartam;
    private GameObject cartas;
    private GameObject cartar;
    private GameObject filaMelee;
    private GameObject filaRanged;
    private GameObject filaSiege;
    private List<GameObject> melee;
    private List<GameObject> ranged;
    private List<GameObject> siege;
    // Start is called before the first frame update
    void Start()
    {
      gameManager = GameObject.FindObjectOfType<GameManager>();
      deck = GameObject.FindGameObjectWithTag("Deck");
      deck1 = GameObject.FindGameObjectWithTag("Deck(1)");
      cardsinhand = deck.GetComponent<Draw>().CardsInHand;
      cardsinhand1 = deck1.GetComponent<Draw>().CardsInHand;
      Cementery = GameObject.Find("Cementery");
      Cementery1 = GameObject.Find("Cementery (1)");
    }
    public void OnClick()
    {
     //asigna la zona que le corresponde a cada carta segun el tipo de carta y la zona a la que afecta y controla que sea el turno del jugador en caso contrario no le asigna una zona y por ende no se mueve
        if(gameManager.PlayerOnePassed == false && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Bravas)
        {
          if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.clima && CardInfo.zonaQueAfecta == SpecialCards.ZonaQueAfecta.Melee)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Bravas)
          {
            if(gameManager.IsPlayerOneTurn && CardInfo.jugada == false)//comprueba que la carta no se haya jugado y que sea el turno del jugador
            {
              if(gameManager.MeleeClima == false)//comprueba que no haya una carta melee afectando a esa zona
              {
              Mover("MeleeClima",cardsinhand);
              Clima("MeleeZone","MeleeZone (1)");//afecta a las cartas de plata
              gameManager.MeleeClimaOn = true;
              gameManager.MeleeClima = true;
              if(gameManager.PlayerTwoPassed == false)//comprueba que el otro jagador no se haya pasado para cambiar el turno
              {
                gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
              }
              }
              else Debug.Log("ya hay una carta clima que afecta esta zona");
            } 
            else Debug.Log("No es tu turno");       
          }
        
          if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.despeje && CardInfo.zonaQueAfecta == SpecialCards.ZonaQueAfecta.Melee)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Bravas)
          {
            if(gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
            {
              Mover("MeleeClima",cardsinhand);
              if(gameManager.MeleeClima == true)
              {
                RemoverClima();
                Despeje("MeleeZone","MeleeZone (1)");
                gameManager.MeleeClima = false;
                gameManager.MeleeClimaOn = false;
              }
              else Debug.Log("no hay cartas climas"); 
              if(gameManager.PlayerTwoPassed == false)
              {
                gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
              }
            } 
            else Debug.Log("No es tu turno");       
          }
        
          else if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.clima && CardInfo.zonaQueAfecta == SpecialCards.ZonaQueAfecta.Ranged)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Bravas)
          {
            if(gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
            {
              if(gameManager.RangedClima == false)
              {
                Mover("RangedClima",cardsinhand);
                Clima("RangedZone", "RangedZone (1)");
                gameManager.RangedClimaOn = true;
                if(gameManager.PlayerTwoPassed == false)
                {
                  gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
                }
                gameManager.RangedClima = true;
              }
              else Debug.Log("YA HAY UNA CARTA CLIMA QUE AFECTA ESTA ZONA");
            }
            else Debug.Log("No es tu turno");   
          
          }
        
          else if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.despeje && CardInfo.zonaQueAfecta == SpecialCards.ZonaQueAfecta.Ranged)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Bravas)
          {
            if(gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
            {
              Mover("RangedClima",cardsinhand);
              if(gameManager.RangedClima == true)
              {
                RemoverClima();
                Despeje("RangedZone", "RangedZone (1)");
                gameManager.RangedClima = false;
                gameManager.RangedClimaOn = false;
              }
              else Debug.Log("no hay cartas climas");   

              if(gameManager.PlayerTwoPassed == false)
              {
                gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
              }
            }
            else Debug.Log("No es tu turno");           
          }
        
          else if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.clima && CardInfo.zonaQueAfecta == SpecialCards.ZonaQueAfecta.Siege)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Bravas)
          {
              if(gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
              {
                if(gameManager.SiegeClima == false)
                {
                  gameManager.SiegeClima = true;
                  Mover("SiegeClima",cardsinhand);
                  Clima("SiegeZone", "SiegeZone (1)");
                  gameManager.SiegeClimaOn = true;
                  if(gameManager.PlayerTwoPassed == false)
                  {
                    gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
                  }
                  }
                  else Debug.Log("ya hay una carta clima que afecta esta zona");
              }
              else Debug.Log("No es tu turno");
          }
        
          else if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.despeje && CardInfo.zonaQueAfecta == SpecialCards.ZonaQueAfecta.Siege)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Bravas)
          {
              if(gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
              {
                Mover("SiegeClima",cardsinhand);
                if(gameManager.MeleeClima == true)
              {
                RemoverClima();
                Despeje("SiegeZone", "SiegeZone (1)");
                gameManager.SiegeClima = false;
                gameManager.SiegeClimaOn = false;
              }   

                if(gameManager.PlayerTwoPassed == false)
                {
                  gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
                }
              }
              else Debug.Log("No es tu turno");
          }
        
          else if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.aumento && CardInfo.zonaQueAfecta == SpecialCards.ZonaQueAfecta.Melee)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Bravas)
          {
            if(gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
            {
              Mover("AumentoMelee",cardsinhand);
              Aumentos("MeleeZone");
              if(gameManager.PlayerTwoPassed == false)
              {
                gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
              }
            }  
            else Debug.Log("No es tu turno");
          }
        
          else if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.aumento && CardInfo.zonaQueAfecta == SpecialCards.ZonaQueAfecta.Ranged)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Bravas)
          {
            if(gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
            {
              Mover("AumentoRanged",cardsinhand);
              Aumentos("RangedZone");
              if(gameManager.PlayerTwoPassed == false)
              {
                gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
              }
            }
            else Debug.Log("No es tu turno");
          }
        
          else if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.aumento && CardInfo.zonaQueAfecta == SpecialCards.ZonaQueAfecta.Siege)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Bravas)
          {
            if(gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
            {
              Mover("AumentoSiege ",cardsinhand);
              Aumentos("SiegeZone");
              if(gameManager.PlayerTwoPassed == false)
              {
                gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
              }
            }           
          else Debug.Log("No es tu turno");
          }
          else if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.senuelo)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Bravas)
          {
            Debug.Log("estoy");
            if (gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
            {
              Senuelo("MeleeZone","SiegeZone","RangedZone", cardsinhand,"Hand");
              CardInfo.jugada = true;
              if(gameManager.PlayerTwoPassed == false)
              {
                gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
              }  
            }
          }
        }       
        if(gameManager.PlayerTwoPassed == false && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Locas)
        {
          if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.clima && CardInfo.zonaQueAfecta == SpecialCards.ZonaQueAfecta.Melee)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Locas)
          {
            if(!gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
            {
              if(gameManager.MeleeClima == false)
              {
                gameManager.MeleeClima = true;
                Mover("MeleeClima",cardsinhand1);
                Clima("MeleeZone","MeleeZone (1)");
                gameManager.MeleeClimaOn = true;
                if(gameManager.PlayerOnePassed == false)
                {
                  gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
                }
                gameManager.MeleeClima = true;
              }
              else Debug.Log("ya hay una carta clima que afecta esta zona");
            } 
            else Debug.Log("No es tu turno");       
          }
        
          else if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.despeje && CardInfo.zonaQueAfecta == SpecialCards.ZonaQueAfecta.Melee)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Locas)
          {
            if(!gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
            {
              Mover("MeleeClima",cardsinhand1);
              if(gameManager.MeleeClima == true)
              {
                 RemoverClima();
                 Despeje("MeleeZone","MeleeZone (1)");
                 gameManager.MeleeClima = false;
                 gameManager.MeleeClimaOn = false;
              }  

              if(gameManager.PlayerOnePassed == false)
              {
                gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
              }
            } 
            else Debug.Log("No es tu turno");       
          }
        
          else if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.clima && CardInfo.zonaQueAfecta == SpecialCards.ZonaQueAfecta.Ranged)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Locas)
          {
            if(!gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
            {
              if(gameManager.RangedClima == false)
              {
                Mover("RangedClima",cardsinhand1);
                Clima("RangedZone", "RangedZone (1)");
                gameManager.RangedClimaOn = true;
                if(gameManager.PlayerOnePassed == false)
                {
                  gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
                }
                gameManager.RangedClima = true;
              }
              else Debug.Log("ya hay una carta clima que afecta esta zona");
            }
            else Debug.Log("No es tu turno");           
          }
        
          else if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.despeje && CardInfo.zonaQueAfecta == SpecialCards.ZonaQueAfecta.Ranged)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Locas)
          {
            if(!gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
            {
              Mover("RangedClima",cardsinhand1);
              if(gameManager.RangedClima == true)
              {
                 RemoverClima();
                 Despeje("RangedZone", "RangedZone (1)");
                 gameManager.RangedClima = false;
                 gameManager.RangedClimaOn = false;
              }   
              if(gameManager.PlayerOnePassed == false)
              {
                gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
              }
            }
            else Debug.Log("No es tu turno");           
          }
          
          else if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.clima && CardInfo.zonaQueAfecta == SpecialCards.ZonaQueAfecta.Siege)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Locas)
          {
              if(!gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
              {
                if(gameManager.SiegeClima == false)
                {
                  Mover("SiegeClima",cardsinhand1);
                  Clima("SiegeZone", "SiegeZone (1)");
                  gameManager.SiegeClimaOn = true;
                  if(gameManager.PlayerOnePassed == false)
                  {
                    gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
                  }
                  gameManager.SiegeClima = true;
                }
                else Debug.Log("ya hay una carta que afecta esta zona");  
              }
          else Debug.Log("No es tu turno");
          }

          else if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.despeje && CardInfo.zonaQueAfecta == SpecialCards.ZonaQueAfecta.Siege)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Locas)
          {
              if(!gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
              {
                Mover("SiegeClima",cardsinhand1);
                if(gameManager.SiegeClima == true)
                {
                  RemoverClima();
                  Despeje("SiegeZone", "SiegeZone (1)");
                  gameManager.SiegeClima = false;
                  gameManager.SiegeClimaOn = false;
                }   
                if(gameManager.PlayerOnePassed == false)
                {
                  gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
                }
              }
          else Debug.Log("No es tu turno");
          }
          
          else if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.aumento && CardInfo.zonaQueAfecta == SpecialCards.ZonaQueAfecta.Melee)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Locas)
          {
            if(!gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
            {
              Mover("AumentoMelee (1)",cardsinhand1);
              Aumentos("MeleeZone (1)");
              if(gameManager.PlayerOnePassed == false)
              {
                gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
              }
            }  
          else Debug.Log("No es tu turno");
          }
          
          else if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.aumento && CardInfo.zonaQueAfecta == SpecialCards.ZonaQueAfecta.Ranged)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Locas)
          {
            if(!gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
            { 
              Mover("AumentoRanged (1)",cardsinhand1);
              Aumentos("RangedZone (1)");
              if(gameManager.PlayerOnePassed == false)
              {
                gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
              }
            }
          else Debug.Log("No es tu turno");
          }
          
          else if( CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.aumento && CardInfo.zonaQueAfecta == SpecialCards.ZonaQueAfecta.Siege)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Locas)
          {
              if(!gameManager.IsPlayerOneTurn && CardInfo.jugada == false)
              {
                Mover("AumentoSiege  (1)",cardsinhand1);
                Aumentos("SiegeZone (1)");
                if(gameManager.PlayerOnePassed == false)
                {
                  gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
                }
              }           
              else Debug.Log("No es tu turno");
          }
          else if(CardInfo.tipoDeCarta == SpecialCards.TipoDeCarta.senuelo)// && CardInfo.faccion == SpecialCards.Faccion.Hormigas_Locas)
          {
            if (!gameManager.PlayerTwoPassed && CardInfo.jugada == false)
            {
              Senuelo("MeleeZone (1)","SiegeZone (1)","RangedZone (1)", cardsinhand1,"Hand (1)");
              CardInfo.jugada = true;
              if(gameManager.PlayerOnePassed == false)
              {
                gameManager.IsPlayerOneTurn = !gameManager.IsPlayerOneTurn;
              }  
            }
          }
        }
  void Senuelo(string meleeTag, string siegeTag, string rangedTag, List<GameObject> cardsInHand, string ZoneTag)
  {
    Zone = GameObject.FindGameObjectWithTag(ZoneTag);//la mano a la que tiene que regresar la carta    
    filaMelee = GameObject.FindGameObjectWithTag(meleeTag);
    melee = filaMelee.GetComponent<Tablero>().CartasEnZona;
    filaRanged = GameObject.FindGameObjectWithTag(rangedTag);
    ranged = filaRanged.GetComponent<Tablero>().CartasEnZona;
    filaSiege = GameObject.FindGameObjectWithTag(siegeTag);
    siege = filaSiege.GetComponent<Tablero>().CartasEnZona;
    int contador_melee = int.MinValue;
    int contador_ranged = int.MinValue;
    int contador_siege = int.MinValue;
    if(melee.Count != 0 || siege.Count != 0 || ranged.Count != 0)
    {
      foreach(GameObject carta in melee)//encuentra la carta mas poder de la zona melee
      {
        if(carta.GetComponent<cardDisplay>().card.tipoDeCarta == global::Card.TipoDeCarta.silver)
        {
        if(carta.GetComponent<cardDisplay>().card.Damage > contador_melee) 
        {
          contador_melee = carta.GetComponent<cardDisplay>().card.Damage;
          cartam = carta;
        }
        }
      }
      foreach(GameObject carta in ranged)//encuentra la carta con mas poderde la zona ranged
      {
        if(carta.GetComponent<cardDisplay>().card.tipoDeCarta == global::Card.TipoDeCarta.silver)
        {
        if(carta.GetComponent<cardDisplay>().card.Damage >  contador_ranged)
        {
          contador_ranged = carta.GetComponent<cardDisplay>().card.Damage;
          cartar = carta;
        }
        }
      } 
      foreach(GameObject carta in siege)//encuentra la carta con mas poder de la zona siege
      {
        if(carta.GetComponent<cardDisplay>().card.tipoDeCarta == global::Card.TipoDeCarta.silver)
        {
        if(carta.GetComponent<cardDisplay>().card.Damage > contador_siege )
        {
          contador_siege = carta.GetComponent<cardDisplay>().card.Damage;
          cartas = carta;
        }
        }
      }
      //comparar cual de las cartas tiene mas poder
      if(contador_melee >= contador_ranged && contador_melee > contador_siege)
      {
        Debug.Log("en melee esta la que menos tiene");
        CambiaCarta(cartam,filaMelee,cardsInHand);
      }
      else if(contador_ranged > contador_melee && contador_ranged >= contador_siege)
      {
        CambiaCarta(cartar, filaRanged,cardsInHand);
        Debug.Log("en ranged esta la que menos tiene");
        }
        else if(contador_siege > contador_melee && contador_siege > contador_ranged)
        {
          CambiaCarta(cartas, filaSiege, cardsInHand);
          Debug.Log("en siege esta la que menos tiene");
        }
        else if(cartam == null && cartar == null && cartas == null)
        {
        Debug.Log("todas son de oro");
        Card.transform.SetParent(filaSiege.transform, false); // mover la carta senuelo a la fila siege
        Card.transform.position = filaSiege.transform.position;
        cardsInHand.Remove(Card);
        }
        }
        else 
        {
          Card.transform.SetParent(filaSiege.transform, false); // mover la carta senuelo a la fila siege
          Card.transform.position = filaSiege.transform.position;
          cardsInHand.Remove(Card);
        }
    }
    void CambiaCarta(GameObject carta, GameObject fila, List<GameObject> cardsInHand)
    {
          carta.transform.SetParent(Zone.transform, false); // mover la carta con mas  poder a la mano 
          carta.transform.position = Zone.transform.position;
          cardsInHand.Add(cartas);
          carta.GetComponent<cardDisplay>().card.jugada = false;
          Card.transform.SetParent(fila.transform, false); // mover la carta senuelo a la fila siege
          Card.transform.position = fila.transform.position;
          cardsInHand.Remove(Card);//remover la carta senuelo de la mano
    }
 void Clima(string tag1, string tag2)
 {
    zone = GameObject.FindGameObjectWithTag(tag1);
    zone1 = GameObject.FindGameObjectWithTag(tag2);
    zone.GetComponent<Tablero>().Suma = 0;
    zone1.GetComponent<Tablero>().Suma = 0;
    Debug.Log("estoy aqui");
    climaAfecta(zone);
    /*foreach(GameObject carta in zone.GetComponent<Tablero>().CartasEnZona)
    {
      if(carta.CompareTag("Card"))
      {
      if(carta.GetComponent<cardDisplay>().card.tipoDeCarta == global::Card.TipoDeCarta.silver)
      {
        carta.GetComponent<cardDisplay>().card.Damage -= 1;
        carta.GetComponent<cardDisplay>().card.AfectadaPorUnClima = true;
      }
      zone.GetComponent<Tablero>().Suma += carta.GetComponent<cardDisplay>().card.Damage;
      } 
    }
    climaAfecta(zone1);
    /*foreach(GameObject carta in zone1.GetComponent<Tablero>().CartasEnZona)
    {
      if(carta.CompareTag("Card"))
      {
      if(carta.GetComponent<cardDisplay>().card.tipoDeCarta == global::Card.TipoDeCarta.silver)
      {
        carta.GetComponent<cardDisplay>().card.Damage -= 1;
        carta.GetComponent<cardDisplay>().card.AfectadaPorUnClima = true;
      }
      zone1.GetComponent<Tablero>().Suma += carta.GetComponent<cardDisplay>().card.Damage;
      }
    }  */
 /*}
 void climaAfecta(GameObject zone)
 {
  foreach(GameObject carta in zone.GetComponent<Tablero>().CartasEnZona)
    {
      if(carta.CompareTag("Card"))
      {
      if(carta.GetComponent<cardDisplay>().card.tipoDeCarta == global::Card.TipoDeCarta.silver)
      {
        carta.GetComponent<cardDisplay>().card.Damage -= 1;
        carta.GetComponent<cardDisplay>().card.AfectadaPorUnClima = true;
      }
      zone.GetComponent<Tablero>().Suma += carta.GetComponent<cardDisplay>().card.Damage;
      }
    }  
 }
 void Aumentos(string tag)
 {
    Debug.Log("voy a aumentar");
    zone = GameObject.FindGameObjectWithTag(tag);
    zone.GetComponent<Tablero>().Suma = 0;
    foreach(GameObject carta in zone.GetComponent<Tablero>().CartasEnZona)
    {
       if(carta.CompareTag("Card"))
       {     
      if(carta.GetComponent<cardDisplay>().card.tipoDeCarta == global::Card.TipoDeCarta.silver)
      {
        carta.GetComponent<cardDisplay>().card.Damage += 2;
        carta.GetComponent<cardDisplay>().card.Aumentada = true;
      } 
      zone.GetComponent<Tablero>().Suma += carta.GetComponent<cardDisplay>().card.Damage;  
       }
    }
 }
 void Despeje(string tag, string tag1)
 {
    zone = GameObject.FindGameObjectWithTag(tag);
    zone.GetComponent<Tablero>().Suma = 0;
    zone1 = GameObject.FindGameObjectWithTag(tag1);
    zone1.GetComponent<Tablero>().Suma = 0;
    AfectaDespeje(zone);
    foreach(GameObject carta in zone.GetComponent<Tablero>().CartasEnZona)
    {
       if(carta.CompareTag("Card"))
       {
      if(carta.GetComponent<cardDisplay>().card.AfectadaPorUnClima == true)
      {
        carta.GetComponent<cardDisplay>().card.Damage += 1;
        carta.GetComponent<cardDisplay>().card.AfectadaPorUnClima = false;
      }
      zone.GetComponent<Tablero>().Suma += carta.GetComponent<cardDisplay>().card.Damage;
       }
    }*/
    //AfectaDespeje(zone1);
    /*
     foreach(GameObject carta in zone1.GetComponent<Tablero>().CartasEnZona)
    {
       if(carta.CompareTag("Card"))
      {
      if(carta.GetComponent<cardDisplay>().card.tipoDeCarta == global::Card.TipoDeCarta.silver)
      {
        carta.GetComponent<cardDisplay>().card.Damage += 1;
        carta.GetComponent<cardDisplay>().card.AfectadaPorUnClima = false;
      }
      zone1.GetComponent<Tablero>().Suma += carta.GetComponent<cardDisplay>().card.Damage;
      }
    } 
  }
   void AfectaDespeje(GameObject zone)
   {
      foreach(GameObject carta in zone.GetComponent<Tablero>().CartasEnZona)
    {
       if(carta.CompareTag("Card"))
       {
      if(carta.GetComponent<cardDisplay>().card.AfectadaPorUnClima == true)
      {
        carta.GetComponent<cardDisplay>().card.Damage += 1;
        carta.GetComponent<cardDisplay>().card.AfectadaPorUnClima = false;
      }
      zone.GetComponent<Tablero>().Suma += carta.GetComponent<cardDisplay>().card.Damage;
       }
    }
   }
   void Mover(string tagZone, List<GameObject> cardsinhand)
    {
        Zone = GameObject.Find(tagZone);
        Card.transform.SetParent(Zone.transform, false);//mover el gameObject a la zona que le  corresponde
        Card.transform.position = Zone.transform.position; 
        cardsinhand.Remove(Card);//remover de la mano
        CardInfo.jugada = true;//ya se jugo la carta
    }
    void RemoverClima()
    {
      foreach(GameObject carta in Zone.GetComponent<SpecialZones>().CartasEnZona)
      {
        if(carta.GetComponent<SpecialCardsDisplay>().specialcard.tipoDeCarta == SpecialCards.TipoDeCarta.clima)
        {
          carta.transform.SetParent(Cementery.transform, false); // mover la carta clima al cementerio al cementerio
          carta.transform.position = Cementery.transform.position;
        }
      }
    }
  }   
}
 */