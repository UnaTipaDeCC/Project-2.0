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
        }
    }
    #region  BravasEffects
    private void RemoveLowestPowerCardFromOpponent()
    {
        Player player = GameContext.Instance.LocasPlayer.GetComponent<Player>();
        CardGame lowestCard = new CardGame();
        GameObject zone = GetCard(player,player.Melee.GetComponent<Zones>().CardsInZone,player.Siege.GetComponent<Zones>().CardsInZone,player.Ranged.GetComponent<Zones>().CardsInZone,lowestCard, true);
        player.Cementery.Add(lowestCard);
        zone.GetComponent<Zones>().CardsInZone.Remove(lowestCard);
        zone.GetComponent<Zones>().RefreshZone();
    }
    private void RemoveGreatestPowerCardFromOpponent()
    {
        Player player = GameContext.Instance.LocasPlayer.GetComponent<Player>();
        CardGame lowestCard = new CardGame();
        GameObject zone = GetCard(player,player.Melee.GetComponent<Zones>().CardsInZone,player.Siege.GetComponent<Zones>().CardsInZone,player.Melee.GetComponent<Zones>().CardsInZone,lowestCard, false);
        player.Cementery.Add(lowestCard);
        zone.GetComponent<Zones>().CardsInZone.Remove(lowestCard);
        zone.GetComponent<Zones>().RefreshZone();
    }
    #endregion
    #region LocasEffects
    private void EqualizeCardPowerToAverageOfOwnFieldCards()
    {
        Player player = GameContext.Instance.LocasPlayer.GetComponent<Player>();
        int averagePower = CalculateAveragePower(player.Melee.GetComponent<Zones>().CardsInZone,player.Ranged.GetComponent<Zones>().CardsInZone,player.Siege.GetComponent<Zones>().CardsInZone);
        SetCardPowerToValue(player.Melee.GetComponent<Zones>().CardsInZone, averagePower,true);
        SetCardPowerToValue(player.Siege.GetComponent<Zones>().CardsInZone, averagePower,true);
        SetCardPowerToValue(player.Ranged.GetComponent<Zones>().CardsInZone, averagePower,true);
        player.Melee.GetComponent<Zones>().RefreshZone();
        player.Ranged.GetComponent<Zones>().RefreshZone();
        player.Siege.GetComponent<Zones>().RefreshZone();
    }
    private void ClearListWithLeastCards()
    {
        Player player = GameContext.Instance.BravasPlayer.GetComponent<Player>();
        GameObject zone = GetZoneWithLeastCards(player,player.Melee.GetComponent<Zones>().CardsInZone,player.Siege.GetComponent<Zones>().CardsInZone,player.Melee.GetComponent<Zones>().CardsInZone);
        foreach(CardGame card in zone.GetComponent<Zones>().CardsInZone) player.Cementery.Add(card);
        zone.GetComponent<Zones>().RefreshZone();
    }

    #endregion
    private void WeatherEffect(string zone)
    {
        Player player = GameContext.Instance.BravasPlayer.GetComponent<Player>();
        Player player1 = GameContext.Instance.BravasPlayer.GetComponent<Player>();
        GameObject zone1 = new GameObject();
        GameObject zone2 = new GameObject();
        switch(zone)
        {
            case "Melee":
            zone1 = player.Melee;
            zone2 = player1.Melee;
            break;
            case "Ranged":
            zone1 = player.Ranged;
            zone2 = player1.Ranged;
            break;
            case "Siege":
            zone1 = player.Siege;
            zone2 = player1.Siege;
            break;
            default: throw new ArgumentException("Invalid range");
        }
        SetCardPowerToValue(zone1.GetComponent<Zones>().CardsInZone,1,false);
        SetCardPowerToValue(zone2.GetComponent<Zones>().CardsInZone,1,false);
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
    private void SetCardPowerToValue(List<CardGame> cards, int powerValue, bool igualate)
    {
        if(igualate)
        {
            foreach (CardGame card in cards)
            {
                card.Damage = powerValue; // Asigna el poder a cada carta
            }
        }
        else 
        {
            foreach (CardGame card in cards)
            {
                if(!card.AffectedByClimate) card.Damage += powerValue; // Actualiza el poder de cada carta
                
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
