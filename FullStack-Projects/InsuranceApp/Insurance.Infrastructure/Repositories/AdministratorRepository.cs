using AutoMapper;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Insurance.Infrastructure.Repositories;

public class AdministratorRepository : IAdministratorRepository
{
    private readonly InsuranceDbContext _context;
    private readonly IMapper _mapper;

    public AdministratorRepository(InsuranceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Administrator?> GetByIdAsync(Guid id)
    {
        var dao = await _context.Administrators.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        return _mapper.Map<Administrator?>(dao);
    }

    public async Task<Administrator?> GetByEmailAsync(string email)
    {
        var dao = await _context.Administrators.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

        return _mapper.Map<Administrator?>(dao);
    }
}
