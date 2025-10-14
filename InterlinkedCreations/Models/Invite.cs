using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InterlinkedCreations.Models;

public class Invite
{
    [Key]
    public int InviteID { get; set; }

    [Required]
    public int FromUser { get; set; }

    [Required]
    public int ToUser { get; set; }

    // Navigation properties
    [ForeignKey(nameof(FromUser))]
    public User? FromUserNavigation { get; set; }

    [ForeignKey(nameof(ToUser))]
    public User? ToUserNavigation { get; set; }
}
