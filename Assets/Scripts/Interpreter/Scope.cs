using System.Collections.Generic;
using System;
//using System.Diagnostics;
using UnityEngine;
public class Scope
{
    public Scope? Parent;
    public Dictionary<string,object> variables;
    public Dictionary<string, ExpressionType> types;
    public EffectPair EffectPair{get; set;} //referencia al efecto y su postAction necesaria luego en el selector
    public Scope()
    {
        Parent = null;
        variables = new Dictionary<string, object>();  
        types = new Dictionary<string, ExpressionType>(); 
    }
    public Scope CreateChild()
    {
        Scope child = new Scope();
        child.Parent = this;
        return child;
    }
    public void Set(string variable, object value)
    {
        if (variables.ContainsKey(variable))
        {
            Console.WriteLine("estoy en el set, si la tiene");
            variables[variable] = value;

        }
        else if (Parent != null)
        {
            Parent.Set(variable, value);
        }
        else
        {
            Console.WriteLine("no la tenia");
            variables.Add(variable, value);
            //Console.WriteLine(variables[name] + " despues de agregarla");
           
        }
    }
    public void SetType(string name, ExpressionType value)
    {
        if (types.ContainsKey(name))
        {
            Console.WriteLine("estoy en el settype, si la tiene");
            types[name] = value;
        }
        else if (Parent != null)
        {
            if(AssignType(name,value))
            {
                return;
            }
            else types.Add(name, value);
            //Parent.SetType(name, value);
        }
        else
        {
            Console.WriteLine("no tenia a : " + name);
            types.Add(name, value);
            Console.WriteLine(types[name] + " despues de agregarla");
            //types.Add(name, value.Type);
        }
    }
    public object? Get(string name)
    {
        if (variables.ContainsKey(name))
        {
            return variables[name];
        }
        else if (Parent != null)
        {
            return Parent.Get(name);
        }
        else
        {
            throw new KeyNotFoundException($"Variable '{name}' not found.");
        }
    }
    private bool AssignType(string name, ExpressionType value)
    {
        if(types.ContainsKey(name))
        {
            types[name] = value;
            return true;
        }
        if(Parent != null)
        {
            return Parent.AssignType(name, value);
        }
        return false;
    }
    public ExpressionType GetType(string name)
    {
        if (types.ContainsKey(name))
        {
            Debug.Log(types[name] + "es el tipo de " + name);
            return types[name];
        }
        else if (Parent != null)
        {
            return Parent.GetType(name);
        }
        else
        {
            Debug.Log("no tenia guardada  la variable");
            return ExpressionType.ErrorType;
        }
    }
    public bool Contains(string name)
    {
        return variables.ContainsKey(name) || (Parent != null && Parent.Contains(name)) || types.ContainsKey(name) ;
    }
}

public class EffectPair
{
    public EffectAction Item1 {get;set;}
    public EffectAction Item2 {get;set;}
    public EffectPair(EffectAction item1, EffectAction item2)
    {
        Item1 = item1;
        Item2 = item2;
    }
}
