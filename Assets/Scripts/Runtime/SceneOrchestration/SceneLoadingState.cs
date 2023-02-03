using System;

namespace Obert.Common.Runtime.SceneOrchestration
{
    [Serializable]
    public class SceneLoadingState
    {
        public Action<float> OnProgress { get; set; }
        public  Action OnComplete { get; set; }
    }
}