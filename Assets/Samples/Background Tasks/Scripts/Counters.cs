using System;
using System.Linq;
using System.Threading;
using Obert.Common.Runtime.Extensions;
using Obert.Common.Runtime.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Samples.Background_Tasks.Scripts
{
    public class Counters : MonoBehaviour
    {
        [SerializeField] private TMP_Text[] counters;
        [SerializeField] private int countTo = 10;

        private CancellationTokenSource _cancellationTokenSource;

        [SerializeField] private UnityEvent tasksCompleted;

        private void Awake()
        {
            counters.ThrowIfEmptyOrNull();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Serial()
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
                _cancellationTokenSource.Cancel();

            _cancellationTokenSource = new CancellationTokenSource();

            var backgroundTasks = counters
                .Select(x => new CounterTask(countTo, x))
                .Cast<BackgroundTask>()
                .ToArray();

            TaskScheduler.Instance.RunTasks(backgroundTasks, () => Debug.Log("All counters are done"),
                _cancellationTokenSource.Token);
        }

        public void Parallel()
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
                _cancellationTokenSource.Cancel();

            _cancellationTokenSource = new CancellationTokenSource();

            var backgroundTasks = counters.Select(x => new CounterTask(countTo, x)).ToArray();

            foreach (var counter in backgroundTasks)
            {
                TaskScheduler.Instance.RunTask(counter, () => Debug.Log($"Counter {counter} complete"),
                    _cancellationTokenSource.Token);
            }
        }
    }
}