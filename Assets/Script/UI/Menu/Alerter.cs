using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Alerter : MonoBehaviour
{
    [SerializeField]
    private GameObject textObject;
    [SerializeField]
    private TextMeshProUGUI alertText;

    private bool show = false;
    private float timeToShow;
    private float timeElapsed;

    private void Update()
    {
        textObject.SetActive(show);

        if (timeElapsed > timeToShow) {
            show = false;
            timeElapsed = 0;
        } else {
            timeElapsed += Time.deltaTime;
        }
    }

    public void ShowAlert(string msg, float time = 2f)
    {
        alertText.text = msg;
        timeToShow = time;
        show = true;
        timeElapsed = 0;
    }
}
