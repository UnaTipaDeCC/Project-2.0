using System.Diagnostics;
using System.Collections.Generic;

public class Declaration : Statement
{
    Expression variable;
    Token operatorToken {get;set;}
    Expression value;
    Scope DeclarationScope;
    CodeLocation location;
    public Declaration(Expression variable, CodeLocation location, Token Operator,  Expression value = null) : base(location)
    {
        this.variable = variable;
        this.operatorToken = Operator;
        this.value = value;
        this.location = location;
    }
    public override void Execute()
    {
        value.Evaluate();
        if(variable is Variable)
        {
            switch(operatorToken.Value)
            {
                case TokenValues.Assign:
                DeclarationScope.Set(variable.ToString(),value.Value);
                break;
                case TokenValues.SubtractionAssignment:
                DeclarationScope.Set(variable.ToString(),(double)value.Value + (double)DeclarationScope.Get(variable.ToString()));
                break;
                case TokenValues.AdditionAssignment:
                DeclarationScope.Set(variable.ToString(),(double)DeclarationScope.Get(variable.ToString()) - (double)value.Value);
                break;
            }   
        }  
        if(variable is Property)      
        {
            variable.Evaluate();
            Property property = (Property)variable;
            CardGame cardGame = (CardGame)property.Caller.Value;
            switch(operatorToken.Value)
            {
                case TokenValues.Assign:
                cardGame.Damage = (int)value.Value;
                break;
                case TokenValues.AdditionAssignment:
                cardGame.Damage = (int)value.Value + (int)variable.Value;
                break;
                case TokenValues.SubtractionAssignment:
                cardGame.Damage = (int)variable.Value - (int)value.Value;
                break;
            }
        }
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        DeclarationScope = scope;
        //chequear semanticamente el valor de la variable
        bool checkValue = value.CheckSemantic(context,scope,errors);
        
        if(variable is Variable)
        {
            if(operatorToken.Value == TokenValues.Assign)
            {
                DeclarationScope.SetType(variable.ToString(),value.Type); 
            }
            else if(operatorToken.Value == TokenValues.SubtractionAssignment || operatorToken.Value == TokenValues.AdditionAssignment)
            {
                if(DeclarationScope.GetType(variable.ToString()) != ExpressionType.Number || value.Type != ExpressionType.Number)
                {
                    errors.Add(new CompilingError(variable.Location,ErrorCode.Invalid,"The variable and the value must be numbers for this operation ('+=' || '-=')"));
                    return false;
                }
            }
        }
        else if(variable is Property)
        {
            variable.CheckSemantic(context,DeclarationScope,errors);
            if(variable.Type != ExpressionType.Number || value.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(location,ErrorCode.Invalid,"The propierty must be 'power' and the value a number"));
                return false;
            }
        }
        //bool checVariable = variable.CheckSemantic(context, scope, errors);
        return checkValue;
        
    }
    public override string ToString()
    {
        return $"variable: {variable.Value}, Valor: {value}";
    }
    


}