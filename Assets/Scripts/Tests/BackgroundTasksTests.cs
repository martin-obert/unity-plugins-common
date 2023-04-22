using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using NUnit.Framework;
using Obert.Common.Runtime.Tasks;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BackgroundTasksTests
    {
        private class BackgroundTaskMock : IBackgroundTask
        {
            private readonly TimeSpan _delay;

            public BackgroundTaskMock(string id, TimeSpan delay)
            {
                _delay = delay;
                ID = id;
            }

            public UniTask Execute(CancellationToken cancellationToken = default) =>
                UniTask.Delay(_delay, cancellationToken: cancellationToken);

            public string ID { get; }
        }


        [UnityTest]
        public IEnumerator TaskSchedulerFacade_RunTest_Pass()
        {
            var id = Guid.NewGuid().ToString();
            new GameObject("TaskSchedulerFacade").AddComponent<TaskSchedulerFacade>();
            var completed = new List<IBackgroundTask>();

            yield return new WaitForFixedUpdate();

            TaskSchedulerFacade.Instance.RunTask(Guid.NewGuid().ToString(), tasks => completed.AddRange(tasks), default,
                new BackgroundTaskMock(id, TimeSpan.FromMilliseconds(500)));

            yield return new WaitForSeconds(1);

            Assert.IsNotEmpty(completed);
            Assert.AreEqual(id, completed.First().ID);
        }

        [UnityTest]
        public IEnumerator TaskSchedulerFacade_BackgroundTask_CompleteCallback_Pass()
        {
            var id = Guid.NewGuid().ToString();
            new GameObject("TaskSchedulerFacade").AddComponent<TaskSchedulerFacade>();
            var completed = new List<IBackgroundTask>();

            yield return new WaitForFixedUpdate();

            TaskSchedulerFacade.Instance.RunTask(Guid.NewGuid().ToString(), tasks =>
                {
                    Assert.IsNotEmpty(tasks);
                    Assert.AreEqual(id, tasks.First().ID);
                    completed.AddRange(tasks);
                }, default,
                new BackgroundTaskMock(id, TimeSpan.FromMilliseconds(500)));

            yield return new WaitForSeconds(1);

            Assert.IsNotEmpty(completed);
            Assert.AreEqual(id, completed.First().ID);
        }

        [UnityTest]
        public IEnumerator TaskSchedulerFacade_RunTasks_Pass()
        {
            new GameObject("TaskSchedulerFacade").AddComponent<TaskSchedulerFacade>();
            yield return new WaitForFixedUpdate();
            var completed = new List<IBackgroundTask>();
           
            const int taskCount = 10;

            var ids = Enumerable.Range(0, taskCount).Select(x => x.ToString()).ToArray();

            var backgroundTasks = Enumerable.Range(0, ids.Length)
                .Select(id => new BackgroundTaskMock(id.ToString(), TimeSpan.FromMilliseconds(500))).ToArray();
            TaskSchedulerFacade.Instance.RunTasks(Guid.NewGuid().ToString(), tasks =>
                {
                    AssertTasksIds(tasks, taskCount, ids);
                    completed.AddRange(tasks);
                }, default,
                backgroundTasks);

            yield return new WaitForSeconds(1);
            AssertTasksIds(completed, taskCount, ids);
        }

        private static void AssertTasksIds(IReadOnlyList<IBackgroundTask> tasks, int taskCount, IReadOnlyList<string> ids)
        {
            Assert.IsNotEmpty(tasks);
            Assert.AreEqual(taskCount, tasks.Count);
            for (var index = 0; index < ids.Count; index++)
            {
                Assert.AreEqual(ids[index], tasks[index].ID);
            }
        }
    }
}