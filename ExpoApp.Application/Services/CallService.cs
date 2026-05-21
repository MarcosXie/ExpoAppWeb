using System.Collections.Concurrent;
using ExpoApp.Domain.Entities.Calls;
using ExpoShared.Domain.Entities.UserLoros;
using FirebaseAdmin.Messaging;

namespace ExpoApp.Application.Services;

public class CallService : ICallService
{
	private static readonly ConcurrentDictionary<string, ActiveCall> ActiveCalls = new();
	private readonly IDailyService _dailyService;
	private readonly IUserLoroRepository _userLoroRepository;

	public CallService(IDailyService dailyService, IUserLoroRepository userLoroRepository)
	{
		_dailyService = dailyService;
		_userLoroRepository = userLoroRepository;
	}

	public async Task<InitiateCallResponseDto> InitiateCallAsync(InitiateCallDto dto)
	{
		var (roomName, roomUrl) = await _dailyService.CreateRoomAsync();

		var call = new ActiveCall
		{
			CallId = Guid.NewGuid().ToString(),
			CallerId = dto.CallerId,
			CallerName = dto.CallerName,
			TargetUserId = dto.TargetUserId,
			RoomUrl = roomUrl,
			RoomName = roomName,
			Status = CallStatus.Ringing,
			CreatedAt = DateTime.UtcNow,
			CallerLang = dto.CallerLang,
			TargetLang = dto.TargetLang
		};

		ActiveCalls[call.CallId] = call;

		// Send FCM push to target user (from user_loro table)
		var targetUser = await _userLoroRepository.GetByIdAsync(Guid.Parse(dto.TargetUserId));
		if (!string.IsNullOrEmpty(targetUser.FcmToken))
		{
			await SendFcmAsync(targetUser.FcmToken, new Dictionary<string, string>
			{
				{ "type", "incoming_call" },
				{ "callId", call.CallId },
				{ "callerName", dto.CallerName },
				{ "callerLang", dto.CallerLang },
				{ "targetLang", dto.TargetLang },
				{ "roomUrl", roomUrl }
			});
		}

		// Schedule 30s timeout for missed call
		_ = Task.Run(async () =>
		{
			await Task.Delay(TimeSpan.FromSeconds(30));
			if (ActiveCalls.TryGetValue(call.CallId, out var c) && c.Status == CallStatus.Ringing)
			{
				c.Status = CallStatus.Missed;
				var callerUser = await _userLoroRepository.GetByIdAsync(Guid.Parse(dto.CallerId));
				if (!string.IsNullOrEmpty(callerUser.FcmToken))
				{
					await SendFcmAsync(callerUser.FcmToken, new Dictionary<string, string>
					{
						{ "type", "call_missed" },
						{ "callId", call.CallId }
					});
				}
				await _dailyService.DeleteRoomAsync(roomName);
				ActiveCalls.TryRemove(call.CallId, out _);
			}
		});

		return new InitiateCallResponseDto
		{
			CallId = call.CallId,
			RoomUrl = roomUrl,
			Status = "ringing"
		};
	}

	public async Task<AcceptCallResponseDto> AcceptCallAsync(string callId)
	{
		if (!ActiveCalls.TryGetValue(callId, out var call))
			throw new KeyNotFoundException($"Call {callId} not found");

		call.Status = CallStatus.Active;

		// Notify caller that call was accepted
		var callerUser = await _userLoroRepository.GetByIdAsync(Guid.Parse(call.CallerId));
		if (!string.IsNullOrEmpty(callerUser.FcmToken))
		{
			await SendFcmAsync(callerUser.FcmToken, new Dictionary<string, string>
			{
				{ "type", "call_accepted" },
				{ "callId", callId }
			});
		}

		return new AcceptCallResponseDto
		{
			RoomUrl = call.RoomUrl,
			CallerLang = call.CallerLang,
			TargetLang = call.TargetLang
		};
	}

	public async Task RejectCallAsync(string callId)
	{
		if (!ActiveCalls.TryGetValue(callId, out var call))
			throw new KeyNotFoundException($"Call {callId} not found");

		call.Status = CallStatus.Rejected;

		// Notify caller
		var callerUser = await _userLoroRepository.GetByIdAsync(Guid.Parse(call.CallerId));
		if (!string.IsNullOrEmpty(callerUser.FcmToken))
		{
			await SendFcmAsync(callerUser.FcmToken, new Dictionary<string, string>
			{
				{ "type", "call_rejected" },
				{ "callId", callId }
			});
		}

		await _dailyService.DeleteRoomAsync(call.RoomName);
		ActiveCalls.TryRemove(callId, out _);
	}

	public async Task EndCallAsync(string callId, string userId)
	{
		if (!ActiveCalls.TryGetValue(callId, out var call))
			throw new KeyNotFoundException($"Call {callId} not found");

		call.Status = CallStatus.Ended;

		// Notify the other participant
		var otherUserId = call.CallerId == userId ? call.TargetUserId : call.CallerId;
		var otherUser = await _userLoroRepository.GetByIdAsync(Guid.Parse(otherUserId));
		if (!string.IsNullOrEmpty(otherUser.FcmToken))
		{
			await SendFcmAsync(otherUser.FcmToken, new Dictionary<string, string>
			{
				{ "type", "call_ended" },
				{ "callId", callId }
			});
		}

		await _dailyService.DeleteRoomAsync(call.RoomName);
		ActiveCalls.TryRemove(callId, out _);
	}

	private static async Task SendFcmAsync(string fcmToken, Dictionary<string, string> data)
	{
		try
		{
			var message = new Message
			{
				Token = fcmToken,
				Data = data,
				Android = new AndroidConfig { Priority = Priority.High }
			};
			await FirebaseMessaging.DefaultInstance.SendAsync(message);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"FCM send error: {ex.Message}");
		}
	}
}
