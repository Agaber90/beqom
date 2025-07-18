
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace beqom.monad;

public struct Option<T>
{
    private T? _value;

    public bool HasValue { get; private set; }

    public T Value => HasValue
        ? _value!
        : throw new InvalidOperationException("Option has no value");

    public static Option<T> FromValue(T value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        return new Option<T>
        {
            _value = value,
            HasValue = true
        };
    }

    public static Option<T> Empty()
    {
        return new Option<T>
        {
            _value = default,
            HasValue = false
        };
    }

    public T ValueOr(Func<T> fallback)
    {
        return HasValue ? _value! : fallback();
    }

    public Option<TResult> Select<TResult>(Func<T, TResult> selector)
    {
        if (!HasValue)
            return Option<TResult>.Empty();

        return Option<TResult>.FromValue(selector(_value!));
    }
}