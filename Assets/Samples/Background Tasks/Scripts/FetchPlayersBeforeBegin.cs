using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Obert.Common.Runtime.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Samples.Background_Tasks.Scripts
{
    public class FetchPlayersBeforeBegin : MonoBehaviour
    {
        [SerializeField] private Image[] images;

        private class AvatarFetch : BackgroundTask
        {
            private readonly string _url;
            private readonly float _waitDelay;
            private readonly Image _image;

            public AvatarFetch(string url, Image image)
            {
                _image = image;
                _url = url;
                _waitDelay = Random.Range(0f, 5f);
            }

            public override async UniTask Execute(CancellationToken cancellationTokenSource = default)
            {
                try
                {
                    var request = await UnityWebRequestTexture.GetTexture(_url).SendWebRequest()
                        .ToUniTask(cancellationToken: cancellationTokenSource);

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        throw new Exception($"Status Code: {request.responseCode} - {request.error}");
                    }

                    await UniTask.Delay(TimeSpan.FromSeconds(_waitDelay), cancellationToken: cancellationTokenSource);

                    var imageSprite = ((DownloadHandlerTexture)request.downloadHandler).texture;
                    _image.sprite = Sprite.Create(imageSprite, new Rect(0, 0, imageSprite.width, imageSprite.height),
                        Vector2.one / 2);

                    while (_image.color.a < 1)
                    {
                        _image.color = new Color(1, 1, 1, Mathf.Clamp01(_image.color.a + Time.deltaTime));
                        await UniTask.NextFrame(cancellationTokenSource);
                    }

                    Debug.Log("Completed");
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        [SerializeField] private string avatarUrl = "https://i.pravatar.cc/300";
        private IBackgroundTaskRunner _runner;

        private void Start()
        {
            _runner = TaskSchedulerFacade.Instance.RunTasks(() => Debug.Log("All images loaded"),
                CancellationToken.None,
                images.Select(image => new AvatarFetch(avatarUrl, image)).ToArray());
        }
    }
}