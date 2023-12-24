using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


namespace FruitSalad3D.scripts.audio.sound
{
    [RequireComponent(typeof(PieceBahaviour))]
    public class SoundOnCollision : MonoBehaviour
    {
        private PieceBahaviour myPieceBehaviour;

        [SerializeField] private SoundType soundType;

        private void Awake()
        {
            if (myPieceBehaviour == null)
                myPieceBehaviour = GetComponent<PieceBahaviour>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            float soundPitch = 1f;
            
            if (myPieceBehaviour != null)
                soundPitch = 1.5f / myPieceBehaviour.GetSize();

            SoundManager._Ref.PlaySound(soundType, collision.relativeVelocity.magnitude / 20f, soundPitch);
        }
    }
}