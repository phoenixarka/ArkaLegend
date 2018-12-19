using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBook : MonoBehaviour {
    private static SkillBook instance;

    public static SkillBook Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SkillBook>();
            }

            return instance;
        }
    }


    [SerializeField]
    private Image castingBar;

    [SerializeField]
    private Skill[] skills;

    private Image castbar_fill;
    private Image castbar_icon;
    private Text castbar_skillName;
    private Text castbar_timeleft;
    private CanvasGroup castbar_canvasGroup;

    private Coroutine skillCoroutine;
    private Coroutine fadeInCoroutine;


    // Use this for initialization
    void Start () {
        castbar_canvasGroup = castingBar.GetComponent<CanvasGroup>();
        Image[] castingBarImgs = castingBar.GetComponentsInChildren<Image>();
        castbar_fill = castingBarImgs[2];
        castbar_icon = castingBarImgs[3];
        Text[] castingBarTxts = castingBar.GetComponentsInChildren<Text>();
        castbar_skillName = castingBarTxts[0];
        castbar_timeleft = castingBarTxts[1];
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Skill CastSkill(string skillName) {
        Skill skill = Array.Find(skills, x => x.Name == skillName);


        castbar_fill.color = skill.CastbarColor;
        castbar_icon.sprite = skill.Icon;
        castbar_skillName.text = skill.Name;

        castbar_fill.fillAmount = 0;

        skillCoroutine = StartCoroutine(Progress(skill));

        fadeInCoroutine = StartCoroutine(FadeInBar(2.0f));

        return skill;
    }

    private IEnumerator Progress(Skill skill) {
        float timeUsed = Time.deltaTime;

        float maxCastTime = skill.CastTime;

        float percentage = 0.0f;

        while (percentage <= 1) {

            castbar_fill.fillAmount = Mathf.Lerp(0,1, percentage);

            percentage += Time.deltaTime / maxCastTime;

            timeUsed += Time.deltaTime;

            float castTimeLeft = maxCastTime - timeUsed;

            castbar_timeleft.text = (castTimeLeft < 0 ? 0.00f :castTimeLeft).ToString("F2");

            yield return null;
        }

        StopCasting();
    }

    private IEnumerator FadeInBar(float fadeTime) {
        float timeLeft = Time.deltaTime;

        float percentage = 0.0f;

        while (percentage <= 1)
        {
            castbar_canvasGroup.alpha = Mathf.Lerp(0, 1, percentage);

            percentage += Time.deltaTime * fadeTime;

            yield return null;
        }

        castbar_canvasGroup.alpha = 1;
    }

    private IEnumerator FadeOutBar(float fadeTime) {
        float timeLeft = Time.deltaTime;
        float percentage = castbar_canvasGroup.alpha;

        while (percentage >= 0) {
            castbar_canvasGroup.alpha = Mathf.Lerp(0, 1, percentage);

            percentage -= Time.deltaTime * fadeTime;

            yield return null;
        }
        castbar_canvasGroup.alpha = 0;

    }

    public void StopCasting() {
        if (skillCoroutine != null)
        {
            StopCoroutine(skillCoroutine);
            skillCoroutine = null;
        }

        if (fadeInCoroutine != null) {
            StopCoroutine(fadeInCoroutine);
            StartCoroutine(FadeOutBar(4));
        }
    }

    public Skill GetSkill(string skillName) {
        return Array.Find(skills, x => x.Name == skillName);
    }
}
