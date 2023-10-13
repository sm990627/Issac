using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUI : MonoBehaviour
{

    [SerializeField] RectTransform[] _menus;
    [SerializeField] RectTransform _arrow;
    [SerializeField] GameObject[] _sfx;
    [SerializeField] GameObject[] _musics;

    int _currentIdx = 0;
    int _sfxIdx = 5;
    int _musicIdx = 5;

    public void Init()
    {
        UpdateSFXUI();
        UpdateMusicUI();
    }
    public void MoveArrowDown()
    {
        if (_currentIdx < _menus.Length - 1)
        {
            _currentIdx++;
        }
        UpdateArrowPosition();
    }

    public void MoveArrowUp()
    {
        if (_currentIdx > 0)
        {
            _currentIdx--;
        }
        UpdateArrowPosition();
    }

    public void UpdateArrowPosition()
    {
        _arrow.anchoredPosition = _menus[_currentIdx].anchoredPosition + new Vector2(-80f, 0);
    }

    void UpdateSFXUI()
    {
        foreach (var temp in _sfx)
        {
            temp.SetActive(false);
        }
        if (_sfxIdx != -1)
        {
            for (int i = 0; i < _sfxIdx; i++)
            {
                _sfx[i].gameObject.SetActive(true);
            }
        }
    }
    void UpdateMusicUI()
    {
        foreach (var temp in _musics)
        {
            temp.SetActive(false);
        }
        if(_musicIdx != -1)
        {
            for (int i = 0; i < _musicIdx; i++)
            {
                _musics[i].gameObject.SetActive(true);
            }
        }
        
    }

    public void VolumeDown()
    {
        switch (_currentIdx)
        {
            case 0:
                {
                    _sfxIdx--;
                    if (_sfxIdx < -1) _sfxIdx = -1;
                    GenericSingleton<UIBase>.Instance.EffectSound(_sfxIdx * 0.1f);
                    UpdateSFXUI();
                    Debug.Log(_sfxIdx * 0.1f);
                }
                break;
            case 1:
                {
                    _musicIdx--;
                    if (_musicIdx < -1) _musicIdx = -1;
                    GenericSingleton<UIBase>.Instance.MusicSound(_musicIdx * 0.1f);
                    UpdateMusicUI();
                }
                break;
        }
    }
    public void VolumeUp()
    {
        switch (_currentIdx)
        {
            case 0:
                {
                    _sfxIdx++;
                    if (_sfxIdx == _sfx.Length + 1) _sfxIdx--;
                    GenericSingleton<UIBase>.Instance.EffectSound(_sfxIdx * 0.1f);
                    UpdateSFXUI();
                    Debug.Log(_sfxIdx * 0.1f);
                }
                break;
            case 1:
                {
                    _musicIdx++;
                    if (_musicIdx == _musics.Length + 1) _musicIdx--;
                    GenericSingleton<UIBase>.Instance.MusicSound(_musicIdx * 0.1f);
                    UpdateMusicUI();
                }
                break;
        }
    }
}
