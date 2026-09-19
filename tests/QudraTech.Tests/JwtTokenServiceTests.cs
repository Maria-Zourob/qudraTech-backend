using Microsoft.Extensions.Configuration;
using QudraTech.Domain.Entities;
using QudraTech.Infrastructure.Services;
using Xunit;

namespace QudraTech.Tests;

public class JwtTokenServiceTests
{
    private JwtTokenService CreateService()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Secret"] = "Test-Secret-Key-For-Unit-Tests-MinLength32Chars",
                ["Jwt:Issuer"] = "QudraTech",
                ["Jwt:Audience"] = "QudraTechUsers",
                ["Jwt:AccessTokenExpiryMinutes"] = "30"
            })
            .Build();

        return new JwtTokenService(config);
    }

    [Fact]
    public void GenerateAccessToken_ReturnsNonEmptyToken()
    {
        var service = CreateService();
        var user = new ApplicationUser {Id = Guid.NewGuid(), Email = "test@qudratech.org"};

        var token = service.GenerateAccessToken(user, new List<string> {"SuperAdmin"});

        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Fact]
    public void GenerateAccessToken_ContainsCorrectRole()
    {
        var service = CreateService();
        var user = new ApplicationUser {Id = Guid.NewGuid(), Email = "test@qudratech.org"};

        var token = service.GenerateAccessToken(user, new List<string> {"InitiativeManager"});
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        var roleClaim = jwt.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role);
        Assert.NotNull(roleClaim);
        Assert.Equal("InitiativeManager", roleClaim.Value);
    }

    [Fact]
    public void GenerateRefreshToken_ReturnsUniqueValues()
    {
        var service = CreateService();

        var token1 = service.GenerateRefreshToken();
        var token2 = service.GenerateRefreshToken();

        Assert.NotEqual(token1, token2);
    }
}