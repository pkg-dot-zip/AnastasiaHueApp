namespace AnastasiaHueApp.Util;

/// <summary>
/// Class that holds one value, either being of type <typeparam name="T1"/> or <typeparam name="T2"/>.
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
public class Either<T1, T2>
{
    private readonly T1? _value1;
    private readonly T2? _value2;

    public Either(T1 value1) : this(value1, default)
    {
    }

    public Either(T2 value2) : this(default, value2)
    {
    }

    public Either(T1? value1 = default, T2? value2 = default)
    {
        _value1 = value1;
        _value2 = value2;

        if (value1 is null && value2 is null)
            throw new ArgumentNullException(
                $"Can not create an instance of {nameof(Either<object, object>)} because both {nameof(value1)} and {nameof(value2)} are null. This is an {nameof(Either<object, object>)} class, not a 'neither' class!");

        if (value1 is not null && value2 is not null)
            throw new ArgumentException(
                $"Can not create an instance of {nameof(Either<object, object>)} because both {nameof(value1)} and {nameof(value2)} have values set. This is an {nameof(Either<object, object>)} class, not a 'both' class!");

        if (typeof(T1) == typeof(T2))
            throw new ArgumentException(
                $"Can not create an instance of {nameof(Either<object, object>)} because the type of {nameof(T1)} ({typeof(T1)}) is the same as the type of {nameof(T2)} ({typeof(T2)})!");
    }

    /// <summary>
    /// Checks whether the set value is of type <typeparamref name="T"/>. If it is of type <typeparamref name="T"/> the value is accessible from the <see langword="out"/> parameter <paramref name="value"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> to attempt retrieval of.</typeparam>
    /// <param name="value">If this method returns <c>true</c>, this will <see langword="out"/> an instance <paramref name="value"/> of <typeparamref name="T"/> with appropriate properties set.</param>
    /// <returns><c>true</c> if <paramref name="value"/> was of type <typeparamref name="T"/> and thus not <see langword="null"/>. <c>false</c> if it was not of type <typeparamref name="T"/> and thus is <see langword="null"/>.</returns>
    public bool IsType<T>(out T? value)
    {
        if (typeof(T) == typeof(T1) && _value1 is T typedValue1)
        {
            value = typedValue1;
            return true;
        }

        if (typeof(T) == typeof(T2) && _value2 is T typedValue2)
        {
            value = typedValue2;
            return true;
        }

        value = default;
        return false;
    }
}