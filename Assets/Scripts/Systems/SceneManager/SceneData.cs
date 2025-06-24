using System;
using Eflatun.SceneReference;

[Serializable]
public class SceneData
{
    public SceneReference Scene;
    public string SceneName => Scene.Name;
    public SceneType SceneType;
}

public enum SceneType
{
    Active,
    UI,
    Base,
    Additive
}