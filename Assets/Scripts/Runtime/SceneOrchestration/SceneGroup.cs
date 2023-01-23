using System.Linq;
using UnityEngine;

namespace Obert.Common.Runtime.SceneOrchestration
{
    [CreateAssetMenu(menuName = "Scene Orchestration/Scene Group", fileName = "Scene Group", order = 0)]
    public class SceneGroup : ScriptableObject, ISceneGroup
    {
        [SerializeField] private SceneMetadata[] items;

        public ISceneMetadata[] Items => items.OfType<ISceneMetadata>().ToArray();
    }
}