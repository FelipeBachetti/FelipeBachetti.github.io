using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergySlider : MonoBehaviour
{
    public Slider slider;
    public Image fill, border;
    
    public void MaxEnergy(float maxEn){
        slider.maxValue = maxEn;
        slider.value = maxEn;
    }
    
    public void SetEnergy(float energy){
        slider.value = energy;
        fill.color = Color.Lerp(Color.red, Color.green, slider.value / 100);
        border.color = fill.color;
    }
}
