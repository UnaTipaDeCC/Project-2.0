using System.Collections.Generic;
using System;
using UnityEngine;
public class Variable : AtomExpression
{
    public string Name;
    Scope VariableScope{get ;set;}
    public CodeLocation Location;
    public Variable(string name, CodeLocation location) : base(location)
    {
        this.Name = name;
        this.Location = location;
    }
    public override string ToString()
    {
        return String.Format(Name);
    }
    public override object? Value { get; set;}
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        VariableScope = scope;
        Debug.Log("en el coso desto de la variable" + VariableScope.Contains(Name));
        //verificar que la variable fue previamente definida
        if(!VariableScope.Contains(Name))
        {
            Debug.Log(Name);
            errors.Add(new CompilingError(Location,ErrorCode.Invalid,"the variable: " + Name + "havent been previously declared"));
            Type = ExpressionType.Identifier;
            return false;
        }
        else
        {
            Type = VariableScope.GetType(Name);
            Debug.Log("en la variable despues de ponerle un tipo, todo ok: + " + Name);
            Debug.Log("el type de la variable es " + Type);
            return true;
        }
        
    }
    public override void Evaluate()
    {
        this.Value = this.VariableScope.Get(Name);
    } 
    public override ExpressionType Type { get; set; }


}