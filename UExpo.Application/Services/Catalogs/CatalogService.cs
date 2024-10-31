using AutoMapper;
using Microsoft.AspNetCore.Http;
using UExpo.Application.Utils;
using UExpo.Domain.Entities.Calendars;
using UExpo.Domain.Entities.Calendars.Fairs;
using UExpo.Domain.Entities.Calendars.Segments;
using UExpo.Domain.Entities.Catalogs;
using UExpo.Domain.Entities.Catalogs.CatalogSegments;
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
	private readonly ICatalogSegmentRepository _catalogSegmentRepository;
	private readonly ICalendarSegmentRepository _calendarSegmentRepository;
	private readonly IMapper _mapper;

	public CatalogService(
		ICatalogRepository repository,
		ICatalogPdfRepository pdfRepository,
		ICatalogItemImageRepository itemImageRepository,
		IFileStorageService fileStorageService,
		ICalendarFairRepository calendarFairRepository,
		ICalendarSegmentRepository calendarSegmentRepository,
		ICalendarRepository calendarRepository,
		ICatalogSegmentRepository catalogSegmentRepository,
		IMapper mapper)
	{
		_repository = repository;
		_pdfRepository = pdfRepository;
		_itemImageRepository = itemImageRepository;
		_fileStorageService = fileStorageService;
		_calendarRepository = calendarRepository;
		_calendarFairRepository = calendarFairRepository;
		_catalogSegmentRepository = catalogSegmentRepository;
		_calendarSegmentRepository = calendarSegmentRepository;
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
				$"The first column contains repeated identifier values: {string.Join(", ", groupedCodes.Where(x => x.Count() > 1).Select(x => x.Key))}");

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

		if (catalog.ItemImages.Where(x => x.ItemId == productId).Count() + images.Count > 6)
			throw new BadRequestException("The maximum number of images for a product is 6");

		List<CatalogItemImage> imagesToCreate = [];

		foreach (var image in images)
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

	private static string GetFileName(string name, params string[] ids)
	{
		string prefix = string.Join('-', ids);

		return $"{prefix}-{Path.GetFileName(name)}";
	}
}
