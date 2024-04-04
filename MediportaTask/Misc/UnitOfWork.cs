
using MediportaTask.Data.Contexts;

namespace MediportaTask.Misc;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly MediportaDbContext _context;

    public UnitOfWork(MediportaDbContext context)
    {
        _context = context;
    }

    public Task CommitAsync() => _context.SaveChangesAsync();
}
