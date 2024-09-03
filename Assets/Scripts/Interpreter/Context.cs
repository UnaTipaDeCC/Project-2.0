using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
public class Context
{
    public List<string> Elements { get; private set; }
    public List<string> Cards { get; private set; }
    public Dictionary<string, ExpressionType> ContextReturnTypes { get; private set; }
    public Dictionary<string, ExpressionType> CardReturnTypes { get; private set; }
    public Dictionary<string, ExpressionType> ListReturnTypes { get; private set; }
    public Dictionary<string,Effect> Effects{ get;set; }
    public Dictionary<string, ExpressionType> ParamsOfMethodsTypes { get; private set; }
    public string[] PossiblesSources { get; private set; }
    public string[] PossiblesTypes { get; private set; }
    public Context()
    {
        Effects = new Dictionary<string,Effect>();
        Elements = new List<string>();
        Cards = new List<string>();
        ContextReturnTypes = new Dictionary<string,ExpressionType>
        {
            {"Board",ExpressionType.List},{"HandOfPlayer",ExpressionType.List},{"GraveyardOfPlayer",ExpressionType.List},
            {"FieldOfPlayer",ExpressionType.List},{"DeckOfPlayer",ExpressionType.List}, {"Hand",ExpressionType.List},
            {"Deck", ExpressionType.List}, {"Field",ExpressionType.List}, {"Graveryard", ExpressionType.List}
        };
        CardReturnTypes = new Dictionary<string,ExpressionType>
        {
            {"Type",ExpressionType.Text}, {"Power", ExpressionType.Number}, {"Name",ExpressionType.Text}, {"Faction",ExpressionType.Text},
            {"Owner", ExpressionType.Text}, {"Range",ExpressionType.List}
        };
        ListReturnTypes = new Dictionary<string,ExpressionType>
        {
            {"Find",ExpressionType.List},{"Push",ExpressionType.Void}, {"SendBottom",ExpressionType.Void},
            {"Pop",ExpressionType.Card}, {"Remove",ExpressionType.Void}, {"Shuffle",ExpressionType.Void}
        };
        PossiblesSources = new string[] 
        {
            "board","hand", "otherHand", "deck", "otherDeck","field", "otherField","parent"
        };
        PossiblesTypes = new string[] {"Oro", "Plata", "Clima" , "Aumento", "Lider", "Líder"};
        ParamsOfMethodsTypes = new Dictionary<string, ExpressionType>
        {
            {"HandOfPlayer", ExpressionType.Number}, {"FieldOfPlayer",ExpressionType.Number}, {"DeckOfPlayer",ExpressionType.Number},
            {"GraveyardOfPlayer",ExpressionType.Number}, {"Push",ExpressionType.Card}, {"Remove",ExpressionType.Card},
            {"SendBottom",ExpressionType.Card} // agregar el find

        };
    }
    public bool Contains(string key) => CardReturnTypes.ContainsKey(key) || ListReturnTypes.ContainsKey(key) || ContextReturnTypes.ContainsKey(key);
    public bool ContainsSource(string key) =>   PossiblesSources.Contains(key);
    public ExpressionType GetType(string key)
    {
        if (CardReturnTypes.TryGetValue(key, out var type)) return type;
        if (ListReturnTypes.TryGetValue(key, out type)) return type;
        if (ContextReturnTypes.TryGetValue(key, out type)) return type;
        return ExpressionType.ErrorType;
    }
    public ExpressionType GetCallerType(string key)
    {
        if(CardReturnTypes.ContainsKey(key)) return ExpressionType.Card;
        else if (ListReturnTypes.ContainsKey(key)) return ExpressionType.List;
        else return ExpressionType.Context;
    } 
}

