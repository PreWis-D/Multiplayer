using UnityEngine;
using UnityEngine.Events;

public class RunAnimationEvent : MonoBehaviour
{
    public event UnityAction DashCompleted;
    public event UnityAction DashStarted;


    public void DashStart()
    {
        DashStarted?.Invoke();
    }
    
    public void DashComplete()
    {
        DashCompleted?.Invoke();
    }
}
