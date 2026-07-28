using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmWindow : MonoBehaviour
{
    public TextMeshProUGUI confText;
    public Button confirmButton;

    private Action onConfirm;

    private void Awake()
    {
        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(HandleConfirm);
        }
    }

    public void Init(string message, Action confirmAction)
    {
        confText.text = message;
        onConfirm = confirmAction;
    }

    private void HandleConfirm()
    {
        onConfirm?.Invoke();
        Destroy(gameObject);
    }
}
