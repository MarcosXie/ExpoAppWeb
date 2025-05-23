using ExpoApp.Domain.Entities.Momento;
using ExpoShared.Domain.Entities.Chats.CallCenterChat;
using ExpoShared.Domain.Entities.Chats.CartChat;
using ExpoShared.Domain.Entities.Chats.GroupChat;
using ExpoShared.Domain.Entities.Chats.RelationshipChat;
using ExpoShared.Domain.Entities.Chats.Shared;
using ExpoShared.Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationsController(
	ICallCenterChatService callCenterService,
	IRelationshipChatService relationshipChatService,
	IGroupChatService groupService,
	ICartChatService cartChatService) : ControllerBase
{
	[HttpPost("{userId}")]
	public async Task<ActionResult> GetNotifications(Guid userId, [FromBody] NotificationConfigDto notificationConfigDto)
	{
		var notifications = new UserRoomNotificationsDto()
		{
			CallCenterNotReadedMessages = notificationConfigDto.IsCallCenterNotReadedMessages ? await callCenterService.GetNotReadedMessagesByUserId(userId) : null,
			RelationshipNotifications = notificationConfigDto.IsRelationshipNotifications ? await relationshipChatService.GetNotReadedMessagesAsync(userId) : null,
			GroupNotifications = notificationConfigDto.IsGroupNotifications ? await groupService.GetNotReadedMessagesAsync(userId) : null,
			CartNotifications = notificationConfigDto.IsCartNotifications ? await cartChatService.GetNotReadedMessagesAsync(userId) : null,
			NewCarts = notificationConfigDto.IsNewCarts ? await cartChatService.GetNewCartsAsync(userId) : null,
		};

		return Ok(notifications);
	}
}