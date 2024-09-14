using AutoMapper;
using UExpo.Application.Utils;
using UExpo.Domain.Entities.Calendars;
using UExpo.Domain.Entities.Relationships;
using UExpo.Domain.Entities.Users;

namespace UExpo.Application.Services.Relationships;

public class RelationshipService : IRelationshipService
{
	private IRelationshipRepository _repository;
	private IUserRepository _userRepository;
	private AuthUserHelper _authUserHelper;
	private IMapper _mapper;

	public RelationshipService(IRelationshipRepository repository, IUserRepository userRepository, IMapper mapper, AuthUserHelper authUserHelper)
	{
		_repository = repository;
		_userRepository = userRepository;
		_authUserHelper = authUserHelper;
		_mapper = mapper;
	}

	public async Task<Guid> CreateAsync(RelationshipDto relationship)
	{
		var buyer = await _userRepository.GetByIdAsync(relationship.BuyerUserId);
		var supplier = await _userRepository.GetByIdAsync(relationship.SupplierUserId);

		var mappedRelationship = _mapper.Map<Relationship>(relationship);

		mappedRelationship.BuyerLang = buyer.Lang;
		mappedRelationship.SupplierLang = supplier.Lang;

		if (await RelationshipAlreadyExistsAsync(mappedRelationship))
			return Guid.NewGuid();
		
		return await _repository.CreateAsync(mappedRelationship);
	}

	public async Task<string> GetMemoAsync(Guid id)
	{
		var relationship = await _repository.GetByIdAsync(id);

		if (_authUserHelper.GetUser().Id == relationship.SupplierUserId)
		{
			return relationship.SupplierMemo;
		}
		else
		{
			return relationship.BuyerMemo;
		}
	}

	public async Task<List<RelationshipResponseDto>> GetRelationshipsByUserIdAsync(Guid? id)
	{
		id ??= _authUserHelper.GetUser().Id;

		List<Relationship> relationships = await _repository.GetByUserIdAsync((Guid)id);

		return MapRelationships(relationships, (Guid)id).OrderBy(x => x.Status).ToList();
	}

	public async Task UpdateMemoAsync(Guid id, RelationshipMemoUpdateDto updateDto)
	{
		var relationship = await _repository.GetByIdAsync(id);

		if (relationship.BuyerUserId == relationship.SupplierUserId)
		{
			relationship.SupplierMemo = updateDto.Memo;
			relationship.BuyerMemo = updateDto.Memo;
		}
		else if (_authUserHelper.GetUser().Id == relationship.SupplierUserId)
		{
			relationship.SupplierMemo = updateDto.Memo;
		}
		else
		{
			relationship.BuyerMemo = updateDto.Memo;
		}

		await _repository.UpdateAsync(relationship);
	}

	public async Task UpdateStatusAsync(Guid id, RelationshipStatusUpdateDto updateDto)
	{
		var relationship = await _repository.GetByIdAsync(id);

		if (relationship.BuyerUserId == relationship.SupplierUserId)
		{
			relationship.SupplierStatus = updateDto.Status;
			relationship.BuyerStatus = updateDto.Status;
		}
		else if (_authUserHelper.GetUser().Id == relationship.SupplierUserId)
		{
			relationship.SupplierStatus = updateDto.Status;
		}
		else
		{
			relationship.BuyerStatus = updateDto.Status;
		}

		await _repository.UpdateAsync(relationship);
	}

	private IEnumerable<RelationshipResponseDto> MapRelationships(List<Relationship> relationships, Guid id)
	{
		foreach (var relationship in relationships.OrderByDescending(x => x.CreatedAt))
		{
			// TYPE É O TIPO DO USUARIO ATUAL DENTRO DO RELACIONAMENTO
			var type = relationship.BuyerUserId == id
				? RelationshipType.Buyer
				: RelationshipType.Supplier;

			var user = type == RelationshipType.Buyer ? relationship.SupplierUser : relationship.BuyerUser;
			user.Images = user.Images.OrderByDescending(x => x.Order).ToList();
			user.Password = string.Empty;

			yield return new RelationshipResponseDto
			{
				Id = relationship.Id,
				Type = type,
				CreatedAt = relationship.CreatedAt,
				UserId = type == RelationshipType.Buyer ? relationship.SupplierUserId : relationship.BuyerUserId,
				UserProfile = _mapper.Map<UserProfileResponseDto>(user),
				Calendar = _mapper.Map<CalendarResponseDto>(relationship.Calendar),
				Status = type == RelationshipType.Buyer ? relationship.BuyerStatus : relationship.SupplierStatus,
			};
		}
	}

	private async Task<bool> RelationshipAlreadyExistsAsync(Relationship mappedRelationship)
	{
		return await _repository.AnyAsync(x => 
			x.BuyerUserId == mappedRelationship.BuyerUserId && 
			x.SupplierUserId == mappedRelationship.SupplierUserId
		); 
	}
}
