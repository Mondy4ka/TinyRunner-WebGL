using PrimeTween;
using UnityEngine;

public class GhostSpike : MonoBehaviour
{
    private Vector3 _startScale;

    public void Initialize() => _startScale = transform.localScale;

    public void HideSpike()
    {
        Tween.Scale(transform, 0f, 0.2f, Ease.InBack)
            .OnComplete(() => gameObject.SetActive(false));
    }

    public void ResetSpike()
    {
        transform.localScale = _startScale;
        gameObject.SetActive(true);
    }
}