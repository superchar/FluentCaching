using System;
using System.Collections.Generic;
using FluentCaching.Keys.Exceptions;
using FluentCaching.Keys.Extensions;
using FluentCaching.Keys.Helpers;
using FluentCaching.Keys.Models;

namespace FluentCaching.Keys.Builders;

internal class KeyContextBuilder<TEntity>(IExpressionsHelper expressionsHelper) : IKeyContextBuilder
{
    private static readonly Type EntityType = typeof(TEntity);
        
    private readonly Dictionary<string, bool> _keys = new (); // Guaranteed to be thread safe when readonly (unlike hashset)

    public void AddKey(string key)
    {
        if (!_keys.TryAdd(key, true))
        {
            throw new KeyPropertyDuplicateException(key, EntityType);
        }
    }

    public KeyContext BuildRetrieveContextFromComplexKey(object complexKey)
    {
        var properties = expressionsHelper.GetProperties(complexKey.GetType());
        var contextDictionary = new Dictionary<string, object?>(properties.Length);
        foreach (var property in properties)
        {
            if (!_keys.ContainsKey(property.Name))
            {
                continue;
            }

            contextDictionary[property.Name] = property.Get(complexKey);
        }

        return contextDictionary.Count == _keys.Count
            ? new KeyContext(contextDictionary)
            : throw new KeyIsNotCompleteException(EntityType);
    }

    public KeyContext BuildRetrieveContextFromScalarKey(object scalarKey)
    {
        switch (_keys.Count)
        {
            case > 1:
                throw new KeyIsNotCompleteException(EntityType);
            case 0:
                return KeyContext.Empty;
            default:
            {
                var retrieveContext = new Dictionary<string, object?>
                {
                    {
                        _keys.FirstKey(), scalarKey
                    } 
                };
                return new KeyContext(retrieveContext);
            }
        }
    }

    public KeyContext BuildCacheContext(object cachedObject) => new(cachedObject);
}