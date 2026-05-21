namespace ExpoApp.Domain.Entities.Calls;

public interface IDailyService
{
	Task<(string RoomName, string RoomUrl)> CreateRoomAsync();
	Task DeleteRoomAsync(string roomName);
}

