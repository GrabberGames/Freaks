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

    [SerializeField]
    private bool fadeOutChk = false;

    private void Start()
    {
        roofRenderer = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>();
    }


    private void Update()
    {
        if (isSetted)
        {
            //roofRenderer.material = essenceSpot.GetNowMaterial();
            SetRoofMaterial();
            remainEssense = essenceSpot.GetRemainEssence();
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == this.gameObject)
                {
                    Destroy();
                }
            }
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
        beConstructed = false;
    }

    private void SetRoofMaterial()
    {
        Material mat = materials[(int)MaterialNum.SmallRoof];
        string materialName = essenceSpot.GetNowMaterial().name;
        if (materialName == "Arrow_Medium (Instance)")
        {
            mat = materials[(int)MaterialNum.MeduimRoof];
        }
        else if(materialName == "Arrow_Large (Instance)")
        {
            mat = materials[(int)MaterialNum.LargeRoof];
        }
        roofRenderer.material = mat;
    }

    private void Destroy()
    {
        Material[] mats = { materials[(int)MaterialNum.SmallRoof], materials[(int)MaterialNum.Leg], materials[(int)MaterialNum.Leg] };

        Material mat = materials[(int)MaterialNum.SmallRoof];
        Color color = mat.color;

        roofRenderer.material.color = new Color(color.r, color.g, color.b, 0);
        print(mat.color.a);
    }
}
