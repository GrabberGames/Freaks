using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemMassage : MonoBehaviour
{
    enum ePrintSequence
    {
        None ,FadeIn, Print, FadeOut
    }
    
    [Header("Object Setting")]
    [SerializeField] private Image massageFrame;
    [SerializeField] private Text massageText;

    [Header("Setting")]
    [Range(0.0f, 10.0f)]
    [SerializeField] private float massagePrintTime;
    [Range(0.0f, 1.0f)]
    [SerializeField] private float massageFadeTime;

    IEnumerator massagePrint;

    ePrintSequence sequence;

    float startTime;
    float tempTime;


    //singleton
    private static SystemMassage mInstance;
    public static SystemMassage Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<SystemMassage>();
            }
            return mInstance;
        }
    }

    private void Awake()
    {
        SetAlpha(0.0f);
    }

    public void PrintSystemMassage(string text)
    {
        if(massagePrint != null)
        {
            StopCoroutine(massagePrint);
            massagePrint = null;
        }

        massageText.text = text;
        massagePrint = coPrintMassage();
        StartCoroutine(massagePrint);
    }

    private void SetAlpha(float alpha)
    {
        massageFrame.color = new Color(1.0f, 1.0f, 1.0f, alpha);
        massageText.color = new Color(1.0f, 1.0f, 1.0f, alpha);
    }

    IEnumerator coPrintMassage()
    {
        sequence = ePrintSequence.FadeIn;
        startTime = Time.time;

        while (true)
        {
            tempTime = Time.time - startTime;

            switch(sequence)
            {
                case ePrintSequence.FadeIn:
                    if (tempTime < massageFadeTime)
                    {
                        SetAlpha(tempTime / massageFadeTime);
                        yield return null;
                    }
                    else
                    {
                        SetAlpha(1.0f);
                        startTime += massageFadeTime;
                        sequence = ePrintSequence.Print;
                    }
                    break;

                case ePrintSequence.Print:
                    if (tempTime < massagePrintTime)
                    {
                        yield return null;
                    }
                    else
                    {
                        sequence = ePrintSequence.FadeOut;
                        startTime += massagePrintTime;
                    }
                    break;

                case ePrintSequence.FadeOut:
                    if (tempTime < massageFadeTime)
                    {
                        SetAlpha(1.0f - (tempTime / massageFadeTime));
                        yield return null;
                    }
                    else
                    {
                        SetAlpha(0.0f);
                        startTime = 0.0f;
                        sequence = ePrintSequence.None;
                    }
                    break;

                default:
                    break;
            }

            if(sequence.Equals(ePrintSequence.None))
            {
                massagePrint = null;
                break;
            }
        }
    }



}
