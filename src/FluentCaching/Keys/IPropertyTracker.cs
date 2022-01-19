namespace FluentCaching.Keys
{
    internal interface IPropertyTracker
    {
        string GetRetrieveKeySimple(string stringKey);

        string GetRetrieveKeyComplex(object obj);

        void TrackStatic<TValue>(TValue value);
    }
}
