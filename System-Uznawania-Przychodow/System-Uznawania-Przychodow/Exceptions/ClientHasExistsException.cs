namespace System_Uznawania_Przychodow.Exceptions;

public class ClientHasExistsException : Exception
{
    public ClientHasExistsException()
    {
    }

    public ClientHasExistsException(string? message) : base(message)
    {
    }

    public ClientHasExistsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}