using Microsoft.AspNetCore.Mvc;

namespace MathAppHTTP.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MathController : ControllerBase
    {
        

        [HttpGet]
        public IActionResult Get([FromQuery] int firstNumber, [FromQuery] int secondNumber, [FromQuery] string operation)
        {
            if(operation == "add")
            {
                return Ok(firstNumber + secondNumber);
            }
            else if (operation == "subtract")
            {
                return Ok(firstNumber - secondNumber);
            }
            else if (operation == "multiply")
            {
                return Ok(firstNumber * secondNumber);
            }
            else if (operation == "divide")
            {
                if (secondNumber == 0)
                {
                    return BadRequest("Cannot divide by zero.");
                }
                return Ok(firstNumber / secondNumber);
            }
            else
            {
                return BadRequest("Invalid operation. Supported operations are: add, subtract, multiply, divide.");
            }


        }
    }
}
