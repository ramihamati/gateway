using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Digitteck.Gateway.DMapper
{
   /// <summary>
     /// The data mapper handler has a list of providers and a default one. The default one is used when adding
     /// maps directly in the handler. 
     /// <code>
     ///    public class AlternativeSourceProvider : DataMapperSourceProvider {
     ///         public override void PreRegisterMaps(IDataMapperHandler dataMapperHandler)
     ///         {
     ///             this.AddMap(new MapCar(dataMapperHandler));
     ///         }
     ///     }
     ///     
     ///       DataMapperHandler handler = new DataMapperHandler();
     /// handler.AddSourceProvider(new AlternativeSourceProvider());
     /// CarSource car = new CarSource { Name = "a", Brand = new Brand { BrandName = "test" } };
     /// CarTarget result = handler.MapFrom&lt;CarSource, CarTarget&gt;(car);
     /// </code>
     /// </summary>
        public abstract class MapDefinitionSet
        {
            internal List<MapDefinition> DataMaps { get; }

            protected MapDefinitionSet()
            {
                this.DataMaps = new List<MapDefinition>();
            }

            public void AddMap<TSource, TTarget, Mapper>() where Mapper : DataMap<TSource, TTarget>
            {
                bool zeroParamCtorFound = false;
                foreach (ConstructorInfo ctinfo in typeof(Mapper).GetConstructors())
                {
                    if (ctinfo.GetParameters().Length == 0)
                    {
                        zeroParamCtorFound = true;
                        break;
                    }
                }

                if (!zeroParamCtorFound)
                {
                    throw new Exception($"Could not find a parameterless constructor for {typeof(Mapper).FullName}");
                }

                DataMaps.Add(new MapDefinition(
                    typeof(TSource),
                    typeof(TTarget),
                    () =>
                    {
                        DataMap<TSource, TTarget> map = Activator.CreateInstance<Mapper>();
                        map.TypeSource = typeof(TSource);
                        map.TypeTarget = typeof(TTarget);
                        return map;
                    }));
            }

            public void AddMap<TSource, TTarget>(DataMap<TSource, TTarget> map)
            {
                map.TypeSource = typeof(TSource);
                map.TypeTarget = typeof(TTarget);
                DataMaps.Add(new MapDefinition(
                    typeof(TSource),
                    typeof(TTarget),
                    () =>
                    {
                        map.TypeSource = typeof(TSource);
                        map.TypeTarget = typeof(TTarget);
                        return map;
                    }));
            }

            public void AddMap<TSource, TTarget, TMap>(DataMapBuilder<TSource, TTarget, TMap> dataMapperBuilder) where TMap : DataMap<TSource, TTarget>
            {
                this.DataMaps.Add(new MapDefinition(
                    typeof(TSource),
                    typeof(TTarget),
                    () =>
                    {
                        TMap map = dataMapperBuilder.Build();
                        map.TypeSource = typeof(TSource);
                        map.TypeTarget = typeof(TTarget);
                        return map;
                    }));
            }

            public void AddMap<TSource, TTarget>(Func<TSource, TTarget> mappingFunction)
            {
                this.DataMaps.Add(new MapDefinition(
                    typeof(TSource),
                    typeof(TTarget),
                    () =>
                    {
                        DataMapExpression<TSource, TTarget> map = new DataMapExpression<TSource, TTarget>(mappingFunction);
                        map.TypeSource = typeof(TSource);
                        map.TypeTarget = typeof(TTarget);
                        return map;
                    }));
            }
        }
}
