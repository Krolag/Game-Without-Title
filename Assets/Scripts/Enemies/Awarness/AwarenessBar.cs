using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwarenessBar : MonoBehaviour
{
    public Slider Slider;
    public Gradient Gradient;
    public Image Fill;
    public void SetMaxAwareness(float awareness)
    {
        Slider.maxValue = awareness;
        Slider.value = 0;
        Fill.color = Gradient.Evaluate(0f);
    }

    public void setAwareness(float awareness)
    {
        Slider.value = awareness;
        Fill.color = Gradient.Evaluate(Slider.normalizedValue);
    }
}
