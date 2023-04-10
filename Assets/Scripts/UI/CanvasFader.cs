using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasFader : MonoBehaviour
{
    [SerializeField] private float _duration;

    private CanvasGroup _canvasGroup;
    private Tween _fadeAction;

    public event UnityAction Showed;
    public event UnityAction Hided;

    private void Awake()
    {
        _canvasGroup= GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
        KillCurrentFade();
        _fadeAction = _canvasGroup?.DOFade(1, _duration);
        Showed?.Invoke();
    }

    public void Hide()
    {
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
        KillCurrentFade();
        _fadeAction = _canvasGroup?.DOFade(0, _duration);
        Hided?.Invoke();
    }

    private void KillCurrentFade()
    {
        _fadeAction?.Kill();
    }
}
