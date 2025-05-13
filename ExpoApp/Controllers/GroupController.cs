using ExpoShared.Domain.Entities.Chats.GroupChat;
using Microsoft.AspNetCore.Mvc;

namespace ExpoApp.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class GroupController(IGroupService groupService) : ControllerBase
{
	/// <summary>
	/// Create Group
	/// </summary>
	/// <param name="createGroupDto"></param>
	/// <returns></returns>
	[HttpPost]
	public async Task<ActionResult> CreateGroupAsync(CreateGroupDto createGroupDto)
	{
		return Ok(await groupService.CreateGroupAsync(createGroupDto));
	}
	
	[HttpGet]
	public async Task<ActionResult> GetGroupsAsync()
	{
		return Ok(await groupService.GetByCurrentUserAsync());
	}
	
	/// <summary>
	/// Update Group Name
	/// </summary>
	/// <param name="groupId"></param>
	/// <param name="name"></param>
	/// <returns></returns>
	[HttpPut("{groupId}")]
	public async Task<ActionResult> UpdateAsync(
		Guid groupId, string name
	)
	{
		await groupService.UpdateAsync(groupId, name);
		
		return Ok();
	}
	
	/// <summary>
	/// Add Member
	/// </summary>
	/// <param name="groupId"></param>
	/// <param name="groupAddMemberDto"></param>
	/// <returns></returns>
	[HttpPatch("{groupId}")]
	public async Task<ActionResult> AddMemberAsync(
		Guid groupId, GroupAddMemberDto groupAddMemberDto
	)
	{
		await groupService.AddMemberAsync(groupId, groupAddMemberDto);
		
		return Ok();
	}
	
	/// <summary>
	/// Make User in an Admin
	/// </summary>
	/// <param name="groupId"></param>
	/// <param name="userId"></param>
	/// <returns></returns>
	[HttpPatch("{groupId}/Admin/{userId}")]
	public async Task<ActionResult> MakeAdminAsync(Guid groupId, Guid userId)
	{
		await groupService.MakeAdminAsync(groupId, userId);
		
		return Ok();
	}
	
	/// <summary>
	/// Remove User from Group
	/// </summary>
	/// <param name="groupId"></param>
	/// <param name="userId"></param>
	/// <returns></returns>
	[HttpDelete("{groupId}/{userId}")]
	public async Task<ActionResult> RemoveUserAsync(Guid groupId, Guid userId)
	{
		await groupService.RemoveUserAsync(groupId, userId);
		
		return Ok();
	}
	
	/// <summary>
	/// Delete Group Async
	/// </summary>
	/// <param name="groupId"></param>
	/// <returns></returns>
	[HttpDelete("{groupId}")]
	public async Task<ActionResult> DeleteGroupAsync(Guid groupId)
	{
		await groupService.DeleteGroupAsync(groupId);
		
		return Ok();
	}
}
