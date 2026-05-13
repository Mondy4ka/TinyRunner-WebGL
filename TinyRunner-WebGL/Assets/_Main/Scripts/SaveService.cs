using System.Collections.Generic;
using YG;

public class SaveService
{
    private readonly CoinService _coinService;
    private readonly ScoreService _scoreService;
    private readonly ShopService _shopService;
    
    public SaveService(CoinService coinService, ScoreService scoreService, ShopService shopService)
    {
        _coinService = coinService;
        _scoreService = scoreService;
        _shopService = shopService;
    }

    public void LoadCoins() => _coinService.Coins = YG2.saves.Coins;

    public void LoadBestScore() => _scoreService.BestScore = YG2.saves.BestScore;
}

namespace YG
{
    public partial class SavesYG
    {
        public int Coins = 0;
        public int BestScore = 0;

        public string EquippedSkin = "Skin";
        public List<string> UnlockedSkins = new();
    }
}