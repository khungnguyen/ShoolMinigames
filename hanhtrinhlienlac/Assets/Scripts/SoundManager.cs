using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public int numberSFXChannels = 2;
    private AudioSource _bgmChannel;
    private List<AudioSource> _sfxChannels;
    public static SoundManager inst;
    void Awake()
    {
        inst = this;
        for (int i = 0; i < numberSFXChannels; i++)
        {
            _sfxChannels.Add(gameObject.AddComponent<AudioSource>());
        }
        _bgmChannel = gameObject.AddComponent<AudioSource>();
    }
    private void PlayAudio(AudioSource source, AudioClip c, bool loop = false, int volume = 100, float secondDelay = 0f)
    {
        source.clip = c;
        source.loop = loop;
        source.volume = volume;
        if (secondDelay != 0)
        {
            source.PlayDelayed(secondDelay);
        }
        else
        {
            source.Play();
        }

    }
    /**
    * PlaySfx Play a sfx sound
    */
    public void PlaySfx(AudioClip c, bool loop = false, int channel = 0, int volume = 100, float secondDelay = 0f)
    {
        PlayAudio(_sfxChannels[channel], c, loop, volume, secondDelay);
    }
    /**
    * PlayBGM play a background music sound.
    */
    public void PlayBGM(AudioClip c, bool loop = false, int volume = 100, float secondDelay = 0f)
    {
        PlayAudio(_bgmChannel, c, loop, volume, secondDelay);
    }
    public void StopSfx(int channel = 0)
    {
        _sfxChannels[channel].Stop();
    }
    public void StopAllSfx()
    {
        _sfxChannels.ForEach(e =>
        {
            if (e != null)
            {
                e.Stop();
            }
        });
    }
    public void StopBGM()
    {
        _bgmChannel.Stop();
    }
    /**
    * Stop all Sound type
    */
    public void StopAllSound()
    {
        StopAllSfx();
        StopBGM();
    }
    public bool IsSoundPlaying(SoundType type, AudioClip c = null, int channel = 0)
    {
        switch (type)
        {
            case SoundType.BGM:
                {
                    if (c != null)
                    {
                        return _bgmChannel.clip.GetHashCode() == c.GetHashCode() && _bgmChannel.isPlaying;
                    }
                    else
                    {
                        return _bgmChannel.isPlaying;
                    }
                }
            case SoundType.SFX:
                {
                    if (c != null)
                    {
                        return _sfxChannels[channel].clip.GetHashCode() == c.GetHashCode() && _bgmChannel.isPlaying;
                    }
                    else
                    {
                        return _sfxChannels[channel].isPlaying;
                    }
                }
        }
        return false;
    }

}
public enum SoundType
{
    BGM,
    SFX
}
