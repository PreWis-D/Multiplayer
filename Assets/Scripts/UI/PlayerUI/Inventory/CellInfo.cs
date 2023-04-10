using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Cell", menuName = "Cell/Create new Cell")]
public class CellInfo : ScriptableObject
{
    [SerializeField] private string _name;
    //[SerializeField] private Image _icon;
    [SerializeField] private CollectableObject _item;

    public string Name => _name;
    //public Image Icon => _icon;
    public CollectableObject Item => _item;
}
