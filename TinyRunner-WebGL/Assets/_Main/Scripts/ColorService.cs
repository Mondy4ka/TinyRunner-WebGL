using PrimeTween;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColorService
{
    private readonly List<TMP_Text> _texts;
    private readonly List<Image> _images;
    private readonly List<SpriteRenderer> _sprites;
    private readonly List<Color> _colors;
    private readonly float _switchDelay;
    private readonly float _switchDuration;

    private int _currentColor;
    private float _timer;

    public ColorService(float switchDuration, float switchDelay, List<Color> colors, List<SpriteRenderer> sprites, List<Image> images, List<TMP_Text> texts)
    {
        _switchDuration = switchDuration;
        _switchDelay = switchDelay;
        _colors = colors;
        _sprites = sprites;
        _images = images;
        _texts = texts;
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

        SwitchColor(0);
    }

    private void NextColor()
    {
        _currentColor++;

        if (_currentColor == _colors.Count - 1)
        {
            _currentColor = 0;
        }

        SwitchColor(_switchDuration);
    }

    private void SwitchColor(float switchDuration)
    {
        SwitchSpritesColor(switchDuration);
        SwitchImagesColor(switchDuration);
        SwitchTextsColor(switchDuration);
    }

    private void SwitchSpritesColor(float switchDuration)
    {
        if (_sprites.Count <= 0) return;

        for (int i = 0; i < _sprites.Count; i++)
        {
            SpriteRenderer sprite = _sprites[i];
            Color color = _colors[_currentColor];
            color.a = sprite.color.a;

            if (sprite.gameObject.activeInHierarchy == false)
                sprite.color = color;
            else
                Tween.Color(sprite, color, switchDuration, Ease.Linear);
        }
    }

    private void SwitchTextsColor(float switchDuration)
    {
        if (_texts.Count <= 0) return;

        for (int i = 0; i < _texts.Count; i++)
        {
            TMP_Text text = _texts[i];
            Color color = _colors[_currentColor];
            color.a = text.color.a;

            if (text.gameObject.activeInHierarchy == false)
                text.color = color;
            else
                Tween.Color(text, color, switchDuration, Ease.Linear);
        }
    }

    private void SwitchImagesColor(float switchDuration)
    {
        if (_images.Count <= 0) return;

        for (int i = 0; i < _images.Count; i++)
            Tween.Color(_images[i], _colors[_currentColor], switchDuration, Ease.Linear);
    }
}