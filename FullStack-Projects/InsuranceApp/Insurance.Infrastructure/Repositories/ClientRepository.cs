using AutoMapper;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Insurance.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly InsuranceDbContext _context;
    private IMapper _mapper;

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
        var clientDao = await _context.Clients.AsNoTracking().FirstOrDefaultAsync(client => client.Id == clientId);
        return _mapper.Map<Client?>(clientDao);
    }

    public async Task<IReadOnlyList<Client>> SearchAsync(string? name, string? identifier, int page, int pageSize)
    {
        var query = _context.Clients.AsNoTracking().AsQueryable();
        if(!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(client => client.Name.Contains(name));
        }

        if(!string.IsNullOrWhiteSpace(identifier))
        {
            query = query.Where(client => client.Cnp == identifier || client.Cui == identifier);
        }

        var clientDaos = await query.OrderBy(client=>client.Name).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return _mapper.Map<IReadOnlyList<Client>>(clientDaos);
    }

    public Task UpdateAsync(Client client)
    {
        var clientDao = _mapper.Map<DAOs.Client>(client);
        _context.Clients.Update(clientDao);
        return Task.CompletedTask;
    }
}
