using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FruitSalad3D.scripts.audio.sound
{
    [CreateAssetMenu(menuName = "ScriptableObjects/SoundList")]

    public class SoundList : ScriptableObject
    {
        [SerializeField] private List<AudioClip> sounds;

        public AudioClip GetAudioClip(SoundType sound)
        {
            return sounds[(int)sound];
        }
    }
}