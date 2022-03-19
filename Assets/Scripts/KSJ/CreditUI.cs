using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditUI : MonoBehaviour
{
    [Header("Element Setting")]
    [SerializeField] private Image backGroundImage;
    [SerializeField] private Text creditText;
    [SerializeField] private GameObject returnButton;

    [Header("Scroll & Fade Out setting")]
    [SerializeField] private float normalSpeed;
    [SerializeField] private float fastSpeed;

    [Range(0.0f, 1.0f)]
    [SerializeField] private float fadeOutSpeed;

    [SerializeField] private float endPosY;

    private IEnumerator creditScroll;
    private IEnumerator fadeOut;

    private RectTransform creditTextTransform;
    private Vector3 normalVelocity;
    private Vector3 fastVelocity;

    private Color bgColor;
    private Color creditTextColor;

    private float tempFloat;
    private bool onFade;

    // Start is called before the first frame update
    private void Awake()
    {
        creditTextTransform = creditText.gameObject.GetComponent<RectTransform>();
        normalVelocity = new Vector3(0.0f, normalSpeed, 0.0f);
        fastVelocity = new Vector3(0.0f, fastSpeed, 0.0f);

        bgColor = backGroundImage.color;
        creditTextColor = creditText.color;
    }

    private void OnEnable()
    {
        creditTextTransform.position = new Vector3(creditTextTransform.position.x , 0.0f, 0.0f);
        backGroundImage.color = bgColor;
        creditText.color = creditTextColor;
        returnButton.SetActive(true);

        onFade = false;

        creditScroll = CreditScroll();
        fadeOut = FadeOut();

        StartCoroutine(creditScroll);
    }
    
    public void Disable()
    {
        Debug.Log(onFade);
        if(gameObject.activeSelf && !onFade)
        {
            StartCoroutine(fadeOut);
        }
    }


    IEnumerator CreditScroll()
    {
        while (true)
        {
            switch (Input.GetKey(KeyCode.Mouse0))
            {
                case true:
                    if(!onFade)
                    {
                        creditTextTransform.position = creditTextTransform.position + (fastVelocity * Time.deltaTime);
                    }
                    else
                    {
                        creditTextTransform.position = creditTextTransform.position + (normalVelocity * Time.deltaTime);
                    }
                    break;

                case false:
                    creditTextTransform.position = creditTextTransform.position + (normalVelocity * Time.deltaTime);
                    break;
            }
            yield return null;

            if(creditTextTransform.position.y > endPosY)
            {
                if(!onFade)
                {                    
                    StartCoroutine(fadeOut);
                }
                break;
            }
        }
    }

    IEnumerator FadeOut()
    {
        onFade = true;
        returnButton.SetActive(false);

        while (true)
        {
            tempFloat = backGroundImage.color.a - (fadeOutSpeed * Time.deltaTime);

            if (tempFloat <= 0)
            {
                gameObject.SetActive(false);
                break;
            }
            else
            {
                backGroundImage.color = new Color(bgColor.r, bgColor.g, bgColor.b, tempFloat);
                creditText.color = new Color(creditTextColor.r, creditTextColor.g, creditTextColor.b, tempFloat);
            }

            yield return null;
        }
    }

    private void OnDisable()
    {
        StopCoroutine(creditScroll);
        StopCoroutine(fadeOut);        
    }

}
