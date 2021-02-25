using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceCD : MonoBehaviour
{
    float time;
    float timer;
    Button button;
    Slider slider;
    private void Start()
    {
        button = GetComponent<Button>();
        slider = GetComponentInChildren<Slider>();
    }
    public void DisablePlace(float time)
    {
        this.time = time;
        slider.value = 1;
        button.enabled = false;
        //StopAllCoroutines();
        StartCoroutine(IETimeCount());
    }
    IEnumerator IETimeCount()
    {
        while (timer < time)
        {
            //Debug.Log("coroutining");
            timer += Time.deltaTime;
            slider.value = 1 - timer / time;
            yield return null;
        }
        timer = 0;
        button.enabled = true;
    }


}
