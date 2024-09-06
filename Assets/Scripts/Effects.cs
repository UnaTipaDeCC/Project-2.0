using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Assertions.Must;

public class Effects : MonoBehaviour
{
    public static Effects Instance { get; private set; }

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
    public void ExecuteEffect(CardGame card)
    {
        switch (card.Effect)
        { 
        case CardGame.effects.RemoveLowestPowerCardFromOpponent:
        RemoveLowestPowerCardFromOpponent();
        break;
        case CardGame.effects.RemoveGreatestPowerCardFromOpponent:
        RemoveGreatestPowerCardFromOpponent();
        break;
        case CardGame.effects.Especial:
        foreach (var effect in card.EffectsList) effect.Execute();
        break;
        case CardGame.effects.Stole:
        GameContext.Instance.ReturnPlayer(card.Owner).Stole(1);
        break;
        case CardGame.effects.EqualizeCardPowerToAverageOfOwnFieldCards:
        EqualizeCardPowerToAverageOfOwnFieldCards();
        break;
        case CardGame.effects.ClearListWithLeastCards:
        ClearListWithLeastCards();
        break;
        case CardGame.effects.None:
        break;
        case CardGame.effects.MultiplyCardPowerByCount:
        MultiplyCardPowerByCount(card);
        break;
        case CardGame.effects.WeatherEffect:
        WeatherEffect(card.Range); 
        break;
        case CardGame.effects.IncreasEffect:
        IncreasEffect(card);
        break;
        case CardGame.effects.BravasLiderEffect:
        BravasLiderEffect();
        break;
        case CardGame.effects.LocasLiderEffect:
        LocasLiderEffect();
        break;
        }
    }
    #region  BravasEffects
    private void RemoveLowestPowerCardFromOpponent()
    {
        Debug.Log("estoy en el efecto de lowest..");
        Player player = GameContext.Instance.LocasPlayer.GetComponent<Player>();
        CardGame lowestCard = new CardGame();
        GameObject zone = GetCard(player,player.Melee.GetComponent<Zones>().CardsInZone,player.Siege.GetComponent<Zones>().CardsInZone,player.Ranged.GetComponent<Zones>().CardsInZone,lowestCard, true);
        Debug.Log(lowestCard.name + "es la de menos puntos");
        if(lowestCard.Type == CardGame.type.Plata)
        {
            player.Cementery.Add(lowestCard);
            zone.GetComponent<Zones>().CardsInZone.Remove(lowestCard);
            zone.GetComponent<Zones>().RefreshZone();
        }  
    }
    private void RemoveGreatestPowerCardFromOpponent()
    {
        Player player = GameContext.Instance.LocasPlayer.GetComponent<Player>();
        CardGame greatestCard = new CardGame();
        GameObject zone = GetCard(player,player.Melee.GetComponent<Zones>().CardsInZone,player.Siege.GetComponent<Zones>().CardsInZone,player.Melee.GetComponent<Zones>().CardsInZone,greatestCard, false);
        {
            player.Cementery.Add(greatestCard);
            zone.GetComponent<Zones>().CardsInZone.Remove(greatestCard);
            zone.GetComponent<Zones>().RefreshZone();
        }
    }
    private void MultiplyCardPowerByCount(CardGame card)
    {
        Player player = GameContext.Instance.BravasPlayer.GetComponent<Player>();
        int contador = 0; // cero o uno
        foreach(CardGame cardGame in player.Ranged.GetComponent<Zones>().CardsInZone)
        {
            if(cardGame.Name == "Hormigatrix")
            {
                contador++;
            }
        } 
        card.Damage *= contador;   
    }
    private void BravasLiderEffect() //amuenta en tres el dano de las cartas de plata de la zona del jugador propio que menos cartas tiene
    {
        Player player = GameContext.Instance.BravasPlayer.GetComponent<Player>();
        GameObject zone = GetZoneWithLeastCards(player,player.Melee.GetComponent<Zones>().CardsInZone,player.Siege.GetComponent<Zones>().CardsInZone,player.Ranged.GetComponent<Zones>().CardsInZone);
        SetCardPowerToValue(zone.GetComponent<Zones>().CardsInZone,3,false,true);
        zone.GetComponent<Zones>().RefreshZone();
    }
    #endregion
    #region LocasEffects
    private void EqualizeCardPowerToAverageOfOwnFieldCards()
    {
        Player player = GameContext.Instance.LocasPlayer.GetComponent<Player>();
        int averagePower = CalculateAveragePower(player.Melee.GetComponent<Zones>().CardsInZone,player.Ranged.GetComponent<Zones>().CardsInZone,player.Siege.GetComponent<Zones>().CardsInZone);
        SetCardPowerToValue(player.Melee.GetComponent<Zones>().CardsInZone, averagePower,true,false);
        SetCardPowerToValue(player.Siege.GetComponent<Zones>().CardsInZone, averagePower,true,false);
        SetCardPowerToValue(player.Ranged.GetComponent<Zones>().CardsInZone, averagePower,true,false);
        player.Melee.GetComponent<Zones>().RefreshZone();
        player.Ranged.GetComponent<Zones>().RefreshZone();
        player.Siege.GetComponent<Zones>().RefreshZone();
    }
   /* private void ClearListWithLeastCards()
    {
        Player player = GameContext.Instance.BravasPlayer.GetComponent<Player>();
        GameObject zone = GetZoneWithLeastCards(player,player.Melee.GetComponent<Zones>().CardsInZone,player.Siege.GetComponent<Zones>().CardsInZone,player.Melee.GetComponent<Zones>().CardsInZone);
        foreach(CardGame card in zone.GetComponent<Zones>().CardsInZone) 
        {
            if(card.Type == CardGame.type.Plata)
            {}
            player.Cementery.Add(card);
        }
        zone.GetComponent<Zones>().RefreshZone();
    }*/
    private void ClearListWithLeastCards()
    {
        // Obtener el jugador
        Player player = GameContext.Instance.BravasPlayer.GetComponent<Player>();
        // Obtener la zona con menos cartas
        GameObject zone = GetZoneWithLeastCards(player, player.Melee.GetComponent<Zones>().CardsInZone, player.Siege.GetComponent<Zones>().CardsInZone, player.Melee.GetComponent<Zones>().CardsInZone);
        // Obtener la lista de cartas en la zona seleccionada
        List<CardGame> cardsInZone = zone.GetComponent<Zones>().CardsInZone;
        
        // Iterar a través de las cartas en la zona
        for (int i = cardsInZone.Count - 1; i >= 0; i--) // Iterar hacia atrás para evitar modificar la lista mientras se itera
        {
            CardGame card = cardsInZone[i];
            
            // Verificar si el tipo de carta es Plata
            if (card.Type == CardGame.type.Plata)
            {
                // Agregar la carta al cementerio
                player.Cementery.Add(card);
                
                // Eliminar la carta de la zona actual
                cardsInZone.RemoveAt(i);
            }
        }
        // Actualizar la zona después de las modificaciones
        zone.GetComponent<Zones>().RefreshZone();
    }
    private void LocasLiderEffect()//aumenta en uno el dano de todas las cartas de plata del campo del jugador
    {
        Player player = GameContext.Instance.LocasPlayer.GetComponent<Player>();
        GameObject melee = player.Melee;
        GameObject siege = player.Siege;
        GameObject ranged = player.Ranged;
        SetCardPowerToValue(melee.GetComponent<Zones>().CardsInZone,1,false,true);
        SetCardPowerToValue(siege.GetComponent<Zones>().CardsInZone,1,false,true);
        SetCardPowerToValue(ranged.GetComponent<Zones>().CardsInZone,1,false,true);
        //ACTUALIZAR LOS PUNTOS DE CADA ZONA
        melee.GetComponent<Zones>().RefreshZone();
        siege.GetComponent<Zones>().RefreshZone();
        ranged.GetComponent<Zones>().RefreshZone();
    }
    #endregion
    #region CommonEffects
    public void IncreasEffect(CardGame cardGame)
    {
        Player player = GameContext.Instance.ReturnPlayer(cardGame.Owner);
        GameObject zone = new GameObject();
        DetermineZone(cardGame.Range, zone, player);
        SetCardPowerToValue(zone.GetComponent<Zones>().CardsInZone,1,false,true);
    }
    private void WeatherEffect(string zone)
    {
        Player player = GameContext.Instance.BravasPlayer.GetComponent<Player>();
        Player player1 = GameContext.Instance.BravasPlayer.GetComponent<Player>();
        GameObject zone1 = new GameObject();
        GameObject zone2 = new GameObject();
        DetermineZone(zone,zone1,player);
        DetermineZone(zone,zone2, player1);
        SetCardPowerToValue(zone1.GetComponent<Zones>().CardsInZone,1,false,false);
        SetCardPowerToValue(zone2.GetComponent<Zones>().CardsInZone,1,false,false);
    }
    #endregion
    #region Utils
    private void DetermineZone(string zoneName, GameObject zone, Player player)
    {
        switch(zoneName)
        {
            case "Melee":
            zone = player.Melee;
            break;
            case "Ranged":
            zone = player.Ranged;
            break;
            case "Siege":
            zone = player.Siege;
            break;
            default: throw new ArgumentException("Invalid range");
        }
    }
    private GameObject GetCard(Player player, List<CardGame> meleeList, List<CardGame> siegeList, List<CardGame> rangedList, CardGame card, bool isLowest)
    {
        GameObject Zone;
        // Crear una lista para almacenar todas las cartas
        List<CardGame> AllCards = new List<CardGame>();

        // Agregar las cartas de cada zona a la lista
        AllCards.AddRange(meleeList);
        AllCards.AddRange(siegeList);
        AllCards.AddRange(rangedList);

        // Encontrar la carta con el menor poder
        card = isLowest ? AllCards.OrderBy(c => c.Damage).FirstOrDefault() : AllCards.OrderByDescending(c => c.Damage).FirstOrDefault();

        // Determinar la zona en función de la carta con menor poder
        if (card != null)
        {
            if (meleeList.Contains(card))
            {
                return player.Melee;
            }
            else if (rangedList.Contains(card))
            {
                return player.Ranged;
            }
            else if (siegeList.Contains(card))
            {
                return player.Siege;
            }
        }
        throw new Exception("Card not found");
    }
    private void SetCardPowerToValue(List<CardGame> cards, int powerValue, bool igualate, bool increas)
    {
        if(!igualate) //significa que debe ser tratada como un efecto de clima
        {
           foreach (CardGame card in cards)
            {
                if(card.Type == CardGame.type.Plata)
                {
                    if(!increas) // verificar que no es un aumento y que por defecto seria un clima
                    {
                        if(!card.AffectedByClimate) 
                        card.Damage += powerValue; // Actualiza el poder de cada carta
                    }
                    else card.Damage += powerValue; // Actualiza el poder de cada carta
                }
            }
        }
        else
        {
            foreach (CardGame card in cards)
            {
                if(card.Type == CardGame.type.Plata) 
                    card.Damage = powerValue; // Asigna el poder a cada carta 
            }
        } 
    }
    private int CalculateAveragePower(List<CardGame> meleeCards, List<CardGame> rangedCards, List<CardGame> siegeCards)
    {
        // Crear una lista que contenga todas las cartas
        List<CardGame> allCards = new List<CardGame>();
        allCards.AddRange(meleeCards);
        allCards.AddRange(rangedCards);
        allCards.AddRange(siegeCards);
        // Calcular la suma total del poder de todas las cartas
        int totalPower = allCards.Sum(card => card.Damage);
        // Calcular el número total de cartas
        int totalCards = allCards.Count;
        // Calcular el promedio del poder
        int averagePower = totalCards > 0 ? totalPower / totalCards : 0;
        return averagePower;
    }
    private GameObject GetZoneWithLeastCards(Player player, List<CardGame> meleeList, List<CardGame> siegeList, List<CardGame> rangedList)
    {
        // Determinar cuál lista tiene menos cartas
        List<CardGame> listWithLeastCards = meleeList;

        if (siegeList.Count < listWithLeastCards.Count)
        {
            listWithLeastCards = siegeList;
        }
        if (rangedList.Count < listWithLeastCards.Count)
        {
            listWithLeastCards = rangedList;
        }
        // Devolver la zona correspondiente
        if (listWithLeastCards == meleeList)
        {
            return player.Melee;
        }
        else if (listWithLeastCards == rangedList)
        {
            return player.Ranged;
        }
        else if (listWithLeastCards == siegeList)
        {
            return player.Siege;
        }
        throw new Exception("No se encontró ninguna zona válida.");
    }
    #endregion
}
