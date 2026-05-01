using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    private Player _player;
    private GameManager _gameManager;

    public void Initialize(Player player, GameManager gameManager)
    {
        _player = player;
        _gameManager = gameManager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spike"))
        {
            _gameManager.GameOver();
            return;
        }

        if (collision.CompareTag("JumpPad"))
        {
            StartCoroutine(_player.Jump());
            return;
        }

        if (collision.CompareTag("Ghost"))
        {
            collision.GetComponent<GhostSpike>().HideSpike();
            return;
        }
    }
}
