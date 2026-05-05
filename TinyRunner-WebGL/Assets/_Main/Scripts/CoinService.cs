using UnityEngine;

public class CoinService
{
    public int Coins
    {
        get => _coins;
        private set
        {
            _coins = Mathf.Max(value, 0);
            _uiManager.UpdateCoinsText(_coins);
        }
    }

    private readonly UIManager _uiManager;

    private int _coins;

    public CoinService(UIManager uiManager) => _uiManager = uiManager;

    public void AddCoins(int count) => Coins += count;

    public bool TrySpend(int price)
    {
        if (price > Coins) return false;

        Coins -= price;

        return true;
    }
}