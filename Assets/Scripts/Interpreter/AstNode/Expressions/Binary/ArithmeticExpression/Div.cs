using System.Collections.Generic;
public class Div: BinaryExpression
    {
        public override ExpressionType Type {get; set;}
        public override object? Value {get; set;}
        Expression? Right{get; set;}
        Expression? Left{get; set;}
        Token Operator{get; set;}
        //CodeLocation location{get; set;}
        public Div(Expression? left,Token Operator,Expression? right,CodeLocation location) : base(location)
        {
            this.Left = left;
            this.Right = right;
            this.Operator = Operator;
            location = Operator.Location;
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
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "No son numeros"));
                Type = ExpressionType.ErrorType;
                return false;
            }
            if((double)Right.Value == 0)
            {
                errors.Add(new CompilingError(Location,ErrorCode.Invalid, "NO ESTA DEFINIDA LA DIVISION POR CERO"));
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
