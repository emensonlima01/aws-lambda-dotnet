using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

namespace LambdaFunction.Tests;

public class FunctionTest
{
    [Fact]
    public void AcceptsBoletoPayment()
    {
        var context = new TestLambdaContext();
        var request = new BoletoPaymentRequest
        {
            ReferenceId = "order-123",
            PayerName = "Maria Silva",
            PayerDocument = "12345678901",
            Amount = 150.75m,
            Currency = "BRL",
            DueDate = new DateTime(2025, 12, 31),
            Barcode = "00190500954014481606906809350314337370000000100",
            DigitableLine = "00190.50095 40144.816069 06809.350314 3 737000000100",
            PaymentDate = new DateTime(2025, 12, 19)
        };

        var result = Function.FunctionHandler(request, context);

        Assert.Equal("accepted", result.Status);
        Assert.False(string.IsNullOrWhiteSpace(result.ReceiptId));
        Assert.Equal(request.Amount, result.Amount);
        Assert.Equal("BRL", result.Currency);
        Assert.Equal(request.ReferenceId, result.ReferenceId);
    }
}
