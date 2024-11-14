using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvent : MonoBehaviour
{
    public static Action<GameObject, string> characterDamaged;
    public static Action<GameObject, string> characterHealed;
    public static Action characterMoveLeft;
    public static Action characterMoveRight;
}
