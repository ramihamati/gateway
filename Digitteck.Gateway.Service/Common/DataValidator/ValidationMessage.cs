using System.Collections.Generic;

namespace Digitteck.Gateway.Service.Common.DataValidator
{
    public sealed class ValidationMessage
    {
        public bool IsValid => ValidationMessages.Count == 0;

        public List<string> ValidationMessages { get; set; }

        public string GetValidationMessage()
        {
            return string.Join("\n", this.ValidationMessages);
        }

        private ValidationMessage() { }

        public static ValidationMessage Success()
        {
            return new ValidationMessage { ValidationMessages = new List<string>() };
        }

        public static ValidationMessage Fail(string message)
        {
            return new ValidationMessage { ValidationMessages = new List<string> { message } };
        }

        public static ValidationMessage Fail(List<string> messages)
        {
            return new ValidationMessage { ValidationMessages = messages };
        }

        public void Add(ValidationMessage validation)
        {
            this.ValidationMessages.AddRange(validation.ValidationMessages);
        }
    }
}
