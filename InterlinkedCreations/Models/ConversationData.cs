using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InterlinkedCreations.Models;

public class ConversationData
{
    [Key]
    public int MessageID { get; set; }

    [Required]
    public int ConversationID { get; set; }

    [Required]
    public int SenderID { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Message { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Attachment { get; set; }

    [Column(TypeName = "nvarchar(max)")]
    public string? Reactions { get; set; }

    public int? Reply { get; set; }

    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;

    public DateTime? DeletedAt { get; set; }

    // Navigation properties
    [ForeignKey(nameof(ConversationID))]
    public Conversation? Conversation { get; set; }

    [ForeignKey(nameof(SenderID))]
    public User? Sender { get; set; }

    [ForeignKey(nameof(Reply))]
    public ConversationData? ReplyToMessage { get; set; }

    public ICollection<ConversationData> Replies { get; set; } = new List<ConversationData>();
}
