using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CellView : MonoBehaviour
{
    [SerializeField] private SettingCellPanel _buttonSettingsPanel;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private TMP_Text _countText;

    private Button _button;
    //private Image _icon;
    private CollectableObject _item;
    private string _tittle;

    public int Count { get; private set; } = 0;
    public CollectableObject CollectableObject => _item;

    public event UnityAction<CellView> ButtonSettingPanelClicked;
    public event UnityAction<CellView> ItemDroped;
    public event UnityAction<CellView> ItemUsed;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnClick);
        _buttonSettingsPanel.UseButtonClicked += OnUseButtonClicked;
        _buttonSettingsPanel.DropButtonClicked += OnDropButtonClicked;
        _buttonSettingsPanel.InfoButtonClicked += OnInfoButtonClicked;
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnClick);
        _buttonSettingsPanel.UseButtonClicked -= OnUseButtonClicked;
        _buttonSettingsPanel.DropButtonClicked -= OnDropButtonClicked;
        _buttonSettingsPanel.InfoButtonClicked -= OnInfoButtonClicked;
    }

    public void Init(/*Image image, */string text, CollectableObject item, int count)
    {
        //_icon = image;
        _tittle = text;
        _item = item;

        _text.text = _tittle;

        if (count > -1)
        {
            _countText.text = "x" + count.ToString();
            Count = count;
        }

    }

    public void ChangeCount(CollectableObject collectableObject)
    {
        if (collectableObject.Count < 0)
            return;

        _countText.text = "x" + collectableObject.Count.ToString();
        Count = collectableObject.Count;
    }

    private void OnClick()
    {
        ButtonSettingPanelClicked?.Invoke(this);
    }

    public void ShowSettingPanel()
    {
        _buttonSettingsPanel.Show();
    }

    public void HideSettingPanel()
    {
        _buttonSettingsPanel.Hide();
    }

    private void OnUseButtonClicked()
    {
        ItemUsed?.Invoke(this);
    }

    private void OnDropButtonClicked()
    {
        ItemDroped?.Invoke(this);
    }

    private void OnInfoButtonClicked()
    {
        Debug.Log("info");
    }
}
