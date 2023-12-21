using FruitSalad3D.scripts.audio.sound;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SoundOnClick : MonoBehaviour
{
    private Button myButton;
    [SerializeField] private SoundType sound;

    private void Awake()
    {
        myButton = GetComponent<Button>();

        myButton.onClick.AddListener(PlaySound);
    }

    private void PlaySound()
    {
        SoundManager._Ref.PlaySound(sound);
    }
}
