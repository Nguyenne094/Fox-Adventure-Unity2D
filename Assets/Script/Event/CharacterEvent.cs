using UnityEngine;
using UnityEngine.Events;

public class CharacterEvent : MonoBehaviour
{
    public static UnityAction<GameObject, string> characterDamaged;
    public static UnityAction<GameObject, string> characterHealed;
}
