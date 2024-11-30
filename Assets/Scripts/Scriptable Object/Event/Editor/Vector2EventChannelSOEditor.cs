using UnityEngine;
using UnityEditor;
using Nguyen.Event;

namespace Nguyen.Event
{
    /// <summary>
    /// Editor script to add a custom Inspector to the Vector2EventChannelSO. This uses a custom
    /// ListView to show all subscribed listeners.
    /// </summary>
    [CustomEditor(typeof(Vector2EventChannelSO))]
    public class Vector2EventChannelSOEditor : GenericEventChannelSOEditor<Vector2>
    {

    }
}
