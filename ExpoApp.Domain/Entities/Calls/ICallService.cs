namespace ExpoApp.Domain.Entities.Calls;

public interface ICallService
{
	Task<InitiateCallResponseDto> InitiateCallAsync(InitiateCallDto dto);
	Task<AcceptCallResponseDto> AcceptCallAsync(string callId);
	Task RejectCallAsync(string callId);
	Task EndCallAsync(string callId, string userId);
}

