using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider slider;

    UnitStats unitStats;

    void Start()
    {
        //unitStats = gameObject.GetComponent<UnitStats>();
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        //slider.value = health;

    }

    public void SetCurrentHealth(float health)
    {
        slider.value = health;
    }

    //void Update()
    //{
    //    SetCurrentHealth(unitStats.currentHealth);
    //}
}
