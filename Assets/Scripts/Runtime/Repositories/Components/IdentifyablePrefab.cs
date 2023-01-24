using UnityEngine;

namespace Obert.Common.Runtime.Repositories.Components
{
    public class IdentifyablePrefab : MonoBehaviour, IPrefabIdentifier
    {
        [SerializeField] private string id;

        public string Id => id;
    }
}