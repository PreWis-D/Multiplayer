using UnityEngine;
using UnityEngine.Events;

public class KillAnimationEvent : MonoBehaviour
{
    public event UnityAction Invoked;

    public void Kill()
    {
        Invoked?.Invoke();
    }
}
