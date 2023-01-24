using System;
using System.Collections;
using System.Threading;
using Obert.Common.Runtime.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Samples.Background_Tasks.Scripts
{
    public class FetchPlayersBeforeBegin : MonoBehaviour
    {
        [SerializeField] private UnityEvent onComplete;

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

            public override IEnumerator Execute(CancellationToken cancellationTokenSource = default)
            {
                var request = UnityWebRequestTexture.GetTexture(_url);

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    OnError(new Exception($"{request.result} = {request.responseCode} - {request.error}"));
                    yield break;
                }

                yield return new WaitForSecondsRealtime(_waitDelay);

                var imageSprite = ((DownloadHandlerTexture)request.downloadHandler).texture;
                _image.sprite = Sprite.Create(imageSprite, new Rect(0, 0, imageSprite.width, imageSprite.height),
                    Vector2.one / 2);

                while (_image.color.a <= 1)
                {
                    _image.color = new Color(1, 1, 1, Mathf.Clamp01(_image.color.a + Time.deltaTime));
                    yield return new WaitForEndOfFrame();
                }

                OnComplete();
            }
        }

        [SerializeField] private string avatarUrl = "https://i.pravatar.cc/300";

        private void Start()
        {
            foreach (var image in images)
            {
                TaskScheduler.Instance.RunTasks(new AvatarFetch(avatarUrl, image) { Error = Debug.LogException });
            }
        }
    }
}