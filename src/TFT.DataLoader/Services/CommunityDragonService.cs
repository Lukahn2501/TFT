using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TFT.Core.DTOs;

namespace TFT.DataLoader.Services;

public class CommunityDragonService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<CommunityDragonService> _logger;

    public CommunityDragonService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<CommunityDragonService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<TftDataDto?> FetchTftDataAsync(CancellationToken cancellationToken = default)
    {
        var baseUrl = _configuration["CommunityDragon:BaseUrl"];
        var language = _configuration["CommunityDragon:Language"];
        var url = $"{baseUrl}{language}.json";

        _logger.LogInformation("Fetching TFT data from: {Url}", url);

        try
        {
            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);

            _logger.LogInformation("Downloaded {Size} bytes", json.Length);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                AllowTrailingCommas = true
            };

            var data = JsonSerializer.Deserialize<TftDataDto>(json, options);

            _logger.LogInformation("Parsed {ItemCount} items, {SetCount} sets, {SetDataCount} setData entries",
                data?.Items.Count ?? 0,
                data?.Sets.Count ?? 0,
                data?.SetData.Count ?? 0);

            return data;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch data from Community Dragon");
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse JSON data");
            throw;
        }
    }
}
