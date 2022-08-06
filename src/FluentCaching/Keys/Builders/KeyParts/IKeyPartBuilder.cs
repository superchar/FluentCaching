using System;
using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Builders.KeyParts;

public interface IKeyPartBuilder
{
    bool IsDynamic { get; }

    string Build(KeyContext keyContext);
}