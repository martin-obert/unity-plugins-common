using System;

namespace Obert.Common.Runtime.SceneOrchestration
{
    [Serializable]
    public sealed class SceneLoadingState
    {
        public SceneLoadingState(float oneStep)
        {
            _oneStep = oneStep;
        }

        private float _oneStep;
        private float _progress;
                
        public void ProgressIn()
        {
            _progress += _oneStep;
            OnProgress?.Invoke(_progress);
            if(_progress>=1) OnComplete?.Invoke();
        }
        
        public Action<float> OnProgress { get; set; }
        public  Action OnComplete { get; set; }
    }
}