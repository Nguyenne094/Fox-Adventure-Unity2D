using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSceneGroupDataName", menuName = "SceneGroupData/Create NewSceneGroupData", order = 0)]
public class SceneGroupDataSO : ScriptableObject
{
    public List<SceneData> Scenes = new();
    
    public SceneData GetSceneByType(SceneType type)
    {
        return Scenes.Find(scene => scene.SceneType == type);
    }
    
    public SceneData GetSceneByName(string name)
    {
        return Scenes.Find(scene => scene.SceneName == name);
    }
}