using System;

namespace Digitteck.Gateway.DMapper
{
    /// <summary>
    /// A lambda function can be used to quickly define a map. 
    /// </summary>
    public sealed class DataMapExpression<TSource, TTarget> : DataMap<TSource, TTarget>
    {
        private Func<TSource, TTarget> _mappingFunction { get; }

        public DataMapExpression(Func<TSource, TTarget> mappingFunction) : base()
        {
            this._mappingFunction = mappingFunction;
        }

        public override TTarget Map(TSource source, DataMapper provider)
        {
            return this._mappingFunction(source);
        }
    }
}
