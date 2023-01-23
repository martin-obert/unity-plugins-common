using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obert.Common.Runtime.Repositories
{
    public class IdentifyablePrefab : MonoBehaviour, IPrefabIdentifier
    {
        [SerializeField] private string id;

        public string Id => id;
    }
}