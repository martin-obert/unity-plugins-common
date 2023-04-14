using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Obert.Common.Runtime.Tasks;
using TMPro;
using UnityEngine;

namespace Samples.Background_Tasks.Scripts
{
    public class CounterTask : BackgroundTask
    {
        private readonly int _countTo;
        private readonly TMP_Text _label;
        public override string ID { get; }

        public CounterTask(int id, int countTo, TMP_Text label)
        {
            ID = id.ToString();
            _countTo = countTo;
            _label = label;
        }

        public override async UniTask Execute(CancellationToken cancellationTokenSource = default)
        {
            var startTime = Time.timeSinceLevelLoad;
            var timeUp = startTime + _countTo;
            await UniTask.SwitchToMainThread(cancellationTokenSource);
            while (Time.timeSinceLevelLoad < timeUp)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    _label.text = 0f.ToString("00");
                    throw new OperationCanceledException("IsCancellationRequested");
                }
                
                _label.text = $"{Time.timeSinceLevelLoad - startTime:00}";
                await UniTask.NextFrame(cancellationTokenSource);
            }
        }
    }
}