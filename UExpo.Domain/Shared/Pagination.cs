namespace UExpo.Domain.Shared;

public class Pagination<T> where T : class
{
	public int Page { get; set; }
	public int TotalCount { get; set; }
	public int RowsPerPage { get; set; }
	public List<T> Data { get; set; } = [];
}

