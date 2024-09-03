using System.Collections.Generic;
public class EffectAction : Statement
{
    public Expression Name {get;private set;}
    public Effect Effect {get;private set;}
    public EffectAction PostAction {get;private set;}
    public Selector Selector{get;private set;}
    //public Dictionary<Token,ExpressionType> ParamsType {get;private set;}
    public List<(Token,Expression)> Params {get;private set;}
    public CodeLocation Location{get;private set;}
    public Scope Scope{get;private set;}
    public EffectAction(Expression name,Selector selector,List<(Token,Expression)> Paramas, EffectAction postAction, CodeLocation location) : base(location)
    {
        Selector = selector;
        PostAction = postAction;
        Name = name;
        Location = location;
        this.Params = Paramas;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        Scope = scope.CreateChild();
        bool checkName = Name.CheckSemantic(context, scope, errors);
        if(Name.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Name.Location,ErrorCode.Expected, "The 'name' must be a text"));
            return false;
        }
        Name.Evaluate();
        string effectName = (string)Name.Value;
        if(context.Effects.ContainsKey(effectName)) Effect = context.Effects[effectName];
        else 
        {
            errors.Add(new CompilingError(Location,ErrorCode.Invalid, "The effect must be previously declared"));
            return false;
        }
        bool checkParams = true;
        foreach (var param in Params)
        {
            checkParams = checkParams && param.Item2.CheckSemantic(context,scope,errors);
            
            if(!Effect.paramsType.ContainsKey(param.Item1.Value))
            {
                errors.Add(new CompilingError(Location,ErrorCode.Invalid, "The params of the card effect must be the same as those previously defined in the effect"));
                return false;
            }
            else if(param.Item2.Type != Effect.paramsType[param.Item1.Value])
            {
                errors.Add(new CompilingError(Location,ErrorCode.Invalid,"The type of the param " + param.Item1.Value + " must be " + Effect.paramsType[param.Item1.Value]));
                return false;
            }
        }
        bool checkPostaction = true;
        if(!(PostAction is null)) checkPostaction = PostAction.CheckSemantic(context,scope,errors);
        bool checkSelector = true;
        if(!(Selector is null))  checkSelector = Selector.CheckSemantic(context,scope,errors); //revisar si elselector puede ser null
        
        return checkPostaction && checkSelector && checkName;
    }
    public override void Execute()
    {
        Effect.Execute();
    }
}