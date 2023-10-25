using Application.Common.Mailing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.IntegrationTests.Mailing;

public class SmtpMailServiceTests : IClassFixture<BaseWebHostFixture<Program>>
{
    private readonly BaseWebHostFixture<Program> _baseWebHost;

    public SmtpMailServiceTests(BaseWebHostFixture<Program> baseWebHost)
    {
        _baseWebHost = baseWebHost;
    }

    [Fact]
    public void SendAsync_SendValidEmail_ShouldNotThrowException()
    {
        // Arrange
        var emailService = _baseWebHost.Server.Services.GetService<IMailService>();
        var mailRequest = new MailRequest(new List<string> { "test@email.com" }, "Test Mail");

        // Act
        Action action = emailService.SendAsync(mailRequest, CancellationToken.None).Wait;

        // Assert
        action.Should().NotThrow();
    }
}