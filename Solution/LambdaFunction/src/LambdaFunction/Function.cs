using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaFunction
{
    public sealed class BoletoPaymentRequest
    {
        public string? ReferenceId { get; set; }
        public string? PayerName { get; set; }
        public string? PayerDocument { get; set; }
        public decimal? Amount { get; set; }
        public string? Currency { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Barcode { get; set; }
        public string? DigitableLine { get; set; }
        public DateTime? PaymentDate { get; set; }
        public Dictionary<string, string>? Metadata { get; set; }
    }

    public sealed class BoletoPaymentResponse
    {
        public const string StatusAccepted = "accepted";
        public const string DefaultCurrency = "BRL";

        public string Status { get; set; } = StatusAccepted;
        public string ReceiptId { get; set; } = string.Empty;
        public DateTime ProcessedAt { get; set; }
        public decimal? Amount { get; set; }
        public string Currency { get; set; } = DefaultCurrency;
        public string? ReferenceId { get; set; }
        public string Message { get; set; } = "Payment received and accepted.";
    }

    public sealed class Function
    {

        /// <summary>
        /// Receives a boleto payment request and returns an accepted response.
        /// </summary>
        /// <param name="input">The event for the Lambda function handler to process.</param>
        /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
        /// <returns></returns>
        public static BoletoPaymentResponse FunctionHandler(BoletoPaymentRequest input, ILambdaContext context)
        {
            var currency = string.IsNullOrWhiteSpace(input?.Currency)
                ? BoletoPaymentResponse.DefaultCurrency
                : input!.Currency!;

            var response = new BoletoPaymentResponse
            {
                ReceiptId = Guid.NewGuid().ToString("N"),
                ProcessedAt = DateTime.UtcNow,
                Amount = input?.Amount,
                Currency = currency,
                ReferenceId = input?.ReferenceId
            };

            context.Logger.LogLine("Accepted boleto payment. ReceiptId=" + response.ReceiptId);

            return response;
        }
    }
}
