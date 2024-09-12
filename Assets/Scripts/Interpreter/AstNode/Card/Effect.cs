using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
public class Effect : Statement
{
    public CodeLocation location;
    public Scope Scope {get; private set;}
    public Expression Name{get; private set;}
    public Statement Action{get; private set;}
    public Dictionary<string, ExpressionType> paramsType {get; private set;}
    public Token Targets {get; private set;}
    public Token Context{get; private set;}
    List<(Token,Token)> list;
    public Effect(CodeLocation location, Expression name, Statement action,Token targets, Token context, List<(Token,Token)> paramsType = null): base(location)
    {
        this.paramsType = new Dictionary<string, ExpressionType>();
        this.location = location;
        Name = name;
        Action = action;
        Targets = targets;
        Context = context;  
        list = paramsType;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.Scope = scope.CreateChild();

        //chequear el nombre y el tipo del mismo
        bool namecheck = Name.CheckSemantic(context, Scope, errors);
        if(Name.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Name.Location,ErrorCode.Invalid,"The effects name must be a text"));
            return false;
        }

        //Chequear que no haya sido declarado previamente otro efecto con ese nombre
        Name.Evaluate();
        if(context.Effects.ContainsKey((string)Name.Value))
        {
            errors.Add(new CompilingError(location,ErrorCode.Invalid,$"An effect with the name {(string)Name.Value} was already declared"));
            return false;
        }
        
        //chequear que los parametros que se le pasan al efecto sean los permitidos
        //y se agregan al diccionario y se actualiza el type
        foreach(var par in list)
        {
            ExpressionType type;
            switch(par.Item2.Value)
            {
                case "Number":
                type = ExpressionType.Number;
                break;
                case "String":
                type = ExpressionType.Text;
                break;
                case "Bool":
                type = ExpressionType.Bool;
                break;
                default: 
                errors.Add(new CompilingError(location,ErrorCode.Invalid, "The params type must be: string, number or bool"));
                return false;
            }
            paramsType.Add(par.Item1.Value,type);
            Scope.SetType(par.Item1.Value,type);
        }
        Debug.Log(Scope.types.ContainsKey(Targets.Value) + " revisando si el scope la tiene");
        //chequear que la variable tarjets no este definida y en ese caso, definirla en el scope
        Debug.Log("Revisando lo del tarjet " + Scope.GetType(Targets.Value));
        if( Scope.GetType(Targets.Value) == ExpressionType.ErrorType)
        {
            Debug.Log("no lo teniamos y lo agregamos");
            Scope.SetType(Targets.Value,ExpressionType.List);
        }
        else
        {
            Debug.Log("viendo lo del target");
            Debug.Log(Scope.GetType(Targets.Value));
            errors.Add(new CompilingError(Targets.Location,ErrorCode.Invalid, "The name " + Targets.Value + " was already declared"));
            return false;
        }

        //chequear que la variable tarjets no este context y en ese caso, definirla en el scope
        if(Scope.GetType(Context.Value) == ExpressionType.ErrorType) Scope.SetType(Context.Value,ExpressionType.Context);
        else
        {
            Debug.Log("estoy en el action y la variable context no esta definida");
            Debug.Log(Scope.GetType(Context.Value));
            errors.Add(new CompilingError(Context.Location,ErrorCode.Invalid, "The name " + Context.Value + "was already declared"));
            return false;
        } 
        //chequear el action del effect
        bool actioncheck = Action.CheckSemantic(context, Scope, errors);
        Debug.Log("despues de lo del action en el effect" + actioncheck);
        // se chequea que todo este bien y se agrega al contexto
        if(actioncheck && namecheck) context.Effects.Add((string)Name.Value,this);
        Debug.Log(context.Effects.ContainsKey((string)Name.Value));
        return namecheck && actioncheck;

    }
    public override void Execute()
    {
        Action.Execute();      
    }
    public override string ToString()
    {
        return $"Effect: \n\t Name: {Name} \n\t Action : ({Targets.Value}, {Context.Value}) => \n\t {Action}";
    }
}