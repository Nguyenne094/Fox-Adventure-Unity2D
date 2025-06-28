using UnityEngine;
using TMPro;
using Nguyen.Event;

public class UserInforCreation : MonoBehaviour
{
    [SerializeField] private TMP_InputField userNameInputField;
    public StringEventChannelSO onAccountCreated;


    private void CreateUserInFirebase(string email, string password)
    {
        // Simulate Firebase user creation
        Debug.Log($"Creating user with Email: {email}, Password: {password}");

        // After successful creation, invoke the event
        onAccountCreated.RaiseEvent(userNameInputField.text);
    }
}