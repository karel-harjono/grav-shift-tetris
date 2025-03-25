using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    public AudioSource musicSource;
    public GameObject soundImage;
    public GameObject muteImage;

    void Update()
    {
        if (musicSource != null)
        {
            UpdateMuteStatusImage();
        }
    }

    public void UpdateMuteStatusImage()
    {
        if (musicSource == null) return; // Prevents errors if musicSource is missing

        if (musicSource.mute)
        {
            soundImage.SetActive(false);
            muteImage.SetActive(true);
        }
        else
        {
            soundImage.SetActive(true);
            muteImage.SetActive(false);
        }
    }

    public void ToggleMusicAndUpdateImage()
    {
        if (AudioManager.Instance == null) return; // Ensure AudioManager exists

        AudioManager.Instance.ToggleMusic();
        AudioManager.Instance.ToggleSFX();

        if (musicSource != null)
        {
            UpdateMuteStatusImage();
        }
    }
}
