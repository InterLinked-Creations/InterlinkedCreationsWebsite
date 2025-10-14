using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InterlinkedCreations.Models;

public class Friend
{
    [Key]
    public int FriendshipID { get; set; }

    [Required]
    public int User1 { get; set; }

    [Required]
    public int User2 { get; set; }

    // Navigation properties
    [ForeignKey(nameof(User1))]
    public User? User1Navigation { get; set; }

    [ForeignKey(nameof(User2))]
    public User? User2Navigation { get; set; }
}
