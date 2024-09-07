/* Lexical analysis. Allows to split a raw Text representing the program into 
 the first abstract elements (tokens). */
public class Compiling
{
    private static LexicalAnalyzer? __LexicalProcess;
    public static LexicalAnalyzer Lexical
    {
        get
        {
            if (__LexicalProcess == null)
            {
                __LexicalProcess = new LexicalAnalyzer();


                __LexicalProcess.RegisterOperator("+", TokenValues.Add);
                __LexicalProcess.RegisterOperator("*", TokenValues.Mul);
                __LexicalProcess.RegisterOperator("-", TokenValues.Sub);
                __LexicalProcess.RegisterOperator("/", TokenValues.Div);
                __LexicalProcess.RegisterOperator("=", TokenValues.Assign);
                __LexicalProcess.RegisterOperator(".",TokenValues.Point);

                __LexicalProcess.RegisterOperator(",", TokenValues.ValueSeparator);
                __LexicalProcess.RegisterOperator(";", TokenValues.StatementSeparator);
                __LexicalProcess.RegisterOperator("(", TokenValues.OpenBracket);
                __LexicalProcess.RegisterOperator(")", TokenValues.ClosedBracket);
                __LexicalProcess.RegisterOperator("{", TokenValues.OpenCurlyBraces);
                __LexicalProcess.RegisterOperator("}", TokenValues.ClosedCurlyBraces);
                __LexicalProcess.RegisterOperator("&&",TokenValues.AndOperator);
                __LexicalProcess.RegisterOperator("||",TokenValues.OrOperator);
                __LexicalProcess.RegisterOperator("@",TokenValues.ConcatenationWithoutSpace);
                __LexicalProcess.RegisterOperator("@@",TokenValues.ConcatenationWithSpace);
                __LexicalProcess.RegisterOperator("=>", TokenValues.Lambda);
                __LexicalProcess.RegisterOperator(">=",TokenValues.GraeterOrEqual);
                __LexicalProcess.RegisterOperator("-=",TokenValues.SubtractionAssignment);
                __LexicalProcess.RegisterOperator("+=",TokenValues.AdditionAssignment);
                __LexicalProcess.RegisterOperator("++",TokenValues.Increment);
                __LexicalProcess.RegisterOperator("--",TokenValues.Decrement);
                __LexicalProcess.RegisterOperator("<",TokenValues.Less);
                __LexicalProcess.RegisterOperator(">",TokenValues.Greater);
                __LexicalProcess.RegisterOperator("==",TokenValues.EqualComparer);
                __LexicalProcess.RegisterOperator("!=",TokenValues.UnEqualComparer);
                __LexicalProcess.RegisterOperator("<=",TokenValues.LessOrEqual);
                __LexicalProcess.RegisterOperator("!",TokenValues.Negation);
                __LexicalProcess.RegisterOperator("^",TokenValues.Pow);
                __LexicalProcess.RegisterOperator(":",TokenValues.DoublePoint);
                __LexicalProcess.RegisterOperator("[",TokenValues.OpenBrace);
                __LexicalProcess.RegisterOperator("]",TokenValues.ClosedBrace);
              
                __LexicalProcess.RegisterKeyword("Range",TokenValues.Range);
                __LexicalProcess.RegisterKeyword("Name",TokenValues.Name);
                __LexicalProcess.RegisterKeyword("Effect",TokenValues.Effect);
                __LexicalProcess.RegisterKeyword("Action",TokenValues.Action);
                __LexicalProcess.RegisterKeyword("Faction",TokenValues.Faction);
                __LexicalProcess.RegisterKeyword("card",TokenValues.card);
                __LexicalProcess.RegisterKeyword("for",TokenValues.for_);
                __LexicalProcess.RegisterKeyword("in",TokenValues.in_);
                __LexicalProcess.RegisterKeyword("while",TokenValues.while_);
                __LexicalProcess.RegisterKeyword("Type",TokenValues.Type);
                __LexicalProcess.RegisterKeyword("Power",TokenValues.Power);
                __LexicalProcess.RegisterKeyword("Params",TokenValues.Params);
                __LexicalProcess.RegisterKeyword("Selector",TokenValues.Selector);
                __LexicalProcess.RegisterKeyword("Single",TokenValues.Single);
                __LexicalProcess.RegisterKeyword("effect",TokenValues.declareEffect);
                __LexicalProcess.RegisterKeyword("Source", TokenValues.Source);
                __LexicalProcess.RegisterKeyword("PostAction",TokenValues.PostAction);
                __LexicalProcess.RegisterKeyword("OnActivation",TokenValues.OnActivation);
                __LexicalProcess.RegisterKeyword("Predicate",TokenValues.Predicate);  
                __LexicalProcess.RegisterKeyword("false",TokenValues.False);
                __LexicalProcess.RegisterKeyword("true",TokenValues.True);   
                __LexicalProcess.RegisterKeyword("if",TokenValues.If);  
                __LexicalProcess.RegisterKeyword("print",TokenValues.print);   
                __LexicalProcess.RegisterKeyword("else",TokenValues.Else);  
                __LexicalProcess.RegisterKeyword("TriggerPlayer",TokenValues.TriggerPlayer);
                __LexicalProcess.RegisterKeyword("Board",TokenValues.Board);
                __LexicalProcess.RegisterKeyword("HandOfPlayer",TokenValues.HandOfPlayer);
                __LexicalProcess.RegisterKeyword("FieldOfPlayer",TokenValues.FieldOfPlayer);
                __LexicalProcess.RegisterKeyword("GraveyardOfPlayer",TokenValues.GraveyardOfPlayer);
                __LexicalProcess.RegisterKeyword("DeckOfPlayer",TokenValues.DeckOfPlayer);
                __LexicalProcess.RegisterKeyword("Owner",TokenValues.Owner);  
                __LexicalProcess.RegisterKeyword("Find",TokenValues.Find);
                __LexicalProcess.RegisterKeyword("Push",TokenValues.Push);
                __LexicalProcess.RegisterKeyword("SendBottom",TokenValues.SendBottom);
                __LexicalProcess.RegisterKeyword("Pop",TokenValues.Pop);
                __LexicalProcess.RegisterKeyword("Remove",TokenValues.Remove);
                __LexicalProcess.RegisterKeyword("Shuffle",TokenValues.Suffle);
                __LexicalProcess.RegisterKeyword("Hand",TokenValues.Hand);
                __LexicalProcess.RegisterKeyword("Field",TokenValues.Field);
                __LexicalProcess.RegisterKeyword("Graveyarr",TokenValues.Graveyard);
                __LexicalProcess.RegisterKeyword("Deck",TokenValues.Deck);

                /*  */
                __LexicalProcess.RegisterText("\"", "\"");
            }

            return __LexicalProcess;
        }
    }
}