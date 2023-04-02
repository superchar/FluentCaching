using FluentCaching.Keys.Builders.KeyParts.Factories;
using FluentCaching.Keys.Helpers;

namespace FluentCaching.Keys.Builders.Factories;

internal class KeyBuilderFactory : IKeyBuilderFactory
{
    public IKeyBuilder CreateKeyBuilder<TEntity>()
    {
        var expressionsHelper = new ExpressionsHelper();
        return new KeyBuilder(new KeyContextBuilder<TEntity>(expressionsHelper),
            expressionsHelper, new KeyPartBuilderFactory(expressionsHelper));
    }
}