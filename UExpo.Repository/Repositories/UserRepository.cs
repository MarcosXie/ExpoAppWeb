using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UExpo.Domain.Dao;
using UExpo.Domain.Entities.Expo;
using UExpo.Domain.Entities.Users;
using UExpo.Repository.Context;
namespace UExpo.Repository.Repositories;

public class UserRepository(UExpoDbContext context, IMapper mapper)
	: BaseRepository<UserDao, User>(context, mapper), IUserRepository
{
	public async Task DeleteUserWithNotValidatedEmailsAsync(string email, CancellationToken cancellationToken = default)
	{
		IQueryable<UserDao> invalidEmails = Database.Where(x => x.Email.ToLower().Equals(email.ToLower()) && !x.IsEmailValidated);

		if (!invalidEmails.Any()) return;

		Database.RemoveRange(invalidEmails);

		await Context.SaveChangesAsync(cancellationToken);
	}

	public async Task<User> GetByIdDetailedAsync(Guid id)
	{
		var entity = await Database
			.Include(x => x.Images)
			.Include(x => x.Catalog)
			.Include(x => x.FairRegisters)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id!.Equals(id));

		return entity is null
			? throw new Exception($"{nameof(User)} com id = {id}")
		: Mapper.Map<User>(entity);
	}

	public async Task<int> GetImageMaxOrderByUserIdAsync(Guid id)
	{
		var images = await Context.UserImages.Where(x => x.UserId == id).ToListAsync();

		return images.Count > 0 ? images.Max(x => x.Order) : 1;
	}

	public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
	{
		UserDao? userDao = await Database.FirstOrDefaultAsync(x => x.Email.ToLower().Equals(email), cancellationToken: cancellationToken);

		return Mapper.Map<User>(userDao);
	}

	public async Task AddImagesAsync(List<UserImage> images)
	{
		var dbImages = Mapper.Map<List<UserImageDao>>(images);

		Context.UserImages.AddRange(dbImages);

		await Context.SaveChangesAsync();
	}

	public async Task RemoveImagesAsync(List<UserImage> images)
	{
		Context.UserImages
			.Where(x => 
				images.Select(dbImage => dbImage.Id).Contains(x.Id)
			)
			.ExecuteDelete();

		await Context.SaveChangesAsync();
	}

	public async Task<List<User>> GetAsync(ExpoSearchDto search)
	{
		var preQuery = await Database
					.Include(x => x.Catalog)
						.ThenInclude(x => x!.Segments)
					.Where(x => x.FairRegisters.Any(f => f.CalendarFair.CalendarId == search.CalendarId && f.IsPaid)).ToListAsync();

		var users = preQuery
					.Where(x =>
						search.Tags.Count == 0 || search.Tags.Any(tag => x.Catalog!.Tags.Split(',', StringSplitOptions.None).ToList().Contains(tag.ToLower()))
					).ToList();

		return Mapper.Map<List<User>>(users);
	}
}
