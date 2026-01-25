using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Insurance.Api.Middleware;

namespace Insurance.UnitTests.Middleware;

public class ExceptionMiddlewareTests
{
    private static async Task<(int StatusCode, string Body)> Invoke(RequestDelegate next, Exception ex)
    {
        var logger = new Mock<ILogger<ExceptionMiddleware>>();

        RequestDelegate throwingNext = _ =>
        {
            throw ex;
        };

        var middleware = new ExceptionMiddleware(throwingNext, logger.Object);

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        await middleware.InvokeAsync(context);

        context.Response.Body.Position = 0;
        var body = await new StreamReader(context.Response.Body, Encoding.UTF8).ReadToEndAsync();

        logger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Exception on")),
                ex,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        return (context.Response.StatusCode, body);
    }

    [Theory]
    [InlineData(typeof(ArgumentException), 400)]
    [InlineData(typeof(InvalidOperationException), 400)]
    [InlineData(typeof(KeyNotFoundException), 404)]
    [InlineData(typeof(UnauthorizedAccessException), 401)]
    public async Task InvokeAsync_ShouldMapKnownExceptionsToStatusCodes(Type exType, int expectedStatus)
    {
        var ex = (Exception)Activator.CreateInstance(exType, "boom")!;
        var (status, body) = await Invoke(_ => Task.CompletedTask, ex);

        status.Should().Be(expectedStatus);

        using var doc = JsonDocument.Parse(body);
        doc.RootElement.GetProperty("statusCode").GetInt32().Should().Be(expectedStatus);

        if (expectedStatus == 500)
        {
            doc.RootElement.GetProperty("message").GetString().Should().Be("An unexpected error occurred.");
        }
        else
        {
            doc.RootElement.GetProperty("message").GetString().Should().Be("boom");
        }
    }

    [Fact]
    public async Task InvokeAsync_ShouldReturn500_WithGenericMessage_ForUnknownException()
    {
        var ex = new Exception("secret");
        var (status, body) = await Invoke(_ => Task.CompletedTask, ex);

        status.Should().Be(500);

        using var doc = JsonDocument.Parse(body);
        doc.RootElement.GetProperty("statusCode").GetInt32().Should().Be(500);
        doc.RootElement.GetProperty("message").GetString().Should().Be("An unexpected error occurred.");
    }
}
