using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
    public class Trigger<T> : MonoBehaviour where T : MonoBehaviour
    {
        public event UnityAction<T> Enter;
        public event UnityAction<T> Stay;
        public event UnityAction<T> Exit;

        private Collider _collider;
        private List<KeyValuePair<Collider, T>> _enteredObjects;

        private void Awake()
        {
            _enteredObjects = new List<KeyValuePair<Collider, T>>();

            _collider = GetComponent<Collider>();
            _collider.isTrigger = true;
        }

        private void Update()
        {
            for (int i = _enteredObjects.Count - 1; i >= 0; i--)
            {
                if (_enteredObjects[i].Key == null)
                    _enteredObjects.RemoveAt(i);
                else if (_enteredObjects[i].Key.enabled == false)
                    OnTriggerExit(_enteredObjects[i].Key);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out T triggered))
            {
                _enteredObjects.Add(new KeyValuePair<Collider, T>(other, triggered));
                Enter?.Invoke(triggered);

                OnEnter(triggered);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent(out T triggered))
            {
                Stay?.Invoke(triggered);
                OnStay(triggered);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out T triggeredObject))
            {
                _enteredObjects.Remove(new KeyValuePair<Collider, T>(other, triggeredObject));
                Exit?.Invoke(triggeredObject);

                OnExit(triggeredObject);
            }
        }

        public void Enable()
        {
            _collider.enabled = true;
        }

        public void Disable()
        {
            _collider.enabled = false;

            foreach (var triggered in _enteredObjects)
                Exit?.Invoke(triggered.Value);

            _enteredObjects.Clear();
        }

        protected virtual void OnEnter(T triggered) { }
        protected virtual void OnStay(T triggered) { }
        protected virtual void OnExit(T triggered) { }
    }
