using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    private Slider volumeSlider;
    // Start is called before the first frame update
    void Awake()
    {
        volumeSlider = GetComponent<Slider>();
        volumeSlider.value = PlayerPrefs.GetFloat("gameAudio", 0.75f);
        AudioListener.volume = volumeSlider.value; 
        volumeSlider.onValueChanged.AddListener(SetVolume); 
    }

    void SetVolume(float sliderValue)
    {
        AudioListener.volume = sliderValue;
        PlayerPrefs.SetFloat("gameAudio", sliderValue);
    }
    
    // Update is called once per frame
    void Update()
    {
    }
}
