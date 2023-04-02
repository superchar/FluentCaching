namespace FluentCaching.Keys.Builders.Factories;

internal interface IKeyBuilderFactory
{
    IKeyBuilder CreateKeyBuilder<TEntity>();
}