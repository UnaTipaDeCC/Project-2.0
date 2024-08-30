using System;
using System.IO;
public class Program
{
    public static void Main(string[] args)
    {
        string file = @"C:\Users\Yo\Desktop\CC\Pro\Proyecto\DSL.txt";
        string file2 = @"C:\Users\Yo\Desktop\CC\Pro\GitHub\Proyecto_2.0\Proyecto\Parseanding.txt";
        string file3 = @"C:\Users\Yo\Desktop\CC\Pro\Proyecto\Parse.txt";
        string text = File.ReadAllText(file2);
        LexicalAnalyzer lex = Compiling.Lexical;
        List<CompilingError> errors = new List<CompilingError>();
        IEnumerable<Token> tokens = lex.GetTokens(" ",text,errors);
        foreach (Token t in tokens)
        {
            Console.WriteLine(t);
        }
        TokenStream stream = new TokenStream(tokens);
        //Parser parser = new Parser(stream);
        
        Scope scope= new Scope();
        //List<Scope> s = new List<Scope>(){ scope };
        Context context = new Context();
        Parse parse = new Parse(stream, errors);
        ElementalProgram program = parse.Parser();
        if(errors.Count > 0)
        {
            foreach(var error in errors)
            {
                Console.WriteLine(error);
            }
        }
        else
        {
            Console.WriteLine("any parsing errors"); 
            program.CheckSemantic(context,scope,errors);
            if(errors.Count > 0)
            {
                foreach(var error in errors)
                {
                    Console.WriteLine(error);
                }
            }
            else 
            {
                program.Evaluate();
                foreach(var card in program.Cards)
                {
                    Console.WriteLine(card);
                }
            }
        }

        }
        //Expression result = parse.Expression();
        
        //StatementBlock statementBlock= new StatementBlock(parse.Parser(), new CodeLocation());
        
        /*statementBlock.CheckSemantic(context,scope,errors);
        if(errors.Count() > 0) foreach (var error in errors) Console.WriteLine(error);
        else statementBlock.Execute();
        //Card card = parse.ParseCards();
        //card.CheckSemantic(context,scope,errors);
        //Effect effect = parse.ParseEffect();
        //effect.CheckSemantic(context,scope,errors);
        //Expression? result = parse.Expression();
        
        //result.CheckSemantic(context,scope,errors);
        /*if(errors.Count > 0)
        {
            foreach(var error in errors)
            {
                Console.WriteLine(error);
            }
        }
        else 
        {
            //result.Evaluate();
            effect.Execute();
            /*card.Evaluate();
            Console.WriteLine("resultado es: " + card);*/
            //Console.WriteLine(effect);
            //Console.WriteLine(result.Value);
        //}
        

        /*Expression? result = parser.Expression();
        result.Evaluate();
        if(result == null) Console.WriteLine("null");
        Console.WriteLine(result.Value.ToString());  */
   // }
}