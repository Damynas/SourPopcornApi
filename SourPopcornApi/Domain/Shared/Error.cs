namespace Domain.Shared;

/// <summary>
/// Represents an error.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Error"/> class.
/// </remarks>
/// <param name="code">The error code.</param>
/// <param name="message">The error message.</param>
public class Error : IEquatable<Error>
{
    /// <summary>
    /// The empty error instance.
    /// </summary>
    public static readonly Error None = new() { Code = string.Empty, Message = string.Empty };

    /// <summary>
    /// The null value error instance.
    /// </summary>
    public static readonly Error NullValue = new() { Code = "Error.NullValue", Message = "The specified result value is null." };

    /// <summary>
    /// The condition not met error instance.
    /// </summary>
    public static readonly Error ConditionNotMet = new() { Code = "Error.ConditionNotMet", Message = "The specified condition was not met." };

    /// <summary>
    /// Gets the error code.
    /// </summary>
    public required string Code { get; init; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public required string Message { get; init; }

    public static implicit operator string(Error error) => error.Code;

    public static bool operator ==(Error? a, Error? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Error? a, Error? b) => !(a == b);

    public virtual bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Code == other.Code && Message == other.Message;
    }

    public override bool Equals(object? obj) => obj is Error error && Equals(error);

    public override int GetHashCode() => HashCode.Combine(Code, Message);

    public override string ToString() => Code;
}
