using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard : CollectableObject
{
    [SerializeField] private int _accessLevel;

    public int AccessLevel => _accessLevel;
}
