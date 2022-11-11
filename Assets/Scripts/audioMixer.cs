using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class audioMixer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] AudioMixerGroup musicMixer;
    [SerializeField] AudioMixerGroup sfxMixer;
    [SerializeField] AudioMixerGroup uiMixer;

    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SFXSlider;
    public Slider UISlider;

    private void Start()
    {
        MasterSlider.value = PlayerPrefs.GetFloat("Master", 1f);
        MusicSlider.value = PlayerPrefs.GetFloat("Music", 1f);
        SFXSlider.value = PlayerPrefs.GetFloat("Sound Effects", 1f);
        UISlider.value = PlayerPrefs.GetFloat("UI", 1f);

    }


    public void SetMasterVolume(float input)
    {
        masterMixer.SetFloat("Master", Mathf.Log10(input) * 20);
        PlayerPrefs.SetFloat("Master", input);
        if (!gameManager.instance.menuUIAudio.isPlaying)
        {
            gameManager.instance.menuUIAudio.Play();
        }
    }

    public void SetMusicVolume(float input)
    {
        masterMixer.SetFloat("Music", Mathf.Log10(input) * 20);
        PlayerPrefs.SetFloat("Music", input);
    }

    public void SetSFXVolume(float input)
    {
        masterMixer.SetFloat("Sound Effects", Mathf.Log10(input) * 20);
        PlayerPrefs.SetFloat("Sound Effects", input);
        if (!gameManager.instance.menuSFXAudio.isPlaying)
        {
            gameManager.instance.menuSFXAudio.PlayOneShot(gameManager.instance.debugGun);
        }
    }

    public void SetUIVolume(float input)
    {
        masterMixer.SetFloat("UI", Mathf.Log10(input) * 20);
        PlayerPrefs.SetFloat("UI", input);
        if (!gameManager.instance.menuUIAudio.isPlaying)
        {
            gameManager.instance.menuUIAudio.Play();
        }
    }

    //public void wakeupSetUP()
    //{
    //    MasterSlider.value = PlayerPrefs.GetFloat("Master", 1f);
    //    MusicSlider.value = PlayerPrefs.GetFloat("Music", 1f);
    //    SFXSlider.value = PlayerPrefs.GetFloat("Sound Effects", 1f);
    //    UISlider.value = PlayerPrefs.GetFloat("UI", 1f);
    //}
}
