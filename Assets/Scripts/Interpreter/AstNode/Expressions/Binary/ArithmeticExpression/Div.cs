using System.Collections.Generic;
public class Div: BinaryExpression
    {
        public override ExpressionType Type {get; set;}
        public override object? Value {get; set;}
        Expression? Right{get; set;}
        Expression? Left{get; set;}
        Token Operator{get; set;}
        public Div(Expression? left,Token Operator,Expression? right,CodeLocation location) : base(location)
        {
            this.Left = left;
            this.Right = right;
            this.Operator = Operator;
        }

        public override void Evaluate()
        {
            Right.Evaluate();
            Left.Evaluate();
            
            Value = (double)Left.Value / (double)Right.Value;
        }

        public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
        {
            bool right = Right.CheckSemantic(context, scope, errors);
            bool left = Left.CheckSemantic(context, scope, errors);
            if (Right.Type != ExpressionType.Number || Left.Type != ExpressionType.Number)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Should be numbers for this operation"));
                Type = ExpressionType.ErrorType;
                return false;
            }
            if((double)Right.Value == 0)
            {
                errors.Add(new CompilingError(Location,ErrorCode.Invalid, "Cant be divided by 0"));
                Type = ExpressionType.ErrorType;
                return false;
            }
            Type = ExpressionType.Number;
            return right && left;
        }

        public override string ToString()
        {
            if (Value == null)
            {
                return $"({Left} / {Right})";
            }
            return Value.ToString();
        }
    }
