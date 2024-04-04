namespace MediportaTask.Misc;

public interface IUnitOfWork
{
    public Task CommitAsync();
}
