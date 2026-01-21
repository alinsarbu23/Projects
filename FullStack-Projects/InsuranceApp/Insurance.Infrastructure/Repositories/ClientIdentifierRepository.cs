using AutoMapper;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repositories;

public class ClientIdentifierRepository : IClientIdentifierRepository
{
    private readonly InsuranceDbContext _context;
    private readonly IMapper _mapper;

    public ClientIdentifierRepository(
        InsuranceDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddAsync(ClientIdentifier identifier)
    {
        var dao = _mapper.Map<DAOs.ClientIdentifier>(identifier);
        await _context.ClientIdentifiers.AddAsync(dao);
    }

    public async Task<int> CountByClientIdAsync(Guid clientId)
    {
        return await _context.ClientIdentifiers.AsNoTracking().CountAsync(x => x.ClientId == clientId);
    }

    public async Task<DateTime?> GetLastChangeDateAsync(Guid clientId)
    {
        return await _context.ClientIdentifiers.AsNoTracking()
            .Where(x => x.ClientId == clientId)
            .OrderByDescending(x => x.ChangedAt)
            .Select(x => x.ChangedAt)
            .FirstOrDefaultAsync();
    }

}
