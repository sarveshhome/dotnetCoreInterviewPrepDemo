using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ValidatePatientFilter : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionArguments.ContainsKey("id"))
        {
            var idValue = context.ActionArguments["id"];
            if (idValue != null && !int.TryParse(idValue.ToString(), out _))
            {
                context.Result = new BadRequestObjectResult(new { message = "Id must be an integer." });
                return;
            }
        }

        if (context.ActionArguments.ContainsKey("patientName"))
        {
            var nameValue = context.ActionArguments["patientName"];
            if (nameValue == null || string.IsNullOrWhiteSpace(nameValue.ToString()))
            {
                context.Result = new BadRequestObjectResult(new { message = "PatientName must be a non-empty string." });
                return;
            }
        }

        await next();
    }
}
