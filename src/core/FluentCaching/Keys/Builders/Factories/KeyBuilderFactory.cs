using FluentCaching.Keys.Builders.KeyParts.Factories;
using FluentCaching.Keys.Helpers;

namespace FluentCaching.Keys.Builders.Factories;

internal class KeyBuilderFactory : IKeyBuilderFactory
{
    public IKeyBuilder CreateKeyBuilder()
    {
        var expressionsHelper = new ExpressionsHelper();
        return new KeyBuilder(new KeyContextBuilder(expressionsHelper),
            expressionsHelper, new KeyPartBuilderFactory(expressionsHelper));
    }
}