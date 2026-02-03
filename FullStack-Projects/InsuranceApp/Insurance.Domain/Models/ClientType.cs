namespace Insurance.Domain.Models;

public class ClientType
{
    public int Id { get; set; }

    public string Code { get; set; } = null!; 

    public string Name { get; set; } = null!;
}
