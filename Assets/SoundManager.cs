using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : GenericSingleton<SoundManager>
{
    [SerializeField] AudioClip _title;
    [SerializeField] AudioClip _basement;
    [SerializeField] AudioClip _boss;
    AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        GenericSingleton<UIBase>.Instance.MusicVolume += Volume;
        GenericSingleton<UIBase>.Instance.SoundInit();
    }
    void Volume(float value)
    {
        _audioSource.volume = value;
    }
    public void SetBasement()
    {
        _audioSource.clip = _basement;
        _audioSource.Play();
    }
    public void Setitle()
    {
        _audioSource.clip = _title;
        _audioSource.Play();
    }
    public void SetBoss()
    {
        _audioSource.clip = _boss;
        _audioSource.Play();
    }
    public void Stop()
    {
        _audioSource.Stop();
    }
}
