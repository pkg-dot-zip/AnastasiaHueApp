namespace AnastasiaHueApp.Util;

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

    // TODO: Make private?! This technically means an instance can contain both (which is the opposite of 'either')!
    public Either(T1? value1 = default, T2? value2 = default)
    {
        _value1 = value1;
        _value2 = value2;

        if (typeof(T1) == typeof(T2))
            throw new ArgumentException(
                $"Can not create an instance of {nameof(Either<object, object>)} because the type of {nameof(T1)} ({typeof(T1)}) is the same as the type of {nameof(T2)} ({typeof(T2)})!");
    }

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