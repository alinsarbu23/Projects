using Insurance.Application.DTOs.Policies;

public class PagedPoliciesDto
{
    public IReadOnlyList<GetPoliciesDto> Items { get; set; } = [];

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalCount { get; set; }
}
