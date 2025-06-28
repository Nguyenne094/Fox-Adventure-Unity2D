using UnityEditor;
using UnityEngine;
using Firebase.Auth;

namespace Firebase.Editor
{
    [CustomEditor(typeof(FirebaseManager))]
    public class FirebaseManagerEditor : UnityEditor.Editor
    {
        private FirebaseManager _firebaseManager;

        public override void OnInspectorGUI()
        {
            _firebaseManager = (FirebaseManager)target;

            // Display user status
            GUILayout.Label("User Status:", EditorStyles.boldLabel);
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;
            var currentUser = auth.CurrentUser;

            if (currentUser != null)
            {
                GUILayout.Label($"User logged in:\nUID: {currentUser.UserId}\nEmail: {currentUser.Email}");
            }
            else
            {
                GUILayout.Label("No user currently logged in.");
            }

            // Refresh button
            if (GUILayout.Button("Refresh User Status"))
            {
                Repaint();
            }

            // Logout button
            if (GUILayout.Button("Logout"))
            {
                _firebaseManager.Logout();
                Repaint();
            }

            // Draw default inspector
            DrawDefaultInspector();
        }
    }
}