namespace Digitteck.Gateway.Service.Common.DataValidator
{
    public abstract class Validation<T>
    {
        public abstract ValidationMessage Validate(T notice);

        public abstract bool ExistIfValidationFails { get; }
    }
}
