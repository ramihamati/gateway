using System;
using System.Runtime.Serialization;

namespace Digitteck.Gateway.Service
{
    [Serializable]
    public abstract class ValueObject<TValue>
       : IComparable<ValueObject<TValue>>, IEquatable<ValueObject<TValue>>, ISerializable
    {
        public TValue Value { get; protected set; }

        protected Type Type => typeof(TValue);

        protected ValueObject()
        {

        }

        /*
         * Contracts 
         */
        protected abstract bool EqualsCore(object obj);

        protected abstract int GetHashCodeCore();

        public abstract int CompareTo(ValueObject<TValue> other);

        public bool Equals(ValueObject<TValue> other) => this.EqualsCore(other);

        /*
         * Methods
         */
        public override bool Equals(object obj) => this.EqualsCore(obj);

        public override int GetHashCode() => this.GetHashCodeCore();

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.Value = (TValue)info.GetValue(nameof(Value), this.Type);
        }

        public static bool operator ==(ValueObject<TValue> first, ValueObject<TValue> second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null)) return true;
            if (ReferenceEquals(first, null) && !ReferenceEquals(second, null)) return false;
            if (!ReferenceEquals(first, null) && ReferenceEquals(second, null)) return false;

            return first.Equals(second);
        }

        public static bool operator !=(ValueObject<TValue> first, ValueObject<TValue> second)
        {
            return !(first == second);
        }

        public static bool operator >(ValueObject<TValue> first, ValueObject<TValue> second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null)) return false;
            if (ReferenceEquals(first, null) && !ReferenceEquals(second, null)) return false;
            if (!ReferenceEquals(first, null) && ReferenceEquals(second, null)) return true;

            return first.CompareTo(second) > 0;
        }

        public static bool operator <(ValueObject<TValue> first, ValueObject<TValue> second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null)) return false;
            if (ReferenceEquals(first, null) && !ReferenceEquals(second, null)) return true;
            if (!ReferenceEquals(first, null) && ReferenceEquals(second, null)) return false;

            return first.CompareTo(second) < 0;
        }

        public static bool operator >=(ValueObject<TValue> first, ValueObject<TValue> second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null)) return true;
            if (ReferenceEquals(first, null) && !ReferenceEquals(second, null)) return false;
            if (!ReferenceEquals(first, null) && ReferenceEquals(second, null)) return true;

            return first.CompareTo(second) >= 0;
        }

        public static bool operator <=(ValueObject<TValue> first, ValueObject<TValue> second)
        {
            if (ReferenceEquals(first, null) && ReferenceEquals(second, null)) return true;
            if (ReferenceEquals(first, null) && !ReferenceEquals(second, null)) return true;
            if (!ReferenceEquals(first, null) && ReferenceEquals(second, null)) return false;

            return first.CompareTo(second) <= 0;
        }

        public static implicit operator TValue(ValueObject<TValue> valueObject)
        {
            return (!(valueObject is null)) ? valueObject.Value : default(TValue);
        }
    }
}
