using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(CanvasFader))]
public abstract class Panel : MonoBehaviourPunCallbacks
{
    private CanvasFader _canvasFader;

    private void Awake()
    {
        _canvasFader= GetComponent<CanvasFader>();
    }

    public void Show()
    {
        _canvasFader.Show();
    }

    public void Hide()
    {
        _canvasFader.Hide();
    }
}
