using Obert.Common.Runtime.Attributes;
using UnityEngine;

namespace Obert.Common.Runtime.SceneOrchestration
{
    [CreateAssetMenu(menuName = "Scene Orchestration/Scene Group", fileName = "Scene Group", order = 0)]
    public class SceneGroups : ScriptableObject, ISceneGroups
    {
        [SerializeField]
        [SceneRef]
        private string sceneName;

        public string SceneName => sceneName;
    }
}