using System.Collections;
using UnityEngine;
using YG;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private float _invincibleTime;

    private GameManager _gameManager;
    private bool _isInvincible;
    private float _invincibleTimer;

    public void Initialize(GameManager gameManager) => _gameManager = gameManager;

    private void OnEnable() => YG2.onRewardAdv += OnRewardedAdv;

    private void OnDisable() => YG2.onRewardAdv -= OnRewardedAdv;

    private void OnRewardedAdv(string id)
    {
        if (id == "Continue")
            StartCoroutine(InvincibleTimer());
    }

    public IEnumerator InvincibleTimer()
    {
        _invincibleTimer = 0;
        _isInvincible = true;

        while (_invincibleTimer < _invincibleTime)
        {
            _invincibleTimer += Time.deltaTime;
            playerVisual.PulseSprite(_invincibleTimer);
            yield return null;
        }

        _isInvincible = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))
        {
            if (_isInvincible) return;

            _gameManager.GameOver();
            return;
        }

        if (collision.CompareTag("JumpPad"))
        {
            StartCoroutine(_gameManager.Player.Jump());
            _gameManager.SoundManager.PlayJumpSound();
            return;
        }

        if (collision.CompareTag("Coin"))
        {
            _gameManager.CoinService.AddCoins(1);
            _gameManager.SoundManager.PlayCoinSound();
            return;
        }
    }
}
