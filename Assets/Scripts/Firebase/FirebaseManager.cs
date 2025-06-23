using Bap.DependencyInjection;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Utils;

public class FirebaseManager : Singleton<FirebaseManager>
{
    public DatabaseReference DbRef { get; private set; }
 
    [Provide]
    public FirebaseManager Provide() => this;
    
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
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

    public bool Register(string email, string password)
    {
        if (!HasActivatingUser())
        {
            FirebaseAuth.DefaultInstance.CreateUserWithEmailAndPasswordAsync(email, password);
            return true;
        }
        Debug.Log("Already had activating user");
        return false;
    }
    
    public bool Login(string email, string password)
    {
        if (!HasActivatingUser())
        {
            FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email, password);
            return true;
        }
        Debug.Log("Already had activating user");
        return false;
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