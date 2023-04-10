using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;

public class GameMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private CanvasFader _panel;

    [Header("Buttons")]
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _exitButton;

    public event UnityAction PanelHided;
    public event UnityAction GameExit;

    private new void OnEnable()
    {
        base.OnEnable();
        _continueButton.onClick.AddListener(OnContinueButtonClick);
        _exitButton.onClick.AddListener(OnExitButtonClick);
    }

    private new void OnDisable()
    {
        base.OnDisable();   
        _continueButton.onClick.RemoveListener(OnContinueButtonClick);
        _exitButton.onClick.RemoveListener(OnExitButtonClick);
    }

    public void ShowGameMenu()
    {
        _panel.Show();
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnContinueButtonClick()
    {
        _panel.Hide();
        Cursor.lockState = CursorLockMode.Locked;
        PanelHided?.Invoke();
    }

    private void OnExitButtonClick() 
    {
        GameExit?.Invoke();
    }
}
