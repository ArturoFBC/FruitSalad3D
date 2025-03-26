using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FruitSalad3D.scripts.audio.music
{
    [CreateAssetMenu(menuName = "ScriptableObjects/TrackList")]

    public class TrackList : ScriptableObject
    {
        [SerializeField]
        private List<MusicTrack> musicTracks = new List<MusicTrack>();

        public MusicTrack GetAudioClip(MusicTrackType trackType)
        {
            return musicTracks[(int)trackType];
        }
    }
}