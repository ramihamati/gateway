namespace Digitteck.Gateway.Service
{
    public sealed class PortObject : ValueObject<int>
    {
        public bool HasValue { get; set; }

        public PortObject(int? port)
        {
            this.HasValue = port.HasValue;
            this.Value = port.Value;
        }

        public override int CompareTo(ValueObject<int> other)
        {
            if (other is null)
            {
                //just a convention since this is never true. We consider that a nul object is at the bottom of the stack
                return 1;
            }

            return this.Value.CompareTo(other.Value);
        }

        protected override bool EqualsCore(object obj)
        {
            return obj is PortObject path && this.Value.Equals(path.Value);
        }

        protected override int GetHashCodeCore()
        {
            return this.Value.GetHashCode();
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}
