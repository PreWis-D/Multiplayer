using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessagePanel : Panel
{
    [SerializeField] private TMP_Text _text;

    public void ShowInfo(string targetText)
    {
        _text.text = targetText;
    }
}
