using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Webshop.Search.Api.Utilities;
using Webshop.Domain.Common;
using Webshop.Domain.ValueObjects;
using System;

namespace Webshop.Search.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Returnerer en succesfuld HTTP 200 respons uden data.
        /// </summary>
        /// <returns>HTTP 200 OK.</returns>
        protected new ActionResult Ok()
        {
            return base.Ok(Envelope.Ok());
        }

        /// <summary>
        /// Returnerer en succesfuld HTTP 200 respons med data.
        /// </summary>
        /// <typeparam name="T">Typen af data.</typeparam>
        /// <param name="result">Data.</param>
        /// <returns>HTTP 200 OK med data.</returns>
        protected ActionResult Ok<T>(T result)
        {
            return base.Ok(Envelope.Ok(result));
        }

        /// <summary>
        /// Returnerer en fejlrespons med en liste af fejlmeddelelser.
        /// </summary>
        /// <param name="errorMessages">Liste af fejlmeddelelser.</param>
        /// <returns>HTTP 400 Bad Request.</returns>
        protected ActionResult Error(List<string> errorMessages)
        {
            var errors = string.Join(";", errorMessages);
            return BadRequest(Envelope.Error(errors));
        }

        /// <summary>
        /// Returnerer en fejlrespons med en enkelt fejlmeddelelse.
        /// </summary>
        /// <param name="errorMessage">Fejlmeddelelse.</param>
        /// <returns>HTTP 400 Bad Request.</returns>
        protected ActionResult Error(string errorMessage)
        {
            return BadRequest(Envelope.Error(errorMessage));
        }

        /// <summary>
        /// Returnerer en fejlrespons med valideringsfejl.
        /// </summary>
        /// <param name="validationErrors">Liste over valideringsfejl.</param>
        /// <returns>HTTP 400 Bad Request.</returns>
        protected ActionResult Error(List<ValidationFailure> validationErrors)
        {
            var errors = validationErrors.Select(x => $"{x.ErrorMessage} ({x.PropertyName})").ToList();
            return Error(errors);
        }

        /// <summary>
        /// Returnerer en fejlrespons baseret på en fejlmodel.
        /// </summary>
        /// <param name="error">Fejlmodel.</param>
        /// <returns>HTTP 400 Bad Request.</returns>
        protected ActionResult Error(Error error)
        {
            if (error == null || string.IsNullOrWhiteSpace(error.Message) || string.IsNullOrWhiteSpace(error.Code))
            {
                return BadRequest(Envelope.Error("An unknown error occurred."));
            }

            // Returnerer en fejlrespons baseret på det tilsendte Error-objekt.
            return BadRequest(Envelope.Error($"{error.Message} ({error.Code})"));
        }



        /// <summary>
        /// Returnerer en respons baseret på et generelt resultat.
        /// </summary>
        /// <param name="result">Resultatmodel.</param>
        /// <returns>HTTP-statuskode baseret på resultatet.</returns>
        protected IActionResult FromResult(Result result)
        {
            if (result.Failure)
                return StatusCodeFromResult(result);

            return base.Ok(Envelope.Ok());
        }

        /// <summary>
        /// Returnerer en respons baseret på et generisk resultat.
        /// </summary>
        /// <typeparam name="T">Typen af resultatdata.</typeparam>
        /// <param name="result">Resultatmodel.</param>
        /// <returns>HTTP-statuskode baseret på resultatet.</returns>
        protected IActionResult FromResult<T>(Result<T> result)
        {
            if (result.Failure)
                return StatusCodeFromResult(result);

            return base.Ok(Envelope.Ok(result.Value));
        }

        /// <summary>
        /// Returnerer en HTTP-statuskode baseret på en fejl i resultatet.
        /// </summary>
        /// <param name="result">Resultatmodel.</param>
        /// <returns>HTTP-statuskode med fejldetaljer.</returns>
        private IActionResult StatusCodeFromResult(Result result)
        {
            return StatusCode(result.Error.StatusCode, Envelope.Error(result.Error.Code));
        }
    }
}
