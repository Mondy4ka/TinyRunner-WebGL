using UnityEngine;
using YG;

public class CoinService
{
    public int Coins
    {
        get => _coins;
        private set
        {
            _coins = Mathf.Max(value, 0);
            _uiManager.UpdateCoinsText(_coins);

            YG2.saves.Coins = _coins;
            YG2.SaveProgress();
        }
    }

    private readonly UIManager _uiManager;
    private readonly int _rewardedAdvCoins;

    private int _coins;

    public CoinService(UIManager uiManager, int rewardedAdvCoins)
    {
        _uiManager = uiManager;
        _rewardedAdvCoins = rewardedAdvCoins;

        YG2.onRewardAdv += OnRewardedAdv;
    }

    public void LoadCoins(int coins) => Coins = coins;

    public void AddCoins(int count) => Coins += count;

    public bool TrySpend(int price)
    {
        if (price > Coins) return false;

        Coins -= price;

        return true;
    }

    private void OnRewardedAdv(string id)
    {
        if (id != "Coins") return;

        Coins += _rewardedAdvCoins;
    }
}