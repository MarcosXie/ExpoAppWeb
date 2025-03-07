using ExpoApp.Domain.Entities.Exhibitors;
using ExpoShared.Application.Utils;
using ExpoShared.Domain.Entities.Exhibitors;
using ExpoShared.Domain.Entities.Relationships;
using ExpoShared.Domain.Entities.Users;

namespace ExpoApp.Application.Services;

public class ExhibitorService(IUserRepository userRepository, IRelationshipRepository relationshipRepository, AuthUserHelper authUserHelper)
	: IExhibitorService
{
	public async Task<List<ExhibitorResponseDto>> GetExhibitorsAsync(		
		string? companyName = null, 
		string? email = null, 
		string? country = null)
	{
		var users = await userRepository.GetAsync(x => 
			x.Type == UserType.Exhibitor &&
			x.Enterprise != null &&
			(companyName == null || x.Enterprise.ToLower().Contains(companyName.ToLower())) &&
			(email == null || x.Email.ToLower().Contains(email.ToLower())) &&
			(country == null || x.Country.ToLower().Equals(country.ToLower())) 
		);
		var relationships = await GetUserRelationshipsAsync();
		
		return BuildExhibitorsResponse(users, relationships).ToList();
	}

	public async Task<ExpoFinderOptionsResponseDto> GetFinderOptionsAsync(		
		string? companyName = null,
		string? email = null,
		string? country = null)
	{
		var users = 
			await userRepository.GetAsync(x => 
				x.Type == UserType.Exhibitor && !string.IsNullOrEmpty(x.Enterprise) && 
				(companyName == null || x.Enterprise.ToLower().Contains(companyName.ToLower())) &&
				(email == null || x.Name.ToLower().Contains(email.ToLower())) &&
				(country == null || x.Country.ToLower().Equals(country.ToLower()))
			);
		
		return new()
		{
			Emails = users.Select(x => x.Email).ToList(),
			CompanyNames = users.Select(x => x.Enterprise ?? "").ToList(),
			Countries = users.Select(x => x.Country).ToList(),
		};
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
			Name = user.Name,
			Country = user.Country,
			Enterprise = user.Enterprise ?? string.Empty,
			Tags = user.Catalog?.Tags,
			ProfileImage = user.ProfileImageUri,
			HasRelationship = relationships.Any(r => r.SupplierUserId == user.Id),
		});
	}
}