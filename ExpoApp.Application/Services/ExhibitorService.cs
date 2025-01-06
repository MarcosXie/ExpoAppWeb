using ExpoApp.Domain.Entities.Exhibitors;
using ExpoShared.Application.Utils;
using ExpoShared.Domain.Entities.Exhibitors;
using ExpoShared.Domain.Entities.Relationships;
using ExpoShared.Domain.Entities.Users;

namespace ExpoApp.Application.Services;

public class ExhibitorService(IUserRepository userRepository, IRelationshipRepository relationshipRepository, AuthUserHelper authUserHelper)
	: IExhibitorService
{
	public async Task<List<ExhibitorResponseDto>> GetExhibitorsAsync(string? companyName = null)
	{
		var users = await userRepository.GetAsync(x => 
			x.Type == UserType.Exhibitor &&
			companyName == null || x.Enterprise.ToLower().Contains(companyName.ToLower())
		);
		var relationships = await GetUserRelationshipsAsync();
		
		return BuildExhibitorsResponse(users, relationships).ToList();
	}

	public async Task<List<string>> GetExhibitorsCompaniesAsync()
	{
		var users = await userRepository.GetAsync(x => 
			x.Type == UserType.Exhibitor && !string.IsNullOrEmpty(x.Enterprise));
		
		return users.Select(x => x.Enterprise!).ToList();
	}

	private async Task<List<Relationship>> GetUserRelationshipsAsync()
	{
		return await relationshipRepository.GetByUserIdAsync(authUserHelper.GetUser().Id);
	}
	
	private IEnumerable<ExhibitorResponseDto> BuildExhibitorsResponse(
		List<User> users,
		List<Relationship> relationships)
	{
		return users.Select(user => new ExhibitorResponseDto()
		{
			Id = user.Id,
			Country = user.Country,
			Enterprise = user.Enterprise ?? string.Empty,
			Tags = user.Catalog?.Tags,
			ProfileImage = user.ProfileImageUri,
			HasRelationship = relationships.Any(r => r.SupplierUserId == user.Id),
		});
	}
}