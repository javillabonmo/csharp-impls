#:sdk Microsoft.NET.Sdk.Web
// dotnet run MathAppHttp.cs > app.log 2>&1

var builder = WebApplication.CreateBuilder();


var app = builder.Build();

app.MapGet("/api/v1/mathapp",
    (int firstNumber, int secondNumber, string operation) =>
    {
        var result = operation switch
        {
            "add" => firstNumber + secondNumber,
            "sub" => firstNumber - secondNumber,
            "mul" => firstNumber * secondNumber,
            "div" when secondNumber != 0 => firstNumber / secondNumber,
            _ => throw new InvalidOperationException($"Invalid operation or division by zero: {operation}")
        };
        return new MathResult(result);

    });

app.Run();
return;
public record MathResult(int Result);