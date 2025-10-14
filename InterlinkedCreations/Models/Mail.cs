using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InterlinkedCreations.Models;

public class Mail
{
    [Key]
    public int MailID { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal SenderID { get; set; }

    [Required]
    public int RecieverID { get; set; }

    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(10000)]
    public string Message { get; set; } = string.Empty;

    [Column(TypeName = "nvarchar(max)")]
    public string? PackageContent { get; set; }

    // Navigation properties
    [ForeignKey(nameof(RecieverID))]
    public User? Receiver { get; set; }
}
