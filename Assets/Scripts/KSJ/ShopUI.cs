using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    GameObject go;

    [Header("스탯증가량")]
    [SerializeField] private float PD;
    [SerializeField] private float ED;
    [SerializeField] private float HP;
    [SerializeField] private float Armor;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float AttackSpeed;
    [SerializeField] private float createWhiteFreaksTime;

    [Header("TextSetting")]
    [SerializeField] private Text textPD;
    [SerializeField] private Text textED;
    [SerializeField] private Text textHP;
    [SerializeField] private Text textArmor;
    [SerializeField] private Text textMoveSpeed;
    [SerializeField] private Text textAttackSpeed;
    [SerializeField] private Text textCreateCount;
    [SerializeField] private Text textFreaksCount;

    [Header("EssenceTextSetting")]
    [SerializeField] private Text textStatPrice;
    [SerializeField] private Text textUnitPrice;
    [SerializeField] private Text textNowEssence;

    [Header("CreateProgress")]
    [SerializeField] private Image progressBar;

    private StringBuilder stringBuilder = new StringBuilder();

    private int statPrice = 200;
    private int unitPrice = 200;

    private IEnumerator essenceCHK;
    private void Awake()
    {
        go = gameObject;
        go.SetActive(false);
    }

    private void Start()
    {
        StageManager.Instance.SetCreateCompleteUIAction(SetTextCreateCount);  
    }

    public void ActiveShopUI()
    {
        go.SetActive(!go.activeSelf);

        if (go.activeSelf)
        {
            if (essenceCHK != null)
            {
                StopCoroutine(essenceCHK);
                essenceCHK = null;
            }

            Init();
            essenceCHK = EssenceCHK();
            StartCoroutine(essenceCHK);
        }
    }

    public void ActiveShopUI(bool value)
    {
        if (!go.activeSelf.Equals(value))
            go.SetActive(value);

        if (value)
        {
            if (essenceCHK != null)
            {
                StopCoroutine(essenceCHK);
                essenceCHK = null;
            }

            Init();
            essenceCHK = EssenceCHK();
            StartCoroutine(essenceCHK);
        }
    }

    private void Init()
    {
        textPD.text = StatStringBuilder(GameManager.Instance.models.playerModel.PlayerPD, PD);
        textED.text = StatStringBuilder(GameManager.Instance.models.playerModel.PlayerED, ED);
        textHP.text = StatStringBuilder(GameManager.Instance.models.playerModel.PlayerMaxHp, HP);
        textArmor.text = StatStringBuilder(GameManager.Instance.models.playerModel.PlayerArmor, Armor);
        textMoveSpeed.text = StatStringBuilder(GameManager.Instance.models.playerModel.PlayerMoveSpeed, MoveSpeed);
        textAttackSpeed.text = StatStringBuilder(GameManager.Instance.models.playerModel.PlayerAttackSpeed, AttackSpeed);

        textCreateCount.text = CountStringBuilder(StageManager.Instance.createFreaksCount);
        textFreaksCount.text = WhiteFreaksManager.Instance.allFreaksCount.ToString();
    }

    private string StatStringBuilder(float stat, float addValue)
    {
        stringBuilder.Clear();
        stringBuilder.Append(stat.ToString());
        stringBuilder.Append("(+");
        stringBuilder.Append(addValue.ToString());
        stringBuilder.Append(")");

        return stringBuilder.ToString();
    }

    private string CountStringBuilder(float value)
    {
        stringBuilder.Clear();
        stringBuilder.Append("x ");
        stringBuilder.Append(value.ToString());

        return stringBuilder.ToString();
    }

    private void SetTextCreateCount()
    {
        textCreateCount.text = CountStringBuilder(StageManager.Instance.createFreaksCount);
    }

    #region 구매버튼 구현
    public void BuyPD()
    {
        if (StageManager.Instance.ChkEssence(statPrice))
        {
            GameManager.Instance.models.playerModel.IncreaseStatus(StatusType.PD, PD);
            StageManager.Instance.UseEssence(statPrice);
            textPD.text = StatStringBuilder(GameManager.Instance.models.playerModel.PlayerPD, PD);
        }
        else
        {
            SystemMassage.Instance.PrintSystemMassage("필요한 정수가 부족합니다.");
        }
    }

    public void BuyED()
    {
        if (StageManager.Instance.ChkEssence(statPrice))
        {
            GameManager.Instance.models.playerModel.IncreaseStatus(StatusType.ED, ED);
            StageManager.Instance.UseEssence(statPrice);
            textED.text = StatStringBuilder(GameManager.Instance.models.playerModel.PlayerED, ED);
        }
        else
        {
            SystemMassage.Instance.PrintSystemMassage("필요한 정수가 부족합니다.");
        }
    }

    public void BuyHP()
    {
        if (StageManager.Instance.ChkEssence(statPrice))
        {
            GameManager.Instance.models.playerModel.IncreaseStatus(StatusType.HP, HP);
            StageManager.Instance.UseEssence(statPrice);
            textHP.text = StatStringBuilder(GameManager.Instance.models.playerModel.PlayerMaxHp, HP);
        }
        else
        {
            SystemMassage.Instance.PrintSystemMassage("필요한 정수가 부족합니다.");
        }
    }
    public void BuyArmor()
    {
        if (StageManager.Instance.ChkEssence(statPrice))
        {
            GameManager.Instance.models.playerModel.IncreaseStatus(StatusType.ARMOR, Armor);
            StageManager.Instance.UseEssence(statPrice);
            textArmor.text = StatStringBuilder(GameManager.Instance.models.playerModel.PlayerArmor, Armor);
        }
        else
        {
            SystemMassage.Instance.PrintSystemMassage("필요한 정수가 부족합니다.");
        }
    }
    public void BuyMoveSpeed()
    {
        if (StageManager.Instance.ChkEssence(statPrice))
        {
            GameManager.Instance.models.playerModel.IncreaseStatus(StatusType.MOVESPEED, MoveSpeed);
            StageManager.Instance.UseEssence(statPrice);
            textMoveSpeed.text = StatStringBuilder(GameManager.Instance.models.playerModel.PlayerMoveSpeed, MoveSpeed);
        }
        else
        {
            SystemMassage.Instance.PrintSystemMassage("필요한 정수가 부족합니다.");
        }
    }
    public void BuyAttackSpeed()
    {
        if (StageManager.Instance.ChkEssence(statPrice))
        {
            GameManager.Instance.models.playerModel.IncreaseStatus(StatusType.ATTACKSPEED, AttackSpeed);
            StageManager.Instance.UseEssence(statPrice);
            textAttackSpeed.text = StatStringBuilder(GameManager.Instance.models.playerModel.PlayerAttackSpeed, AttackSpeed);
        }
        else
        {
            SystemMassage.Instance.PrintSystemMassage("필요한 정수가 부족합니다.");
        }
    }

    public void BuyWhiteFreaks()
    {
        if (StageManager.Instance.ChkEssence(unitPrice))
        {
            StageManager.Instance.CreateWhiteFreaks(createWhiteFreaksTime);
            StageManager.Instance.UseEssence(unitPrice);
        }
        else
        {
            SystemMassage.Instance.PrintSystemMassage("필요한 정수가 부족합니다.");
        }
    }
    #endregion

    IEnumerator EssenceCHK()
    {
        textStatPrice.text = statPrice.ToString();
        textUnitPrice.text = unitPrice.ToString();

        while (true)
        {
            textFreaksCount.text = WhiteFreaksManager.Instance.allFreaksCount.ToString();
            progressBar.fillAmount = StageManager.Instance.GetCreateWhiteFreaksProgress();
            //textCreateCount.text = CountStringBuilder(StageManager.Instance.createFreaksCount);
            //textNowEssence.text = StageManager.Instance.essence.ToString();

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                ActiveShopUI(false);
            }

            yield return null;
        }
    }

    private void OnDisable()
    {
        if (essenceCHK != null)
        {
            StopCoroutine(essenceCHK);
            essenceCHK = null;
        }
    }
}
