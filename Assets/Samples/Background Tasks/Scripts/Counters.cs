using System;
using System.Linq;
using System.Threading;
using Obert.Common.Runtime.Attributes;
using Obert.Common.Runtime.Extensions;
using Obert.Common.Runtime.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Samples.Background_Tasks.Scripts
{
    public class Counters : MonoBehaviour
    {
        [Serializable]
        public class CountersIds
        {
            [Id] [SerializeField] private string id;
            public string Id => id;
        }

        public CountersIds[] ids;
        [SerializeField] private TMP_Text[] counters;
        [SerializeField] private int countTo = 10;

        private CancellationTokenSource _cancellationTokenSource;

        [SerializeField] private UnityEvent tasksCompleted;

        private void Awake()
        {
            counters.ThrowIfEmptyOrNull();
            _cancellationTokenSource = new CancellationTokenSource();
        }


        public void Parallel()
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
                _cancellationTokenSource.Cancel();

            _cancellationTokenSource = new CancellationTokenSource();

            var backgroundTasks = counters.Select(x => new CounterTask(countTo, x)).ToArray();
            _runner?.Dispose();
            _runner = TaskSchedulerFacade.Instance.RunTasks(() => Debug.Log($"All counters complete"),
                _cancellationTokenSource.Token,
                backgroundTasks);
        }

        private IBackgroundTaskRunner _runner;
    }
}