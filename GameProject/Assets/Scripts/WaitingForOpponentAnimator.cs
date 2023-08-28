using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitingForOpponentAnimator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waitingText;
    [SerializeField] private float animateSpeed = 0.75f;
    
    private string textDisplay;
    private int iterations;
    private void Awake()
    {
        textDisplay = waitingText.text;
    }

    private void OnEnable()
    {
        StartCoroutine(AnimateString());
    }

    private IEnumerator AnimateString()
    {

        yield return new WaitForSeconds(animateSpeed);
        if (iterations == 4)
        {
            waitingText.text = textDisplay;
            iterations = 0;
        }
        else
        {
            iterations++;
            waitingText.text += " .";
        }
        StartCoroutine(AnimateString());
    }

    private void OnDisable()
    {
        waitingText.text = textDisplay;
        StopAllCoroutines();
    }
}
