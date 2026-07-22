using System.ComponentModel.DataAnnotations;

namespace LocumInvoiceTracker.Models;

public sealed class Hospital
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(2)]
    public string Province { get; set; } = "AB";

    public ICollection<WorkShift> WorkShifts { get; set; } =
        new List<WorkShift>();
}