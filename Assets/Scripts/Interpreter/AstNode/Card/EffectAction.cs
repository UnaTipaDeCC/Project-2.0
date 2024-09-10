using System.Collections.Generic;
public class EffectAction : Statement
{
    public Expression Name {get;private set;}
    public Effect Effect {get; set;}
    public PostAction PostAction {get;private set;}
    public Selector Selector{get;private set;}
    //public Dictionary<Token,ExpressionType> ParamsType {get;private set;}
    public List<(Token,Expression)> Params {get;private set;}
    public CodeLocation Location{get;private set;}
    public Scope Scope{get;private set;}
    public EffectAction(Expression name,Selector selector,List<(Token,Expression)> Params, PostAction postAction, CodeLocation location) : base(location)
    {
        Selector = selector;
        PostAction = postAction;
        Name = name;
        Location = location;
        this.Params = Params;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.Scope = scope.CreateChild();
        //chequea semanticamente el nombre y verifica que sea del tipo valido
        bool checkName = Name.CheckSemantic(context, Scope, errors);
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

        //chequea que la cantidad de params sea la correcta
        if(Params.Count != Effect.paramsType.Count)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "The params are invalid"));
            return false;
        }

        //verificar que los parametros del efecto sean los correctos
        bool checkParams = true;
        foreach (var param in Params)
        {
            checkParams = checkParams && param.Item2.CheckSemantic(context,Scope,errors);
            
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
            else
            {
                param.Item2.Evaluate();
                Effect.Scope.Set(param.Item1.Value,param.Item2.Value);
            } 
        }
        
        bool checkSelector;

        if (!(Selector is null))
        {  
            Selector.IsPost = false;
            checkSelector = Selector.CheckSemantic(context,Scope,errors);
            
        }
        else 
        {
            errors.Add(new CompilingError(Selector.Location, ErrorCode.Invalid, "The selection must be declared if the effect isnt a postAction"));
            return false;
        }
        bool checkPostaction = true;
        if(!(PostAction is null))
        {
            //en caso de que el postAction no tenga un selector se le asigna este
            if(PostAction.effectAction.Selector == null)
            {
                PostAction.effectAction.Selector = Selector; 
            }
            //actualiza el selector para que sepa que es un postAction
            PostAction.effectAction.Selector.IsPost = true;
            //se chequea el postAction
            checkPostaction = PostAction.CheckSemantic(context,Scope,errors);
            //se actualiza el scope con el efecto y su postAction
            PostAction.Scope.EffectPair = new EffectPair(this, PostAction.effectAction);
            
        }
        return checkPostaction && checkSelector && checkName;
    }
    public override void Execute()
    {
        Selector.Execute();
        //se asignan los valores del targets y el context del efecto 
        Effect.Scope.Set(Effect.Targets.Value, Selector.FiltredCards);
        Effect.Scope.Set(Effect.Context.Value, GameContext.Instance);
        //se corre el efecto
        Effect.Execute();
        //se corre el postAction
        if(PostAction != null) PostAction.Execute();
    } 
}
public class PostAction: Statement
{
    public EffectAction effectAction;
    public Scope Scope { get; set; }
    public PostAction(EffectAction effectAction, CodeLocation location) : base(location)
    {
        this.effectAction = effectAction;
    }
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        this.Scope = scope.CreateChild();
       
        //verificar el nombre y que el tipo sea valido
        bool checkName = effectAction.Name.CheckSemantic(context, Scope, errors);
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

        //chequea que la cantidad de params sea la correcta
        if(effectAction.Params.Count != effectAction.Effect.paramsType.Count)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "The params are invalid"));
            return false;
        }
        
        //verificar que los parametros del efecto sean los correctos
        bool checkParams = true;
        foreach (var param in effectAction.Params)
        {
            checkParams = checkParams && param.Item2.CheckSemantic(context,Scope,errors);
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
        
        //chequea el selector
        bool checkSelector = effectAction.Selector.CheckSemantic(context,Scope,errors);
        
        //chequea el postAction si tiene
        bool checkPostAction = true;
        if(!(effectAction.PostAction is null))
        {
            effectAction.PostAction.effectAction.Selector.IsPost = true;
            checkPostAction = effectAction.PostAction.CheckSemantic(context,Scope,errors);
        }
        return checkParams && checkSelector && checkPostAction && checkName;
    }
    public override void Execute()
    {
        effectAction.Execute();
    }
}