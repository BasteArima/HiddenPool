/// <summary>
/// Значение на изменениие которого можно подписаться
/// </summary>
public class EventValue<Type>
{
    private Type _value;
    public event ValueHandler onChanged;
    public delegate void ValueHandler(Type newValue, Type oldValue);

    public EventValue(Type value)
    {
        _value = value;
    }

    public Type Value
    {
        get => _value;
        set
        {
            onChanged?.Invoke(value, _value);
            _value = value;
        }
    }
}
