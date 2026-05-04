using PrimeTween;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager
{
    private readonly List<Image> _images;
    private readonly List<SpriteRenderer> _sprites;
    private readonly List<Color> _colors;
    private readonly float _switchDelay;
    private readonly float _switchDuration;

    private int _currentColor;
    private float _timer;

    public ColorManager(float switchDuration, float switchDelay, List<Color> colors, List<SpriteRenderer> sprites, List<Image> images)
    {
        _switchDuration = switchDuration;
        _switchDelay = switchDelay;
        _colors = colors;
        _sprites = sprites;
        _images = images;
    }

    public void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= _switchDelay)
        {
            _timer = 0;
            NextColor();
        }
    }

    public void ResetColor()
    {
        _timer = 0;

        if (_currentColor == 0) return;
        _currentColor = 0;

        SwitchColor();
    }

    private void NextColor()
    {
        _currentColor++;

        if (_currentColor == _colors.Count - 1)
        {
            _currentColor = 0;
        }

        SwitchColor();
    }
    
    private void SwitchColor()
    {
        for (int i = 0; i < _sprites.Count; i++)
        {
            SpriteRenderer sprite = _sprites[i];
            Color color = _colors[_currentColor];
            color.a = sprite.color.a;

            if (sprite.gameObject.activeInHierarchy == false)
                sprite.color = color;
            else
                Tween.Color(sprite, color, _switchDuration, Ease.Linear);
        }

        for (int i = 0; i < _images.Count; i++)
        {
            Image image = _images[i];
            Color color = _colors[_currentColor];

            if (image.gameObject.activeInHierarchy == false)
                image.color = color;
            else
                Tween.Color(image, color, _switchDuration, Ease.Linear);
        }
    }
}