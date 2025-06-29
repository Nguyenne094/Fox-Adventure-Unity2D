using UnityEngine;
using Firebase.Auth;
using TMPro;
using UnityEngine.UI;
using Firebase.Database;
using System.Threading.Tasks;

/// <summary>
/// Check whether a user logged in
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
    [SerializeField] private ScrollRect friendListScrollRect;


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

                // friendListScrollRect.content.GetComponent<TMP_Text>().text = "Friend List:\n";
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to retrieve user information: {ex.Message}");
            }
        }
        else
        {
            Debug.LogWarning("No user is currently logged in.");
        }
    }
}