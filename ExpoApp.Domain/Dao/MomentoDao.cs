﻿using ExpoApp.Domain.Entities.Momento;
using ExpoShared.Domain.Dao;
using ExpoShared.Domain.Dao.Shared;

namespace ExpoApp.Domain.Dao;

public class MomentoDao : BaseDao
{
	public required Guid UserId { get; set; }
	public required UserDao User { get; set; }
	public required Guid TargetUserId { get; set; }
	public UserDao? TargetUser { get; set; }
	public required string Value { get; set; }
	public string? Comment { get; set; }
	public int Order { get; set; } = 1;
	public required MomentoType Type { get; set; }
}