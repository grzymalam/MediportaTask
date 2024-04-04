namespace MediportaTask.Entities;

public sealed class Tag
{
    public Tag(uint count, decimal countPercentageShare, string name)
    {
        Count = count;
        CountPercentageShare = countPercentageShare;
        Name = name;
    }

    public int Id { get; private set; }
    public uint Count { get; init; }
    public decimal CountPercentageShare { get; init; }
    public string Name { get; init; }
}
