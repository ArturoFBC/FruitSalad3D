using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FruitSalad3D.scripts.audio.sound
{
    public enum SoundType
    {
        OPEN_MENU,
        CLOSE_MENU,
        COMBINE,
        DROP
    }


    public class SoundManager : MonoBehaviour
    {
        private static SoundManager _ref;
        public static SoundManager _Ref
        {
            get
            {
                if (_ref == null)
                {
                    _ref = FindObjectOfType<SoundManager>();

                    if (_ref == null)
                        _ref = new GameObject("InstantiatedSoundManager", typeof(SoundManager)).GetComponent<SoundManager>();
                }

                return _ref;
            }
            private set
            {
                _ref = value;
            }
        }

        public bool muted { get; private set; }

        [SerializeField] private SoundList mySoundList;

        private void Awake()
        {
            if (_Ref != null && _Ref != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(this.gameObject);
        }

        public void PlaySound(SoundType soundType, float volume = 1f, float pitch = 1f)
        {
            if (muted == false)
            {
                AudioClip clipToPlay = mySoundList.GetAudioClip(soundType);
                StartCoroutine(PlaySoundAsync(clipToPlay,volume,pitch));
            }
        }

        private IEnumerator PlaySoundAsync(AudioClip clip, float volume, float pitch)
        {
            AudioSource currentAudioSource = gameObject.AddComponent<AudioSource>();
            currentAudioSource.clip = clip;
            currentAudioSource.volume = volume;
            currentAudioSource.pitch = pitch;
            currentAudioSource.Play();

            yield return new WaitUntil(() => muted == true || currentAudioSource.isPlaying == false);

            Destroy(currentAudioSource);
        }

        public void SetMute(bool newMuted)
        {
            if (muted == newMuted)
                return;

            muted = newMuted;
        }
    }
}