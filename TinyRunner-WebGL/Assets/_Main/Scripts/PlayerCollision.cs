using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private GameManager _gameManager;

    public void Initialize(GameManager gameManager) => _gameManager = gameManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))
        {
            _gameManager.GameOver();
            return;
        }

        if (collision.CompareTag("JumpPad"))
        {
            StartCoroutine(_gameManager.Player.Jump());
            return;
        }

        if (collision.CompareTag("Coin"))
        {
            _gameManager.CoinService.AddCoins(1);
            return;
        }
    }
}
