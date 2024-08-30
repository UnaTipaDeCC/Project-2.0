using System.Collections;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;
//using System.Reflection.Metadata;

public class Parse 
{
    public List<CompilingError> Errors{get;set;}
    public TokenStream Stream{get;set;}
    Scope scopes;
    public Parse(TokenStream stream, List<CompilingError> errors)
    {
        this.Errors = errors;
        this.Stream = stream;
    }
    public ElementalProgram Parser()
    {
        List<Effect> effects = new List<Effect>();
        List<Card> cards= new List<Card>();
        while(!Stream.End)
        {
            try
            {
                if(Stream.Match(TokenValues.declareEffect) && Stream.Match(TokenValues.OpenCurlyBraces)) effects.Add(ParseEffect());
                else if(Stream.Match(TokenValues.card) && Stream.Match(TokenValues.OpenCurlyBraces)) cards.Add(ParseCards());
            }
            catch(CompilingError error)
            {
                //Console.WriteLine(error);
            }
        }
        //Console.WriteLine(effects.Count);
        //Console.WriteLine(cards.Count);
        return new ElementalProgram(effects,cards,Errors,new CodeLocation());
    }
    #region Expressions
    public Expression? Expression()
    {
        Expression? exp = Equality();
       // exp.Evaluate();
        return exp;
    }
    private Expression Equality()
    {
        Expression expr = Comparation();
        
        while (Stream.Match(TokenValues.EqualComparer, TokenValues.UnEqualComparer))
        {
            //Console.WriteLine("equality");
            Token Operator = Stream.Previous();
            //Console.WriteLine(expr);
            //Console.WriteLine(Operator);
            Expression right = Comparation();
            if(Operator.Value == TokenValues.EqualComparer)
            {
                expr = new Equal(expr, Operator, right,Operator.Location);
            }
            if(Operator.Value == TokenValues.UnEqualComparer)
            {
                expr = new Unequal(expr, Operator, right,Operator.Location);
            }
        }
        //Console.WriteLine("ya voy a retornar la expression");
        return expr;
    }
    private Expression Comparation()
    {
        Expression expr = Term();
        Console.WriteLine("comparanding");
        while(Stream.Match(TokenValues.Greater,TokenValues.Less,TokenValues.GraeterOrEqual,TokenValues.LessOrEqual))
        {
            Console.WriteLine("algo");
            Token Operator = Stream.Previous();
            Expression right = Term();
            switch (Operator.Value)
            {
                case TokenValues.Greater: 
                expr = new Greater(expr,Operator,right,Operator.Location);
                break;
                case TokenValues.Less:
                expr = new Less(expr,Operator,right,Operator.Location);
                break;
                case TokenValues.LessOrEqual:
                expr = new LessOrEqual(expr,Operator,right,Operator.Location);
                break;
                case TokenValues.GraeterOrEqual:
                expr = new GreaterOrEqual(expr,Operator,right,Operator.Location);
                break;
            }      
        }
        return expr;
    }
    private Expression Term()
    {
        Expression expr = Factor();
        Console.WriteLine("TERM");
        while(Stream.Match(TokenValues.Add ,TokenValues.Sub)) 
        {
            Token Operator = Stream.Previous();
            Console.WriteLine("{0}", Operator.Value);
            Expression right = Factor();
            if(Operator.Value == TokenValues.Add)
            {
                Console.WriteLine("creanding");
                 expr = new Add(expr,Operator,right,Operator.Location);
            }
            if(Operator.Value == TokenValues.Sub) expr = new Sub(expr,Operator,right,Operator.Location);
            Console.WriteLine($"Created Binary node: {Operator.Value} with left: {expr} and right: {right}");
        }
        return expr;
    }
    private Expression Factor()
    {
        Expression expr = Power();
        //Expression expr = Unary();
        while(Stream.Match(TokenValues.Div,TokenValues.Mul))// / *
        {
            Token Operator = Stream.Previous();
            Expression? right = Unary();
            if(Operator.Value == TokenValues.Div)
            {
                expr = new Div(expr,Operator,right,Operator.Location);
            }
            else if(Operator.Value == TokenValues. Mul)
            {
                expr = new Mul(expr,Operator,right,Operator.Location);
            }
        }
        return expr;
    }
    Expression Power()
    {
        Expression expr = Bool();
        while(Stream.Match(TokenValues.Pow))
        {
            expr = new Pow(expr,Stream.Previous(),Bool(),Stream.Previous().Location);
        }
        return expr;
    }
    private Expression Bool()
    {
        Expression expr = Unary();
        while(Stream.Match(TokenValues.AndOperator,TokenValues.OrOperator))
        {
            Token tokenoperator = Stream.Previous();
            switch(tokenoperator.Value)
            {
                case TokenValues.AndOperator:
                expr = new And(expr,Unary(),tokenoperator.Location); 
                break;
                case TokenValues.OrOperator:
                expr = new Or(expr,Unary(),tokenoperator.Location);
                break;
            }
        }
        return expr;
    }

