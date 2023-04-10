using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryView : Panel
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private CellView _cellPrefab;
    [SerializeField] private Transform _cellsContainer;

    private List<CellView> _cellViews = new List<CellView>();
    private CellView _currentCellView;

    public event UnityAction Hided;

    private new void OnEnable()
    {
        base.OnEnable();
        _inventory.ItemAdded += OnItemAdded;
        _inventory.ItemRemoved += OnItemRemoved;
        _inventory.ItemCountChanged += OnItemCountChanged;
    }

    private new void OnDisable()
    {
        base.OnDisable();
        _inventory.ItemAdded -= OnItemAdded;
        _inventory.ItemRemoved -= OnItemRemoved;
        _inventory.ItemCountChanged -= OnItemCountChanged;
    }

    public new void Show()
    {
        base.Show();
        _inventory.ChangeInventoryViewOpenedState(true);
    }

    public new void Hide()
    {
        if (_currentCellView != null)
        {
            _currentCellView.HideSettingPanel();
            _inventory.ChangeInventoryViewOpenedState(false);
            _currentCellView = null;
        }

        Hided?.Invoke();
        base.Hide();
    }

    private void OnItemAdded(CollectableObject collectable)
    {
        if (_cellViews.Count > 0)
        {
            foreach (var cellView in _cellViews)
            {
                if (cellView.CollectableObject.Key == collectable.Key)
                {
                    TryShowCount(collectable, cellView);
                    return;
                }
            }
        }

        CellView cell = Instantiate(_cellPrefab, _cellsContainer);
        cell.Init(collectable.Cell.Name, collectable.Cell.Item, collectable.Count);
        cell.ButtonSettingPanelClicked += OnButtonSettingPanelClicked;
        cell.ItemDroped += OnItemDropedButtonClick;
        cell.ItemUsed += OnItemUsed;
        _cellViews.Add(cell);
    }

    private void OnItemRemoved(CollectableObject collectableObject)
    {
        foreach (var item in _cellViews)
        {
            if (item.CollectableObject.Key == collectableObject.Key)
            {
                DestroyObject(item);
                break;
            }
        }
    }

    private void OnItemCountChanged(CollectableObject collectable)
    {
        foreach (var item in _cellViews)
        {
            if (item.CollectableObject.Key == collectable.Key)
            {
                TryShowCount(collectable, item);
                break;
            }
        }
    }

    private void OnItemDropedButtonClick(CellView cellView)
    {
        _inventory.TryDropItem(cellView.CollectableObject);
    }

    private void DestroyObject(CellView cellView)
    {
        _cellViews.Remove(cellView);
        cellView.ButtonSettingPanelClicked -= OnButtonSettingPanelClicked;
        cellView.ItemDroped -= OnItemDropedButtonClick;
        cellView.ItemUsed -= OnItemUsed;
        Destroy(cellView.gameObject);
    }

    private void OnItemUsed(CellView cellView)
    {
        _inventory.TryItemUsed(cellView.CollectableObject);
        Hide();
    }

    private void OnButtonSettingPanelClicked(CellView cellView)
    {
        if (_currentCellView == null)
        {
            _currentCellView = cellView;
            _currentCellView.ShowSettingPanel();
        }
        else if (_currentCellView == cellView)
        {
            _currentCellView.HideSettingPanel();
            _currentCellView = null;
        }
        else
        {
            _currentCellView.HideSettingPanel();
            _currentCellView = cellView;
            _currentCellView.ShowSettingPanel();
        }
    }

    private void TryShowCount(CollectableObject item, CellView cellView)
    {
        cellView.ChangeCount(item);
    }
}
