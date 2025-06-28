using Firebase;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class LoginController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FirebaseManager _firebaseManager;
        [SerializeField] private TMP_InputField _email;
        [SerializeField] private TMP_InputField _password;
        [SerializeField] private TMP_InputField _userName;
        [SerializeField, Tooltip("For Register Only")] private TMP_InputField _confirmPassword;

        [Header("Events")]
        [SerializeField] private UnityEvent _registerSuccess;
        [SerializeField] private UnityEvent _loginSuccess;


        public async void Register()
        {
            IsEmailOrPasswordEmpty(_email.text, _password.text);

            // Validate the email format
            if (!IsValidEmail(_email.text))
            {
                Debug.LogWarning("Invalid email format.");
                return;
            }

            if (_confirmPassword.text == _password.text)
            {
                try
                {
                    await _firebaseManager.Register(_email.text, _password.text, _userName.text);
                    _registerSuccess.Invoke();
                }
                catch (FirebaseException e)
                {
                    Debug.LogError($"Firebase registration failed: {e.Message}");
                }
            }
            else
            {
                Debug.LogWarning("Passwords do not match");
            }
        }

        public async void Login()
        {
            IsEmailOrPasswordEmpty(_email.text, _password.text);

            // Validate the email format
            if (!IsValidEmail(_email.text))
            {
                Debug.LogWarning("Invalid email format.");
                return;
            }

            try
            {
                await _firebaseManager.Login(_email.text, _password.text);
                _loginSuccess.Invoke();
            }
            catch (FirebaseException e)
            {
                Debug.LogError($"Firebase login failed: {e.Message}");
            }
        }

        private bool IsEmailOrPasswordEmpty(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(email))
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
}