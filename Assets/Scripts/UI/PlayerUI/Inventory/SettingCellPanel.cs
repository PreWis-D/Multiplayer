using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingCellPanel : Panel
{
    [SerializeField] private Button _useButton;
    [SerializeField] private Button _dropButton;
    [SerializeField] private Button _infoButton;

    public event UnityAction UseButtonClicked;
    public event UnityAction DropButtonClicked;
    public event UnityAction InfoButtonClicked;

    private void OnEnable()
    {
        _useButton.onClick.AddListener(OnClickUseButton);
        _dropButton.onClick.AddListener(OnClickDropButton);
        _infoButton.onClick.AddListener(OnClickInfoButton);
    }

    private void OnDisable()
    {
        _useButton.onClick.RemoveListener(OnClickUseButton);
        _dropButton.onClick.RemoveListener(OnClickDropButton);
        _infoButton.onClick.RemoveListener(OnClickInfoButton);
    }

    private void OnClickUseButton()
    {
        UseButtonClicked?.Invoke();
    }

    private void OnClickDropButton()
    {
        DropButtonClicked?.Invoke();
    }

    private void OnClickInfoButton()
    {
        InfoButtonClicked?.Invoke();
    }
}
