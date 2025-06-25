using UnityEngine;
using Firebase.Auth;

/// <summary>
/// Check whether a user logged in,
/// if true active loggedInPanel, else active notLoggedInPanel
/// </summary>
public class LoginManager : MonoBehaviour
{
    [SerializeField] private GameObject loggedInPanel;
    [SerializeField] private GameObject notLoggedInPanel;

    void Start()
    {
        CheckUserLogin();
    }

    private void CheckUserLogin()
    {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        if (currentUser != null)
        {
            // User đang đăng nhập
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
            loggedInPanel.gameObject.SetActive(true);
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
}