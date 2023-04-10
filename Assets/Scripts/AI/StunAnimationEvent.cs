using UnityEngine;
using UnityEngine.Events;

public class StunAnimationEvent : MonoBehaviour
{
    public event UnityAction StunFinished;

    public void FinishStun()
    {
        StunFinished?.Invoke();
    }
}
