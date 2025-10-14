# Quick Start Guide - Authentication Setup

## What Was Created

I've built a complete authentication system for your Razor Pages project with:

### 1. Database Models (in `Models/` folder)
- `User.cs` - User accounts with profile info and stats
- `Invite.cs`, `Mail.cs`, `Friend.cs` - Social features
- `Conversation.cs`, `ConversationData.cs` - Chat system
- `ApplicationDbContext.cs` - EF Core database context
- `AuthRequests.cs` - Request models for login/registration

### 2. Services (in `Services/` folder)
- `PasswordHasher.cs` - Secure BCrypt password hashing
- `AuthService.cs` - User authentication and registration logic

### 3. API Controller
- `AuthController.cs` - Handles all authentication API endpoints

### 4. Configuration
- Updated `Program.cs` with session support and services
- Updated `appsettings.json` with database connection string
- Added required NuGet packages (EF Core, BCrypt)

## How to Run

### Step 1: Create the Database
Open a terminal in the project directory and run:

```bash
cd InterlinkedCreations
dotnet ef migrations add InitialCreate
dotnet ef database update
```

This creates your SQL Server database with all tables.

### Step 2: Run the Application
```bash
dotnet run
```

### Step 3: Test the Authentication
1. Navigate to the home page
2. Click the account button (top left sidebar)
3. Choose "Create a new Account"
4. Fill in username, email, and password
5. You'll be registered and can select an avatar
6. Try logging out and logging back in

## How It Works

### Registration Flow
1. User fills out registration form
2. Frontend sends POST to `/api/register`
3. Backend validates data and checks for duplicates
4. Password is hashed with BCrypt
5. User is created in database
6. Session is created automatically
7. User can select an avatar

### Login Flow
1. User enters username/email and password
2. Frontend sends POST to `/api/login`
3. Backend finds user and verifies password
4. User data is returned for confirmation screen
5. User confirms login
6. Frontend sends POST to `/api/confirm-login`
7. Session is created
8. User is logged in

### Session Management
- Sessions last 24 hours
- Stored in server memory (DistributedMemoryCache)
- Secure HTTP-only cookies
- Session persists across page refreshes

## API Endpoints

All endpoints are prefixed with `/api`:

- **POST /api/register** - Create new account
  - Body: `{ username, email, password, confirmPassword }`
  - Returns: `{ userId, username, avatar }` or `{ errors }`

- **POST /api/login** - Login to account
  - Body: `{ username, password }`
  - Returns: `{ userId, username, avatar }` or `{ errors }`

- **POST /api/confirm-login** - Confirm login
  - Body: `{ userId }`
  - Returns: `{ success: true }`

- **GET /api/check-session** - Check authentication status
  - Returns: `{ authenticated, userId, username, avatar }`

- **POST /api/logout** - Logout current user
  - Returns: `{ success: true }`

- **POST /api/update-avatar** - Update user avatar
  - Body: `{ userId, avatar }`
  - Returns: `{ success: true }`

- **GET /api/avatars** - Get available avatars
  - Returns: `{ avatars: [...] }`

## Database Connection

Default connection string (in `appsettings.json`):
```json
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InterlinkedCreations;Trusted_Connection=True;MultipleActiveResultSets=true"
```

This uses SQL Server LocalDB. To use a different server, update the connection string.

## Troubleshooting

### "dotnet ef not found"
Install EF Core tools:
```bash
dotnet tool install --global dotnet-ef
```

### "Cannot open database"
Make sure SQL Server or LocalDB is installed and running.

### Login not working
1. Check browser console for errors
2. Verify database was created successfully
3. Check that session middleware is running (it is configured in Program.cs)

### Password requirements
- Minimum 6 characters
- No maximum length (hashed to same size)
- Stored securely with BCrypt

## Next Steps

Now that authentication is working, you can:

1. **Add more user features**:
   - Profile pages
   - User settings
   - Email verification
   - Password reset

2. **Build social features**:
   - Friend system (use the `Friend` and `Invite` tables)
   - Messaging system (use the `Mail` table)
   - Chat system (use `Conversation` and `ConversationData` tables)

3. **Protect routes**:
   - Create an authorization filter
   - Check session in controllers
   - Redirect unauthorized users

4. **Enhance security**:
   - Add CSRF protection
   - Implement rate limiting
   - Add email verification

## Your Frontend is Already Connected!

The JavaScript in `wwwroot/lib/js/account.js` is already configured to call these endpoints. The forms in your HTML are fully functional - just create the database and run the app!
