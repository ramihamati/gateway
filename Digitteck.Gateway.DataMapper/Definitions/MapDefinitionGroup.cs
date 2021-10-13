using System;
using System.Collections.Generic;
using System.Linq;

namespace Digitteck.Gateway.DMapper
{
    public sealed class MapDefinitionGroup
    {
        public List<MapDefinitionSet> SourceProviders { get; }

        public MapDefinitionSet DefaultSourceProvider { get; }

        public MapDefinitionGroup()
        {
            SourceProviders = new List<MapDefinitionSet>();

            //add at least one
            DefaultSourceProvider = new DefaultMapDefinitionSet();
        }

        public void AddSourceProvider(MapDefinitionSet sourceProvider)
        {
            this.SourceProviders.Add(sourceProvider);
        }

        public void AddMap<TSource, TTarget>(DataMap<TSource, TTarget> map)
        {
            this.DefaultSourceProvider.AddMap(map);
        }

        public void AddMap<TSource, TTarget, TMap>() where TMap : DataMap<TSource, TTarget>
        {
            this.DefaultSourceProvider.AddMap<TSource, TTarget, TMap>();
        }

        public void AddMap<TSource, TTarget, TMap>(DataMapBuilder<TSource, TTarget, TMap> dataMapperBuilder) where TMap : DataMap<TSource, TTarget>
        {
            this.DefaultSourceProvider.AddMap(dataMapperBuilder);
        }

        public void AddMap<TSource, TTarget>(Func<TSource, TTarget> mappingFunction)
        {
            this.DefaultSourceProvider.AddMap(mappingFunction);
        }

        internal IEnumerable<MapDefinition> GetMapConstructors()
        {
            foreach (MapDefinition mapConstructor in DefaultSourceProvider.DataMaps)
            {
                yield return mapConstructor;
            }

            foreach (MapDefinition mapConstructor in this.SourceProviders.SelectMany(x => x.DataMaps))
            {
                yield return mapConstructor;
            }
        }

        public DataMapper Build()
        {
            return new DataMapper(this);
        }
    }
}
