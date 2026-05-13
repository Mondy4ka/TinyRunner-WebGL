using PrimeTween;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private ParticleSystem _trail;
    [SerializeField] private ParticleSystem _deathEffect;
    [SerializeField] private float _pulseSpeed;

    private SoundManager _soundManager;

    public void Initialize(SoundManager soundManager) => _soundManager = soundManager;

    public void ActivateTrail() => _trail.Play();

    public void DeactivateTrail() => _trail.Stop();

    public void SetSprite(Sprite newSprite) => _spriteRenderer.sprite = newSprite;

    public void SetTrailMaterial(Material newMaterial) => _trail.GetComponent<ParticleSystemRenderer>().material = newMaterial;

    public void PulseSprite(float time)
    {
        float alpha = Mathf.PingPong(time * _pulseSpeed, 1.00f);

        Color newColor = _spriteRenderer.color;

        newColor.a = alpha;

        _spriteRenderer.color = newColor;
    }

    public void Revert()
    {
        _spriteRenderer.transform.localScale = Vector3.one;
        _deathEffect.Stop();
    }

    public void Death()
    {
        DeactivateTrail();
        Tween.Scale(_spriteRenderer.transform, 0, 0.1f, Ease.InBack)
            .OnComplete(PlayDeathEffect);
    }

    private void PlayDeathEffect()
    {
        _deathEffect.Play();
        _soundManager.PlayDeathSound();
    }
}