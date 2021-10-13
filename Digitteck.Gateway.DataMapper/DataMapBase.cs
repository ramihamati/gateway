using System;

namespace Digitteck.Gateway.DMapper
{
    /// <summary>
    /// A non generic version of the data mapper which represents the base object for the list of maps
    /// </summary>
    public abstract class DataMapBase
    {
        public Type TypeSource { get; set; }

        public Type TypeTarget { get; set; }

        private string MapFullName => $"{TypeSource}.FullName->{TypeTarget}.FullName";

        /// <summary>
        /// Stores the map function as a non-generic method to be called in the provider
        /// </summary>
        public abstract object MapInternal(object source, DataMapper provider);

        public override bool Equals(object obj)
        {
            if (obj is DataMapBase otherMapper)
            {
                return MapFullName == otherMapper.MapFullName;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return MapFullName.GetHashCode();
        }
    }
}
