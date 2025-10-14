# Database Setup Instructions

## Overview
This project uses Entity Framework Core with SQL Server to manage the database based on your Node.js SQLite schema.

## Models Created
- **User**: User accounts with stats (Stars, Silver, Gold, Gems, Points, Level)
- **Invite**: Friend invitation system
- **Mail**: In-game mail/message system with package content support
- **Friend**: Friend relationships between users
- **Conversation**: Group chat conversations
- **ConversationData**: Chat messages with reactions and replies

## Database Connection
The connection string is configured in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InterlinkedCreations;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

You can modify this to point to your SQL Server instance.

## Creating the Database

### Step 1: Install EF Core Tools (if not already installed)
```bash
dotnet tool install --global dotnet-ef
```

### Step 2: Create Initial Migration
```bash
cd InterlinkedCreations
dotnet ef migrations add InitialCreate
```

### Step 3: Update Database
```bash
dotnet ef database update
```

This will create the database and all tables according to the schema.

## Key Features

### JSON Support
- **Mail.PackageContent**: Stored as nvarchar(max), can store JSON data
- **Conversation.Members**: Stored as nvarchar(max), can store member list as JSON
- **ConversationData.Reactions**: Stored as nvarchar(max), can store reactions as JSON

### Relationships
- Users can send/receive invites
- Users can have multiple friendships
- Users can send/receive mail
- Users can participate in conversations and send messages
- Messages can reply to other messages

### Indexes
- Unique indexes on `User.Email` and `User.UserName`
- Unique index on `Friend(User1, User2)` to prevent duplicate friendships

### Soft Deletes
- `ConversationData.DeletedAt`: Supports soft deletion of messages

## Usage Example

### Injecting DbContext in a Razor Page
```csharp
public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var users = await _context.Users.ToListAsync();
        return Page();
    }
}
```

### Creating a New User
```csharp
var user = new User
{
    UserName = "JohnDoe",
    Password = "hashedpassword", // Remember to hash passwords!
    Email = "john@example.com",
    Avatar = "Colors/FillBlack"
};

_context.Users.Add(user);
await _context.SaveChangesAsync();
```

### Querying with Relationships
```csharp
// Get user with all their received mail
var user = await _context.Users
    .Include(u => u.MailsReceived)
    .FirstOrDefaultAsync(u => u.UserID == userId);

// Get conversation with all messages
var conversation = await _context.Conversations
    .Include(c => c.Messages)
    .ThenInclude(m => m.Sender)
    .FirstOrDefaultAsync(c => c.ConversationID == conversationId);
```

## Migration from SQLite
If you have existing SQLite data, you'll need to:
1. Export data from SQLite
2. Transform data as needed (especially JSON fields)
3. Import into SQL Server

Note: The `Mail.SenderID` is stored as `decimal` in the original schema - you may want to change this to `int` if it should reference a User.

## Next Steps
1. Run the migrations to create the database
2. Consider adding:
   - Password hashing (use ASP.NET Core Identity or BCrypt)
   - Data validation attributes
   - API controllers or Razor Pages for CRUD operations
   - Authentication and authorization
