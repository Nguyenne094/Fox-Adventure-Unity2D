using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controller for user login and registration.
/// Handles user input for email, password, and username.
/// </summary> <summary>
/// 
/// </summary>
public class LoginController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FirebaseManager _firebaseManager;
    [SerializeField] private LoginManager _loginManager;
    [SerializeField] private TMP_InputField _email;
    [SerializeField] private TMP_InputField _password;
    [SerializeField] private TMP_InputField _userName;
    [SerializeField, Tooltip("For Register Only")] private TMP_InputField _confirmPassword;

    [Header("Events")]
    [SerializeField] private UnityEvent _registerSuccess;
    [SerializeField] private UnityEvent _loginSuccess;


    public async void Register()
    {
        if (IsEmailOrPasswordEmpty(_email.text, _password.text)) return;
        if (!IsValidEmail(_email.text)) return;
        if (_confirmPassword.text != _password.text)
        {
            Debug.LogWarning("Passwords do not match");
            return;
        }

        bool success = await _firebaseManager.Register(_email.text, _password.text, _userName.text);
        if (success)
        {
            _registerSuccess?.Invoke();
            _loginManager.UpdateUserInforUI();
        }
        else
        {
            Debug.LogWarning("Register failed.");
        }
    }

    public async void Login()
    {
        if (IsEmailOrPasswordEmpty(_email.text, _password.text)) return;
        if (!IsValidEmail(_email.text)) return;

        bool success = await _firebaseManager.Login(_email.text, _password.text);
        if (success)
        {
            _loginSuccess?.Invoke();
            _loginManager.UpdateUserInforUI();
        }
        else
        {
            Debug.LogWarning("Login failed.");
        }
    }

    public void Logout()
    {
        _firebaseManager.Logout();
    }


    private bool IsEmailOrPasswordEmpty(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogWarning("Email and password cannot be empty.");
            return true;
        }
        return false;
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            Debug.LogWarning("Invalid email format.");
            return false;
        }
    }
}