    private Expression Unary()
    {
        if(Stream.Match(TokenValues.Negation,TokenValues.Sub))// ! - 
        {
            Token Operator = Stream.Previous();
            Expression right  = Unary();
            return new Unary(Operator,right,Operator.Location);
        }
        //return Primary();
        return Concatenation();
    }
    private Expression Concatenation()
    {
        Expression expr = Primary();
        Console.WriteLine("concatenanding");
        while(Stream.Match(TokenValues.ConcatenationWithSpace,TokenValues.ConcatenationWithoutSpace))
        {
            Token tokenoperator = Stream.Previous();
            expr = new Concatenation(expr,tokenoperator,Primary(),tokenoperator.Location);
        }
        return expr;
    }
    private Expression Primary()
    {
        if (Stream.Match(TokenValues.False)) return new Bool(false,Stream.Previous().Location);
        if(Stream.Match(TokenValues.True)) return new Bool(true,Stream.Previous().Location); 
        Console.WriteLine(Stream.Position);
        Console.WriteLine("a ver si es un numero");
        if (!Stream.End && Stream.Match(TokenType.Number)) 
        {
            Console.WriteLine("numero");
            return new Number(double.Parse(Stream.Previous().Value),Stream.Previous().Location);
        }
        Console.WriteLine("no es un numero es un texto");
        if(!Stream.End && Stream.Match(TokenType.Text))
        {
            Console.WriteLine("es un texto");
            return new Text(Stream.Previous().Value,Stream.Previous().Location);
        }
        if(!Stream.End && Stream.Match(TokenType.Identifier))
        {
            Console.WriteLine("es un identificador");
            Token variable = Stream.Previous();
            return HandleIdentifier(variable);
        }
        if (Stream.Match(TokenValues.OpenBracket)) 
        { 
            Expression expr = Expression();
            Console.WriteLine("en grouping " + expr);
            if( Stream.Match(TokenValues.ClosedBracket))
            {
                Grouping exp = new Grouping(expr,expr.Location);
                return exp;
            }
            else if(!(expr is Predicate)) throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Expected, "se esperaba ')'");  
            return expr;

        }
        throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Expected, "no hay na");     
    }
    private Expression HandleIdentifier(Token variable)
    {
    Expression v = new Variable(variable.Value, variable.Location);
    if (!Stream.End && (Stream.Match(TokenValues.Increment) || Stream.Match(TokenValues.Decrement)))
    {
        return new Unary(Stream.Previous(),v,v.Location);//new VariableModifier(v, Stream.Previous(),v.Location);
    }
    while (true)
    {
        if (!Stream.End && Stream.Match(TokenValues.Point))
        {
            if (!Stream.End && Stream.Match(TokenType.Keyword))
            {
                Token property = Stream.Previous();
                // Verificar si el keyword está seguido de un paréntesis de apertura
                if (!Stream.End && Stream.Match(TokenValues.OpenBracket))
                {
                    Expression argument = null;
                    if (!Stream.End && !Stream.Match(TokenValues.ClosedBracket))
                    {
                        argument = Expression();
                    }
                    // Verificar si se cierra el paréntesis
                    if (!Stream.End && Stream.Match(TokenValues.ClosedBracket))
                    {
                        v = new Method(property, v, property.Location, argument);
                    }
                    else throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing ')' after method arguments");
                }
                else v = new Property(property, v, property.Location);
            }
            else
            {
                throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Expected keyword after '.'");
            }
        }
        else if (!Stream.End && Stream.Match(TokenValues.OpenBrace))
        {
            Expression index = Expression();
            if (!Stream.End && Stream.Match(TokenValues.ClosedBrace))
            {
                v = new Indexer(v, index, variable.Location);
            }
            else
            {
                throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing ']' after an index");
            }
        }
        else if(!Stream.End && Stream.Match(TokenValues.ClosedBracket))
        {
            // Aquí manejamos el caso del lambda
            if (!Stream.End && Stream.Match(TokenValues.Lambda))
            {
                // Llamar al método predicate
                v = new Predicate(v,Expression(),Stream.Previous().Location);
            }
        }
        else
        {
            break; // Salir del bucle si no hay más accesos a propiedades, índices o métodos
        }
    }
    return v;
}
    #endregion
    //haciendo alguito con las cartas a ver que tal
    #region Otros
    public Effect ParseEffect()
    {
        CodeLocation location = new CodeLocation();
        Expression name = null;
        Statement action = null;
        Token target = null;
        Token context= null;
        List<(Token,Token)> paramsType = new List<(Token,Token)>(); 
        do
        {
            try
            {
                if(Stream.End) throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Invalid,"Invalid effect declaration, missing params");
                else if(Stream.Match(TokenValues.Name)){ name = Assign(); Console.WriteLine("nombre");} 
                else if(Stream.Match(TokenValues.Params))
                {
                    Console.WriteLine("params");
                    if(!Stream.Match(TokenValues.DoublePoint)) throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Expected, "Missing ':' ");
                    if(!Stream.Match(TokenValues.OpenCurlyBraces)) throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Expected, "Missing '{' ");
                    while(Stream.Match(TokenType.Identifier))
                    {
                        Console.WriteLine("Hay algun param");
                        paramsType.Add(Param());
                        Console.WriteLine("Param agregado con exito");
                    }
                    Console.WriteLine(Stream.Previous().Value);
                    if(!Stream.Match(TokenValues.ClosedCurlyBraces)) throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Expected, "Missing '}' after a param declaration");
                    if(!Stream.Match(TokenValues.ValueSeparator)) throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Expected, "Missing ',' in param declaration ");
                    Console.WriteLine(paramsType.Count());
                    Console.WriteLine("param superado con exito");
                }  
                else if(Stream.Match(TokenValues.Action))
                {
                    Console.WriteLine("action");
                    if(!Stream.Match(TokenValues.DoublePoint)) throw new CompilingError(Stream.Previous().Location,ErrorCode.Expected, "Missing ':' after 'Action'");
                    Console.WriteLine("en efecto habian dos puntos");
                    if(!Stream.Match(TokenValues.OpenBracket)) throw new CompilingError(Stream.Previous().Location,ErrorCode.Expected, "Missing '(' after 'Action'");
                    Console.WriteLine("en efecto habia un (");
                    if(Stream.Match(TokenType.Identifier))
                    { 
                        target = Stream.Previous(); 
                        Console.WriteLine("habia un target");
                    }
                    else throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing target");
                    if(!Stream.Match(TokenValues.ValueSeparator)) throw new CompilingError(location,ErrorCode.Expected, "Missing ',' after target");
                    if(Stream.Match(TokenType.Identifier)) {context = Stream.Previous(); Console.WriteLine("habia un context");}
                    else throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing context");
                    if(!Stream.Match(TokenValues.ClosedBracket)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing ')' ");
                    Console.WriteLine("habian un ) ");
                    if(!Stream.Match(TokenValues.Lambda)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing '=>' in Action declaration ");
                    if(Stream.Match(TokenValues.OpenCurlyBraces)) 
                    {
                        action = Statements(); 
                        Console.WriteLine("ya tengo el body"); 
                        Stream.MoveBack();
                        Console.WriteLine(Stream.LookAhead().Value);
                    }
                    else action = SimpleStatements();
                    Console.WriteLine("termine el action");
                    Console.WriteLine(Stream.LookAhead().Value);
                    if(action == null) throw new CompilingError(location,ErrorCode.Invalid, " An action must be declared");
                }
            }
            catch(CompilingError error)
            {
                Errors.Add(error);

                Console.WriteLine(error);//"algo fue mal mientras declarabas el efecto");        
            }
        }while(!Stream.Match(TokenValues.ClosedCurlyBraces));
        Console.WriteLine("fuera del action");
        if(name == null) throw new CompilingError(location,ErrorCode.Invalid, "The name must be declared");
        if(action == null ) throw new CompilingError(location,ErrorCode.Invalid, "An Action must be declared");
        return new Effect(location, name, action,target,context,paramsType);
    }
    public (Token,Token) Param()
    {
        Token param = Stream.Previous();
        Token type = null;
        if(!Stream.Match(TokenValues.DoublePoint)) throw new CompilingError(param.Location,ErrorCode.Expected, "Missing ':' in param declaration");
        if(!Stream.Match(TokenType.Identifier)) throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Expected, "Missing type after the name in param declaration");
        else type = Stream.Previous();
        return (param,type);
    }
    public Card ParseCards()
    {
        CodeLocation location = new CodeLocation();//Stream.LookAhead(-2).Location;
        Expression power = null;
        Expression name = null;
        Expression faction = null;
        Expression type = null;
        List<Expression> range = new List<Expression>();
        List<EffectAction> effects = new List<EffectAction>();
        do
        {
            try
            {
                if(Stream.End) break;
                else if(Stream.Match(TokenValues.Power)) power = Assign(); 
                else if(Stream.Match(TokenValues.Name)) name = Assign();
                else if(Stream.Match(TokenValues.Type)) type = Assign();
                else if(Stream.Match(TokenValues.Faction)) faction = Assign();
                else if(Stream.Match(TokenValues.Range))
                {
                    Console.WriteLine("en el range");
                    if(!Stream.Match(TokenValues.DoublePoint)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, " Missing ':' ");
                    
                    if(!Stream.Match(TokenValues.OpenBrace)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Bad Range declaration, missing '[' ");
                    do
                    {
                        Console.WriteLine("todo bien hasta aqui");
                        range.Add(Expression());
                        if(!Stream.Match(TokenValues.ClosedBrace))
                        {
                            if(!Stream.Match(TokenValues.ValueSeparator))throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Bad Range declaration, missing ',' ");
                        }
                        else Stream.MoveBack(1);    
                        
                    }
                    while(!Stream.Match(TokenValues.ClosedBrace));
                    if(!Stream.Match(TokenValues.ValueSeparator)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Bad Range declaration, missing ',' ");
                }
                else if(Stream.Match(TokenValues.OnActivation))
                {
                    Console.WriteLine("en   el onActivation");
                    if(!Stream.Match(TokenValues.DoublePoint)) throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Missing ':' after OnActivadion declaration");
                    if(Stream.Match(TokenValues.OpenBrace))
                    {
                        do
                        {
                            effects.Add(EffectAssign());
                            Console.WriteLine(Stream.LookAhead().Value + "Despues de agregar el effect");
                            if(!Stream.Match(TokenValues.ClosedBrace))
                            {
                                if(!Stream.Match(TokenValues.ValueSeparator))throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Bad Range declaration, missing ',' ");
                                Console.WriteLine("si era un , ");
                            }
                            else Stream.MoveBack(1);   
                        }
                        while(!Stream.End && !Stream.Match(TokenValues.ClosedBrace));
                        //Console.WriteLine(Stream.LookAhead().Value);
                    }
                    else
                    {
                        effects.Add(EffectAssign());
                        if(!Stream.Match(TokenValues.ValueSeparator)) throw new CompilingError(location,ErrorCode.Expected,"Missing ',' after de effect ");
                    }
                    //Console.WriteLine("Despues del onActivation" + Stream.LookAhead().Value);
                }
                else throw new CompilingError(location, ErrorCode.Invalid, "Missing params");
            }
            catch(CompilingError error)//revisar lo de los errores
            {
                Console.WriteLine("Algo no pincho bien" + error);
                break;
            }    
        }
        while(!Stream.Match(TokenValues.ClosedCurlyBraces));
        if(power == null || faction == null || type == null || faction == null || name == null || range.Count == 0) throw new CompilingError(location,ErrorCode.Invalid, "No llenaste todo");
        return new Card(power,type,name,faction,range,effects, location);//agragar lo de los efectos
    }
    private EffectAction EffectAssign()
    {
        CodeLocation effectLocation = Stream.LookAhead(-1).Location;
        Expression name = null;
        List<(Token, Expression)> parameters = new List<(Token, Expression)>();
        Selector selector = null;
        EffectAction postAction = null;
        Effect effect = null;
        Console.WriteLine(Stream.LookAhead().Value + "en el effect");
        if(!Stream.Match(TokenValues.OpenCurlyBraces)) throw new CompilingError(Stream.Previous().Location,ErrorCode.Expected, "Missing '{'");
        do
        {
            try
            {
                if(Stream.End) throw new CompilingError(effectLocation,ErrorCode.Invalid, "Unfinished declaration");
                else if(Stream.Match(TokenValues.Effect))
                {
                    Console.WriteLine("bien senores y senoras, estamos en el effect");
                    if(!Stream.Match(TokenValues.DoublePoint)) throw new CompilingError(Stream.Previous().Location,ErrorCode.Expected, "Missing ':'");
                    if(Stream.Match(TokenValues.OpenCurlyBraces)) EffectStatement(ref name,ref parameters);
                    else name = Expression();
                    if(!Stream.Match(TokenValues.ValueSeparator)) throw new CompilingError(Stream.Previous().Location,ErrorCode.Expected, "Missing ', '");
                }
                else if(Stream.Match(TokenValues.Selector))
                {
                    Console.WriteLine("en el selector");
                    CodeLocation selectorLocation = Stream.Previous().Location;
                    if(!Stream.Match(TokenValues.DoublePoint)) throw new CompilingError(Stream.Previous().Location,ErrorCode.Expected, "Missing ':'");
                    if(!Stream.Match(TokenValues.OpenCurlyBraces)) throw new CompilingError(Stream.Previous().Location,ErrorCode.Expected, "Missing '{'");
                    selector = Selector(selectorLocation);
                    Console.WriteLine("termine el selector");
                }
                else throw new CompilingError(effectLocation,ErrorCode.Invalid,"Unfinished OnActivation declaration");
            }
            catch(CompilingError error)
            {
                Console.WriteLine(error);
                break;
            }
        }
        while(!Stream.Match(TokenValues.ClosedCurlyBraces));
        if(name == null) throw new CompilingError(effectLocation,ErrorCode.Invalid,"The effect name cant be null");
        if(selector != selector) throw new CompilingError(effectLocation,ErrorCode.Invalid,"The effect selector cant be null");
        Console.WriteLine("bue, to ok");
        Console.WriteLine(Stream.LookAhead().Value);
        return new EffectAction(name,selector,parameters,postAction,effectLocation);        
    }
    private void EffectStatement(ref Expression name, ref List<(Token,Expression)> paramsValue)
    {
        Console.WriteLine("dentro del effect");
        do
        {
            try
            {
                if(Stream.End) throw new CompilingError(Stream.Previous().Location,ErrorCode.Invalid,"Unfinished effect declaration");
                else if(Stream.Match(TokenValues.Name)) name = Assign();
                else if(Stream.Match(TokenType.Identifier)) paramsValue.Add((Stream.Previous(),Assign()));
                //falta la manera de manejar el post action
            }
            catch (CompilingError error)
            {
                Console.WriteLine(error);
            }
        }
        while(!Stream.Match(TokenValues.ClosedCurlyBraces));
        Console.WriteLine("terminamos por aqui");
        Console.WriteLine(Stream.LookAhead().Value);
    }
    private Selector Selector(CodeLocation location, Selector selectorParent = null)
    {
        Expression source = null;
        Expression single = null;
        Expression predicate = null;
        do
        {
            try
            {
            if(Stream.Match(TokenValues.Source)) source = Assign();
            else if(Stream.Match(TokenValues.Single)) single = Assign();
            else if(Stream.Match(TokenValues.Predicate)) predicate = Assign();
            else throw new CompilingError(location, ErrorCode.Expected, "Missing source and predicate");
            }
            catch(CompilingError error)
            {
                Console.WriteLine(error);
            }
        }
        while(!Stream.Match(TokenValues.ClosedCurlyBraces));
        if(source == null) new CompilingError(location, ErrorCode.Invalid, "Missing source");
        if(predicate == null)  new CompilingError(location, ErrorCode.Expected, "Missing predicate");
        return new Selector(source,single,predicate,location);
    }
    private Expression Assign()
    {
        Expression? expr = null;
        if(Stream.Match(TokenValues.DoublePoint))
        {
            expr = Expression();
        }
        else throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing ':' ");
        if(!Stream.Match(TokenValues.ValueSeparator)) throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Expected,"Missing ',' ");
        return expr;
    } 
    private Statement While()
    {
        Console.WriteLine("estoy en el while");
        CodeLocation location = Stream.Previous().Location;
        Statement? body = null;
        if(!Stream.Match(TokenValues.OpenBracket)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing '(' ");
        Expression? codition = Expression();
        if(!Stream.Match(TokenValues.ClosedBracket)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing ') in while declaration' ");
        if(Stream.Match(TokenValues.OpenCurlyBraces)) body = Statements();
        else body  = SimpleStatements();
        return new While(codition,body,location);

    }
    private Statement For()
    {
        //Console.WriteLine("en el for");
        CodeLocation location = Stream.Previous().Location;
        Statement body;
        Token item;
        Token colletion;
        if(Stream.Match(TokenType.Identifier)) item = Stream.Previous();
        else throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing identifier after for");
        if(!Stream.Match(TokenValues.in_)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing keyword 'in' ");
        colletion = Stream.NextToken();
        Stream.MoveNext();
        if(Stream.Match(TokenValues.OpenCurlyBraces)) body = Statements();
        else body = SimpleStatements();
        return new For(item,colletion,body,location);//revisar bien
    }
    private Statement Statements()
    {
        List<Statement> stmts = new List<Statement>();
        CodeLocation location = Stream.Previous().Location;
        StatementBlock block = null;
        do
        {
            try
            {
            if(Stream.End) throw new CompilingError(location, ErrorCode.Invalid, "Invalid statement declaration ");
            else if(Stream.Match(TokenValues.while_)) stmts.Add(While());
            else if(Stream.Match(TokenValues.for_)) stmts.Add(For());
            else stmts.Add(SimpleStatements());
            }
            catch(CompilingError error)
            {
                //Console.WriteLine(error);
                
            }
        } while(!Stream.Match(TokenValues.ClosedCurlyBraces));
        
        if(Stream.Match(TokenValues.StatementSeparator)) //Console.WriteLine(Stream.LookAhead().Value + " al salir del state");
        block = new StatementBlock(stmts,location);
        return block;
        /*Console.WriteLine(stmts.Count());
        Console.WriteLine(Stream.LookAhead().Value);
        Console.WriteLine("VOY A RETORNAR ESTA HISTORIA");*/
       
    }
    private Statement SimpleStatements()
    {
        Statement statement;
        /*Console.WriteLine("estoy en siple");
        Console.WriteLine(Stream.LookAhead().Value);*/
        if(Stream.Match(TokenType.Identifier)) 
        {
            //Console.WriteLine("voy a mandar a declarar");
            statement = Declaration();
        }
        else if(Stream.Match(TokenValues.print))
        {
            statement = new Print(Expression(),Stream.Previous().Location);
            //Console.WriteLine(Stream.LookAhead().Value);
        }
        else throw new CompilingError(Stream.Previous().Location, ErrorCode.Invalid, "no declara nada"); 
        if(!Stream.Match(TokenValues.StatementSeparator)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing ';' after a simple statement ");
        return statement;    
    }
    private Statement Declaration()
    {
        Stream.MoveBack();
        //Console.WriteLine("Estoy declarando");
        Expression variable = Expression();
        Expression expr = null;
        Token operatorToken = null; 
        if(Stream.Match(TokenValues.Assign,TokenValues.AdditionAssignment,TokenValues.SubtractionAssignment))
        {
            operatorToken = Stream.Previous();
            expr = Expression(); 
           //if(!Stream.Match(TokenValues.StatementSeparator))throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Missing ';' after a declaration");
            return new Declaration(variable, variable.Location, operatorToken,expr);
        }
        //Console.WriteLine("es aqui");
        throw new CompilingError(variable.Location, ErrorCode.Invalid,"Bad declaration of the variable " + variable.Value);
    }
    /*private bool ErrorsControl(CompilingError error, string Value)
    {
        Errors.Add(error);
        while(!Stream.Match(Value))
        {
            if(Stream.End) return true;
                if(Stream.)
        }
    }*/
    #endregion
}