using System;

namespace Digitteck.Gateway.DMapper
{
    public class MapDefinition
    {
        private readonly Func<DataMapBase> _mapProvider;

        public MapDefinition(Type source, Type target, Func<DataMapBase> mapProvider)
        {
            _mapProvider = mapProvider;
            this.SourceType = source;
            this.TargetType = target;
        }

        public Type SourceType { get; internal set; }
        public Type TargetType { get; internal set; }

        public DataMapBase Get()
        {
            return _mapProvider();
        }
    }
}
