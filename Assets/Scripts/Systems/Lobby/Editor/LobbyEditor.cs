using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LobbyManager))]
public class LobbyEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        LobbyManager lobbyManager = (LobbyManager)target;

        if (GUILayout.Button("Initialize Services")) {
            lobbyManager.InitializationOptions();
        }

        if (GUILayout.Button("Sign In"))
        {
            lobbyManager.SignIn();
        }
        if (GUILayout.Button("Login (Anonymous)"))
        {
            lobbyManager.Login();
        }
        if (GUILayout.Button("Create Lobby"))
        {
            lobbyManager.CreateLobby();
        }
        if (GUILayout.Button("List Lobbies"))
        {
            lobbyManager.ListLobbies();
        }
        if (GUILayout.Button("Join Lobby (by Code)"))
        {
            string code = EditorUtility.DisplayDialogComplex("Join Lobby", "Enter Lobby Code in Console", "OK", "Cancel", "").ToString();
            // Thực tế nên dùng EditorGUILayout.TextField để nhập code, demo đơn giản:
            lobbyManager.JoinLobby(code);
        }
    }
}