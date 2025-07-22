namespace CatFactApplication.Exceptions;

public class FailedDeserializationException(string message) : Exception(message);