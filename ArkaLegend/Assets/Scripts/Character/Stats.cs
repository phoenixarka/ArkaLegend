using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stats : MonoBehaviour {

    private Image content;

    [SerializeField]
    private Text statVal;

    private float currentFill;

    [SerializeField]
    private float lerpSpeed;

    private float maxVal;

    private float currVal;

    public float MaxVal
    {
        get
        {
            return maxVal;
        }

        set
        {
            maxVal = value;
        }
    }

    public float CurrVal
    {
        get
        {
            return currVal;
        }

        set
        {
            if (value > MaxVal)
            {
                currVal = MaxVal;
            }
            else if (value < 0) {
                currVal = 0;
            }
            else
            {
                currVal = value;
            }

            currentFill = currVal / MaxVal;

            if (statVal != null) {
                statVal.text = currVal + " / " + MaxVal;
            }
        }
    }

    // Use this for initialization
    void Start () {
        content = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if (currentFill != content.fillAmount) {
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }

    public void Initialize(float currentValue, float maxValue)
    {
        MaxVal = maxValue;
        CurrVal = currentValue;
        if (content != null) {
            content.fillAmount = CurrVal / MaxVal;
        }
    }
}
