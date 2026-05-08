using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YG;

public class ShopService
{
    private readonly List<Skin> _skinDatas;
    private readonly SkinCell _skinCellPrefab;
    private readonly Transform _cellsParent;
    private readonly PlayerVisual _playerVisual;
    private readonly CoinService _coinService;

    private readonly List<SkinCell> _skinCells = new();
    private SkinCell _currentSelectedCell;

    public ShopService(List<Skin> skinDatas, SkinCell skinCellPrefab, Transform cellsParent, PlayerVisual playerVisual, CoinService coinService)
    {
        _skinDatas = skinDatas;
        _skinCellPrefab = skinCellPrefab;
        _cellsParent = cellsParent;
        _playerVisual = playerVisual;
        _coinService = coinService;
    }

    public void Initialize()
    {
        for (int i = 0; i < _skinDatas.Count; i++)
        {
            SkinCell cell = Object.Instantiate(_skinCellPrefab, _cellsParent);
            cell.Initialize(_skinDatas[i]);
            cell.OnClick += OnClickHandler;
            _skinCells.Add(cell);
            if (cell.SkinData.IsDefaultSkin)
            {
                cell.Unlock();
                cell.Equip();
            }
        }
    }

    public void LoadSkins(List<string> skinNames, string eqipmentSkin)
    {
        SkinCell skin;

        for (int i = 0; i < skinNames.Count; i++)
        {
            skin = _skinCells.FirstOrDefault(s => s.SkinData.name == skinNames[i]);
            skin.Unlock();
        }

        skin = _skinCells.FirstOrDefault(s => s.SkinData.name == eqipmentSkin);
        EquipSkin(skin);
    }

    private void OnClickHandler(SkinCell cell)
    {
        if (!cell.IsUnlocked)
        {
            if (_coinService.TrySpend(cell.SkinData.Price))
            {
                cell.Unlock();
                YG2.saves.UnlockedSkins.Add(cell.SkinData.name);
            }
            else
            {
                return;
            }
        }

        EquipSkin(cell);
        YG2.saves.EquippedSkin = cell.SkinData.name;
    }

    private void EquipSkin(SkinCell newCell)
    {
        if (_currentSelectedCell != null)
            _currentSelectedCell.Unequip();

        newCell.Equip();
        _currentSelectedCell = newCell;
        _playerVisual.SetSprite(newCell.SkinData.Sprite);
    }
}