using System.Collections.Generic;
using UnityEngine;

namespace Mine.Framework.Manager.SoundManager
{
    [System.Serializable]
    public class SoundClip
    {
        [SerializeField] public AudioClip audioClip;
        [SerializeField, Range(0, 1)] public float defaultVolume = 0.5f;
    }
    
    public class SoundManager
    {
        private AudioSource bgmAudioSource;
        private readonly List<AudioSource> sfxAudioSources = new();
        private GameObject audioPlayer;
        private AudioClip CurrentBGM;

        #region LifeCycle

        private void Start()
        {
            CreateBGMPlayer();
            CreateSFXPlayer();
            CurrentBGM = bgmAudioSource.clip;
        }

        #endregion
        
        #region Private Method

        private void CreateBGMPlayer()
        {
            audioPlayer = new GameObject("AudioPlayer");
            bgmAudioSource = audioPlayer.AddComponent<AudioSource>();
            bgmAudioSource.loop = true;
            bgmAudioSource.playOnAwake = false;
        }
        
        private AudioSource CreateSFXPlayer()
        {
            audioPlayer = new GameObject("AudioPlayer");
            var audioSource = audioPlayer.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            sfxAudioSources.Add(audioSource);
            return audioSource;
        }
        
        private void Play(AudioSource source, SoundClip clip, float volume)
        {
            source.clip = clip.audioClip;
            source.volume = volume.Equals(0)? clip.defaultVolume : volume;
            source.Play();
        }

        #endregion

        #region Public Method

        public void PlayBGM(SoundClip clip, float volume = 0f, bool isMute = false)
        {
            if (CurrentBGM.Equals(clip.audioClip)) return;
            if (!bgmAudioSource) CreateBGMPlayer();
            bgmAudioSource.mute = isMute;
            Play(bgmAudioSource, clip, volume);
        }
        
        public void PlaySFX(SoundClip clip, float volume = 0f, bool isMute = false)
        {
            if(isMute) return;
            if (!audioPlayer) CreateSFXPlayer();
            var audioSource = sfxAudioSources.Find(source => !source.isPlaying);
            if(audioSource.Equals(null)) audioSource = CreateSFXPlayer();
            Play(audioSource, clip, volume);
        }
        
        public void StopBGM()
        {
            if (!bgmAudioSource) return;
            bgmAudioSource.Stop();
        }
        
        public void StopSFX()
        {
            if (!audioPlayer) return;
            foreach (var audioSource in sfxAudioSources)
            {
                audioSource.Stop();
            }
        }
        
        public void MuteBGM(bool isMute)
        {
            if (!bgmAudioSource) return;
            bgmAudioSource.mute = isMute;
        }

        #endregion
    }
}