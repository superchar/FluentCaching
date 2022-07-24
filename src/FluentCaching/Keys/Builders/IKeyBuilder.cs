namespace FluentCaching.Keys.Builders
{
    internal interface IKeyBuilder
    {
        string BuildFromStringKey(string stringKey);

        string BuildFromObjectKey(object objectKey);

        string BuildFromStaticKey();

        void AppendStatic<TValue>(TValue value);
        
    }
}
