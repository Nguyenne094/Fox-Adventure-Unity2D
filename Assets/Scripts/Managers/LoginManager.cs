using UnityEngine;
using Firebase.Auth;
using TMPro;
using UnityEngine.UI;
using Firebase.Database;
using System.Threading.Tasks;
using System.Collections.Generic;

/// <summary>
/// Handles user login on starting game and displays user
/// </summary>
public class LoginManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject loggedInPanel;
    [SerializeField] private GameObject notLoggedInPanel;

    [Header("User Information")]
    [SerializeField] private TMP_Text userIdText;
    [SerializeField] private TMP_Text userNameText;
    [SerializeField] private TMP_Text userEmailText;
    [SerializeField] private RectTransform contentOfFriendListScrollView;
    [SerializeField] private GameObject friendViewPrefab;

    void Start()
    {
        CheckUserLogin();
    }

    private void CheckUserLogin()
    {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        if (currentUser != null)
        {
            ActivateLoggedInUI();
        }
        else
        {
            // Chưa có user đăng nhập
            ActivateNotLoggedInUI();
        }
    }

    private void ActivateLoggedInUI()
    {
        if (loggedInPanel != null)
        {
            loggedInPanel.gameObject.SetActive(true);
            UpdateUserInforUI();
        }
        if (notLoggedInPanel != null)
            notLoggedInPanel.gameObject.SetActive(false);
    }

    private void ActivateNotLoggedInUI()
    {
        if (notLoggedInPanel != null)
            notLoggedInPanel.gameObject.SetActive(true);
        if (loggedInPanel != null)
            loggedInPanel.gameObject.SetActive(false);
    }

    public async void UpdateUserInforUI()
    {
        var auth = FirebaseAuth.DefaultInstance;
        var database = FirebaseDatabase.DefaultInstance;

        if (auth == null || database == null)
        {
            Debug.LogWarning("Firebase Auth or Database is not initialized.");
            return; // Ensure early exit if Firebase is not initialized
        }

        if (auth.CurrentUser != null)
        {
            userIdText.text = $"{auth.CurrentUser.UserId}";
            userEmailText.text = $"{auth.CurrentUser.Email}";

            try
            {
                var userNameSnapshot = await database.GetReference("users")
                    .Child(auth.CurrentUser.UserId)
                    .Child("userName")
                    .GetValueAsync();

                if (userNameSnapshot.Exists)
                {
                    userNameText.text = $"{userNameSnapshot.Value}";
                }
                else
                {
                    userNameText.text = "Not Found";
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to retrieve user information: {ex.Message}");
            }
            await LoadActiveUsers();
        }
        else
        {
            Debug.LogWarning("No user is currently logged in.");
        }
    }

    public async Task LoadActiveUsers()
    {
        var db = FirebaseDatabase.DefaultInstance;
        var usersSnapshot = await db.GetReference("users").GetValueAsync();

        if (usersSnapshot.Exists)
        {
            List<UserData> friendsInfor = new List<UserData>();

            foreach (var user in usersSnapshot.Children)
            {
                string name = "";
                bool isActive = false;

                if (user.Child("userName").Exists)
                    name = user.Child("userName").Value.ToString();

                if (user.Child("active").Exists)
                    isActive = bool.Parse(user.Child("active").Value.ToString());

                if (isActive && user.Key != FirebaseAuth.DefaultInstance.CurrentUser.UserId)
                {
                    friendsInfor.Add(new UserData
                    {
                        userId = user.Key,
                        userName = name,
                        email = user.Child("email").Exists ? user.Child("email").Value.ToString() : "",
                    });
                }
            }

            // Clear existing friends list
            foreach (Transform child in contentOfFriendListScrollView)
            {
                Destroy(child.gameObject);
            }

            // Populate the friends list with active users
            foreach (var friend in friendsInfor)
            {
                GameObject friendView = Instantiate(friendViewPrefab, contentOfFriendListScrollView);
                friendView.GetComponentInChildren<TMP_Text>().text = friend.userName;
            }
        }
        else
        {
        }
    }
}

[System.Serializable]
public struct UserData
{
    public string userId;
    public string userName;
    public string email;
    public string password;
}