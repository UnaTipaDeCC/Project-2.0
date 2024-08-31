using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Effect : Statement
{
    public CodeLocation location;
    public Scope scope;
    public Expression name;
    public Statement action;
    public Dictionary<string, ExpressionType> paramsType ;
    public Token targets;
    public Token context;//mas adelante sera un contexto o sea una clase contexto, creo
    public Effect(CodeLocation location, Expression name, Statement action,Token targets, Token context, List<(Token,Token)> paramsType = null): base(location)
    {
        this.paramsType = new Dictionary<string, ExpressionType>();
        this.location = location;
        this.name = name;
        this.action = action;
        this.targets = targets;
        this.context = context;
        //this.paramsType = paramsType.ToDictionary(pair => pair.Item1, pair => pair.Item2);
        foreach(var par in paramsType)
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
                default: throw new CompilingError(location,ErrorCode.Invalid, "The params type must be: string, number or bool");
            }
            this.paramsType.Add(par.Item1.Value,type);
        }
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.scope = scope;
        /*if(!scope.Contains(targets.Value))
        {
            scope.types.Add(targets.Value,ExpressionType.List);
            Console.WriteLine(targets.Value + "la agregamos con exito");
        }
        else
        {
            errors.Add(new CompilingError(targets.Location,ErrorCode.Invalid, "The name " + targets.Value + " was already declared"));
            return false;
        }*/
       /* if(!scope.Contains(this.context.Value)) scope.types.Add(this.context.Value,ExpressionType.Context);
        else
        {
            errors.Add(new CompilingError(targets.Location,ErrorCode.Invalid, "The name " + this.context.Value + "was already declared"));
            return false;
        } //supongo que una vez que se crea una una instancia se crea el contexto*/
        bool namecheck = name.CheckSemantic(context, scope, errors);
        bool actioncheck = action.CheckSemantic(context, scope,errors);
        if(name.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(name.Location,ErrorCode.Invalid,"The effects name must be a text"));
            return false;
        }
        /*foreach(var par in paramsType)
        {
            if(par.Value.Value != "Bool" && par.Value.Value != "Number" && par.Value.Value != "String")
            errors.Add(new CompilingError(name.Location,ErrorCode.Invalid,"The params type must be: string, number o bool"));
            return false;
        }*/
        name.Evaluate();
        /*foreach(var par in context.Effects)
        {
            Console.WriteLine("vamos a ver que hay en el contexto");
            Console.WriteLine(par.Key);
        }*/
        context.Effects.Add((string)name.Value,this);
        return namecheck && actioncheck;

    }
    public override void Execute()
    {
        name.Evaluate();
        action.Execute();
        scope.Set(targets.Value,null);       
    }
    public override string ToString()
    {
        return $"Effect: \n\t Name: {name} \n\t Action : ({targets.Value}, {this.context.Value}) => \n\t {action}";
    }
}