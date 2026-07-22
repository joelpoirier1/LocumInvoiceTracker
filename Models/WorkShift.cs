using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocumInvoiceTracker.Models;

public sealed class WorkShift
{
    public int Id { get; set; }

    [Required]
    public DateTime Date { get; set; } = DateTime.Today;

    [Range(typeof(decimal), "0.25", "24")]
    public decimal HoursWorked { get; set; } = 8;

    [Range(typeof(decimal), "1", "10000")]
    public decimal HourlyRate { get; set; } = 65;

    public bool Paid { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Select a hospital.")]
    public int HospitalId { get; set; }

    public Hospital? Hospital { get; set; }

    [NotMapped]
    public decimal Total => HoursWorked * HourlyRate;
}