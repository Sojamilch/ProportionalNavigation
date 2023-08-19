using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MissileSpawner : MonoBehaviour
{


    [SerializeField]
    public GameObject prefab;

    [SerializeField]
    public Transform spawner;

    [SerializeField]
    public Transform target; 



    public void SpawnMissile()
    {
        GameObject Missile = Instantiate(prefab);

        Missile.transform.position = spawner.position;
        Missile.transform.LookAt(target);

        Missile MissileScript = Missile.GetComponent<Missile>();

        ConfigureMissile(MissileScript);

        MissileScript.fire = true;
    }

    float GetSliderValue(string sliderName)
    {
        float value = GameObject.Find(sliderName).GetComponent<Slider>().value;

        return value;

    }

    void ConfigureMissile(Missile missileScript)
    {
        float proxyFuse = GetSliderValue("Proxy Fuse");
        float acceleration = GetSliderValue("Acceleration");
        float maxSpeed = GetSliderValue("Max Speed");
        float turnRate = GetSliderValue("Turn Rate");

        TMP_Dropdown dropdown = GameObject.Find("Guidance Modes").GetComponent<TMP_Dropdown>();
        

        GuidanceModes guidanceMode = (GuidanceModes)Enum.Parse(typeof(GuidanceModes), dropdown.options[dropdown.value].text);

        missileScript.target = target;
        missileScript.fieldOfView = 120;
        missileScript.proximityFuseRange = proxyFuse;
        missileScript.acceleration = acceleration;
        missileScript.maxSpeed = maxSpeed;
        missileScript.turnRate = turnRate;
        missileScript.mode = guidanceMode;
        missileScript.navigationDelay = 0.2f;
    }
}
