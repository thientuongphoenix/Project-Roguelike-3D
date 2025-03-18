using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    None,
    ButtonClick,
    PlayerJump,
    PlayerShoot,
    EnemyHit,
    EnemyDeath,
    BackgroundMusic,
    GameOver,
    Victory
}

[System.Serializable]
public class SoundData
{
    public SoundType soundType; // Enum âm thanh
    public AudioClip clip; // File âm thanh tương ứng
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton

    [Header("Audio Clips")]
    public List<SoundData> soundList = new List<SoundData>(); // Danh sách âm thanh

    [Header("Audio Sources")]
    public AudioSource musicSource; // Âm thanh nền (Loop)
    public AudioSource sfxSource; // Hiệu ứng âm thanh (One-shot)

    private Dictionary<SoundType, AudioClip> soundDictionary = new Dictionary<SoundType, AudioClip>();

    private void Awake()
    {
        // Đảm bảo Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Không bị hủy khi load Scene mới
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Đưa danh sách âm thanh vào Dictionary để dễ truy xuất
        foreach (SoundData sound in soundList)
        {
            if (!soundDictionary.ContainsKey(sound.soundType))
            {
                soundDictionary.Add(sound.soundType, sound.clip);
            }
        }

        this.PlayMusic(SoundType.BackgroundMusic);
    }

    /// <summary>
    /// Phát âm thanh 1 lần
    /// </summary>
    public void PlaySFX(SoundType soundType)
    {
        if (soundDictionary.ContainsKey(soundType))
        {
            sfxSource.PlayOneShot(soundDictionary[soundType]);
        }
        else
        {
            Debug.LogWarning($"Âm thanh {soundType} chưa được gán!");
        }
    }

    /// <summary>
    /// Phát nhạc nền (Loop)
    /// </summary>
    public void PlayMusic(SoundType soundType)
    {
        if (soundDictionary.ContainsKey(soundType))
        {
            musicSource.clip = soundDictionary[soundType];
            musicSource.loop = true;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Nhạc nền {soundType} chưa được gán!");
        }
    }

    /// <summary>
    /// Dừng nhạc nền
    /// </summary>
    public void StopMusic()
    {
        musicSource.Stop();
    }
}
