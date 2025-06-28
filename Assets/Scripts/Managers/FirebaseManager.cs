using Bap.DependencyInjection;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using Utils;
using System.Threading.Tasks;

public class FirebaseManager : Singleton<FirebaseManager>
{

    public DatabaseReference DbRef { get; private set; }
 
    [Provide]
    public FirebaseManager Provide() => this;
    
    public override void Awake()
    {
        base.Awake();
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

    public async Task Register(string email, string password, string userName)
    {
        if (!HasActivatingUser())
        {
            try
            {
                var auth = FirebaseAuth.DefaultInstance;
                var database = FirebaseDatabase.DefaultInstance;

                if (auth == null)
                {
                    Debug.LogError("FirebaseAuth.DefaultInstance is null. Ensure Firebase is initialized.");
                    return;
                }

                // Check if username already exists
                var usernameExists = false;
                await database.GetReference("users")
                    .OrderByValue()
                    .EqualTo(userName)
                    .GetValueAsync()
                    .ContinueWithOnMainThread(task => {
                        if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                        {
                            usernameExists = task.Result.ChildrenCount > 0;
                        }
                        else
                        {
                            Debug.LogError("Failed to check username existence: " + task.Exception);
                        }
                    });

                if (usernameExists)
                {
                    Debug.LogError("Username already exists in the database.");
                    return;
                }

                await auth.CreateUserWithEmailAndPasswordAsync(email, password)
                    .ContinueWithOnMainThread(task => {
                        if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                        {
                            Debug.Log("Firebase registration successful");

                            // Sign in the user after registration
                            auth.SignInWithEmailAndPasswordAsync(email, password)
                                .ContinueWithOnMainThread(signInTask => {
                                    if (signInTask.IsCompleted && !signInTask.IsFaulted && !signInTask.IsCanceled)
                                    {
                                        Debug.Log("Firebase login successful after registration");

                                        var currentUser = auth.CurrentUser;
                                        if (currentUser != null)
                                        {
                                            database
                                                .GetReference("users")
                                                .Child(currentUser.UserId)
                                                .SetValueAsync(userName)
                                                .ContinueWithOnMainThread(dbTask => {
                                                    if (dbTask.IsCompleted && !dbTask.IsFaulted && !dbTask.IsCanceled)
                                                    {
                                                        Debug.Log("User data saved successfully");
                                                    }
                                                    else
                                                    {
                                                        Debug.LogError($"Failed to save user data: {dbTask.Exception}");
                                                    }
                                                });
                                        }
                                        else
                                        {
                                            Debug.LogError("CurrentUser is null after login.");
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError($"Firebase login failed after registration: {signInTask.Exception}");
                                    }
                                });
                        }
                        else
                        {
                            Debug.LogError($"Firebase registration failed: {task.Exception}");
                        }
                    });
            }
            catch (FirebaseException e)
            {
                Debug.LogError($"Firebase registration failed: {e.Message}");
            }
        }

        Debug.Log("Already had activating user");
    }
    
    public async Task Login(string email, string password)
    {
        if (!HasActivatingUser())
        {
            try
            {
                await FirebaseAuth.DefaultInstance.SignInWithEmailAndPasswordAsync(email, password);
                Debug.Log("Firebase login successful");
            }
            catch (FirebaseException e)
            {
                Debug.LogError($"Firebase login failed: {e.Message}");
            }
        }
        Debug.Log("Already had activating user");
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