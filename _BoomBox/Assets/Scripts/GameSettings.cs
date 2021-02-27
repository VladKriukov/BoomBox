using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [SerializeField] Toggle upwardsModifierToggle;
    [SerializeField] Toggle bombDelayToggle;
    [SerializeField] Toggle vfxToggle;
    [SerializeField] Toggle shakeOnToggle;
    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle sfxToggle;
    [SerializeField] GameObject gameParticles;
    public static bool bombUpwardsModifier = false;
    public static bool bombDelay = true;
    public static bool vfx = true;
    public static bool shakeOn = true;
    public static bool musicOn = true;
    public static bool sfxOn = true;

    void Start()
    {
        LoadSettings();
    }

    public void HasUpwardsModifier()
    {
        bombUpwardsModifier = upwardsModifierToggle.isOn;
        Analytics.CustomEvent("screen_visit", new Dictionary<string, object>
        {
            { "screen_name", "bomb_upwards_modifier" },
            { "value", bombUpwardsModifier }
        });
        SaveSettings();
    }

    public void HasBombDelay()
    {
        bombDelay = bombDelayToggle.isOn;
        Analytics.CustomEvent("screen_visit", new Dictionary<string, object>
        {
            { "screen_name", "bomb_delay" },
            { "value", bombDelay }
        });
        SaveSettings();
    }

    public void HasVFX(){
        vfx = vfxToggle.isOn;
        Analytics.CustomEvent("screen_visit", new Dictionary<string, object>
        {
            { "screen_name", "vfx" },
            { "value", vfx }
        });
        SaveSettings();
    }

    public void HasShake(){
        shakeOn = shakeOnToggle.isOn;
        Analytics.CustomEvent("screen_visit", new Dictionary<string, object>
        {
            { "screen_name", "shake" },
            { "value", shakeOn }
        });
        SaveSettings();
    }

    public void HasMusic()
    {
        musicOn = musicToggle.isOn;
        Analytics.CustomEvent("screen_visit", new Dictionary<string, object>
        {
            { "screen_name", "music" },
            { "value", musicOn }
        });
        SaveSettings();
    }

    public void HasSfx()
    {
        sfxOn = sfxToggle.isOn;
        Analytics.CustomEvent("screen_visit", new Dictionary<string, object>
        {
            { "screen_name", "sfx" },
            { "value", sfxOn }
        });
        SaveSettings();
    }

    void LoadSettings()
    {
        if (PlayerPrefs.HasKey("UpwardsMod"))
        {
            if (PlayerPrefs.GetInt("UpwardsMod") == 1)
            {
                bombUpwardsModifier = true;
            }
            else
            {
                bombUpwardsModifier = false;
            }

            if (PlayerPrefs.GetInt("Delay") == 1)
            {
                bombDelay = true;
            }
            else
            {
                bombDelay = false;
            }

            if (PlayerPrefs.GetInt("VFX") == 1)
            {
                vfx = true;
            }
            else
            {
                vfx = false;
            }

            if (PlayerPrefs.GetInt("Shake") == 1)
            {
                shakeOn = true;
            }
            else
            {
                shakeOn = false;
            }

            if (PlayerPrefs.GetInt("Music") == 1)
            {
                musicOn = true;
            }
            else
            {
                musicOn = false;
            }

            if (PlayerPrefs.GetInt("Sfx") == 1)
            {
                sfxOn = true;
            }
            else
            {
                sfxOn = false;
            }
        }
        upwardsModifierToggle.isOn = bombUpwardsModifier;
        bombDelayToggle.isOn = bombDelay;
        vfxToggle.isOn = vfx;
        shakeOnToggle.isOn = shakeOn;
        musicToggle.isOn = musicOn;
        sfxToggle.isOn = sfxOn;
    }

    void SaveSettings()
    {
        if (bombUpwardsModifier == true) PlayerPrefs.SetInt("UpwardsMod", 1);
        else PlayerPrefs.SetInt("UpwardsMod", 0);

        if (bombDelay == true) PlayerPrefs.SetInt("Delay", 1);
        else PlayerPrefs.SetInt("Delay", 0);

        if (vfx == true) PlayerPrefs.SetInt("VFX", 1);
        else PlayerPrefs.SetInt("VFX", 0);

        if (shakeOn ==true) PlayerPrefs.SetInt("Shake", 1);
        else PlayerPrefs.SetInt("Shake", 0);

        if (musicOn == true) PlayerPrefs.SetInt("Music", 1);
        else PlayerPrefs.SetInt("Music", 0);

        if (sfxOn == true) PlayerPrefs.SetInt("Sfx", 1);
        else PlayerPrefs.SetInt("Sfx", 0);

        PlayerPrefs.Save();
    }

}
