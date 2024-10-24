using AutoMapper;
using Microsoft.AspNetCore.Http;
using UExpo.Application.Utils;
using UExpo.Domain.Entities.Calendars;
using UExpo.Domain.Entities.Calendars.Fairs;
using UExpo.Domain.Entities.Catalogs;
using UExpo.Domain.Entities.Catalogs.ItemImages;
using UExpo.Domain.Entities.Catalogs.Pdfs;
using UExpo.Domain.Exceptions;
using UExpo.Domain.FileStorage;
using UExpo.Domain.Shared;

namespace UExpo.Application.Services.Catalogs;

public class CatalogService : ICatalogService
{
    private readonly ICatalogRepository _repository;
    private readonly ICatalogPdfRepository _pdfRepository;
    private readonly ICatalogItemImageRepository _itemImageRepository;
    private readonly IFileStorageService _fileStorageService;
	private readonly ICalendarRepository _calendarRepository;
	private readonly ICalendarFairRepository _calendarFairRepository;
	private readonly IMapper _mapper;

    public CatalogService(
        ICatalogRepository repository,
        ICatalogPdfRepository pdfRepository,
        ICatalogItemImageRepository itemImageRepository,
        IFileStorageService fileStorageService,
		ICalendarFairRepository calendarFairRepository,
		ICalendarRepository calendarRepository,
		IMapper mapper)
    {
        _repository = repository;
        _pdfRepository = pdfRepository;
        _itemImageRepository = itemImageRepository;
        _fileStorageService = fileStorageService;
		_calendarRepository = calendarRepository;
		_calendarFairRepository = calendarFairRepository;
		_mapper = mapper;
    }

    public async Task<CatalogResponseDto> GetOrCreateAsync(Guid id)
    {
        Catalog? catalog = await _repository.GetByUserIdOrDefaultAsync(id);

        if (catalog is null)
        {
            catalog = new()
            {
                UserId = id
			};

            await _repository.CreateAsync(catalog);
        }

        catalog.ItemImages = await _itemImageRepository.GetMainImagesByCatalogAsync(catalog.Id);

        return _mapper.Map<CatalogResponseDto>(catalog);
    }

	public async Task<CatalogResponseDto> GetByCartIdAsync(Guid cartId)
	{
		Catalog catalog = await _repository.GetByCartIdAsync(cartId);

		catalog.ItemImages = await _itemImageRepository.GetMainImagesByCatalogAsync(catalog.Id);

		return _mapper.Map<CatalogResponseDto>(catalog);
	}
	public async Task<CatalogPdfResponseDto> AddPdfAsync(CatalogPdfDto pdf)
    {
        Catalog catalog = await _repository.GetByIdAsync(pdf.CatalogId);
        string fileName = GetFileName(pdf.File.FileName, catalog.Id.ToString());

        CatalogPdf dbPdf = new()
        {
            CatalogId = catalog.Id,
            Name = Path.GetFileName(pdf.File.FileName),
            Uri = await _fileStorageService.UploadFileAsync(pdf.File, fileName, FileStorageKeys.CatalogPdf)
        };

        await _pdfRepository.CreateAsync(dbPdf);

        return _mapper.Map<CatalogPdfResponseDto>(dbPdf);
    }

    public async Task DeletePdfAsync(Guid id, Guid pdfId)
    {
        Catalog catalog = await _repository.GetByIdAsync(id);

        CatalogPdf pdf = await _pdfRepository.GetByIdAsync(pdfId);

        await Task.WhenAll(
            _fileStorageService.DeleteFileAsync(FileStorageKeys.CatalogPdf, GetFileName(pdf.Name, catalog.Id.ToString())),
            _pdfRepository.DeleteAsync(pdfId)
        );
    }

    public async Task<List<Dictionary<string, object>>> AddCatalogDataAsync(Guid id, IFormFile data)
    {
        Catalog catalog = await _repository.GetByIdAsync(id);

        catalog.JsonTable = data.ToDictionary();

        var groupedCodes = catalog.JsonTable.GroupBy(x => x[x.Keys.First()]);

        if (groupedCodes.Any(g => g.Count() > 1))
            throw new BadRequestException(
                $"The first column contains repeated identifier values: {
                    string.Join(", ", groupedCodes.Where(x => x.Count() > 1).Select(x => x.Key))
                }");

        await _repository.UpdateAsync(catalog);

        return catalog.JsonTable;
    }

    public async Task<ValidationErrorResponseDto> ValidadeAddCatalogDataAsync(Guid id, IFormFile data)
    {
        Catalog catalog = await _repository.GetByIdAsync(id);

        catalog.JsonTable = data.ToDictionary();

        var groupedCodes = catalog.JsonTable.GroupBy(x => x[x.Keys.First()]);

        return new()
        {
            IsError = !groupedCodes.Any(g => g.Count() > 1),
            Message = $"The first column contains repeated identifier values: {string.Join(", ", groupedCodes.Where(x => x.Count() > 1).Select(x => x.Key))}"
        }; 
    }

