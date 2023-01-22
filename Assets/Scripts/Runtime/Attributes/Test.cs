using UnityEngine;

namespace Obert.Common.Runtime.Attributes
{
    public class Test : MonoBehaviour
    {
        [SceneRef]
        public string SceneName;
        
        [Tag]
        public string tag;
    }
}