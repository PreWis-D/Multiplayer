using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DetectorCollectable : CollectableObject
{
    [SerializeField] private EnemyTrigger _enemyTrigger;
    [SerializeField] private float _scanFrequency = 60;
    [SerializeField] private AudioSource _audioSource;


    private List<EnemyStateMachine> _enemies;
    public bool IsScanning { get; private set; }

    public float Radius => _enemyTrigger.GetComponent<SphereCollider>().radius;
    public event UnityAction<float> DistanceCalculated;

    private void Awake()
    {
        _enemies = new List<EnemyStateMachine>();
    }

    private new void OnEnable()
    {
        base.OnEnable();
        _enemyTrigger.Enter += OnEnter;
        _enemyTrigger.Exit += OnExit;
    }

    private new void OnDisable()
    {
        base.OnDisable();
        _enemyTrigger.Enter -= OnEnter;
        _enemyTrigger.Exit -= OnExit;
    }

    public void Scan(bool isScanning)
    {
        IsScanning = isScanning;

        if (isScanning)
            StartCoroutine(Scanning());
    }

    private IEnumerator Scanning()
    {
        _audioSource.Play();
        
        while (IsScanning)
        {
            if (_enemies.Count > 0)
            {
                float[] distances = new float[_enemies.Count];
                int index = 0;
                
                foreach (var enemy in _enemies)
                {
                    distances[index] = (enemy.transform.position - transform.position).magnitude;
                    index++;
                }
                
                DistanceCalculated?.Invoke(distances.Min());
            }
            else
            {
                DistanceCalculated?.Invoke(-1);
            }
            yield return new WaitForSeconds(1 / _scanFrequency);
        }
        
        _audioSource.Stop();
    }

    private void OnEnter(EnemyStateMachine enemy)
    {
        if (!_enemies.Contains(enemy))
        {
            Scan(false);
            _enemies.Add(enemy);
            Scan(true);
        }
            
    }

    private void OnExit(EnemyStateMachine enemy)
    {
        if (_enemies.Contains(enemy))
        {
            Scan(false);
            _enemies.Remove(enemy);
            Scan(true);
        }
    }
}
