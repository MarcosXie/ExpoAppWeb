using AutoMapper;
using Microsoft.AspNetCore.Http;
using UExpo.Application.Utils;
using UExpo.Domain.Catalogs;
using UExpo.Domain.Catalogs.ItemImages;
using UExpo.Domain.Catalogs.Pdfs;
using UExpo.Domain.Exceptions;
using UExpo.Domain.FileStorage;

namespace UExpo.Application.Services.Catalogs;

public class CatalogService : ICatalogService
{
    private readonly ICatalogRepository _repository;
    private readonly ICatalogPdfRepository _pdfRepository;
    private readonly ICatalogItemImageRepository _itemImageRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IMapper _mapper;

    public CatalogService(
        ICatalogRepository repository,
        ICatalogPdfRepository pdfRepository,
        ICatalogItemImageRepository itemImageRepository,
        IFileStorageService fileStorageService,
        IMapper mapper)
    {
        _repository = repository;
        _pdfRepository = pdfRepository;
        _itemImageRepository = itemImageRepository;
        _fileStorageService = fileStorageService;
        _mapper = mapper;
    }

    public async Task<CatalogResponseDto> GetOrCreateAsync(string id)
    {
        Guid parsedId = Guid.Parse(id);
        Catalog? catalog = await _repository.GetByUserIdOrDefaultAsync(parsedId);

        if (catalog is null)
        {
            catalog = new()
            {
                UserId = parsedId
            };

            await _repository.CreateAsync(catalog);
        }

        return _mapper.Map<CatalogResponseDto>(catalog);
    }

    public async Task<Guid> AddPdfAsync(CatalogPdfDto pdf)
    {
        Catalog catalog = await _repository.GetByIdAsync(pdf.CatalogId);
        string fileName = GetFileName(pdf.File.FileName, catalog.Id.ToString());

        CatalogPdf dbPdf = new()
        {
            CatalogId = catalog.Id,
            Name = Path.GetFileName(pdf.File.FileName),
            Uri = await _fileStorageService.UploadFileAsync(pdf.File, fileName, FileStorageKeys.CatalogPdf)
        };

        return await _pdfRepository.CreateAsync(dbPdf);
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

    public async Task AddImagesAsync(Guid id, string productId, List<IFormFile> images)
    {
        Catalog catalog = await _repository.GetByIdDetailedAsync(id);

        var product = (catalog.JsonTable?.FirstOrDefault(x => x[x.First().Key].ToString() == productId))
            ?? throw new NotFoundException(productId);

        int order = 1;

        foreach(var image in images)
        {
            string fileName = GetFileName(image.FileName, catalog.Id.ToString(), productId);

            CatalogItemImage catalogItemImage = new()
            {
                CatalogId = catalog.Id,
                ItemId = productId,
                Order = order++,
                Uri = await _fileStorageService.UploadFileAsync(image, fileName, FileStorageKeys.CatalogProductsImages)
            };

            await _itemImageRepository.CreateAsync(catalogItemImage);
        }
    }

    private static string GetFileName(string name, params string[] ids)
    {
        string prefix = string.Join('-', ids);

        return $"{prefix}-{Path.GetFileName(name)}";
    }

}
