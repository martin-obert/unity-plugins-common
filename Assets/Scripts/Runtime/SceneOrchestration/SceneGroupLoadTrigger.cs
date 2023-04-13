using UnityEngine;

namespace Obert.Common.Runtime.SceneOrchestration
{
    public class SceneGroupLoadTrigger : MonoBehaviour
    {
        [SerializeField]
        private SceneGroup group;

        public void Trigger()
        {
            SceneGroupManagerFacade.Instance.LoadGroup(group);
        }
    }
}