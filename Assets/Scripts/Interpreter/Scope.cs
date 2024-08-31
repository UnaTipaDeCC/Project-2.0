//using System.Reflection.Metadata;
using System.Collections.Generic;
using System;
public class Scope
{
    public Scope? Parent;
    public Dictionary<string,object> variables;
    public Dictionary<string, ExpressionType> types;
    public Scope()
    {
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
        //string name = variable.Value;
        //if(value == null) throw new CompilingError(new CodeLocation(), ErrorCode.Expected, "Value missing mi loco");
        if (variables.ContainsKey(variable))
        {
            Console.WriteLine("estoy en el set, si la tiene");
            variables[variable] = value;
            //types[name] = value.Type;  
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
            //types[name] = value.Type;  
        }
        else if (Parent != null)
        {
            Parent.SetType(name, value);
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
    public ExpressionType GetType(string name)
    {
        Console.WriteLine("vamo a ver q hay");
        foreach(var v in variables) Console.WriteLine(v.Key);
        if (types.ContainsKey(name))
        {
            Console.WriteLine(types[name] + "es el tipo de " + name);
            return types[name];
        }
        else if (Parent != null)
        {
            return Parent.GetType(name);
        }
        else
        {
            Console.WriteLine("no tenia guardada  la variable");
            return ExpressionType.ErrorType;
        }
    }
    public bool Contains(string name)
    {
        return variables.ContainsKey(name) || (Parent != null && Parent.Contains(name)) || types.ContainsKey(name) ;
    }
}
