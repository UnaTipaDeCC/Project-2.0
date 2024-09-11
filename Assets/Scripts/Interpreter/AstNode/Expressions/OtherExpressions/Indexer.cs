using System.Collections.Generic;
using UnityEngine;
public class Indexer : Expression
{
    Expression exp;
    Expression index;
    CodeLocation location;
    public Indexer(Expression expression, Expression index, CodeLocation location) : base(location)
    {
        exp = expression;
        this.index = index;
        this.location = location;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool checkExp = exp.CheckSemantic(context,scope,errors);
        bool checkIndex = index.CheckSemantic(context,scope,errors);
        if(exp.Type != ExpressionType.List )
        {
            errors.Add(new CompilingError(exp.Location,ErrorCode.Invalid, "Can't index something that is not a list"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        Debug.Log(index.Type);
        if(index.Type != ExpressionType.Number)
        {
            errors.Add(new CompilingError(exp.Location,ErrorCode.Invalid, "Index must be a number"));
            Type = ExpressionType.ErrorType;
            return false;
        }
        index.Evaluate();

        /*if(!(index.Value is int))
        {
            errors.Add(new CompilingError(exp.Location,ErrorCode.Invalid, "Index must be an int number"));
            Type = ExpressionType.ErrorType;
            return false;
        }*/
        Type = ExpressionType.Card;
        return checkExp && checkIndex;
    }
    public override void Evaluate()
    {
        index.Evaluate();
        exp.Evaluate();
        double a = (double)index.Value; 
        List<CardGame> lista = (List<CardGame>)exp.Value;
        Value = lista[(int)a];//[(int)index.Value];;
    }
    public override ExpressionType Type { get; set; }
    public override object? Value { get; set; }
}