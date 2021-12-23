namespace FluentCaching.Keys
{
    internal interface IPropertyTracker
    {
        string GetRetrieveKeySimple(string stringKey);

        string GetRetrieveKeyComplex(object obj);
    }
}
