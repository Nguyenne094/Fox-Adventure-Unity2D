using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class ButtonController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FirebaseManager _firebaseManager;
        [SerializeField] private TMP_InputField _email;
        [SerializeField] private TMP_InputField _password;
        [SerializeField, Tooltip("For Register Only")] private TMP_InputField _confirmPassword;
        
        [Header("Events")] 
        [SerializeField] private UnityEvent _registerSuccess;
        [SerializeField] private UnityEvent _loginSuccess;
        

        public void Register()
        {
            if(_confirmPassword.text == _password.text)
            {
                if (_firebaseManager.Register(_email.text, _password.text))
                    _registerSuccess.Invoke();
            }
            else
            {
                Debug.LogWarning("Passwords do not match");
            }
        }

        public void Login()
        {
            if (_firebaseManager.Login(_email.text, _password.text))
                _loginSuccess.Invoke();
        }
        
        public async void LoadScene(SceneGroupDataSO sceneGroup)
        {
            if (sceneGroup == null)
            {
                Debug.LogError("SceneGroupData is null");
                return;
            }

            await SceneLoader.Instance.LoadSceneGroup(sceneGroup).ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Failed to load scene group: " + task.Exception);
                }
            });
            await Task.Yield();
        }
    }
}