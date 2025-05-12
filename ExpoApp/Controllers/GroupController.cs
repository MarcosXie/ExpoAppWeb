using ExpoShared.Domain.Entities.Chats.GroupChat;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class GroupController(IGroupService groupService) : ControllerBase
{
	[HttpPost]
	public async Task<ActionResult> CreateGroupAsync(CreateGroupDto createGroupDto)
	{
		return Ok(await groupService.CreateGroupAsync(createGroupDto));
	}
	
	[HttpPut("{groupId}")]
	public async Task<ActionResult> UpdateAsync(
		Guid groupId, string name
	)
	{
		await groupService.UpdateAsync(groupId, name);
		
		return Ok();
	}
	
	[HttpPatch("{groupId}")]
	public async Task<ActionResult> AddMemberAsync(
		Guid groupId, GroupAddMemberDto groupAddMemberDto
	)
	{
		await groupService.AddMemberAsync(groupId, groupAddMemberDto);
		
		return Ok();
	}
	
	[HttpPatch("{groupId}/Admin/{userId}")]
	public async Task<ActionResult> MakeAdminAsync(Guid groupId, Guid userId)
	{
		await groupService.MakeAdminAsync(groupId, userId);
		
		return Ok();
	}
	
	[HttpDelete("{groupId}/{userId}")]
	public async Task<ActionResult> RemoveUserAsync(Guid groupId, Guid userId)
	{
		await groupService.RemoveUserAsync(groupId, userId);
		
		return Ok();
	}
	
	[HttpDelete("{groupId}")]
	public async Task<ActionResult> DeleteGroupAsync(Guid groupId)
	{
		await groupService.DeleteGroupAsync(groupId);
		
		return Ok();
	}
}
