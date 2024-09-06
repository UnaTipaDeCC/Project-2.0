//using System.Reflection.Metadata;
using System.Collections.Generic;
using System;
public class Token
{
    public string Value { get; private set; }
    public TokenType Type { get; private set; }
    public CodeLocation Location { get; private set; }
    public Token(TokenType type,string value, CodeLocation location)
    {
        this.Type = type;
        this.Value = value;
        this.Location = location;
    }

    public override string ToString()
    {
        return string.Format("{0} [{1}]", Value, Type);
    }
}

public struct CodeLocation
{
    public string File;
    public int Line;
    public int Column;
    public override string ToString()
    {
        return String.Format("Line: {0}",Line);
    }
}

public enum TokenType
{
    Number,
    Text,
    Keyword,
    Symbol,
    Identifier,
    End,
    Unknown
}
public class TokenValues
{
    protected TokenValues() { }

    public const string Add = "Addition"; // +
    public const string Sub = "Subtract"; // -
    public const string Mul = "Multiplication"; // *
    public const string Div = "Division"; // /

    public const string Assign = "Assign"; // =
    public const string Point = "Point"; // .
    public const string DoublePoint = "DoublePoint"; // : 
    public const string ValueSeparator = "ValueSeparator"; // ,
    public const string StatementSeparator = "StatementSeparator"; // ;
    public const string EqualComparer = "EqualComparer";// ==
    public const string UnEqualComparer = "UnEqualComparer"; // !=
    public const string GraeterOrEqual = "GreaterOrEqual";// >=
    public const string LessOrEqual = "LessOrEqual";// <=
    public const string Increment = "Increment";// ++
    public const string Decrement = "Decrement";//--
    public const string Less ="Less"; // <
    public const string Greater = "Greater"; // >
    public const string AdditionAssignment = "AdditionAssignment"; // +=
    public const string SubtractionAssignment = "SubtractionAssignment"; // -=
    public const string Lambda = "Lambda";// =>
    public const string Negation = "Negation"; // ! 
    public const string Pow = "Pow"; // ^


    
// >= <= => ++ -- += -= > <

    public const string OpenBracket = "OpenBracket"; // (
    public const string ClosedBracket = "ClosedBracket"; // )
    public const string OpenBrace = "OpenBrace"; // [
    public const string ClosedBrace  = "ClosedBrace"; // ]
    public const string OpenCurlyBraces = "OpenCurlyBraces"; // {
    public const string ClosedCurlyBraces = "ClosedCurlyBraces"; // }
    public const string OrOperator = "OrOperator";// ||
    public const string AndOperator = "AndOperator";// &&
    public const string ConcatenationWithSpace = "ConcatenationWithSpace";// @@
    public const string ConcatenationWithoutSpace = "ConcatenationWithoutSpace"; // @
    

    public const string Effect = "Effect";
    public const string Action = "Action";
    public const string for_ = "for";
    public const string If = "if";
    public const string in_ = "in";
    public const string Else = "else";
    public const string while_ = "while";
    public const string Name = "Name";
    public const string Params = "Params";
    public const string card = "card";
    public const string Type = "Type";
    public const string Faction = "Faction";
    public const string Power = "Power";
    public const string Range = "Range";
    public const string Selector = "Selector";
    public const string Single = "Single";
    public const string declareEffect = "declareEffect";// effect
    public const string PostAction = "PostAction";
    public const string Predicate = "Predicate";
    public const string OnActivation = "OnActivation";
    public const string Source = "Source";
    public const string False = "false";
    public const string True = "true";
    public const string print = "print";
    public const string TriggerPlayer = "TriggerPlayer";
    public const string Board = "Board";
    public const string HandOfPlayer = "HandOfPlayer"; 
    public const string FieldOfPlayer = "FieldOfPlayer";
    public const string GraveyardOfPlayer = "GraveyardOfPlayer";
    public const string DeckOfPlayer = "DeckOfPlayer";
    public const string Owner = "Owner";
    public const string Find = "Find";
    public const string Push = "Push";
    public const string SendBottom = "SendBottom";
    public const string Pop = "Pop";
    public const string Remove = "Remove";
    public const string Suffle = "Suffle";
    public const string Hand = "Hand"; 
    public const string Field = "Fiel";
    public const string Graveyard = "Graveyard";
    public const string Deck = "Deck";
}   

