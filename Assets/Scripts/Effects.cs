/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

public class Effects : MonoBehaviour
{
    private GameObject filaMelee;
    private GameObject hand;
    private GameObject deck;
    private List<GameObject> cardsinhand;
    private List<GameObject> cardsindeck;
    private GameObject filaRanged;
    private GameObject filaSiege;
    private List<GameObject> melee;
    private List<GameObject> ranged;
    private List<GameObject> siege;
    private GameObject cartam;
    private GameObject cartar;
    private GameObject cartas;
    private GameManager gameManager;
    private GameObject Cementery;
    private bool ActivoElEfecto = false;
    private bool condicion;


    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }
    public void RobarUnaCarta()
    {
        deck = GameObject.FindGameObjectWithTag("Deck(1)");
        if(ActivoElEfecto == false)
        {
            if(!gameManager.IsPlayerOneTurn)
            {  
            deck.GetComponent<Draw>().ROBAR();
            ActivoElEfecto = true;
            }
        }
        else Debug.Log("YA ACTIVASTE EL EFECTO");
    }
    public void EliminarCartaConMenosPoderDelRival()
    {
        if(gameManager.IsPlayerOneTurn)
        {
            if(ActivoElEfecto == false)
            {
                Debug.Log("activo esto xq es mi turno");
                filaMelee = GameObject.FindGameObjectWithTag("MeleeZone (1)");
                melee = filaMelee.GetComponent<Tablero>().CartasEnZona;
                filaRanged = GameObject.FindGameObjectWithTag("RangedZone (1)");
                ranged = filaRanged.GetComponent<Tablero>().CartasEnZona;
                filaSiege = GameObject.FindGameObjectWithTag("SiegeZone (1)");
                siege = filaSiege.GetComponent<Tablero>().CartasEnZona;
                Cementery = GameObject.FindGameObjectWithTag("Cementery (1)");    
                if(melee.Count!= 0 || ranged.Count!= 0 || siege.Count!= 0)
                {
                    if(melee.Count != 0) cartam = melee[0];
                    else if(siege.Count != 0) cartam = siege[0];
                    else if(ranged.Count != 0) cartam = ranged[0];
                    ComprobarMenor(melee);
                    ComprobarMenor(siege);
                    ComprobarMenor(ranged);
                    /*foreach(GameObject carta in melee)
                    {
                        if(carta.CompareTag("Card"))
                        {
                            if(carta.GetComponent<cardDisplay>().card.Damage < cartam.GetComponent<cardDisplay>().card.Damage)
                            {
                                cartam = carta;
                            }
                        }
                    }
                    foreach(GameObject carta in siege)
                    {
                        if(carta.CompareTag("Card"))
                        {
                            if(carta.GetComponent<cardDisplay>().card.Damage < cartam.GetComponent<cardDisplay>().card.Damage)
                            {
                                cartam = carta;
                            }
                        }
                    }
                    foreach(GameObject carta in ranged)
                    {
                        if(carta.CompareTag("Card"))
                        {
                            if(carta.GetComponent<cardDisplay>().card.Damage < cartam.GetComponent<cardDisplay>().card.Damage)
                            {
                                cartam = carta;
                            }
                        }
                    }
                    cartam.transform.SetParent(Cementery.transform, false); // mover la carta a la zona deseada 
                    cartam.transform.position = Cementery.transform.position;
                }
                ActivoElEfecto = true;
            }    
        }
    }
    void ComprobarMenor(List<GameObject> zona)
    {
        foreach(GameObject carta in zona)
        {
            if(carta.CompareTag("Card"))
            {
                if(carta.GetComponent<cardDisplay>().card.Damage < cartam.GetComponent<cardDisplay>().card.Damage)
                {
                    cartam = carta;
                }
            }
        }
    }
    public void EliminarCartaConMasPoderDelRival()
    {
        if(gameManager.IsPlayerOneTurn)
        {
            if(ActivoElEfecto == false)
            {
                Debug.Log("activo esto xq es mi turno");
                filaMelee = GameObject.FindGameObjectWithTag("MeleeZone (1)");
                melee = filaMelee.GetComponent<Tablero>().CartasEnZona;
                filaRanged = GameObject.FindGameObjectWithTag("RangedZone (1)");
                ranged = filaRanged.GetComponent<Tablero>().CartasEnZona;
                filaSiege = GameObject.FindGameObjectWithTag("SiegeZone (1)");
                siege = filaSiege.GetComponent<Tablero>().CartasEnZona;
                Cementery = GameObject.FindGameObjectWithTag("Cementery (1)");
                if(melee.Count != 0 || siege.Count != 0 || ranged.Count != 0)
                {
                    if(melee.Count != 0) cartam = melee[0];
                    else if(siege.Count != 0) cartam = siege[0];
                    else if(ranged.Count != 0) cartam = ranged[0];
                    ComprobarMayor(melee);
                    ComprobarMayor(siege);
                    ComprobarMayor(ranged);
                    /*foreach(GameObject carta in melee)
                    {
                    if(carta.CompareTag("Card"))
                        {
                            if(carta.GetComponent<cardDisplay>().card.Damage > cartam.GetComponent<cardDisplay>().card.Damage) 
                            {
                                cartam = carta;
                            }
                        }    
                    }
                    foreach(GameObject carta in siege)
                    {
                        if(carta.CompareTag("Card"))
                        {
                            if(carta.GetComponent<cardDisplay>().card.Damage > cartam.GetComponent<cardDisplay>().card.Damage) 
                            {
                                cartam = carta;
                            }
                        }
                    }   
                    foreach(GameObject carta in ranged)
                    {
                        if(carta.CompareTag("Card"))
                        {
                            if(carta.GetComponent<cardDisplay>().card.Damage > cartam.GetComponent<cardDisplay>().card.Damage) 
                            {
                                cartam = carta;
                            }
                        }
                    } 
                    cartam.transform.SetParent(Cementery.transform, false); // mover la carta a la zona deseada 
                    cartam.transform.position = Cementery.transform.position;                
                }
                ActivoElEfecto = true;
            }
            else Debug.Log("ya active el efecto");
        }
        else Debug.Log("no afecto porque no es mi turno");
    }
    void ComprobarMayor(List<GameObject> zona)
    {
        foreach(GameObject carta in zona)
        {
            if(carta.CompareTag("Card"))
            {
                if(carta.GetComponent<cardDisplay>().card.Damage > cartam.GetComponent<cardDisplay>().card.Damage)
                {
                    cartam = carta;
                }
            }
        }
    }
    public void MultiplicarPor_n_ElAtaque()//siendo n la cantidad de cartas igual a ella en el campo
    {
        if(gameManager.IsPlayerOneTurn)
        {
            if(ActivoElEfecto == false)
            {
                Debug.Log("activo esto xq es mi turno");
                filaRanged = GameObject.FindGameObjectWithTag("RangedZone");
                ranged = filaRanged.GetComponent<Tablero>().CartasEnZona;
                int contador = 1;
                foreach(GameObject carta in ranged)
                {
                    if(carta.CompareTag("Card"))
                    {
                        if(carta.GetComponent<cardDisplay>().card.name == "Hormigatrixx")
                        {
                            contador++;
                        }
                    }
                }
                GetComponent<cardDisplay>().card.Damage *= contador;
                ActivoElEfecto = true;
            }  
            else Debug.Log("ya active el efecto");  
        }
        else Debug.Log("no afecto xq no es mi turno");
    }
   public void IgualarPoderDeCartasAlPromedioDeCartasDelCampoPropio()
    {
        if(!gameManager.IsPlayerOneTurn)
        {
            if(ActivoElEfecto == false)
            {
                Debug.Log("activo esto xq es mi turno");
                filaMelee = GameObject.FindGameObjectWithTag("MeleeZone (1)");
                melee = filaMelee.GetComponent<Tablero>().CartasEnZona;
                filaRanged = GameObject.FindGameObjectWithTag("RangedZone (1)");
                ranged = filaRanged.GetComponent<Tablero>().CartasEnZona;
                filaSiege = GameObject.FindGameObjectWithTag("SiegeZone (1)");
                siege = filaSiege.GetComponent<Tablero>().CartasEnZona;
                if(melee.Count != 0 || ranged.Count != 0 || siege.Count != 0)
                {
                    int promedio = (GameObject.FindGameObjectWithTag("SiegeZone (1)").GetComponent<Tablero>().Suma + GameObject.FindGameObjectWithTag("MeleeZone (1)").GetComponent<Tablero>().Suma + GameObject.FindGameObjectWithTag("RangedZone (1)").GetComponent<Tablero>().Suma)/(melee.Count + siege.Count + ranged.Count);
                    Debug.Log("EL PROMEDIO ES" + promedio);
                    GameObject.FindGameObjectWithTag("SiegeZone (1)").GetComponent<Tablero>().Suma = 0;
                    GameObject.FindGameObjectWithTag("MeleeZone (1)").GetComponent<Tablero>().Suma = 0;
                    GameObject.FindGameObjectWithTag("RangedZone (1)").GetComponent<Tablero>().Suma = 0;
                    IgualarPoder(melee,promedio,"MeleeZone (1)");
                    IgualarPoder(siege,promedio,"SiegeZone (1)");
                    IgualarPoder(ranged,promedio,"RangedZone (1)");
                }
                ActivoElEfecto = true;
            }       
            else Debug.Log("ya active el efecto");
        }
        else Debug.Log("no afecto xq no es mi turno");
    }
    void IgualarPoder(List<GameObject> lista, int promedio, string tag)
    {
        foreach(GameObject carta in lista)
                    {
                        if(carta.CompareTag("Card"))
                        {
                            carta.GetComponent<cardDisplay>().card.Damage = promedio;
                            GameObject.FindGameObjectWithTag(tag).GetComponent<Tablero>().Suma += carta.GetComponent<cardDisplay>().card.Damage;
                        }
                    }
    }
    public void LimpiaLaFilaConMenosUnidades()
    {
        if(!gameManager.IsPlayerOneTurn)
        {
            if(ActivoElEfecto == false)
            {
                Debug.Log("activo esto xq es mi turno");
                filaMelee = GameObject.FindGameObjectWithTag("MeleeZone");
                melee = filaMelee.GetComponent<Tablero>().CartasEnZona;
                filaRanged = GameObject.FindGameObjectWithTag("RangedZone");
                ranged = filaRanged.GetComponent<Tablero>().CartasEnZona;
                filaSiege = GameObject.FindGameObjectWithTag("SiegeZone");
                siege = filaSiege.GetComponent<Tablero>().CartasEnZona;
                Cementery = GameObject.FindGameObjectWithTag("Cementery");
                if(GetListaNoVaciaMenorElementos(melee,ranged,siege).Count > 0)
                {
                    foreach(GameObject carta in GetListaNoVaciaMenorElementos(melee,ranged,siege))
                    {
                        carta.transform.SetParent(Cementery.transform, false);
                        carta.transform.position = Cementery.transform.position; 
                    }
                }
                else Debug.Log("todas las listas estan vacias");
                ActivoElEfecto = true;
            }
            else Debug.Log("YA ACTIVE EL EFECTO");
        }
        else Debug.Log("no afecto xq no es mi turno");
    }
    public static List<GameObject> GetListaNoVaciaMenorElementos(List<GameObject> lista1, List<GameObject> lista2, List<GameObject> lista3)
    {
    // Una lista de listas no vacías
    List<List<GameObject>> listasNoVacias = new List<List<GameObject>>();
    // Agrega las listas no vacías a la lista de listas
    if (lista1.Count > 0) {listasNoVacias.Add(lista1);}
    if (lista2.Count > 0) {listasNoVacias.Add(lista2);}
    if (lista3.Count > 0) {listasNoVacias.Add(lista3);}
    // Si no hay ninguna lista no vacía, devolver una lista vacía
    if (listasNoVacias.Count == 0) {return new List<GameObject>();}
    // Ordena las listas no vacías por la cantidad de elementos
    listasNoVacias = listasNoVacias.OrderBy(l => l.Count).ToList();
    // Devuelve la primera lista (la que tiene menos elementos)
    return listasNoVacias.First();
    }
}*/