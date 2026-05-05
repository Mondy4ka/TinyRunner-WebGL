using PrimeTween;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private float _animationDuration;

    private Vector3 _startScale;

    public void Initialize() => _startScale = transform.localScale;

    public void Revert()
    {
        transform.localScale = _startScale;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            Tween.Scale(transform, 0f, _animationDuration, Ease.InBack)
                .OnComplete(() => gameObject.SetActive(false));
    }
}
