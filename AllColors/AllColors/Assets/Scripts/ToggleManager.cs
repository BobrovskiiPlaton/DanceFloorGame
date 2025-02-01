using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ToggleManager : MonoBehaviour
{
    public Toggle toggleRed;
    public Toggle toggleGreen;
    public Toggle toggleBlue;
    private Volume volume;

    private ChannelMixer channelMixer;

    void Start()
    {
        volume = FindObjectOfType<Volume>();
        // Получаем Channel Mixer из Volume
        if (volume.profile.TryGet<ChannelMixer>(out channelMixer))
        {
            toggleRed.onValueChanged.AddListener(delegate { ToggleValueChanged(toggleRed); });
            toggleGreen.onValueChanged.AddListener(delegate { ToggleValueChanged(toggleGreen); });
            toggleBlue.onValueChanged.AddListener(delegate { ToggleValueChanged(toggleBlue); });

            // Инициализируем состояние Toggle при загрузке сцены
            ToggleValueChanged(toggleRed);
            ToggleValueChanged(toggleGreen);
            ToggleValueChanged(toggleBlue);
        }
        else
        {
            Debug.LogError("Channel Mixer не найден в Volume.");
        }
    }

    void ToggleValueChanged(Toggle changedToggle)
    {
        if (changedToggle.isOn)
        {
            SetTogglesInteractable(false, changedToggle);
            ApplyChannelMixerSettings(changedToggle);
        }
        else
        {
            SetTogglesInteractable(true, null);
        }
    }

    void SetTogglesInteractable(bool state, Toggle activeToggle)
    {
        if (activeToggle != toggleRed)
        {
            toggleRed.interactable = state;
            SetToggleTransparency(toggleRed, state);
        }
        if (activeToggle != toggleGreen)
        {
            toggleGreen.interactable = state;
            SetToggleTransparency(toggleGreen, state);
        }
        if (activeToggle != toggleBlue)
        {
            toggleBlue.interactable = state;
            SetToggleTransparency(toggleBlue, state);
        }
    }

    void SetToggleTransparency(Toggle toggle, bool state)
    {
        Color color = toggle.GetComponentInChildren<Text>().color;
        color.a = state ? 1f : 0.5f;
        toggle.GetComponentInChildren<Text>().color = color;
    }

    void ApplyChannelMixerSettings(Toggle activeToggle)
    {
        if (activeToggle == toggleRed)
        {
            channelMixer.redOutRedIn.value = -50f;
            channelMixer.redOutGreenIn.value = 100f;
            channelMixer.redOutBlueIn.value = 100;

            channelMixer.greenOutRedIn.value = 0f;
            channelMixer.greenOutGreenIn.value = 100f;
            channelMixer.greenOutBlueIn.value = 0f;

            channelMixer.blueOutRedIn.value = 0f;
            channelMixer.blueOutGreenIn.value = 0f;
            channelMixer.blueOutBlueIn.value = 100f;
        }
        else if (activeToggle == toggleGreen)
        {
            channelMixer.redOutRedIn.value = 100f;
            channelMixer.redOutGreenIn.value = 0f;
            channelMixer.redOutBlueIn.value = 0f;

            channelMixer.greenOutRedIn.value = 100f;
            channelMixer.greenOutGreenIn.value = -50f;
            channelMixer.greenOutBlueIn.value = 100f;

            channelMixer.blueOutRedIn.value = 0f;
            channelMixer.blueOutGreenIn.value = 0f;
            channelMixer.blueOutBlueIn.value = 100f;
        }
        else if (activeToggle == toggleBlue)
        {
            channelMixer.redOutRedIn.value = 100f;
            channelMixer.redOutGreenIn.value = 0f;
            channelMixer.redOutBlueIn.value = 0f;

            channelMixer.greenOutRedIn.value = 0f;
            channelMixer.greenOutGreenIn.value = 100f;
            channelMixer.greenOutBlueIn.value = 0f;

            channelMixer.blueOutRedIn.value = 100f;
            channelMixer.blueOutGreenIn.value = 100f;
            channelMixer.blueOutBlueIn.value = -50f;
        }
        else
        {
            channelMixer.redOutRedIn.value = 100f;
            channelMixer.redOutGreenIn.value = 0f;
            channelMixer.redOutBlueIn.value = 0f;
            
            channelMixer.greenOutRedIn.value = 0f;
            channelMixer.greenOutGreenIn.value = 100f;
            channelMixer.greenOutBlueIn.value = 0f;
            
            channelMixer.blueOutRedIn.value = 0f;
            channelMixer.blueOutGreenIn.value = 0f;
            channelMixer.blueOutBlueIn.value = 100f;
        }
    }
}