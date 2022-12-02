using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
    
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;

    public Image fill;

    public void Start()//пока что так
    {
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    public void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

}
