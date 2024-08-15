using AutoMapper;
using UExpo.Domain.Catalogs;
using UExpo.Domain.Catalogs.ItemImages;
using UExpo.Domain.Catalogs.Pdfs;
using UExpo.Domain.FileStorage;

namespace UExpo.Application.Services.Catalogs;

public class CatalogService : ICatalogService
{
    private ICatalogRepository _repository;
    private ICatalogPdfRepository _pdfRepository;
    private ICatalogItemImageRepository _itemImageRepository;
    private IFileStorageService _fileStorageService;
    private IMapper _mapper;

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
        var parsedId = Guid.Parse(id);
        var catalog = await _repository.GetByUserIdOrDefaultAsync(parsedId);

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
        var catalog = await _repository.GetByIdAsync(pdf.CatalogId);
        var fileName = GetFileName(pdf.File.FileName, catalog);
        
        var dbPdf = new CatalogPdf()
        {
            CatalogId = catalog.Id,
            Name = Path.GetFileName(pdf.File.FileName),
            Uri = await _fileStorageService.UploadFileAsync(pdf.File, fileName, FileStorageKeys.CatalogPdf)
        };

        return await _pdfRepository.CreateAsync(dbPdf);
    }

    public async Task DeletePdfAsync(Guid id, Guid pdfId)
    {
        var catalog = await _repository.GetByIdAsync(id);

        var pdf = await _pdfRepository.GetByIdAsync(pdfId);

        await Task.WhenAll(
            _fileStorageService.DeleteFileAsync(FileStorageKeys.CatalogPdf, GetFileName(pdf.Name, catalog)),
            _pdfRepository.DeleteAsync(pdfId)
        );
    }

    private static string GetFileName(string name, Catalog catalog)
    {
        return $"{catalog.Id}-{Path.GetFileName(name)}";
    }
}
