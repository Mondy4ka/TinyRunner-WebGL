using PrimeTween;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private ParticleSystem _trail;
    [SerializeField] private ParticleSystem _deathEffect;

    public void ActivateTrail() => _trail.Play();

    public void DeactivateTrail() => _trail.Stop();

    public void Revert()
    {
        _spriteRenderer.transform.localScale = Vector3.one;
        _deathEffect.Stop();
    }

    public void Death()
    {
        DeactivateTrail();
        Tween.Scale(_spriteRenderer.transform, 0, 0.15f, Ease.InBack)
            .OnComplete(() => _deathEffect.Play());
    }
}
