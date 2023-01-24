using System;
using System.Collections;
using System.Threading;
using Obert.Common.Runtime.Tasks;
using TMPro;
using UnityEngine;

namespace Samples.Background_Tasks.Scripts
{
    public class CounterTask : BackgroundTask
    {
        private readonly int _countTo;
        private readonly TMP_Text _label;

        public CounterTask(int countTo, TMP_Text label)
        {
            _countTo = countTo;
            _label = label;
        }

        public override IEnumerator Execute(CancellationToken cancellationTokenSource = default)
        {
            var startTime = Time.timeSinceLevelLoad;
            var timeUp = startTime + _countTo;

            while (Time.timeSinceLevelLoad < timeUp)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    _label.text = 0f.ToString("00");
                    OnError(new OperationCanceledException("IsCancellationRequested"));
                    break;
                }
                
                _label.text = $"{Time.timeSinceLevelLoad - startTime:00}";
                yield return new WaitForEndOfFrame();
            }
            
            OnSuccess();
            OnComplete();
        }
    }
}