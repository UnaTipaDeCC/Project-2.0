using System.Collections.Generic;
public class ElementalProgram : ASTNode
{
    public List<Effect> Effects;
    public List<Card> Cards;
    public List<CompilingError> Errors;
    public ElementalProgram(List<Effect> effects, List<Card> cards, List<CompilingError> errors, CodeLocation location) : base(location)
    {
        Errors = errors;
        Cards = cards;
        Effects = effects;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool checkCards = true;
        bool checkEffects = true;
        //chequear semanticamente todos los efectos
        foreach(Effect effect in Effects)
        {
            effect.Name.Evaluate();
            checkEffects = checkEffects && effect.CheckSemantic(context, scope, errors);
        }
        //chequear semanticamente todas las cartas
        foreach(var card in Cards)
        {
            checkCards = checkCards && card.CheckSemantic(context, scope,errors);
        }
        return checkCards && checkEffects;
    }
    public void Evaluate()
    {
        foreach(Card card in Cards)
        {
            card.Build();
        }
    }
    public override string ToString()
    {
        return base.ToString();
    }
}