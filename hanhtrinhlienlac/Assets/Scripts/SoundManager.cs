using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] int _numberSFXChannels = 10;
    private AudioSource _bgmChannel;
    private List<AudioSource> _sfxChannels;
    public static SoundManager inst;
    public List<AudioClip> soundData;
    private static bool _mute = false;
    private void Awake()
    {
        inst = this;
        for (int i = 0; i < _numberSFXChannels; i++)
        {
            var go = new GameObject();
            go.transform.parent = transform;
            go.transform.name = "sfx" + i;
            var source = go.AddComponent<AudioSource>();
        }
    }
    void Start()
    {
        _sfxChannels = new List<AudioSource>(gameObject.GetComponentsInChildren<AudioSource>());
        _bgmChannel = gameObject.AddComponent<AudioSource>();
    }
    public void Mute(bool enable)
    {
        _mute = enable;
    }
    private void PlayAudio(AudioSource source, AudioClip c, bool loop = false, int volume = 100, float secondDelay = 0f)
    {
        if (!_mute)
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
    }
    /**
    * PlaySfx Play a sfx sound
    * channel == -1 : Find free channel to play
    * channel !=1 : play correct channel
    */
    public void PlaySfx(AudioClip c, bool loop = false, int channel = -1, int volume = 100, float secondDelay = 0f)
    {
        if (channel != -1)
        {
            PlayAudio(_sfxChannels[channel], c, loop, volume, secondDelay);
        }
        else
        {
            var sfx = _sfxChannels.Find(e => !e.isPlaying);
            PlayAudio(sfx, c, loop, volume, secondDelay);
        }

    }
    /**
    * PlayBGM play a background music sound.
    */
    public void PlayBGM(AudioClip c, bool loop = false, int volume = 100, float secondDelay = 0f)
    {
        PlayAudio(_bgmChannel, c, loop, volume, secondDelay);
    }
    public void StopSfx(int channel = -1)
    {
        if (channel == -1)
        {
            StopAllSfx();
        }
        else
        {
            _sfxChannels[channel].Stop();
        }

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
    public bool IsSoundPlaying(SoundType type, AudioClip c = null, int channel = -1)
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
                        if (channel == -1)
                        {
                            return _sfxChannels.Find(e => (e.clip.GetHashCode() == c.GetHashCode()) && e.isPlaying);
                        }
                        else
                        {
                            return _sfxChannels[channel].clip.GetHashCode() == c.GetHashCode() && _sfxChannels[channel].isPlaying;
                        }

                    }
                    else
                    {
                        return _sfxChannels.Find(e => e.isPlaying);
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
