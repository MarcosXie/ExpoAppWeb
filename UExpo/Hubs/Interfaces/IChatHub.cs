using UExpo.Domain.Entities.Chats.Shared;

namespace UExpo.Api.Hubs.Interfaces;

public interface IChatHub
{
	Task ChangeUserLang(ChatDto callCenterChat);
	Task<JoinChatResponseDto> JoinChatRoom(ChatDto joinChatDto);
	Task SendMessage(SendMessageDto message);
	Task DeleteMessage(Guid messageId);
	Task VisualizeMessages(ChatDto chat);
}