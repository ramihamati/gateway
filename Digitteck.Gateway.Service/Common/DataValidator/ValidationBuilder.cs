using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Digitteck.Gateway.Service.Common.DataValidator
{
    public class ValidationBuilder<T> : Validation<T>
    {
        private readonly List<Validation<T>> _validators;

        public ValidationBuilder()
        {
            _validators = new List<Validation<T>>();
        }

        public override bool ExistIfValidationFails => _validators.Any(x=>x.ExistIfValidationFails);

        public void AddValidation(Expression<Func<T, bool>> expression, string invalidMsg, bool preventNextValidations)
        {
            _validators.Add(new ExpressionValidation<T>(expression, invalidMsg, preventNextValidations));
        }

        public void AddValidation(Validation<T> validator)
        {
            _validators.Add(validator);
        }

        public override ValidationMessage Validate(T notice)
        {
            ValidationMessage message = ValidationMessage.Success();

            foreach (var validator in _validators)
            {
                ValidationMessage validation = validator.Validate(notice);
                
                message.Add(validation);

                if (validator.ExistIfValidationFails && !validation.IsValid)
                {
                    break;
                }
                
            }

            return message;
        }
    }
}
