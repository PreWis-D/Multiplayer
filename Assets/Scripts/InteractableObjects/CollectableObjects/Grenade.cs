using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : CollectableObject
{
    [SerializeField] private AudioSource _audioSource;

    [Header("Setting")]
    [SerializeField] private int _grenadeThrowPower = 6;
    [SerializeField] private float _radius;
    [SerializeField] private float _delay;

    public bool IsPreparation { get; private set; } = false;

    public void OnThrowCanceled()
    {
        ThrowPower = 0;
        IsPreparation = false;
    }

    public void ChangeThrowPower()
    {
        ThrowPower = _grenadeThrowPower;
        IsPreparation = true;
    }

    public void StartTimer()
    {
        StartCoroutine(DelayExplode(_delay));
        IsPreparation = false;
    }

    private IEnumerator DelayExplode(float delay)
    {
        yield return new WaitForSeconds(delay);

        Explode();
    }

    private void Explode()
    {
        Debug.Log("Explode");
    }
}
