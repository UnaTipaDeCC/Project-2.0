using System.Collections;
//using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
//using System.Reflection.Metadata;

public class Parse 
{
    public List<CompilingError> Errors{get;set;}
    public TokenStream Stream{get;set;}
    public Parse(TokenStream stream, List<CompilingError> errors)
    {
        this.Errors = errors;
        this.Stream = stream;
    }
    public ElementalProgram Parser()
    {
        List<Effect> effects = new List<Effect>();
        List<Card> cards= new List<Card>();
        while(!Stream.Check(TokenType.End))
        {
            try
            {
                if(Stream.Match(TokenValues.declareEffect) && Stream.Match(TokenValues.OpenCurlyBraces)) effects.Add(ParseEffect(Stream.LookAhead(-2).Location));
                else if(Stream.Match(TokenValues.card) && Stream.Match(TokenValues.OpenCurlyBraces)) cards.Add(ParseCards(Stream.LookAhead().Location));
                else throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Invalid,"Invaid declaration, expected card or effect declaration");
            }
            catch(CompilingError error)
            {
                if(ErrorsControl(error)) break;
            }
        }
        return new ElementalProgram(effects,cards,Errors,new CodeLocation());
    }
    #region Expressions
    public Expression? Expression()
    {
        Expression? exp = Equality();
        return exp;
    }
    private Expression Equality()
    {
        Expression expr = Comparation();
        
        while (Stream.Match(TokenValues.EqualComparer, TokenValues.UnEqualComparer))
        {
            Token Operator = Stream.Previous();
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
        return expr;
    }
    private Expression Comparation()
    {
        Expression expr = Term();
        while(Stream.Match(TokenValues.Greater,TokenValues.Less,TokenValues.GraeterOrEqual,TokenValues.LessOrEqual))
        {
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
        while(Stream.Match(TokenValues.Add ,TokenValues.Sub)) 
        {
            Token Operator = Stream.Previous();
            Expression right = Factor();
            if(Operator.Value == TokenValues.Add)
            {
                 expr = new Add(expr,Operator,right,Operator.Location);
            }
            if(Operator.Value == TokenValues.Sub) expr = new Sub(expr,Operator,right,Operator.Location);
        }
        return expr;
    }
    private Expression Factor()
    {
        Expression expr = Power();
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
        return Concatenation();
    }
    private Expression Concatenation()
    {
        Expression expr = Primary();
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
        if (!Stream.End && Stream.Match(TokenType.Number)) 
        {
            return new Number(double.Parse(Stream.Previous().Value),Stream.Previous().Location);
        }
        if(!Stream.End && Stream.Match(TokenType.Text))
        {
            return new Text(Stream.Previous().Value,Stream.Previous().Location);
        }
        if(!Stream.End && Stream.Match(TokenType.Identifier))
        {
            Token variable = Stream.Previous();
            return HandleIdentifier(variable);
        }
        if (Stream.Match(TokenValues.OpenBracket)) 
        { 
            Expression expr = Expression();
            if( !Stream.Match(TokenValues.ClosedBracket)) throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Missing ) in the primary expression)");
            if(Stream.Match(TokenValues.Lambda))
            {
                expr = Predicate(expr);
                return expr;
            }
            return new Grouping(expr,Stream.Previous().Location);
            /*if( Stream.Match(TokenValues.ClosedBracket))
            {
                Grouping exp = new Grouping(expr,expr.Location);
                return exp;
            }
            else if(!(expr is Predicate)) throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Expected, "se esperaba ')'");  
            return expr;*/

        }
        throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Expected, "no hay na");     
    }
    private Expression Predicate(Expression variable)
    {
        Expression condition = Expression();
        return new Predicate(variable,condition,variable.Location);
    }
    private Expression HandleIdentifier(Token variable)
    {
        Debug.Log(Stream.LookAhead().Value + "despues del identificador");
    Expression v = new Variable(variable.Value, variable.Location);
    if (!Stream.End && (Stream.Match(TokenValues.Increment) || Stream.Match(TokenValues.Decrement)))
    {
        return new Unary(Stream.Previous(),v,v.Location);
    }
    while (true)
    {
        if (!Stream.End && Stream.Match(TokenValues.Point))
        {
            if (!Stream.End && Stream.Match(TokenType.Keyword))
            {
                Token property = Stream.Previous();
                // Verificar si el keyword está seguido de un paréntesis de apertura
                Debug.Log("AFTER " + property.Value + "lo que hay es un " + Stream.LookAhead().Value);
                if (!Stream.End && Stream.Match(TokenValues.OpenBracket))
                {
                    Debug.Log("entre xq habia un (");
                    Expression argument = null;
                    if (!Stream.End && !Stream.Match(TokenValues.ClosedBracket))
                    {
                        argument = Expression();
                        if(!Stream.Match(TokenValues.ClosedBracket)) throw new CompilingError(Stream.Previous().Location,ErrorCode.Expected, "Missing ')' after method argument");
                        v = new Method(property,v,property.Location,argument);
                    }
                    else 
                    {
                        Debug.Log("lo q habia era un )");
                        v = new Method(property,v,property.Location);
                    }
                    /*Debug.Log(Stream.LookAhead().Value);
                    // Verificar si se cierra el paréntesis
                    if (!Stream.End && Stream.Match(TokenValues.ClosedBracket))
                    {
                        v = new Method(property, v, property.Location, argument);
                    }
                    else throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing ')' after method arguments");
                */}
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
        /*else if(!Stream.End && Stream.Match(TokenValues.ClosedBracket))
        {
            // Aquí manejamos el caso del lambda
            if (!Stream.End && Stream.Match(TokenValues.Lambda))
            {
                // Llamar al método predicate
                v = new Predicate(v,Expression(),Stream.Previous().Location);
            }
            //si lo que viene no es un lambda volver al parentesis cerrado y salir del bucle
            else Stream.MoveBack(); break;
        }*/
        else
        {
            break; // Salir del bucle si no hay más accesos a propiedades, índices o métodos
        }
    }
    return v;
}
    #endregion
    //haciendo alguito con las cartas a ver que tal
    #region Effect
    public Effect ParseEffect(CodeLocation location)
    {
        Expression name = null;
        Statement action = null;
        Token target = null;
        Token context= null;
        List<(Token,Token)> paramsType = new List<(Token,Token)>(); 
        do
        {
            try
            {
                if(Stream.Check(TokenType.End)) throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Invalid,"Invalid effect declaration, missing params");
                else if(Stream.Match(TokenValues.Name)){ name = Assign();} 
                else if(Stream.Match(TokenValues.Params))
                {
                    if(!Stream.Match(TokenValues.DoublePoint)) throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Expected, "Missing ':' ");
                    if(!Stream.Match(TokenValues.OpenCurlyBraces)) throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Expected, "Missing '{' ");
                    while(Stream.Match(TokenType.Identifier))
                    {
                        paramsType.Add(Param());
                    }
                    if(!Stream.Match(TokenValues.ClosedCurlyBraces)) throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Expected, "Missing '}' after a param declaration");
                    if(!Stream.Match(TokenValues.ValueSeparator)) throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Expected, "Missing ',' in param declaration ");
                }  
                else if(Stream.Match(TokenValues.Action))
                {
                    Debug.Log("en le action");
                    if(!Stream.Match(TokenValues.DoublePoint)) throw new CompilingError(Stream.Previous().Location,ErrorCode.Expected, "Missing ':' after 'Action'");
                    Debug.Log("dos puntos");
                    if(!Stream.Match(TokenValues.OpenBracket)) throw new CompilingError(Stream.Previous().Location,ErrorCode.Expected, "Missing '(' after 'Action'");
                    Debug.Log("(");
                    if(Stream.Match(TokenType.Identifier))
                    { 
                        target = Stream.Previous(); 
                        Debug.Log("un target");
                    }
                    else throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing target");
                    if(!Stream.Match(TokenValues.ValueSeparator)) throw new CompilingError(location,ErrorCode.Expected, "Missing ',' after target");
                    if(Stream.Match(TokenType.Identifier)) context = Stream.Previous();
                    else throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing context");
                    if(!Stream.Match(TokenValues.ClosedBracket)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing ')' ");
                    Debug.Log(")");
                    if(!Stream.Match(TokenValues.Lambda)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing '=>' in Action declaration ");
                    if(Stream.Match(TokenValues.OpenCurlyBraces)) 
                    {
                        action = Statements(); 
                        Debug.Log("ya tengo el body"); 
                        //Stream.MoveBack();//revisar 
                        Debug.Log(Stream.LookAhead().Value);
                    }
                    else action = SimpleStatements();
                    Debug.Log("termine el action");
                    Debug.Log(Stream.LookAhead().Value);
                    if(action == null) throw new CompilingError(location,ErrorCode.Invalid, " An action must be declared");
                //falta alguna coma?
                }
                else throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Expected 'name',params o action");
            }
            catch(CompilingError error)
            {
                if(ErrorsControl(error,TokenValues.ValueSeparator))
                break; 
            }
        }while(!Stream.Match(TokenValues.ClosedCurlyBraces));
        Debug.Log("fuera del action");
        if(name == null) throw new CompilingError(location,ErrorCode.Invalid, "The name must be declared");
        if(action == null ) throw new CompilingError(location,ErrorCode.Invalid, "An Action must be declared");
        Debug.Log("termine el effect " + name.ToString());
        Debug.Log(Stream.LookAhead().Value);
         //Stream.MoveNext();//POR PROBAR
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
    #endregion
    #region Card
    public Card ParseCards(CodeLocation location)
    {
        //CodeLocation location = new CodeLocation();//Stream.LookAhead(-2).Location;
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
                if(Stream.End) throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Declaration wasnt finished");
                else if(Stream.Match(TokenValues.Power)) power = Assign(); 
                else if(Stream.Match(TokenValues.Name)) name = Assign();
                else if(Stream.Match(TokenValues.Type)) type = Assign();
                else if(Stream.Match(TokenValues.Faction)) faction = Assign();
                else if(Stream.Match(TokenValues.Range))
                {
                    Debug.Log("en el range");
                    CodeLocation rangeLoc = Stream.Previous().Location;
                    if(!Stream.Match(TokenValues.DoublePoint)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, " Missing ':' ");
                    
                    if(!Stream.Match(TokenValues.OpenBrace)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Bad Range declaration, missing '[' ");
                    do
                    {
                        Debug.Log("todo bien hasta aqui");
                        range.Add(Expression());
                        if(!Stream.Match(TokenValues.ClosedBrace))
                        {
                            if(!Stream.Match(TokenValues.ValueSeparator))throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Bad Range declaration, missing ',' ");
                        }
                        else Stream.MoveBack(1);    
                        
                    }
                    while(!Stream.Match(TokenValues.ClosedBrace));
                    if(!Stream.Match(TokenValues.ValueSeparator)) throw new CompilingError(rangeLoc, ErrorCode.Expected, "Bad Range declaration, missing ',' ");
                }
                else if(Stream.Match(TokenValues.OnActivation))
                {
                    CodeLocation onActivationLoc = Stream.Previous().Location;
                    Debug.Log("en el onActivation");
                    if(!Stream.Match(TokenValues.DoublePoint)) throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "Missing ':' after OnActivadion declaration");
                    if(Stream.Match(TokenValues.OpenBrace))
                    {
                        while(Stream.Match(TokenValues.OpenCurlyBraces))
                        {
                            effects.Add(EffectAssign());
                            if(!Stream.Match(TokenValues.ClosedBrace))
                            {
                                if(!Stream.Match(TokenValues.ValueSeparator))throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Bad  declaration, missing ',' ");
                                //Console.WriteLine("si era un , ");
                            }
                            else Stream.MoveBack(1);
                        }
                        if(!Stream.Match(TokenValues.ClosedBrace)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing ']' after an OnActivation declaration");
                        /*do
                        {
                            effects.Add(EffectAssign());
                            Debug.Log(Stream.LookAhead().Value + " Despues de agregar el effect");
                            Debug.Log(Stream.Position + " Despues de agregar el effect"); 
                            Debug.Log(Stream.End);
                            //serciorarse de que lo que sigue sea una coma en caso de que no sea un '}'
                            if(!Stream.Match(TokenValues.ClosedBrace))
                            {
                                if(!Stream.Match(TokenValues.ValueSeparator))throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Bad OnActivation declaration, missing ',' ");
                                //Console.WriteLine("si era un , ");
                            }
                            else Stream.MoveBack(1);   
                            Debug.Log("aqui");
                        }
                        while(!Stream.End && !Stream.Match(TokenValues.ClosedBrace));
                        Debug.Log("algo");
                        //Debug.Log(Stream.LookAhead().Value);
                    */}
                    else
                    {
                        throw new CompilingError(onActivationLoc, ErrorCode.Expected, "Missing '[' after onActivation declaration");
                        /*effects.Add(EffectAssign());
                        if(!Stream.Match(TokenValues.ValueSeparator)) throw new CompilingError(location,ErrorCode.Expected,"Missing ',' after de effect ");
                    */}
                }
                else throw new CompilingError(location, ErrorCode.Invalid, "Missing params in the card");
            }
            catch(CompilingError error)
            {
                if(ErrorsControl(error, TokenValues.ValueSeparator)) break;
            }    
        }
        while(!Stream.Match(TokenValues.ClosedCurlyBraces));
        if(power == null || faction == null || type == null || faction == null || name == null || range.Count == 0) throw new CompilingError(location,ErrorCode.Invalid, "Missing properties in card declaration");
        return new Card(power,type,name,faction,range,effects, location);//agragar lo de los efectos
    }
    private EffectAction EffectAssign()
    {
        CodeLocation effectLocation = Stream.LookAhead(-1).Location;
        Expression name = null;
        List<(Token, Expression)> parameters = new List<(Token, Expression)>();
        Selector selector = null;
        PostAction postAction = null;
        Debug.Log(Stream.LookAhead().Value + "en el effect");
        //if(!Stream.Match(TokenValues.OpenCurlyBraces)) throw new CompilingError(Stream.Previous().Location,ErrorCode.Expected, "Missing '{'");
        do
        {
            try
            {
                if(Stream.End) throw new CompilingError(effectLocation,ErrorCode.Invalid, "Unfinished effect declaration, missing '}'");
                else if(Stream.Match(TokenValues.Effect))
                {
                    Debug.Log("bien senores y senoras, estamos en el effect");
                    if(!Stream.Match(TokenValues.DoublePoint)) throw new CompilingError(Stream.Previous().Location,ErrorCode.Expected, "Missing ':' after de Effect");
                    if(Stream.Match(TokenValues.OpenCurlyBraces)) EffectStatement(ref name,ref parameters);
                    else name = Expression();
                    //if(!Stream.Match(TokenValues.ValueSeparator)) throw new CompilingError(Stream.Previous().Location,ErrorCode.Expected, "Missing ',' after the  '");
                }
                else if(Stream.Match(TokenValues.Selector))
                {
                    Debug.Log("en el selector");
                    CodeLocation selectorLocation = Stream.Previous().Location;
                    if(!Stream.Match(TokenValues.DoublePoint)) throw new CompilingError(Stream.Previous().Location,ErrorCode.Expected, "Missing ':' after Selector");
                    if(!Stream.Match(TokenValues.OpenCurlyBraces)) throw new CompilingError(Stream.Previous().Location,ErrorCode.Expected, "Missing '{'");
                    selector = Selector(selectorLocation);
                    Debug.Log("termine el selector");
                    Debug.Log(Stream.LookAhead().Value + "EN EL Q ESTOY DESPUES DEL SELECTOR" + Stream.LookAhead(1).Value + "el q sigue");
                }
                else if(Stream.Match(TokenValues.PostAction))
                {
                    Debug.Log("en el postAction");
                    if(!Stream.Match(TokenValues.DoublePoint)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing ':' after 'PostAction'");
                    if(!Stream.Match(TokenValues.OpenCurlyBraces)) throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Expected,"Missing '{' after ':' in a PostAction declaration");
                    postAction = new PostAction(EffectAssign(),new CodeLocation()); //revisar bien el location
                }
                else throw new CompilingError(effectLocation,ErrorCode.Invalid,"Unfinished OnActivation declaration");
                if(!Stream.Match(TokenValues.ValueSeparator)) throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Expected,"Missing ','");
                
            }
            catch(CompilingError error)
            {
                if(ErrorsControl(error,TokenValues.ValueSeparator)) break;
            }
        }
        while(!Stream.Match(TokenValues.ClosedCurlyBraces));
        //se verifica que el nombre no sea nulo, los demas parametros si pueden serlo y se verifica en el chequeo 
        if(name == null) throw new CompilingError(effectLocation,ErrorCode.Invalid,"The effect name cant be null");
        //if(selector is null) throw new CompilingError(effectLocation,ErrorCode.Invalid,"The effect selector cant be null"); 
        Debug.Log("bue, to ok");
        Debug.Log(Stream.LookAhead().Value);
        return new EffectAction(name,selector,parameters,postAction,effectLocation);        
    }
    private void EffectStatement(ref Expression name, ref List<(Token,Expression)> paramsValue)
    {
        Debug.Log("dentro del effect");
        do
        {
            try
            {
                if(Stream.Check(TokenType.End)) throw new CompilingError(Stream.Previous().Location,ErrorCode.Invalid,"Unfinished effect declaration");
                else if(Stream.Match(TokenValues.Name)){ name = Assign(); Debug.Log("terminado el nombre del efecto en la carta");}
                else if(Stream.Match(TokenType.Identifier)) paramsValue.Add((Stream.Previous(),Assign()));
                else throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Invalid, "Invalid effect assignation");
            }
            catch (CompilingError error)
            {
                if(ErrorsControl(error,TokenValues.ValueSeparator)) break;
            }
        }
        while(!Stream.Match(TokenValues.ClosedCurlyBraces));
        Debug.Log("terminamos por aqui, los params son " + paramsValue.Count);
        Debug.Log(Stream.LookAhead().Value);
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
                if(Stream.End) throw new CompilingError(Stream.Previous().Location, ErrorCode.Invalid, "Selector declaration unfinished, missing '}'");
                else if(Stream.Match(TokenValues.Source)) source = Assign();
                else if(Stream.Match(TokenValues.Single)) single = Assign();
                else if(Stream.Match(TokenValues.Predicate)) predicate = Assign();
                else throw new CompilingError(location, ErrorCode.Expected, "Missing source and predicate");
            }
            catch(CompilingError error)
            {
                if(ErrorsControl(error,TokenValues.ValueSeparator)) break;
            }
        }while(!Stream.Match(TokenValues.ClosedCurlyBraces));
        Debug.Log(source is null || single is null || predicate is null);
        if(source == null) new CompilingError(location, ErrorCode.Invalid, "Missing source");
        if(predicate == null)  new CompilingError(location, ErrorCode.Invalid, "Missing predicate");
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
     #endregion
    #region Statements
    private Statement While()
    {
        Debug.Log("estoy en el while");
        CodeLocation location = Stream.Previous().Location;
        Statement? body = null;
        if(!Stream.Match(TokenValues.OpenBracket)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing '(' ");
        Expression? codition = Expression();
        Debug.Log(Stream.LookAhead().Value + " despues del while");
        if(!Stream.Match(TokenValues.ClosedBracket)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing ')' in while declaration' ");
        if(Stream.Match(TokenValues.OpenCurlyBraces))
        {
            body = Statements();
            Debug.Log(Stream.LookAhead().Value + " despues del body lo qu hay es eso");
            if(!Stream.Match(TokenValues.StatementSeparator)) throw new CompilingError(Stream.Previous().Location, ErrorCode.Expected, "missing ';' despues del while");
        } 
        else body  = SimpleStatements();
        return new While(codition,body,location);

    }
    private Statement For()
    {
        Debug.Log("en el for");
        CodeLocation location = Stream.Previous().Location;
        Statement body;
        Token item;
        Token colletion;
        if(Stream.Match(TokenType.Identifier)) item = Stream.Previous();
        else throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing identifier after for");
        if(!Stream.Match(TokenValues.in_)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing keyword 'in' ");
        if(Stream.Check(TokenType.Identifier))
        {
            colletion = Stream.LookAhead();
            Stream.MoveNext();
        }
        else throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing identifier 'collection' in for declaration");
        //colletion = Stream.LookAhead();
        //Debug.Log(colletion.Value);
        //Stream.MoveNext();
        if(Stream.Match(TokenValues.OpenCurlyBraces)) 
        {
            body = Statements();
            if(!Stream.Match(TokenValues.StatementSeparator)) throw new CompilingError(Stream.LookAhead().Location,ErrorCode.Expected, "Missing ';' after an statement block for");
        }
        else body = SimpleStatements();
        return new For(item,colletion,body,location);//revisar bien
    }
    private Statement Statements()
    {
        List<Statement> stmts = new List<Statement>();
        CodeLocation location = Stream.Previous().Location;
        //StatementBlock block = null;
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
                ErrorsControl(error,TokenValues.StatementSeparator); break;

            }
        } while(!Stream.Match(TokenValues.ClosedCurlyBraces));
        Debug.Log("despues del statetment lo que hay es " + Stream.LookAhead().Value);
        //if(Stream.Match(TokenValues.StatementSeparator)) Debug.Log(Stream.LookAhead().Value + " al salir del state");
        return new StatementBlock(stmts,location);
    }
    private Statement SimpleStatements()
    {
        Statement statement;
        Debug.Log("estoy en siple");
        Debug.Log(Stream.LookAhead().Value);
        if(Stream.Match(TokenValues.print))
        {
            statement = new Print(Expression(),Stream.Previous().Location);
            Debug.Log(Stream.LookAhead().Value);
        }
        else if(Stream.Match(TokenType.Identifier)) 
        {
            CodeLocation location = Stream.Previous().Location;
            Stream.MoveBack();
            Expression variable = Expression();
            statement = new StmtExpression(variable, variable.Location);
            Debug.Log("voy a mandar a declarar");
            if(Stream.Match(TokenValues.Assign,TokenValues.AdditionAssignment,TokenValues.SubtractionAssignment))
            {
                statement = Declaration(variable);
            }
        }
        else throw new CompilingError(Stream.Previous().Location, ErrorCode.Invalid, "Invalid expression or invalid statement");
        if(!Stream.Match(TokenValues.StatementSeparator)) throw new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Missing ';' after a simple statement ");
        return statement;    
    }
    private Statement Declaration(Expression variable)
    {
        Debug.Log("Estoy declarando");
        Token operatorToken = Stream.Previous();
        Expression expr = Expression(); 
        return new Declaration(variable, variable.Location, operatorToken,expr);
    }
    private bool ErrorsControl(CompilingError error, string Value = TokenValues.ClosedCurlyBraces)
    {
        Errors.Add(error);
        while(!Stream.Match(Value))
        {
            if(Stream.Check(TokenType.End) || Stream.LookAhead().Value == TokenValues.ClosedCurlyBraces) return true;
            Stream.MoveNext();
        }
        return false;
    }
    #endregion
}