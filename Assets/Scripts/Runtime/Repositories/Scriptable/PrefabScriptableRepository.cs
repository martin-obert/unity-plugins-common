using Obert.Common.Runtime.Repositories.Components;
using UnityEngine;

namespace Obert.Common.Runtime.Repositories.Scriptable
{
    [CreateAssetMenu(menuName = "Repositories/Prefab Repository", fileName = "Prefab Repository", order = 0)]
    public class PrefabScriptableRepository : ScriptableObjectsRepository<IdentifyablePrefab>
    {

    }
}