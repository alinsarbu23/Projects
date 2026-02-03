using AutoMapper;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Insurance.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly InsuranceDbContext _context;
    private readonly IMapper _mapper;

    public ClientRepository(InsuranceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task AddAsync(Client client)
    {
        var clientDao = _mapper.Map<DAOs.Client>(client);
        await _context.Clients.AddAsync(clientDao);
    }

    public async Task<int> CountAsync(string? name, string? identifier)
    {
        var query = _context.Clients.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(c => c.Name.Contains(name));
        }
            
        if (!string.IsNullOrWhiteSpace(identifier))
        {
            query = query.Where(c => c.Cnp == identifier || c.Cui == identifier);
        }
            

        return await query.CountAsync();
    }

    public Task<bool> ExistsAsync(Guid clientId)
    {
        return _context.Clients.AsNoTracking().AnyAsync(c => c.Id == clientId);
    }

    public Task<bool> ExistsByCnpAsync(string cnp)
    {
        return _context.Clients.AsNoTracking().AnyAsync(client => client.Cnp == cnp);
    }

    public Task<bool> ExistsByCuiAsync(string cui)
    {
        return _context.Clients.AsNoTracking().AnyAsync(client => client.Cui == cui);
    }

    public async Task<Client?> GetByIdAsync(Guid clientId)
    {
        var dao = await _context.Clients
            .AsNoTracking()
            .Include(c => c.ClientType)
            .FirstOrDefaultAsync(c => c.Id == clientId);

        return _mapper.Map<Client?>(dao);
    }

    public async Task<IReadOnlyList<Client>> SearchAsync(
        string? name,
        string? identifier,
        int page,
        int pageSize)
    {
        var query = _context.Clients
            .AsNoTracking()
            .Include(c => c.ClientType)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(c => c.Name.Contains(name));

        if (!string.IsNullOrWhiteSpace(identifier))
            query = query.Where(c => c.Cnp == identifier || c.Cui == identifier);

        var daos = await query
            .OrderBy(c => c.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return _mapper.Map<IReadOnlyList<Client>>(daos);
    }

    public async Task UpdateAsync(Client client)
    {
        var existingDao = await _context.Clients
            .FirstAsync(c => c.Id == client.Id);

        _mapper.Map(client, existingDao);
    }
}
