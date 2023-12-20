using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class MuteToggle : MonoBehaviour
{
    private Toggle myToggle;
    [SerializeField] private string sceneName;

    private void Awake()
    {
        myToggle = GetComponent<Toggle>();

        myToggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool mute)
    {
       MusicManager._Ref.SetMute(mute);
    }
}
