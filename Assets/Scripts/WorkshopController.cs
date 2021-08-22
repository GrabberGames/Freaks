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
    }


    private void Update()
    {
        if (isSetted)
        {
            roofRenderer.material = essenceSpot.GetNowMaterial();
            remainEssense = essenceSpot.GetRemainEssence();
        }
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


    public override void SetOpacity(bool isTransparent)
    {
        // Empty
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
}
