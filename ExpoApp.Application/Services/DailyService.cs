using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using ExpoApp.Domain.Entities.Calls;
using Microsoft.Extensions.Configuration;

namespace ExpoApp.Application.Services;

public class DailyService : IDailyService
{
	private readonly HttpClient _httpClient;

	public DailyService(IConfiguration config)
	{
		var apiKey = config["DailyCo:ApiKey"]!;
		var baseUrl = config["DailyCo:BaseUrl"] ?? "https://api.daily.co/v1/";

		_httpClient = new HttpClient
		{
			BaseAddress = new Uri(baseUrl)
		};
		_httpClient.DefaultRequestHeaders.Authorization =
			new AuthenticationHeaderValue("Bearer", apiKey);
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

		var response = await _httpClient.PostAsJsonAsync("rooms", body);
		response.EnsureSuccessStatusCode();

		var result = await response.Content.ReadFromJsonAsync<DailyRoomResponse>();
		return (result!.Name, result.Url);
	}

	public async Task DeleteRoomAsync(string roomName)
	{
		await _httpClient.DeleteAsync($"rooms/{roomName}");
	}
}

public class DailyRoomResponse
{
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("url")]
	public string Url { get; set; } = string.Empty;
}

