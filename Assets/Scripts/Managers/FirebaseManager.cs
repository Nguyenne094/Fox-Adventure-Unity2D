using Bap.DependencyInjection;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Utils;
using System.Threading.Tasks;
using Manager;

/// <summary>
/// Initializes Firebase and provides methods for user registration, login, and logout.
/// </summary>
public class FirebaseManager : Singleton<FirebaseManager>
{

    public DatabaseReference DbRef { get; private set; }

    [Provide]
    public FirebaseManager Provide() => this;

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

    void Start()
    {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        if(currentUser != null){
            var database = FirebaseDatabase.DefaultInstance;
            database.GetReference("users")
                .Child(currentUser)
                .Child("active")
                .SetValueAsync(true);
        }
    }

    public async Task<bool> Register(string email, string password, string userName)
    {
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

        await database.GetReference("users")
            .Child(currentUser.UserId)
            .Child("active")
            .SetValueAsync(true);

        GameManager.Instance.CurrentUserData = new UserData
        {
            userId = currentUser.UserId,
            userName = userName,
            email = email,
            password = password
        };

        return true;
    }

    public async Task<bool> Login(string email, string password)
    {
        if (HasActivatingUser()) return false;

        var auth = FirebaseAuth.DefaultInstance;
        var database = FirebaseDatabase.DefaultInstance;

        var signIn = await auth.SignInWithEmailAndPasswordAsync(email, password);
        var currentUser = auth.CurrentUser;
        var userId = currentUser.UserId;

        if (signIn == null || currentUser == null) return false;

        var userSnapshot = await database
            .GetReference("users")
            .Child(userId)
            .Child("userName")
            .GetValueAsync();

        await database.GetReference("users")
            .Child(userId)
            .Child("active")
            .SetValueAsync(true);

        GameManager.Instance.CurrentUserData = new UserData
        {
            userId = userId,
            userName = userSnapshot.Exists ? userSnapshot.Value.ToString() : "",
            email = email,
            password = password
        };

        return true;
    }

    public bool Logout()
    {
        if (HasActivatingUser())
        {
            string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            FirebaseDatabase.DefaultInstance.GetReference("users")
                .Child(userId)
                .Child("active")
                .SetValueAsync(false);
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

    private void OnApplicationQuit()
    {
        SetUserActive(false);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus) // app chuyển nền hoặc bị kill
            SetUserActive(false);
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
            SetUserActive(true);
    }

    private void SetUserActive(bool isActive)
    {
        if (HasActivatingUser())
        {
            string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            FirebaseDatabase.DefaultInstance.GetReference("users")
                .Child(userId)
                .Child("active")
                .SetValueAsync(isActive);
        }
    }
}