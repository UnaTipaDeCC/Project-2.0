using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CreatedCards
{
    public static List<CardGame> BravasCards = new List<CardGame>();
    public static List<CardGame> LocasCards = new List<CardGame>();

    public static void AddToDeck()
    {
        foreach (var card in BravasCards) GameContext.Instance.BravasPlayer.GetComponent<Player>().Deck.Add(card);
        foreach (var card in LocasCards) GameContext.Instance.LocasPlayer.GetComponent<Player>().Deck.Add(card);
    }

}
