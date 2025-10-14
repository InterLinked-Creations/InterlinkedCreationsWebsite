using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InterlinkedCreations.Models;

public class User
{
    [Key]
    public int UserID { get; set; }

    [Required]
    [MaxLength(50)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(100)]
    public string Avatar { get; set; } = "Colors/FillBlack";

    public long Stars { get; set; } = 0;

    public long Silver { get; set; } = 0;

    public long Gold { get; set; } = 0;

    public long Gems { get; set; } = 0;

    public long Points { get; set; } = 0;

    public long Level { get; set; } = 1;

    // Navigation properties
    public ICollection<Invite> InvitesSent { get; set; } = new List<Invite>();
    public ICollection<Invite> InvitesReceived { get; set; } = new List<Invite>();
    public ICollection<Mail> MailsReceived { get; set; } = new List<Mail>();
    public ICollection<Friend> FriendsAsUser1 { get; set; } = new List<Friend>();
    public ICollection<Friend> FriendsAsUser2 { get; set; } = new List<Friend>();
    public ICollection<ConversationData> Messages { get; set; } = new List<ConversationData>();
}
