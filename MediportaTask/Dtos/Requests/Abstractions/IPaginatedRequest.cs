using MediportaTask.Dtos.Requests.Enums;
using System.ComponentModel.DataAnnotations;

namespace MediportaTask.Dtos.Requests.Abstractions;

public interface IPaginatedRequest
{
    [Required]
    public uint PageNumber { get; set; }
    
    [Required]
    public uint PageSize { get; set; }
    
    [Required]
    public OrderDirection OrderDirection { get; set; }
    
    [Required]
    public string OrderPropertyName { get; set; }
}
