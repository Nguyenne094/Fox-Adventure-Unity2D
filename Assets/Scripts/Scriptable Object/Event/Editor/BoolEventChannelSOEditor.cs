using UnityEditor;
using Nguyen.Event;

namespace Nguyen.Event
{
    [CustomEditor(typeof(BoolEventChannelSO))]
    public class BoolEventChannelSOEditor : GenericEventChannelSOEditor<bool>
    {
    }
}
