using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using ExpoApp.Domain.Entities.Calls;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ExpoApp.Application.Services;

public class DailyService : IDailyService
{
	private readonly HttpClient _httpClient;
	private readonly ILogger<DailyService> _logger;

	public DailyService(IConfiguration config, ILogger<DailyService> logger)
	{
		_logger = logger;
		var apiKey = config["DailyCo:ApiKey"];
		const string baseUrl = "https://api.daily.co/v1/";

		_logger.LogInformation("[DailyService] Initialized with BaseUrl={BaseUrl}, ApiKey present={HasKey}",
			baseUrl, !string.IsNullOrEmpty(apiKey));

		_httpClient = new HttpClient
		{
			BaseAddress = new Uri(baseUrl)
		};
		_httpClient.DefaultRequestHeaders.Authorization =
			new AuthenticationHeaderValue("Bearer", apiKey ?? "");
	}

	public async Task<(string RoomName, string RoomUrl)> CreateRoomAsync()
	{
		var expiry = DateTimeOffset.UtcNow.AddMinutes(30).ToUnixTimeSeconds();
		var body = new
		{
			properties = new
			{
				exp = expiry,
				enable_chat = false,
				start_audio_off = true
			}
		};

		_logger.LogInformation("[DailyService] Creating room with expiry={Expiry}", expiry);

		var response = await _httpClient.PostAsJsonAsync("rooms", body);

		var responseBody = await response.Content.ReadAsStringAsync();
		_logger.LogInformation("[DailyService] Daily.co response: Status={Status}, Body={Body}",
			(int)response.StatusCode, responseBody);

		if (!response.IsSuccessStatusCode)
		{
			throw new Exception($"Daily.co CreateRoom failed: HTTP {(int)response.StatusCode} - {responseBody}");
		}

		var result = System.Text.Json.JsonSerializer.Deserialize<DailyRoomResponse>(responseBody);
		_logger.LogInformation("[DailyService] Room created: Name={Name}, Url={Url}", result!.Name, result.Url);
		return (result.Name, result.Url);
	}

	public async Task DeleteRoomAsync(string roomName)
	{
		_logger.LogInformation("[DailyService] Deleting room: {RoomName}", roomName);
		var response = await _httpClient.DeleteAsync($"rooms/{roomName}");
		_logger.LogInformation("[DailyService] Delete response: {Status}", (int)response.StatusCode);
	}
}

public class DailyRoomResponse
{
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("url")]
	public string Url { get; set; } = string.Empty;
}
