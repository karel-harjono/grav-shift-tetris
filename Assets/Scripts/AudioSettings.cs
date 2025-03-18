using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    public AudioSource musicSource;
    public GameObject soundImage;
    public GameObject muteImage;

    void Update()
    {
        UpdateMuteStatusImage();
    }

    public void UpdateMuteStatusImage()
    {
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
        AudioManager.Instance.ToggleMusic();
        AudioManager.Instance.ToggleSFX();
        UpdateMuteStatusImage();
    }
}
