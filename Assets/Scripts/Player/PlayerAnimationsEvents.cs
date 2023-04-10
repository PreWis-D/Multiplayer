using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationsEvents : MonoBehaviour
{
    public event UnityAction Throwed;

    public void Throw()
    {
        Throwed?.Invoke();
        print(nameof(Throw));
    }
}
