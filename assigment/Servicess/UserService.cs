using Serilog; // Ensure Serilog is installed via NuGet [cite: 62]
using assigment.Models;
namespace assigment.Servicess;
public class UserService
{
    private static List<User> _userDb = new List<User>();

    public bool RegisterUser(string username, string password)
    {
        try // Task 3.1: Proper error handling [cite: 60]
        {
            var user = new User
            {
                Username = username,
                HashedPassword = User.HashPassword(password)
            };
            _userDb.Add(user);
            Log.Information("User {User} registered successfully", username);
            return true;
        }
        catch (Exception ex)
        {
             Log.Error(ex, "Error registering user {User}", username); 
            return false;
        }
    }

    public bool Login(string username, string password)
    {
        try
        {
            var user = _userDb.FirstOrDefault(u => u.Username == username);
            if (user == null) return false;

            bool isAuth = user.HashedPassword == User.HashPassword(password); 
         Log.Information("Login attempt for {User}: {Status}", username, isAuth); 
            return isAuth;
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Authentication error"); 
            return false;
        }
    }
}