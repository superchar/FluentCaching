namespace FluentCaching.Keys.Exceptions;

public class KeyPartMissingException : KeyPartException
{
    public KeyPartMissingException() : base("The key part is missing")
    {
    }
}