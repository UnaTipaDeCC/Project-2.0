using System.Collections;
using System.Collections.Generic;

 /* This stream has functions to operate over a list of tokens.
 The methods are simple, you can understand them easily */
public class TokenStream : IEnumerable<Token>
{
    private List<Token> tokens;
    private int position;
    public int Position { get { return position; } }

    public TokenStream(IEnumerable<Token> tokens)
    {
        this.tokens = new List<Token>(tokens);
        position = 0;
    }
    public bool End => position >= tokens.Count;
    public void MoveNext(int k = 1) => position += k;
    public void MoveBack(int k = 1)
    {
        if(position != -1) position -= k;
    }
    
    public bool CanLookAhead(int k = 0) => tokens.Count - position > k;
    
    public Token LookAhead(int k = 0) => tokens[position + k];
    
    public Token Previous() =>  tokens[position -1];
    
    public Token NextToken() => tokens[position + 1];
    
    public bool Check(TokenType type)
    {
        if(position > tokens.Count - 1) return false;
        return type == tokens[position].Type;
    }
     /* The next methods are used to scroll through the token list 
     if a condition is satisfied */

     /* In this case, the condition is to have a next position */
    public bool Next()
    {
        if (position < tokens.Count - 1)
        {
            position++;
        }
        return position < tokens.Count;
    }

    public bool Match(params string[] value)
    {
        if (position > tokens.Count-1)
        {
            return false;
        }
        string nextToken = LookAhead().Value;
        foreach (string valu in value)
        {
            if (nextToken == valu)
            {
                position++;
                return true;
            }
        }
        return false;
    }
    public bool Match(TokenType type)
    {
    if (position < tokens.Count && tokens[position].Type == type)
    {
        position++;
        return true;
    }
    return false;
    }
    
    public bool Next( TokenType type )
    {
        if (position < tokens.Count-1 && LookAhead(1).Type == type)
        {
            position++;
            return true;
        }
        return false;
    }
    /* In this case, the next position must match the given value */
    public bool Next(string value)
    {            
        if (position < tokens.Count-1 && LookAhead(1).Value == value)
        {
            position++;
            return true;
        }
        return false;
    }
    
    public IEnumerator<Token> GetEnumerator()
    {
        for (int i = position; i < tokens.Count; i++)
            yield return tokens[i];
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}