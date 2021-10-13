namespace Digitteck.Gateway.DMapper
{
    /// <summary>
    /// A data mapper builder eases the process of adding data maps to the handler.
    /// <code>
    /// DataMapperHandler handler = new DataMapperHandler();
    /// handler.AddMap(new MapCarBuilder());
    /// CarSource car = new CarSource { Name = "a", Brand = new Brand { BrandName = "test" } };
    /// CarTarget result = handler.MapFrom&lt;CarSource, CarTarget&gt;(car);
    /// Assert.AreEqual(car.Name, result.Name);
    /// Assert.AreEqual(car.Brand.BrandName, result.Brand.BrandName);
    /// </code>
    /// </summary>
    public abstract class DataMapBuilder<TFrom, TTo, TMap>
         where TMap : DataMap<TFrom, TTo>
    {
        public abstract TMap Build();
    }
}
