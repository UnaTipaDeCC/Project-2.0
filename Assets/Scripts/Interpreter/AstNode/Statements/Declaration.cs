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
        this.DeclarationScope = DeclarationScope;
        this.location = location;
    }
    public override void Execute()
    {
        try
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
        }
        catch(CompilingError){//Console.WriteLine("algo anda mal en lo de la declaracion");}
        }
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool checkValue = value.CheckSemantic(context,scope,errors);
        DeclarationScope = scope;
        if(variable is Variable)
        {
            //Console.WriteLine("ENEFECTO ERA UNA VARIABLE");
            if(operatorToken.Value == TokenValues.Assign)
            {
                //Console.WriteLine("EN EFECTO ERA UN =");
                DeclarationScope.SetType(variable.ToString(),this.value.Type); 
            }
            else if(operatorToken.Value == TokenValues.SubtractionAssignment || operatorToken.Value == TokenValues.AdditionAssignment)
            {
                if(DeclarationScope.GetType(variable.ToString()) != ExpressionType.Number || value.Type != ExpressionType.Number)
                {
                    errors.Add(new CompilingError(variable.Location,ErrorCode.Invalid,"The variable and the value must be a numbers for this operation ('+=' || '-=')"));
                    return false;
                }
            }
        }
        else if(variable is Predicate)
        {
            if(variable.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(location,ErrorCode.Invalid,"The propierty must be 'power'"));
                return false;
            }
        }
        bool checVariable = variable.CheckSemantic(context, scope, errors);
        return checVariable && checkValue;
        
    }
    public override string ToString()
    {
        return $"variable: {variable.Value}, Valor: {value}";
    }
    


}