namespace System_Uznawania_Przychodow.Exceptions;

public class ClientHasContractException : Exception
{
    public ClientHasContractException()
    {
    }

    public ClientHasContractException(string? message) : base(message)
    {
    }

    public ClientHasContractException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}