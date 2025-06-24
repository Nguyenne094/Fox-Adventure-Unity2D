using UnityEditor;
using UnityEngine.UIElements;
using Firebase.Auth;
using UnityEngine;

namespace Firebase.Editor
{
    [CustomEditor(typeof(FirebaseManager))]
    public class FirebaseManagerEditor : UnityEditor.Editor
    {
        private Label _userStatusLabel;
        private FirebaseManager _firebaseManager;

        public override VisualElement CreateInspectorGUI()
        {
            _firebaseManager = (FirebaseManager)target;

            var root = new VisualElement();
            
            var checkbox = new Toggle("Persist");
            checkbox.value = false;

            checkbox.RegisterValueChangedCallback(evt =>
            {
                _firebaseManager.Persist = evt.newValue;
            });

            root.Add(checkbox);

            _userStatusLabel = new Label("Checking user status...");
            root.Add(_userStatusLabel);

            // Refresh user status when the inspector is opened
            UpdateUserStatus();

            // Refresh button
            var refreshButton = new Button(() => UpdateUserStatus())
            {
                text = "Refresh User Status"
            };
            root.Add(refreshButton);

            // Logout button
            var logoutButton = new Button(() =>
            {
                _firebaseManager.Logout();
                UpdateUserStatus();
            })
            {
                text = "Logout"
            };
            root.Add(logoutButton);

            // Add the default inspector below
            var defaultInspector = base.CreateInspectorGUI();
            root.Add(defaultInspector);

            return root;
        }

        private void UpdateUserStatus()
        {
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;
            var currentUser = auth.CurrentUser;

            if (currentUser != null)
            {
                _userStatusLabel.text = $"User logged in:\nUID: {currentUser.UserId}\nEmail: {currentUser.Email}";
            }
            else
            {
                _userStatusLabel.text = "No user currently logged in.";
            }
        }
    }
}