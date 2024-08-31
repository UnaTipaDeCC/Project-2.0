using System.Collections.Generic;
public abstract class Statement : ASTNode
{
    public abstract void Execute();
    public Scope scope{get;set;} 
    public Statement(CodeLocation location): base(location){}
    
}