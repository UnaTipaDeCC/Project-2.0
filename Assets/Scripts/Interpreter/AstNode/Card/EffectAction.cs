using System.Collections.Generic;
public class EffectAction : Statement
{
    public Expression Name {get;private set;}
    public Effect Effect {get; set;}
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
        //chequea semanticamente el nombre y verifica que sea del tipo valido
        bool checkName = Name.CheckSemantic(context, scope, errors);
        if(Name.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(Name.Location,ErrorCode.Expected, "The 'name' must be a text"));
            return false;
        }
        //verificar que el effecto haya sido previamente definido y de ser asi, se le asigna
        Name.Evaluate();
        string effectName = (string)Name.Value;
        if(context.Effects.ContainsKey(effectName)) Effect = context.Effects[effectName];
        else 
        {
            errors.Add(new CompilingError(Location,ErrorCode.Invalid, "The effect must be previously declared"));
            return false;
        }
        //verificar que los parametros del efecto sean los correctos
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
        if(Params.Count != Effect.paramsType.Count)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "The params are invalid"));
            return false;
        }
        bool checkPostaction = true;
        if(!(PostAction is null)) checkPostaction = PostAction.CheckSemantic(context,scope,errors);
        bool checkSelector = true;
        if(!(Selector is null))  checkSelector = Selector.CheckSemantic(context,scope,errors); //revisar si elselector puede ser null
        
        return checkPostaction && checkSelector && checkName;
    }
    public override void Execute()
    {
        Selector.Execute();
        //se asignan los valores del targets y el context del efecto 
       // Effect.Scope.Set(Effect.Targets.Value, Selector.Value);
        Effect.Scope.Set(Effect.Context.Value, GameContext.Instance);
        //se corre el efecto
        Effect.Execute();
        //se corre el postAction
        if(PostAction != null) PostAction.Execute();
    }
}
public class PostAction: Statement
{
    EffectAction effectAction;
    public Scope Scope { get; set; }
    public PostAction(EffectAction effectAction, CodeLocation location) : base(location)
    {
        this.effectAction = effectAction;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        Scope = scope.CreateChild();
        //verificar el nombre y que el tipo sea valido
        bool checkName = effectAction.Name.CheckSemantic(context, scope, errors);
        if(effectAction.Name.Type != ExpressionType.Text)
        {
            errors.Add(new CompilingError(effectAction.Name.Location,ErrorCode.Expected, "The 'name' must be a text"));
            return false;
        }
        //verificar que el effecto haya sido previamente definido y de ser asi, se le asigna
        effectAction.Name.Evaluate();
        string effectName = (string)effectAction.Name.Value;
        if(context.Effects.ContainsKey(effectName)) effectAction.Effect = context.Effects[effectName];
        else 
        {
            errors.Add(new CompilingError(Location,ErrorCode.Invalid, "The effect must be previously declared"));
            return false;
        }
        //verificar que los parametros del efecto sean los correctos
        bool checkParams = true;
        foreach (var param in effectAction.Params)
        {
            checkParams = checkParams && param.Item2.CheckSemantic(context,scope,errors);
            if(!effectAction.Effect.paramsType.ContainsKey(param.Item1.Value))
            {
                errors.Add(new CompilingError(Location,ErrorCode.Invalid, "The params of the card effect must be the same as those previously defined in the effect"));
                return false;
            }
            else if(param.Item2.Type != effectAction.Effect.paramsType[param.Item1.Value])
            {
                errors.Add(new CompilingError(Location,ErrorCode.Invalid,"The type of the param " + param.Item1.Value + " must be " + effectAction.Effect.paramsType[param.Item1.Value]));
                return false;
            }
        }
        return checkParams 
    }
    public override void Execute()
    {
        throw new System.NotImplementedException();
    }
}