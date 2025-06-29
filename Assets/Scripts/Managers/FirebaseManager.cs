using Bap.DependencyInjection;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Utils;
using System.Threading.Tasks;
using Manager;

public class FirebaseManager : Singleton<FirebaseManager>
{

    public DatabaseReference DbRef { get; private set; }
 
    [Provide]
    public FirebaseManager Provide() => this;
    
    public bool RegisterSuccess { get; set; }
    public bool LoginSuccess { get; set; }

    public override void Awake()
    {
        base.Awake();
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                DbRef = FirebaseDatabase.DefaultInstance.RootReference;
            }
            else
            {
                Debug.LogError("Could not resolve Firebase dependencies." + task.Exception);
            }
        });
    }

    public async Task<bool> Register(string email, string password, string userName)
    {
        RegisterSuccess = false;
        if (HasActivatingUser()) return false;

        var auth = FirebaseAuth.DefaultInstance;
        var database = FirebaseDatabase.DefaultInstance;

        // Check username exists
        var usernameExistsSnapshot = await database.GetReference("users")
            .OrderByValue()
            .EqualTo(userName)
            .GetValueAsync();

        if (usernameExistsSnapshot.ChildrenCount > 0) return false;

        // Create user
        var userCredential = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
        if (userCredential == null) return false;

        var signIn = await auth.SignInWithEmailAndPasswordAsync(email, password);
        if (signIn == null) return false;

        var currentUser = auth.CurrentUser;
        if (currentUser == null) return false;

        await database.GetReference("users")
            .Child(currentUser.UserId)
            .Child("userName")
            .SetValueAsync(userName);

        GameManager.Instance.UserInfo = new UserInfor
        {
            userId = currentUser.UserId,
            userName = userName,
            email = email,
            password = password
        };

        RegisterSuccess = true;
        return true;
    }

    public async Task<bool> Login(string email, string password)
    {
        LoginSuccess = false;
        if (HasActivatingUser()) return false;

        var signIn = await FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email, password);
        if (signIn == null || FirebaseAuth.DefaultInstance.CurrentUser == null) return false;

        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;

        var userSnapshot = await FirebaseDatabase.DefaultInstance
            .GetReference("users")
            .Child(userId)
            .Child("userName")
            .GetValueAsync();

        GameManager.Instance.UserInfo = new UserInfor
        {
            userId = userId,
            userName = userSnapshot.Exists ? userSnapshot.Value.ToString() : "",
            email = email,
            password = password
        };

        LoginSuccess = true;
        return true;
    }

    public bool Logout()
    {
        if (HasActivatingUser())
        {
            FirebaseAuth.DefaultInstance.SignOut();
            return true;
        }
        Debug.Log("No user found");
        return false;
    }
    
    public void LogoutForButton()
    {
        if (HasActivatingUser())
        {
            FirebaseAuth.DefaultInstance.SignOut();
            return;
        }
        Debug.Log("No user found");
    }

    private bool HasActivatingUser()
    {
        var auth = FirebaseAuth.DefaultInstance;
        return auth.CurrentUser != null;
    }
}