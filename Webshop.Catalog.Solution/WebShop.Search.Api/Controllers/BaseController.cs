using Microsoft.AspNetCore.Mvc;
using Webshop.Domain.Common;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace Webshop.Search.Api.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        protected new ActionResult Ok()
        {
            return base.Ok(Envelope.Ok());
        }

        protected ActionResult Ok<T>(T result)
        {
            return base.Ok(Envelope.Ok(result));
        }

        protected ActionResult Error(List<string> errorMessages)
        {
            string errors = string.Join(";", errorMessages);
            return BadRequest(Envelope.Error(errors));
        }

        protected ActionResult Error(string errorMessage)
        {
            return BadRequest(Envelope.Error(errorMessage));
        }

        protected ActionResult Error(List<ValidationFailure> validationErrors)
        {
            List<string> errors = validationErrors.Select(x => x.ErrorMessage + " (" + x.PropertyName + ")").ToList();
            return Error(errors);
        }

        protected ActionResult Error(Error error)
        {
            return BadRequest(Envelope.Error(error.Message + " (" + error.Code + ")"));
        }
    }
}
