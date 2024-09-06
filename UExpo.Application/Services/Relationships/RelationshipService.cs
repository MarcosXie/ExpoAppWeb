using AutoMapper;
using UExpo.Application.Utils;
using UExpo.Domain.Entities.Calendars;
using UExpo.Domain.Entities.Relationships;
using UExpo.Domain.Entities.Users;

namespace UExpo.Application.Services.Relationships;

public class RelationshipService : IRelationshipService
{
	private IRelationshipRepository _repository;
	private AuthUserHelper _authUserHelper;
	private IMapper _mapper;

	public RelationshipService(IRelationshipRepository repository, IMapper mapper, AuthUserHelper authUserHelper)
	{
		_repository = repository;
		_authUserHelper = authUserHelper;
		_mapper = mapper;
	}

	public Task<Guid> CreateAsync(RelationshipDto relationship)
	{
		var mappedRelationship = _mapper.Map<Relationship>(relationship);
		//TODO: Create Chat

		return _repository.CreateAsync(mappedRelationship);
	}

	public async Task<List<RelationshipResponseDto>> GetRelationshipsByUserIdAsync(Guid? id)
	{
		id ??= _authUserHelper.GetUser().Id;

		List<Relationship> relationships = await _repository.GetByUserIdAsync((Guid)id);

		return MapRelationships(relationships, (Guid)id).ToList();
	}

	private IEnumerable<RelationshipResponseDto> MapRelationships(List<Relationship> relationships, Guid id)
	{
		foreach (var relationship in relationships.OrderByDescending(x => x.CreatedAt))
		{
			var type = relationship.BuyerUserId == id 
				? RelationshipType.Supplier 
				: RelationshipType.Buyer;

			var user = type == RelationshipType.Buyer ? relationship.BuyerUser : relationship.SupplierUser;
			user.Images = user.Images.OrderByDescending(x => x.Order).ToList();
			user.Password = string.Empty;

			yield return new RelationshipResponseDto
			{
				Type = type,
				CreatedAt = relationship.CreatedAt,
				UserId = type == RelationshipType.Buyer ? relationship.BuyerUserId : relationship.SupplierUserId,
				UserProfile = _mapper.Map<UserProfileResponseDto>(user),
				Calendar = _mapper.Map<CalendarResponseDto>(relationship.Calendar),
			};
		}
	}
}
