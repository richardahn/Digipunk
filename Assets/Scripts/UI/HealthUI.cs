using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    // Parameters
    [SerializeField]
    protected CharacterCore character;
    [SerializeField]
    protected float sliderSmoothingFactor = 50f;

    // Dependents
    protected Slider healthbarSlider;

    // Lifecycle
    private void Awake()
    {
        healthbarSlider = GetComponent<Slider>();
    }
    private void Update()
    {
        healthbarSlider.value = Mathf.MoveTowards(healthbarSlider.value, character.GetPercentHealth(), 1f / sliderSmoothingFactor);
    }
}
