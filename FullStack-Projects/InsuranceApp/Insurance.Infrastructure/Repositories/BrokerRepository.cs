using AutoMapper;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repositories;

public class BrokerRepository : IBrokerRepository
{
    private readonly InsuranceDbContext _context;
    private readonly IMapper _mapper;

    public BrokerRepository(InsuranceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddAsync(Broker broker)
    {
        await _context.Brokers.AddAsync(_mapper.Map<DAOs.Broker>(broker));
    }

    public async Task UpdateAsync(Broker broker)
    {
        var dao = await _context.Brokers.FirstAsync(x => x.Id == broker.Id);

        _mapper.Map(broker, dao);
    }

    public async Task<Broker?> GetByIdAsync(Guid id)
    {
        var dao = await _context.Brokers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        return _mapper.Map<Broker?>(dao);
    }

    public async Task<Broker?> GetByCodeAsync(string brokerCode)
    {
        var dao = await _context.Brokers.AsNoTracking().FirstOrDefaultAsync(x => x.BrokerCode == brokerCode);

        return _mapper.Map<Broker?>(dao);
    }

    public async Task<Broker?> GetByEmailAsync(string email)
    {
        var dao = await _context.Brokers.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

        return _mapper.Map<Broker?>(dao);
    }

    public Task<bool> IsActiveAsync(Guid brokerId)
    {
        return _context.Brokers.AsNoTracking().AnyAsync(x => x.Id == brokerId && x.Status == "Active");
    }

    public async Task<IReadOnlyList<Broker>> GetAllAsync()
    {
        var daos = await _context.Brokers.AsNoTracking().OrderBy(x => x.Name).ToListAsync();

        return _mapper.Map<IReadOnlyList<Broker>>(daos);
    }
}
