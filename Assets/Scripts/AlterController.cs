using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AlterController : Stat, InterfaceRange, ITarget
{

    [SerializeField] private GameObject VFXAlterDestroy;
    [SerializeField] private AudioSource SFXAlterDestroy;
    [SerializeField] private AudioSource SFXAlterComplete;
    
    [SerializeField] private GameObject circle;
    public void OpenCircle()
    {
        circle.SetActive(true);
    }

    public void CloseCircle()
    {
        circle.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    OpenCircle();
                }
                else
                {
                    CloseCircle();
                }
            }
        }
    }


    private GameObject BuildRange;

    GameObject go;

    protected override void Init()
    {
        base.Init();
        go = this.gameObject;

    }

    private void Start()
    {
        Init();
        BuildRange = transform.GetChild(2).gameObject;
        BuildRange.SetActive(false);
        transform.GetChild(4).gameObject.SetActive(false);
        SetHpBar();
 
    }
 
    bool isFirst = true;
    public override void DeadSignal()
    {
        if (isFirst == true)
        {
            if (HP <= 0)
            {
                StartCoroutine(AlterDestroy());

            }
            isFirst = false;
        }
    }

    IEnumerator AlterDestroy()
    {
        SFXAlterDestroy.Play();
        VFXAlterDestroy.SetActive(true);
        yield return YieldInstructionCache.WaitForSeconds(2.0f);
        go.transform.GetChild(0).gameObject.SetActive(false);
        yield return YieldInstructionCache.WaitForSeconds(1.8f);
        go.SetActive(false);
        GameManager.Instance.DefeatShow();
 
    }
  
    public void BuildingRangeON(bool check)
    {
        if (check)
            BuildRange.SetActive(true);
        else
            BuildRange.SetActive(false);
    }


    public GameObject hpBar;
    public Vector3 hpBarOffset = new Vector3(0, 5, 0);
    private RectTransform rect;
    private Image hpBarImage;
    private Text hpBarText;
    void SetHpBar()
    {
        hpBar = BarPooling.instance.GetObject(BarPooling.bar_name.ally_bar);
        rect = (RectTransform)hpBar.transform;
        rect.sizeDelta = new Vector2(140, 21);
        hpBarImage = hpBar.GetComponentsInChildren<Image>()[1];
        hpBarImage.rectTransform.sizeDelta = rect.sizeDelta;
        hpBarText = hpBar.GetComponentsInChildren<Text>()[0];
        hpBarText.text = HP.ToString();
        var _hpbar = hpBar.GetComponent<HpBar>();
        _hpbar.target = this.gameObject;
        _hpbar.offset = hpBarOffset;
        _hpbar.what = HpBar.targets.Alter;
    }


    public override void OnAttackSignal()
    {
        hpBarImage.fillAmount = HP / MAX_HP;
        if (HP >= 0)
            hpBarText.text = HP.ToString();
    }

}