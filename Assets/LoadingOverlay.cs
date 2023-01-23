using System;
using Obert.Common.Runtime.SceneOrchestration;
using TMPro;
using UnityEngine;

public class LoadingOverlay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text progress;
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void OnProgress(SceneLoadingState state)
    {
        gameObject.SetActive(true);
        state.OnComplete = () => gameObject.SetActive(false);
        state.OnProgress = f => progress.text = $"{Math.Floor(f * 100)} %";
    }
}