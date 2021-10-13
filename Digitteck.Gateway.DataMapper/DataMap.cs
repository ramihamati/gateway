namespace Digitteck.Gateway.DMapper
{
    /// <summary>
    /// A base class used to create a data mapper between one type and another. This is a simple mapper that allows the user
    /// to manually define the target object from the source object
    /// <code>
    ///  DataMapperHandler handler = new DataMapperHandler();
    /// handler.AddMap(new MapCar(handler));
    /// CarSource car = new CarSource { Name = "a", Brand = new Brand { BrandName = "test" } };
    /// CarTarget result = handler.MapFrom&lt;CarSource, CarTarget&gt;(car);
    /// </code>
    /// </summary>
    public abstract class DataMap<TSource, TTarget> : DataMapBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataMap{TSource, TTarget}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The handler is the provider of all maps. This is passed to the data mapper because the user may need to perform nested mappings.</param>
        protected DataMap()
        {
            this.TypeSource = typeof(TSource);
            this.TypeTarget = typeof(TTarget);
        }

        public abstract TTarget Map(TSource source, DataMapper provider);

        public override object MapInternal(object source, DataMapper provider)
        {
            return Map((TSource)source, provider);
        }
    }
}
