using MediportaTask.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediportaTask.Data.Contexts.Configs;

public sealed class TagConfig : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder
            .HasKey(tag => tag.Id);
        
        builder
            .Property(tag => tag.Name)
            .IsRequired();

        builder
            .Property(tag => tag.CountPercentageShare)
            .IsRequired();

        builder
            .Property(tag => tag.Count)
            .IsRequired();
    }
}
