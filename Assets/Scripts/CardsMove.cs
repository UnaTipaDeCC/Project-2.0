using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using UnityEditor;
using UnityEngine.XR;

public class CardsMove : MonoBehaviour
{
    
    public static CardsMove Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para mover la carta
    public void MoveCard(CardGame card)
    {
        Player owner = GameContext.Instance.ReturnPlayer(card.Owner);
        GameObject zone ;
        switch (card.Type)
        {
            case CardGame.type.Aumento:
            if(card.Range == "Melee") zone = owner.MeleeIncrement;
            else if (card.Range == "Siege") zone = owner.SiegeIncrement;
            else if(card.Range == "Ranged") zone = owner.RangedIncrement;
            break;
            case CardGame.type.Clima:
            zone = GameContext.Instance.WeatherZone;
            break;
            default: 
            if(card.Range == "Melee") zone = owner.Melee;
            else if (card.Range == "Siege") zone = owner.Siege;
            else if(card.Range == "Ranged") zone = owner.Ranged;
            break;
        }
        switch (card.Range)
        {
            case "Melee":
            Move(owner.Melee, card,owner);
            break;
            case "Ranged":
            Move(owner.Ranged, card,owner);
            break;
            case "Siege":
            Move(owner.Siege, card,owner);
            break;
            //LAS DE AUMENTO Y LAS CLIMAS 
        }
        Debug.Log("Moviendo la carta: " + card.Name);
    }
    public void Move(GameObject zone, CardGame cardGame, Player owner)
    {
        zone.GetComponent<Zones>().CardsInZone.Add(cardGame);
        owner.Hand.GetComponent<Zones>().CardsInZone.Remove(cardGame);
        //update zonde 
        
        
        //card.ExecuteEffect();
        //ContextGame.contextGame.UpdateFront();
        /*zone.GetComponent<Zones>().CardsInZone.Add(cardGame);
        hand.GetComponent<Zones>().CardsInZone.Remove(cardGame);
        zone.GetComponent<Zones>().RefreshZone();
        //InstantiateCard(zone,cardGame);
        //deberia actualizar las listas de los players


        /*cardGame.transform.SetParent(zone.transform, false);//mover el gameObject a la zona que le  corresponde
        cardGame.transform.position = zone.transform.position;
        cardGame.Card.Played = true; */
    }
    /*public void InstantiateCard(GameObject zone, CardGame cardGame)
    {
        Debug.Log("estoy en el instatiate");
        if (cardGame == null)
{
    Debug.LogError("El objeto CardGame proporcionado es null.");
    return;
}
if (zone == null)
{
    Debug.LogError("El objeto GameObject de la zona proporcionado es null.");
    return;
}
        string cardPath = "Assets/Prefabs/Card.prefab";
        GameObject cardPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(cardPath);
        if (cardPrefab != null && cardGame != null)
        {
            // Crear una instancia del prefab
            GameObject cardInstance = Instantiate(cardPrefab, zone.transform.position, Quaternion.identity);
            // Asignar el Scriptable Object al componente de visualización de la carta
            cardInstance.GetComponent<CardDisplay>().Card = cardGame;
            Debug.Log("Carta instanciada: " + cardGame.Name);
        }
        else
        {
            Debug.LogError("No se pudo cargar el prefab o el Scriptable Object.");
        }
    }*/
}
