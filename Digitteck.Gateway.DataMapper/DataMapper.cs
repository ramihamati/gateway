using System;
using System.Collections.Generic;
using System.Linq;

namespace Digitteck.Gateway.DMapper
{
    public sealed class DataMapper
    {
        internal List<MapDefinition> DataMapperConstructors { get; }

        //maps are constructed from MapDefintions when they are requested
        internal List<DataMapBase> Maps { get; }

        internal DataMapper(MapDefinitionGroup dataMapperHandler)
        {
            DataMapperConstructors = dataMapperHandler.GetMapConstructors().ToList();
            Maps = new List<DataMapBase>();
        }

        internal List<DataMapBase> GetMapsForSource(Type source)
        {
            IEnumerable<MapDefinition> ctors = DataMapperConstructors.Where(x => x.SourceType.Equals(source));

            foreach(var ctor in ctors)
            {
                if (!this.Maps.Any(x=>x.TypeSource.Equals(ctor.SourceType) && x.TypeTarget.Equals(ctor.TargetType)))
                {
                    var dataMapBase = ctor.Get();
                    this.Maps.Add(dataMapBase);
                }
            }

            //check if they are build
            List<DataMapBase> maps = Maps.Where(x => x.TypeSource.Equals(source)).ToList();

            return maps;
        }

        internal DataMapBase GetMapFor(Type source, Type target)
        {
            //check if they are build
            DataMapBase dataMapperBase = Maps.FirstOrDefault(x => x.TypeSource.Equals(source) && x.TypeTarget.Equals(target));

            if (dataMapperBase == null)
            {
                //check if there is something to be built
                MapDefinition ctor = this.DataMapperConstructors.FirstOrDefault(x => x.SourceType.Equals(source) && x.TargetType.Equals(target));

                if (ctor != null)
                {
                    dataMapperBase = ctor.Get();

                    this.Maps.Add(dataMapperBase);
                }
            }

            return dataMapperBase;
        }

        internal DataMapBase GetMapFor<TSource, TTarget>()
        {
            return GetMapFor(typeof(TSource), typeof(TTarget));
        }

        public bool HasMap(Type source, Type target)
        {
            return this.DataMapperConstructors.Any(x => x.SourceType.Equals(source) && x.TargetType.Equals(target));
        }

        public bool HasMap<TSource, TTarget>()
        {
            return this.HasMap(typeof(TSource), typeof(TTarget));
        }

        public TTarget Map<TSource, TTarget>(TSource value)
        {
            if (value == null)
            {
                return default(TTarget);
            }

            DataMapBase dataMapperBase = GetMapFor<TSource, TTarget>();

            //check for child types
            if (dataMapperBase == null)
            {
                Type actualType = value.GetType();
                //find valid maps
                IEnumerable<DataMapBase> maps = GetMapsForSource(actualType);

                foreach (DataMapBase validMap in maps)
                {
                    if (typeof(TSource).IsAssignableFrom(validMap.TypeSource)
                        && typeof(TTarget).IsAssignableFrom(validMap.TypeTarget))
                    {
                        dataMapperBase = validMap;
                        break;
                    }
                }
            }

            if (dataMapperBase == null)
            {
                throw new Exception($"Could not find a match for a map from {typeof(TSource).Name} to {typeof(TTarget).Name}");
            }

            return (TTarget)dataMapperBase.MapInternal(value, this);
        }

        public List<TTarget> Map<TSource, TTarget>(IEnumerable<TSource> values)
        {
            if (values == null) return new List<TTarget>();
            if (!values.Any()) return new List<TTarget>();

            DataMapBase dataMapperBase = GetMapFor<TSource, TTarget>();

            if (dataMapperBase == null)
            {
                //if it's a base type and it has multiple sub types, for each one we check a map
                return values.Select(x => Map<TSource, TTarget>(x)).ToList();
            }

            if (dataMapperBase == null)
            {
                throw new Exception($"Could not find a match for a map from {typeof(TSource).Name} to {typeof(TTarget).Name}");
            }

            List<TTarget> conversions = new List<TTarget>();

            foreach (var model in values)
            {
                conversions.Add((TTarget)dataMapperBase.MapInternal(model, this));
            }

            return conversions;
        }

        public TTarget Map<TTarget>(object value)
        {
            if (value == null)
            {
                return default(TTarget);
            }

            DataMapBase dataMapperBase = GetMapFor(value.GetType(), typeof(TTarget));

            //check for child types
            if (dataMapperBase == null)
            {
                //check for children
                Type actualType = value.GetType();
                //find valid maps
                IEnumerable<DataMapBase> maps = GetMapsForSource(actualType);

                foreach (DataMapBase validMap in maps)
                {
                    if (typeof(TTarget).IsAssignableFrom(validMap.TypeTarget))
                    {
                        dataMapperBase = validMap;
                        break;
                    }
                }
            }

            if (dataMapperBase == null)
            {
                throw new Exception($"Could not find a match for a map from {value.GetType().FullName} to {typeof(TTarget).Name}");
            }

            return (TTarget)dataMapperBase.MapInternal(value, this);
        }

        public IList<TTarget> Map<TTarget>(IEnumerable<object> values)
        {
            if (values == null)
            {
                return new List<TTarget>();
            }

            DataMapBase dataMapperBase = GetMapFor(values.GetType(), typeof(TTarget));

            //check for child types
            if (dataMapperBase == null)
            {
                //if it's a base type and it has multiple sub types, for each one we check a map
                return values.Select(x => Map<TTarget>(x)).ToList();
            }

            if (dataMapperBase == null)
            {
                throw new Exception($"Could not find a match for a map from {values.GetType().FullName} to {typeof(TTarget).Name}");
            }

            List<TTarget> conversions = new List<TTarget>();

            foreach (var model in values)
            {
                conversions.Add((TTarget)dataMapperBase.MapInternal(model, this));
            }

            return conversions;
        }
    }
}
