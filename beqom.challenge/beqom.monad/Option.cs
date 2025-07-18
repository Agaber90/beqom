namespace beqom.monad;

public struct Option<T>
{
    private T _value;
    public bool HasValue { get; private set; }

    public T Value
    {
        get
        {
            if (!HasValue)
            {
                throw new InvalidOperationException("Option has no value.");
            }

            return _value;
        }
    }

    internal Option(T value, bool hasValue)
    {
        _value = value;
        HasValue = hasValue;
    }

    public static Option<T> Empty()
    {
        return new Option<T>(default!, false);
    }

    public static Option<T> FromValue(T value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return new Option<T>(value, true);
    }

    public static bool operator ==(Option<T> left, Option<T> right)
    {
        if (!left.HasValue && !right.HasValue)
        {
            return true;
        }
        if (left.HasValue != right.HasValue)
        {
            return false;
        }

        return Equals(left._value, right._value);
    }

    public static bool operator !=(Option<T> left, Option<T> right) => !(left == right);

    public override bool Equals(object? obj)
    {
        if (obj is Option<T> other)
        {
            return this == other;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HasValue ? 1 : 0;
    }

    public TResult ValueOr<TResult>(Func<TResult> elseBranch)
    {
        if (HasValue)
        {
            return (TResult)(object)_value!;
        }

        return elseBranch();
    }

    public T ValueOr(Func<T> elseBranch)
    {
        if (HasValue)
        {
            return _value;
        }

        return elseBranch();
    }

    public Option<TResult> Select<TResult>(Func<T, TResult> selector)
    {
        if (!HasValue)
        {
            return Option<TResult>.Empty();
        }

        return Option<TResult>.FromValue(selector(_value));
    }
}

public static class Option
{
    public static Option<T> Empty<T>()
    {
        return Option<T>.Empty();
    }

    public static Option<T> FromValue<T>(T value)
    {
        return Option<T>.FromValue(value);
    }
}