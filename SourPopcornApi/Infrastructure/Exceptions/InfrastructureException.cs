namespace Infrastructure.Exceptions;

public class InfrastructureException(string message, Exception exception) : ApplicationException(message, exception);
