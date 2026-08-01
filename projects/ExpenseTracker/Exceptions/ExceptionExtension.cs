namespace ExpenseTracker.Exceptions;

public static class ExceptionExtensions
{
    public static T AddData<T>(
        this T exception,
        string key,
        object? value)
        where T : Exception
    {
        if (!exception.Data.Contains(key))
        {
            exception.Data[key] = value;
        }

        return exception;
    }
}
