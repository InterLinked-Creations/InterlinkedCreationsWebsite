using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InterlinkedCreations.Models;

public class Conversation
{
    [Key]
    public int ConversationID { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Members { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? ConversationTitle { get; set; }

    [MaxLength(255)]
    public string? ConversationLogo { get; set; }

    // Navigation properties
    public ICollection<ConversationData> Messages { get; set; } = new List<ConversationData>();
}
