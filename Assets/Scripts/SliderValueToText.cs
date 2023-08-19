using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueToText : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider sliderComponent;
    private TextMeshProUGUI textComponent;
    private string text;


    void Start()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
        text = textComponent.text;
        ShowSliderValue();

    }

    public void ShowSliderValue()
    {


        textComponent.text = text + sliderComponent.value;
    }
}