    public async Task<List<CatalogItemImageResponseDto>> AddImagesAsync(Guid id, string productId, List<IFormFile> images)
    {
        Catalog catalog = await _repository.GetByIdDetailedAsync(id);

        var product = (catalog.JsonTable?.FirstOrDefault(x => x[x.First().Key].ToString() == productId))
            ?? throw new NotFoundException(productId);

        int order = await _itemImageRepository.GetMaxOrderByProductAsync(productId) + 1;

		if (catalog.ItemImages.Where(x => x.ItemId == productId).Count() + images.Count > 15)
			throw new BadRequestException("The maximum number of images for a product is 15");

        List<CatalogItemImage> imagesToCreate = [];

        foreach(var image in images)
        {
            string fileName = GetFileName(image.FileName, catalog.Id.ToString(), productId, order.ToString());

            CatalogItemImage catalogItemImage = new()
            {
                CatalogId = catalog.Id,
                ItemId = productId,
                Order = order++,
                Name = Path.GetFileName(image.FileName),
                Uri = await _fileStorageService.UploadFileAsync(image, fileName, FileStorageKeys.CatalogProductsImages)
            };

            imagesToCreate.Add(catalogItemImage);

            await _itemImageRepository.CreateAsync(catalogItemImage);
        }

        return imagesToCreate.Select(_mapper.Map<CatalogItemImageResponseDto>).ToList();
    }

    public async Task<List<CatalogItemImageResponseDto>> GetImagesByProductAsync(Guid id, string productId)
    {
        List<CatalogItemImage> images = await _repository.GetImagesByProductIdAsync(id, productId);

        return images.Select(_mapper.Map<CatalogItemImageResponseDto>).OrderByDescending(x => x.Order).ToList();
    }

    public async Task DeleteImageAsync(Guid id, string productId, Guid imageId)
    {
        Catalog catalog = await _repository.GetByIdAsync(id);

        var img = await _itemImageRepository.GetByIdAsync(imageId);

        await Task.WhenAll(
            _fileStorageService.DeleteFileAsync(FileStorageKeys.CatalogProductsImages, GetFileName(img.Name, catalog.Id.ToString(), productId)),
            _itemImageRepository.DeleteAsync(imageId)
        );
    }

    public async Task<CatalogTagSegmentsResponseDto> GetTagsAsync(Guid id)
    {
        var catalog = await _repository.GetByIdDetailedAsync(id);
		var calendar = await _calendarRepository.GetNextDetailedAsync(true);

		CatalogTagSegmentsResponseDto response = new()
		{
			Tags = catalog.Tags,
			Fairs = _mapper.Map<List<CalendarFairOptionResponseDto>>(calendar.Fairs.Where(x => 
				x.FairRegisters.Any(fr => fr.User.Id == catalog.UserId)
			))
		};

		response.Segments = response.Fairs.SelectMany(f => f.Segments).ToList();

		foreach (var segment in catalog.Segments.Where(x => x.CalendarId == calendar.Id))
		{
			var resSegment = response.Segments.FirstOrDefault(s => s.Id == segment.Id);

			if (resSegment is not null)
			{
				resSegment.IsSelected = true;
			}
		}

		return response;
    }

    public async Task UpdateTagsAsync(Guid id, CatalogTagDto tags)
    {
		tags.Tags = tags.Tags.ToLower();
        var catalog = await _repository.GetByIdAsync(id);

        catalog.Tags = tags.Tags;

        await _repository.UpdateTagsAsync(catalog);
    }

	public async Task GenerateFairTagsAsync(Guid UserId, List<Guid> fairIds)
	{
		var catalog = await _repository.GetByUserIdOrDefaultAsync(UserId);
		var fairs = await _calendarFairRepository.GetByIdsDetailedAsync(fairIds);

		List<string> tagsToAdd = new();

		foreach (var fair in fairs) 
		{
			List<string> tempTagsToAdd = [];

			tagsToAdd.Add(fair.Name);

			foreach (var segment in fair.Segments)
			{
				tagsToAdd.Add(segment.Name);
			}
		}

		catalog!.Tags = string.Join(',', tagsToAdd.Where(x => !string.IsNullOrEmpty(x)).Distinct()) + ',' + catalog!.Tags;

		catalog!.Tags = catalog!.Tags.ToLower();

		await _repository.UpdateTagsAsync(catalog);
	}

	private static string GetFileName(string name, params string[] ids)
    {
        string prefix = string.Join('-', ids);

        return $"{prefix}-{Path.GetFileName(name)}";
    }
}
