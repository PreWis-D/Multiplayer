using JetBrains.Annotations;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviourPunCallbacks
{
    [SerializeField] private string _description;

    public string Description => _description;
    public event UnityAction Executed;
    
    public virtual void Execute()
    {
        Executed?.Invoke();
    }
}
