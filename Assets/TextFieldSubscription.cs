using System;
using Obert.Common.Runtime.Extensions;
using TMPro;
using UnityEngine;

public class TextFieldSubscription : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    private IDisposable _subscription;
    
    private void OnEnable()
    {
        _subscription = inputField.onValueChanged.Subscribe(Debug.Log);
    }

    private void OnDisable()
    {
        _subscription?.Dispose();
    }
}