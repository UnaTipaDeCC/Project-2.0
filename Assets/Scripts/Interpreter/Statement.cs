using System.Collections.Generic;
public abstract class Statement
{
    CodeLocation location;
    public abstract void Execute();
    public Scope scope{get;set;}
    public abstract bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors);
    public Statement(CodeLocation location){}
    
}