namespace Insurance.Application.DTOs.Clients;

public class PagedClientsDto
{
    public IReadOnlyList<GetClientsDto> Items { get; set; } = [];

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }
}
