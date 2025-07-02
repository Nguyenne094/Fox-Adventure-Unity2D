using Bap.DependencyInjection;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Utils;
using System.Threading.Tasks;
using Manager;
using Nguyen.Event;

/// <summary>
/// Initializes Firebase and provides methods for user registration, login, and logout.
/// </summary>
public class FirebaseManager : Singleton<FirebaseManager>
{
    [Header("Firebase Configuration")]
    [SerializeField] private VoidEventChannelSO OnFirebaseInitialized;

    public DatabaseReference DbRef { get; private set; }
    public UserData CurrentUserData { get; private set; }

    [Provide]
    public FirebaseManager Provide() => this;

    public override async void Awake()
    {
        base.Awake();
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                DbRef = FirebaseDatabase.DefaultInstance.RootReference;
                OnFirebaseInitialized.RaiseEvent();
            }
            else
            {
                Debug.LogError("Firebase dependencies unresolved: " + task.Exception);
            }
        });
    }

    public void Start() {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }


    public async void SetupOnFirebaseInitialized()
    {
        var userId = FirebaseAuth.DefaultInstance.CurrentUser?.UserId;
        try
        {
            await DbRef.Child("users").Child(userId).Child("active").SetValueAsync(true);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error setting user active: " + e.Message);
        }
    }

    public async Task<bool> Register(string email, string password, string userName)
    {
        if (HasActivatingUser()) return false;

        var auth = FirebaseAuth.DefaultInstance;
        var database = FirebaseDatabase.DefaultInstance;

        // Check userName unique manually
        var usersSnapshot = await database.GetReference("users").GetValueAsync();
        foreach (var child in usersSnapshot.Children)
        {
            if (child.Child("userName").Value?.ToString() == userName)
                return false;
        }

        try
        {
            var credential = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            if (credential == null) return false;

            var user = auth.CurrentUser;
            if (user == null) return false;

            string userId = user.UserId;

            await database.GetReference("users").Child(userId).Child("userName").SetValueAsync(userName);
            await database.GetReference("users").Child(userId).Child("active").SetValueAsync(true);

            CurrentUserData = new UserData(userId, userName, email);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Register failed: " + e.Message);
            return false;
        }
    }

    public async Task<bool> Login(string email, string password)
    {
        if (HasActivatingUser()) return false;

        var auth = FirebaseAuth.DefaultInstance;
        try
        {
            var signIn = await auth.SignInWithEmailAndPasswordAsync(email, password);
            var user = auth.CurrentUser;
            if (signIn == null || user == null) return false;

            string userId = user.UserId;

            var snapshot = await DbRef.Child("users").Child(userId).Child("userName").GetValueAsync();
            string userName = snapshot.Exists ? snapshot.Value.ToString() : "";

            await DbRef.Child("users").Child(userId).Child("active").SetValueAsync(true);

            CurrentUserData = new UserData(userId, userName, email);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Login failed: " + e.Message);
            return false;
        }
    }

    public bool Logout()
    {
        if (!HasActivatingUser())
        {
            Debug.Log("No user to log out.");
            return false;
        }

        string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        FirebaseDatabase.DefaultInstance.GetReference("users").Child(userId).Child("active").SetValueAsync(false);
        FirebaseAuth.DefaultInstance.SignOut();
        CurrentUserData = default;
        return true;
    }

    private bool HasActivatingUser()
    {
        return FirebaseAuth.DefaultInstance.CurrentUser != null;
    }

    private void OnApplicationQuit()
    {
        SetUserActive(false);
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SetUserActive(false);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus) SetUserActive(true);
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
    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token) {
        Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e) {
        Debug.Log("Received a new message from: " + e.Message.From);
    }
}
