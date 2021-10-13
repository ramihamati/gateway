using System;

namespace Digitteck.Gateway.DMapper
{
    internal sealed class DataMapExpressionBuilder<TSource, TTarget> : DataMapBuilder<TSource, TTarget, DataMapExpression<TSource, TTarget>>
    {
        private Func<TSource, TTarget> _mappingFunction { get; }

        public DataMapExpressionBuilder(Func<TSource, TTarget> mappingFunction)
        {
            this._mappingFunction = mappingFunction;
        }

        public override DataMapExpression<TSource, TTarget> Build()
        {
            return new DataMapExpression<TSource, TTarget>(this._mappingFunction);
        }
    }
}
