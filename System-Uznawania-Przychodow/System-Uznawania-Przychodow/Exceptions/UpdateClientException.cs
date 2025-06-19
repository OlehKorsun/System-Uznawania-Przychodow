namespace System_Uznawania_Przychodow.Exceptions;

public class UpdateClientException : Exception
{
    public UpdateClientException()
    {
    }

    public UpdateClientException(string? message) : base(message)
    {
    }

    public UpdateClientException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}