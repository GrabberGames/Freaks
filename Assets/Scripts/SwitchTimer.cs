using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTimer : MonoBehaviour
{
    public bool isTimerON = false;
    Material switchMaterial;
    [SerializeField] private int timer = 90;

    float c_r, c_g, c_b, c_a, f;
    Color c;


    public AudioSource SFXswitchOn;

    private void Update()
    {
        if (isTimerON)
        {
            StartCoroutine(Timer());
            isTimerON = false;
        }
    }

    float startTime;
    //����  Ÿ�̸� ���ľ���!!!!!
    public IEnumerator Timer()
    {
        startTime = Time.time;


        while (true) //���Ŀ� HP���� ���� ���� �� �����Ƿ� �ð� ���� ������
        {

            if (Time.time - startTime >= timer)
                break;

        }



        switchMaterial = transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material;

        SFXswitchOn.Play();
        for (int i = 30; i >= 0; i--)
        {


            f = i / 30.0f;

            c_r = switchMaterial.color.r;
            c_g = switchMaterial.color.g;
            c_b = switchMaterial.color.b;
            c_a = switchMaterial.color.a;
            c = new Color(c_r, c_g, c_b);
            c.a = f;

            switchMaterial.color = c;

            yield return YieldInstructionCache.WaitForSeconds(0.03f);

        }

        yield return null;

    }



}