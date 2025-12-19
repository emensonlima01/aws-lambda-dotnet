using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaFunction;

public class BoletoPaymentRequest
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

public class BoletoPaymentResponse
{
    public string Status { get; set; } = "accepted";
    public string ReceiptId { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
    public decimal? Amount { get; set; }
    public string Currency { get; set; } = "BRL";
    public string? ReferenceId { get; set; }
    public string Message { get; set; } = "Payment received and accepted.";
}

public class Function
{
    public static BoletoPaymentResponse FunctionHandler(BoletoPaymentRequest input, ILambdaContext context)
    {
        var response = new BoletoPaymentResponse
        {
            ReceiptId = Guid.NewGuid().ToString("N"),
            ProcessedAt = DateTime.UtcNow,
            Amount = input?.Amount,
            Currency = string.IsNullOrWhiteSpace(input?.Currency) ? "BRL" : input!.Currency!,
            ReferenceId = input?.ReferenceId
        };

        context.Logger.LogLine($"Accepted boleto payment. ReceiptId={response.ReceiptId}");

        return response;
    }
}
