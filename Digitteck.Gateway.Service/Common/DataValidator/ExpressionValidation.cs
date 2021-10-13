using System;
using System.Linq.Expressions;

namespace Digitteck.Gateway.Service.Common.DataValidator
{
    internal sealed class ExpressionValidation<T> : Validation<T>
    {
        private readonly bool _exitValidationsIfThisFails;

        public ExpressionValidation(Expression<Func<T, bool>> expression, string errorMessage, bool exitValidationsIfThisFails)
        {
            ValidationExpression = expression;
            ErrorMessage = errorMessage;
            _exitValidationsIfThisFails = exitValidationsIfThisFails;
        }

        public Expression<Func<T, bool>> ValidationExpression { get; }
        public string ErrorMessage { get; }

        public override bool ExistIfValidationFails => _exitValidationsIfThisFails;

        public override ValidationMessage Validate(T notice)
        {
            if (!ValidationExpression.Compile()(notice))
            {
                return ValidationMessage.Fail(ErrorMessage);
            }

            return ValidationMessage.Success();
        }
    }
}
