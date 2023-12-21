using FruitSalad3D.scripts.audio.sound;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FruitSalad3D.scripts.audio.music
{
    public enum MusicTrackType
    {
        MAIN_MENU,
        GAME
    }

    [System.Serializable]
    public struct MusicTrack
    {
        public AudioClip intro;
        public AudioClip loop;
    }

    public class MusicManager : MonoBehaviour
    {
        private static MusicManager _ref;
        public static MusicManager _Ref
        {
            get
            {
                if (_ref == null)
                {
                    _ref = FindObjectOfType<MusicManager>();

                    if (_ref == null)
                        _ref = new GameObject("InstantiatedMusicManager", typeof(MusicManager)).GetComponent<MusicManager>();
                }

                return _ref;
            }
            private set
            {
                _ref = value;
            }
        }

        [SerializeField] private TrackList myTrackList;

        public bool muted { get; private set; }
        [SerializeField] private float topVolume = 0.5f;

        private AudioSource musicSourceA;
        private AudioSource musicSourceB;

        private bool is_A_TheActiveMusicSource;

        [SerializeField] private MusicTrackType currentTrack;

        /// <summary>
        /// This is used to store the coroutine everytime a coroutine to play with intro is called, so that the old one can be cancelled when calling a new one to avoid them overlapping.
        /// </summary>
        private Coroutine playWithIntroCoroutineHandler;

        private void Awake()
        {
            if (_Ref != null && _Ref != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(this.gameObject);
            musicSourceA = gameObject.AddComponent<AudioSource>();
            musicSourceB = gameObject.AddComponent<AudioSource>();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            StartCrossFade(GetTrackForScene(scene));
        }

        public void StartMusic(MusicTrackType track)
        {
            if (muted) return;

            AudioSource currentSource = (is_A_TheActiveMusicSource ? musicSourceA : musicSourceB);

            if (currentSource.isPlaying && track == currentTrack)
                return;

            currentSource.volume = topVolume;
            playWithIntroCoroutineHandler = StartCoroutine(PlayTrack(currentSource, myTrackList.GetAudioClip(track)));

            currentTrack = track;
        }

        public void StartFade(MusicTrackType track, float transitionTime = 2f)
        {
            if (muted) return;

            AudioSource currentSource = (is_A_TheActiveMusicSource ? musicSourceA : musicSourceB);

            if (currentSource.isPlaying && track == currentTrack)
                return;

            StartCoroutine(StartMusicFade(currentSource, track, transitionTime));

            currentTrack = track;
        }

        public void StartCrossFade(MusicTrackType track, float transitionTime = 2f)
        {
            if (muted) return;

            AudioSource currentSource = (is_A_TheActiveMusicSource ? musicSourceA : musicSourceB);

            if (currentSource.isPlaying && track == currentTrack)
                return;

            AudioSource nextSource = (is_A_TheActiveMusicSource ? musicSourceB : musicSourceA);

            is_A_TheActiveMusicSource = !is_A_TheActiveMusicSource;

            nextSource.Stop();
            nextSource.volume = topVolume;
            playWithIntroCoroutineHandler = StartCoroutine(PlayTrack(nextSource, myTrackList.GetAudioClip(track)));
            StartCoroutine(StartMusicCrossFade(currentSource, nextSource, transitionTime));

            currentTrack = track;
        }

        public void SetMute(bool newMuted)
        {
            if (muted == newMuted)
                return;

            muted = newMuted;

            if (muted)
                StopMusic();
            else
                StartMusic(GetTrackForScene(SceneManager.GetActiveScene()));
        }

        private void StopMusic()
        {
            StopAllCoroutines();
            musicSourceA.Stop();
            musicSourceB.Stop();
        }

        private IEnumerator StartMusicFade(AudioSource source, MusicTrackType track, float transitionTime)
        {
            for (float transition = 0; transition < transitionTime; transition += Time.unscaledDeltaTime)
            {
                source.volume = (1 - (transition / transitionTime)) * topVolume;
                yield return null;
            }

            source.volume = 0;
            source.Stop();
            playWithIntroCoroutineHandler = StartCoroutine(PlayTrack(source, myTrackList.GetAudioClip(track)));

            for (float transition = 0; transition < transitionTime; transition += Time.unscaledDeltaTime)
            {
                source.volume = transition / transitionTime * topVolume;
                yield return null;
            }
            source.volume = topVolume;
        }

        private IEnumerator StartMusicCrossFade(AudioSource current, AudioSource next, float transitionTime)
        {
            for (float transition = 0; transition < transitionTime; transition += Time.unscaledDeltaTime)
            {
                current.volume = (1 - (transition / transitionTime)) * topVolume;
                next.volume = transition / transitionTime;
                yield return null;
            }
            next.volume = topVolume;

            current.Stop();
        }

        private IEnumerator PlayTrack(AudioSource source, MusicTrack track)
        {
            if (playWithIntroCoroutineHandler != null)
                StopCoroutine(playWithIntroCoroutineHandler);

            if (track.intro != null)
            {
                source.loop = false;
                source.clip = track.intro;
                source.Play();
                yield return new WaitWhile(() => source.isPlaying);
            }
            source.loop = true;
            source.clip = track.loop;
            source.Play();
        }

        private MusicTrackType GetTrackForScene(Scene currentScene)
        {
            if (currentScene.name.Contains("Menu"))
                return MusicTrackType.MAIN_MENU;
            else
                return MusicTrackType.GAME;
        }

    }
}