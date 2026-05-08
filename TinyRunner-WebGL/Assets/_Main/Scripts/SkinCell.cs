using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkinCell : MonoBehaviour, IPointerClickHandler
{
    public event Action<SkinCell> OnClick;

    public Skin SkinData => _skinData;
    public bool IsUnlocked => _isUnlocked;
    public bool IsEquipped => _isEquipped;

    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _priceText;

    private bool _isUnlocked;
    private bool _isEquipped;

    private Skin _skinData;

    public void Initialize(Skin skinData)
    {
        _skinData = skinData;

        _icon.sprite = _skinData.Sprite;

        UpdateUI();
    }

    public void Unlock()
    {
        if (_isUnlocked) return;

        _isUnlocked = true;
        UpdateUI();
    }

    public void Equip()
    {
        if (!_isUnlocked || _isEquipped) return;

        _isEquipped = true;
        UpdateUI();
    }

    public void Unequip()
    {
        if (!_isEquipped) return;

        _isEquipped = false;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (_isUnlocked && !_isEquipped)
        {
            _priceText.SetText("Select");
        }
        else if (_isUnlocked && _isEquipped)
        {
            _priceText.SetText("Selected");
        }
        else if (!_isUnlocked)
        {
            _priceText.SetText(_skinData.Price.ToString());
        }
    }

    public void OnPointerClick(PointerEventData eventData) => OnClick?.Invoke(this);
}