using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Builders.KeyParts;

public interface IKeyPartBuilder<T>
    where T : class
{
    bool IsDynamic { get; }

    string Build(KeyContext<T> keyContext);
}