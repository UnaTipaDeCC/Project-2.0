using UnityEngine;
using System.Collections;
using System.Collections.Generic;
    public class And: BinaryExpression
    {
        Expression? Right{get; set;}
        Expression? Left{get; set;}
        public And(Expression? left, Expression? right, CodeLocation location) :base(location)
        {
            this.Left = left;
            this.Right = right;
        }
        public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
        {
            bool right = Right.CheckSemantic(context,scope,errors);
            bool left = Left.CheckSemantic(context,scope,errors);
            if (Right.Type != ExpressionType.Bool || Left.Type != ExpressionType.Bool)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Should be boolean expressions for 'and' operation"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.Bool;
            return right && left;

        }
        public override void Evaluate()
        {
            Right.Evaluate();
            Left.Evaluate();
            Value =  (bool)Left.Value && (bool)Right.Value;
        }
        public override ExpressionType Type
        {
            get
            {
                return ExpressionType.Bool;
            }
            set {}
        }
        public override object? Value { get; set; }
        public override string ToString()
        {
            if (Value == null)
            {
                return $"({Left} && {Right})";
            }
            return Value.ToString();
        }
    }
