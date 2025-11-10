using System.Net.Http;

namespace dotnetCoreInterviewPrepDemo.Services;

public interface IUserService
{
    Task<string> GetUserDataAsync(CancellationToken cancellationToken = default);
}

public sealed class UserService(HttpClient httpClient) : IUserService
{
    private static readonly Uri DefaultBaseAddress = new("https://api.example.com/");
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string> GetUserDataAsync(CancellationToken cancellationToken = default)
    {
        if (_httpClient.BaseAddress is null)
        {
            _httpClient.BaseAddress = DefaultBaseAddress;
        }

        using var response = await _httpClient.GetAsync("user/123", cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync(cancellationToken);
    }
}
