using ExpoShared.Domain.Entities.Chats.Shared;

namespace ExpoApp.Api.Hubs.Interfaces;

public interface IChatHub
{
	Task ChangeUserLang(ChatDto callCenterChat);
	Task<JoinChatResponseDto> JoinChatRoom(ChatDto joinChatDto);
	Task SendMessage(SendMessageDto message);
	Task DeleteMessage(DeleteMsgDto msgDto);
	Task VisualizeMessages(ChatDto chat);
}