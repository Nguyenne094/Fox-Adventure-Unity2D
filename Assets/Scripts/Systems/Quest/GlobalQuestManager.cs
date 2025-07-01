using UnityEngine;
using Utils;

namespace Quest
{
    public class GlobalQuestManager : Singleton<GlobalQuestManager>
    {
        private FirebaseQuestManager firebaseQuestManager;

        public override void Awake() {
            firebaseQuestManager = FirebaseQuestManager.Instance;
            firebaseQuestManager.LoadProgress();
        }
    }
}