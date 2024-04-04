namespace MediportaTask.Dtos.Responses.Abstractions;

public interface IPaginatedResponse<T> where T : class
{
    public uint Total { get; set; }
    public uint PageSize { get; set; }
    public uint PageNumber { get; set; }
    public IEnumerable<T> Items { get; set; }
}
