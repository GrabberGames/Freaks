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
    private GameObject whiteFreeks = null;
    private Renderer roofRenderer;

    private EssenceSpot essenceSpot;
    private bool isSetted;
    [SerializeField] private Material[] materials;

    private float freeksActiveDelay = 2.0f;

    private bool isFreeksEnter = false;
    private bool beConstructed = true;
    private int currentRoofNum;

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

    private void Start()
    {
        currentRoofNum = GetRoofNum();
        roofRenderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isFreeksEnter && collision.transform.gameObject == whiteFreeks)
        {
            isFreeksEnter = true;
            whiteFreeks.SetActive(false);
            StartCoroutine(SetActive());
            StartCoroutine(FreeksEnter());

            if (beConstructed) ConstructBuilding();
        }
    }
    public void Init(GameObject mark)
    {
        essenceSpot = mark.GetComponent<EssenceSpot>();
        this.transform.position = mark.transform.position;
        isSetted = true;
    }
    #region MiningCoroutine
    IEnumerator SetActive()
    {
        yield return new WaitForSeconds(freeksActiveDelay);
        whiteFreeks.SetActive(true);
        essenceSpot.Digging();
    }

    IEnumerator FreeksEnter()
    {
        yield return new WaitForSeconds(freeksActiveDelay + 0.5f);
        isFreeksEnter = false;
    }
    #endregion


    public void ChangeRoof()
    {
        if (!IsRoofChange()) return;

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
        if (roofNum > 2) roofNum = 2;
        return roofNum;
    }

    public override void SetOpacity(bool isTransparent)
    {
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
