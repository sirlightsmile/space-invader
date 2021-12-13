using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using SmileProject.Generic.ResourceManagement;

namespace SmileProject.Generic.Audio
{
    /// <summary>
    /// Addressable resource loader manager
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private Transform _audioSourcesContainer;

        [SerializeField]
        private List<AudioSource> _audioSources;

        private IResourceLoader _resourceLoader;
        private Dictionary<int, AudioSource> _playingSource;
        private Dictionary<string, AudioMixerGroup> _mixerMap;
        private int _playId = 0;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _audioSources = new List<AudioSource>(_audioSourcesContainer.GetComponentsInChildren<AudioSource>());
            _playingSource = new Dictionary<int, AudioSource>();
            _mixerMap = new Dictionary<string, AudioMixerGroup>();
        }

        /// <summary>
        /// Initialize audio manager
        /// </summary>
        /// <param name="resourceLoader">resource loader</param>
        /// <param name="mainMixerKey">mixer asset key</param>
        /// <returns></returns>
        public async Task Initialize(IResourceLoader resourceLoader, string mainMixerKey)
        {
            _resourceLoader = resourceLoader;
            await InitMixer(mainMixerKey);
        }

        /// <summary>
        /// Preload sound from sound key
        /// </summary>
        /// <param name="soundKeys"></param>
        /// <returns></returns>
        public async Task PreloadSounds(SoundKeys[] soundKeys)
        {
            List<string> preloadList = new List<string>();
            foreach (SoundKeys soundKey in soundKeys)
            {
                preloadList.Add(soundKey.GetAssetKey());
            }
            await _resourceLoader.Preload(preloadList);
        }

        /// <summary>
        /// Load clip then play sound through selected mixer if that mixer is exist when initialize
        /// </summary>
        /// <param name="soundKey">asset key of audio clip</param>
        /// <param name="loop">is loop sound</param>
        /// <param name="mixer">selected mixer</param>
        /// <returns>play id</returns>
        public async Task<int> PlaySound(SoundKeys soundKey, bool loop = false)
        {
            AudioClip clip = await _resourceLoader.Load<AudioClip>(soundKey.GetAssetKey());
            AudioSource source = GetAvaliableAudioSource();
            string mixerKey = soundKey.GetMixerKey();
            if (mixerKey != null && _mixerMap.TryGetValue(mixerKey, out AudioMixerGroup mixerGroup))
            {
                source.outputAudioMixerGroup = mixerGroup;
            }
            else
            {
                source.outputAudioMixerGroup = null;
            }
            source.clip = clip;
            source.loop = loop;
            source.Play();
            Debug.Log($"Play sound : {soundKey.ToString()}");
            _playingSource.Add(_playId, source);
            return _playId++;
        }

        /// <summary>
        /// Stop sound that already play
        /// </summary>
        /// <param name="playId">play id</param>
        public void StopSound(int playId)
        {
            if (_playingSource.TryGetValue(playId, out AudioSource source))
            {
                if (source.isPlaying)
                {
                    source.Stop();
                }
                _playingSource.Remove(playId);
            }
        }

        /// <summary>
        /// Clear all audio sources references for free audio clip from memories
        /// </summary>
        /// <param name="ignorePlayingSource">Ignore clear reference from playing sources</param>
        public void CleanAll(bool ignorePlayingSource = false)
        {
            foreach (AudioSource audioSource in _audioSources)
            {
                if (audioSource.isPlaying)
                {
                    if (ignorePlayingSource)
                    {
                        continue;
                    }
                    audioSource.Stop();
                }
                CleanAudioSource(audioSource);
            }
        }

        private AudioSource GetAvaliableAudioSource()
        {
            AudioSource audioSource = _audioSources.Find(item => !item.isPlaying);
            if (audioSource == null)
            {
                GameObject audioSourceObj = new GameObject("AudioSource");
                audioSource = audioSourceObj.AddComponent<AudioSource>();
                audioSourceObj.transform.SetParent(_audioSourcesContainer);
                audioSource.playOnAwake = false;
                _audioSources.Add(audioSource);
            }
            else
            {
                CleanAudioSource(audioSource);
            }
            return audioSource;
        }

        /// <summary>
        /// Clean audio source from clip resources reference
        /// </summary>
        /// <param name="audioSource"></param>
        private void CleanAudioSource(AudioSource audioSource)
        {
            if (audioSource.clip != null)
            {
                var clipRef = audioSource.clip;
                audioSource.clip = null;
                _resourceLoader.Release(clipRef);
            }
        }

        private async Task InitMixer(string mixerKey)
        {
            if (!string.IsNullOrEmpty(mixerKey))
            {
                AudioMixer mixer = await _resourceLoader.Load<AudioMixer>(mixerKey);
                if (mixer != null)
                {
                    // find all mixer group
                    AudioMixerGroup[] groups = mixer.FindMatchingGroups(string.Empty);
                    foreach (var group in groups)
                    {
                        _mixerMap.Add(group.name, group);
                    }
                }
            }
        }
    }
}