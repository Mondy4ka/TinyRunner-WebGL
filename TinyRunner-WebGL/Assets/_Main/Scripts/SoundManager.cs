using UnityEngine;

public class SoundManager
{
    private readonly AudioSource _sfxSource;
    
    private readonly AudioClip _switchLineSound;
    private readonly AudioClip _jumpSound;
    private readonly AudioClip _coinSound;
    private readonly AudioClip _deathSound;
    private readonly AudioClip _purchaseSound;

    public SoundManager(AudioSource sfxSource, AudioClip switchLineSound, AudioClip jumpSound, AudioClip coinSound, AudioClip deathSound, AudioClip purchaseSound)
    {
        _sfxSource = sfxSource;
        _switchLineSound = switchLineSound;
        _jumpSound = jumpSound;
        _coinSound = coinSound;
        _deathSound = deathSound;
        _purchaseSound = purchaseSound;
    }

    public void PlaySwitchLineSound() => _sfxSource.PlayOneShot(_switchLineSound);

    public void PlayJumpSound() => _sfxSource.PlayOneShot(_jumpSound);

    public void PlayCoinSound() => _sfxSource.PlayOneShot(_coinSound);

    public void PlayDeathSound() => _sfxSource.PlayOneShot(_deathSound);

    public void PlayPurchaseSound() => _sfxSource.PlayOneShot(_purchaseSound);
}