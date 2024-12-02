using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Threading.Tasks;
using Webshop.Payment.Api.Models;
using Webshop.Payment.Api.Repository;
using Webshop.Payment.Api.Services;

namespace Webshop.Payment.Api.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private IMemoryRepository transactionRepository;
        private IThrottleService throttleService;
        private ILogger<PaymentController> logger;
        public PaymentController(IMemoryRepository transactionRepository, IThrottleService throttleService, ILogger<PaymentController> logger)
        {
            this.throttleService = throttleService;
            this.transactionRepository = transactionRepository;
            this.logger = logger;
        }

        [HttpPost]
        [Route("process")]
        public Result<Transaction> ProcessPayment(PaymentRequest request)
        {
            //check for throttling
            if (!throttleService.CanExecute())
            {
                return Result.Fail<Transaction>("The Payment service is not ready");
            }
            //simulate process
            Result<Webshop.Payment.Api.Models.Payment> paymentResult = Webshop.Payment.Api.Models.Payment.Create(request.CardNumber, request.ExpirationDate, request.CVC);

            if (paymentResult.IsSuccess)
            {
                Result<Transaction> transactionResult = Transaction.Create(request.Amount, paymentResult.Value);
                if (transactionResult.IsSuccess)
                {
                    Result result = transactionRepository.AddTransaction(transactionResult.Value);
                    if (result.IsFailure)
                    {
                        return Result.Fail<Transaction>(result.Error);
                    }
                }
                return transactionResult;
            }
            else
            {
                logger.LogWarning("Unable to create new Payment object with the following error: {error}", new { error = paymentResult.Error });
                return Result.Fail<Transaction>(paymentResult.Error);
            }
        }
    }
}
