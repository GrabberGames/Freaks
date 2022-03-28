using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkshopController : Building
{
    private enum MaterialNum
    {
        SmallRoof,
        MeduimRoof,
        LargeRoof,
        Leg,
        Green,
        Red
    }

    public int remainEssense = 1000;

    [SerializeField] private Material[] materials;

    private GameObject whiteFreeks = null;
    private Renderer roofRenderer;
    private EssenceSpot essenceSpot;

    private bool isSetted;
    private bool isFreeksEnter = false;
    private bool beConstructed = true;
    private bool isSwitch = false;
    private float freeksActiveDelay = 2.0f;
    private int currentRoofNum;


    private void Start()
    {
        currentRoofNum = GetRoofNum();
        roofRenderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();

       // StartCoroutine("FadeOut"); //³ªÁß¿¡ fadeout ½ÃÀÛÇÒ¶§!
    }


    private void Update()
    {
        if (isSetted)
        {
            roofRenderer.material = SetRoofMaterial();
            remainEssense = essenceSpot.GetRemainEssence();
        }
    }

    private Material SetRoofMaterial()
    {
        Material material = materials[(int) MaterialNum.SmallRoof];
        Material spotMaterial = essenceSpot.GetNowMaterial();

        if (spotMaterial.name == "Arrow_Small (Instance)")
        {
            material = materials[(int) MaterialNum.SmallRoof];
        }
        else if (spotMaterial.name == "Arrow_Medium (Instance)")
        {
            material = materials[(int)MaterialNum.MeduimRoof];
        }else if (spotMaterial.name == "Arrow_Large (Instance)")
        {
            material = materials[(int)MaterialNum.LargeRoof];
        }

        return material;
    }


    public void SetMiningFreeks(GameObject whiteFreeks)
    {
        this.whiteFreeks = whiteFreeks;
    }


    private void OnCollisionEnter(Collision collision)
    {
        // ¿öÅ©¼¥ °Ç¼³
        if (!isFreeksEnter && collision.transform.gameObject == whiteFreeks)
        {
            isFreeksEnter = true;
            
            whiteFreeks.SetActive(false);
            StartCoroutine(SetActive());
            StartCoroutine(EnterFreeks());

            if (beConstructed)
            {
                ConstructBuilding();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(isSwitch && collision.transform.gameObject == whiteFreeks)
        {
            GameObject.Find("SwitchController").GetComponent<SwitchController>().SwitchFX(gameObject.transform.position, "ON");
            Destroy(this.gameObject);
        }
    }


    public void Init(GameObject mark)
    {
        if(mark.GetComponent<EssenceSpot>())
        {
            essenceSpot = mark.GetComponent<EssenceSpot>();
            this.transform.position = mark.transform.position;
            isSetted = true;
        }
        else
        {
            isSwitch = true;
            beConstructed = true;
        }
    }
    #region MiningCoroutine
    IEnumerator SetActive()
    {
        if(isSwitch)
        {
            yield return new WaitForSeconds(5.0f);
            whiteFreeks.SetActive(true);
            whiteFreeks.GetComponent<WhiteFreaksController>().FinishMining();
        }
        else
        {
            yield return new WaitForSeconds(freeksActiveDelay);
            essenceSpot.Digging();
            whiteFreeks.SetActive(true);
        }
    }

    IEnumerator EnterFreeks()
    {
        if(isSwitch)
        {
            yield return new WaitForSeconds(5.5f);
        }
        else 
        { 
            yield return new WaitForSeconds(freeksActiveDelay + 1.5f);
        }
        isFreeksEnter = false;
    }
    #endregion


    public void ChangeRoof()
    {
        if (!IsRoofChange())
        {
            return;
        }

        currentRoofNum = GetRoofNum();
        roofRenderer.material = materials[currentRoofNum];
    }


    private bool IsRoofChange()
    {
        return currentRoofNum != GetRoofNum();
    }


    private int GetRoofNum()
    {
        int roofNum = remainEssense / 500;

        if (roofNum > 2)
        {
            roofNum = 2;
        }

        return roofNum;
    }


    public override void SetMaterial(bool canBuild)
    {
        if (canBuild)
        {
            Material[] mats = { materials[(int)MaterialNum.Green], materials[(int)MaterialNum.Green], materials[(int)MaterialNum.Green] };
            roofRenderer.materials = mats;
        }
        else
        {
            Material[] mats = { materials[(int)MaterialNum.Red], materials[(int)MaterialNum.Red], materials[(int)MaterialNum.Red] };
            roofRenderer.materials = mats;
        }
    }


    private void ConstructBuilding()
    {
        Material[] mats = { materials[(int)MaterialNum.LargeRoof], materials[(int)MaterialNum.Leg], materials[(int)MaterialNum.Leg] };
        roofRenderer.materials = mats;
        beConstructed = false;
    }




    IEnumerator FadeOut()
    {
        Debug.Log("2ÃÊÈÄ¿¡ fadeout ");
        yield return new WaitForSeconds(2f);
        Debug.Log(" fadeout !! ");

        Material[] materials = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials;


        MeshRenderer mr = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>();
        mr.materials[0].shader = Shader.Find("UI/Unlit/Transparent"); 
        mr.materials[1].shader = Shader.Find("UI/Unlit/Transparent"); 
        mr.materials[2].shader = Shader.Find("UI/Unlit/Transparent");

        // materials[0] : ¿öÅ©¼¥ ÁöºØ , materials[1],[2] : ¿öÅ©¼¥ ±âµÕ
        for (int i = 30; i >= 0; i--)
        {


            float f = i / 30.0f;
            
            for (int j = 0; j < 3; j++)
            {
                float c_r = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[j].color.r;
                float c_g = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[j].color.g;
                float c_b = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[j].color.b;
                float c_a = transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[j].color.a;
                Color c = new Color(c_r, c_g, c_b);
                c.a = f;

                transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[j].color = c;
            }

            yield return new WaitForSeconds(0.04f);
        }
     
    }


}
