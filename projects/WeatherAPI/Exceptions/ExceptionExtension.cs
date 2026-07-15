namespace WeatherAPI.Exceptions;


    public static class ExceptionExtensions
    {
        public static T AddData<T>(
            this T exception,
            string key,
            object? value)
            where T : Exception
        {
            exception.Data[key] = value;
            return exception;
        }
    }

