using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
public class InfoUI : MonoBehaviour
{
    [SerializeField] CSVReader csvReader;
    
    [SerializeField] List<Image> skillImages;

    [SerializeField] GameObject largeWindow;
    [SerializeField] Image  skillImage;
    [SerializeField] GameObject mediumWindow;
    [SerializeField] GameObject smallWindow;

    StringBuilder stringBuilder;
    StringBuilder frontAppend;
    StringBuilder backAppend;

    Vector2[] largePos;
    Vector2[] mediumPos;
    Vector2[] smallPos;


    Stat playerStat;
   

    private void Start()
    {
        playerStat = GameManager.Instance.Player.GetComponent<Stat>();
        largePos = new Vector2[5];
        
        largePos[0] = new Vector2(-445, 302);
        largePos[1] = new Vector2(-350, 302);
        largePos[2] = new Vector2(-251, 302);
        largePos[3] = new Vector2(-152, 302);
        largePos[4] = new Vector2(-50, 302);


        mediumPos = new Vector2[4];
        mediumPos[0] = new Vector2(414, 236);
        mediumPos[1] = new Vector2(479, 236);
        mediumPos[2] = new Vector2(582, 236);
        mediumPos[3] = new Vector2(689, 236);

        smallPos = new Vector2[9];
        smallPos[0] = new Vector2(251, 928);
        smallPos[1] = new Vector2(251, 871);
        smallPos[2] = new Vector2(414, 903);
        smallPos[3] = new Vector2(719, 903);
        smallPos[4] = new Vector2(531, 272);
        smallPos[5] = new Vector2(531, 237);
        smallPos[6] = new Vector2(531, 197);
        smallPos[7] = new Vector2(531, 161);
        smallPos[8] = new Vector2(531, 133);

        stringBuilder = new StringBuilder();
        frontAppend = new StringBuilder();
        backAppend = new StringBuilder();


        frontAppend.Append("<b><size=21>");
        backAppend.Append("</size></b> ");


        largeWindow.SetActive(false);
        mediumWindow.SetActive(false);
        smallWindow.SetActive(false);
    }

    public void InActiveInfoUI()
    {
    
            largeWindow.SetActive(false);
     
            mediumWindow.SetActive(false);
       
            smallWindow.SetActive(false);
    }

    int num = 0;
    int calRes;
    public void LargeInfoON(int key)
    {

        if (GameManager.Instance.selectHero == eHeroType.Waron)
            num = 0;
        else if (GameManager.Instance.selectHero == eHeroType.Kyle)
            num = 5;


        //��ų �̹��� ÷��
        if (key == 0)
        {
            skillImage.sprite = skillImages[0].sprite;
        }
        else if (key == 1)
        {
            skillImage.sprite = skillImages[1].sprite;
        }
        else if (key == 2)
        {
            skillImage.sprite = skillImages[2].sprite;
        }
        else if (key == 3)
        {
            skillImage.sprite = skillImages[3].sprite;
        }
        else if (key == 4)
        {
            skillImage.sprite = skillImages[4].sprite;          
        }
        largeWindow.GetComponent<RectTransform>().anchoredPosition = largePos[key];

        stringBuilder.Clear();
        stringBuilder.Append(frontAppend);
        stringBuilder.Append(csvReader.skillInfos[num + key].name_kr);
        stringBuilder.Append(backAppend);
        stringBuilder.Append(csvReader.skillInfos[num + key].name_en);
            
        largeWindow.transform.GetChild(0).GetComponent<Text>().text = stringBuilder.ToString();

        stringBuilder.Clear();
        stringBuilder.Append("��Ÿ��: ");
        stringBuilder.Append(csvReader.skillInfos[num + key].coolTime.ToString());
        stringBuilder.Append("��");
        largeWindow.transform.GetChild(1).GetComponent<Text>().text = stringBuilder.ToString();


        if (csvReader.skillInfos[num + key].key == 'p')
            largeWindow.transform.GetChild(2).GetComponent<Text>().text = "�нú�";
        else
            largeWindow.transform.GetChild(2).GetComponent<Text>().text = csvReader.skillInfos[num + key].key.ToString();

        if(csvReader.skillInfos[num + key].info2 =="0")
            largeWindow.transform.GetChild(3).GetComponent<Text>().text = csvReader.skillInfos[num + key].info;
        else
        {
            if(csvReader.skillInfos[num + key].cal3 == "PD")
            {
                calRes = csvReader.skillInfos[num + key].cal1 + (int)(csvReader.skillInfos[num + key].cal2 * playerStat.PD);
            }
            else
                calRes = csvReader.skillInfos[num + key].cal1 + (int)(csvReader.skillInfos[num + key].cal2 * playerStat.ED);



            stringBuilder.Clear();
            stringBuilder.Append(csvReader.skillInfos[num + key].info);
            stringBuilder.Append(calRes.ToString());
            stringBuilder.Append(csvReader.skillInfos[num + key].info2);


            largeWindow.transform.GetChild(3).GetComponent<Text>().text = stringBuilder.ToString();
        }

        largeWindow.SetActive(true);
    }
  

    public void MediumInfoOn(int key)
    {

        if (key == 0)
        {
            mediumWindow.transform.GetChild(0).GetComponent<Text>().text = "<b><size=21>����</size></b>  (Shop)";
            mediumWindow.transform.GetChild(1).GetComponent<Text>().text = "����â���� �̵�";
            mediumWindow.transform.GetChild(2).GetComponent<Text>().text = "<color=white>������ �ɷ�ġ ��� �� ȭ��Ʈ������ ������ ���� ����â���� �̵��մϴ�. </color>";
        }
        else if (key == 1)
        {
            mediumWindow.transform.GetChild(0).GetComponent<Text>().text = "<b><size=21>����</size></b>  (Alter)";
            mediumWindow.transform.GetChild(1).GetComponent<Text>().text = "���� ��ġ ������ ���� �Ǽ� ����";
            mediumWindow.transform.GetChild(2).GetComponent<Text>().text = "[�ʿ� ���� 600]  <color=white>���͸� ���� ��ġ���� ���ο� ��ġ�� �̵���ŵ�ϴ�. </color>";
        }
        else if (key == 2)
        {
            mediumWindow.transform.GetChild(0).GetComponent<Text>().text = "<b><size=21>ȭ��ƮŸ��</size></b>  (White Tower)";
            mediumWindow.transform.GetChild(1).GetComponent<Text>().text = "ȭ��ƮŸ�� �Ǽ� ����";
            mediumWindow.transform.GetChild(2).GetComponent<Text>().text = "[�ʿ� ���� 500]  <color=white>�����ϴ� ���� �����ϴ� ���� �ǹ��� ȭ��ƮŸ���� �Ǽ��մϴ�. </color>";
        }
        else if (key == 3)
        {
           mediumWindow.transform.GetChild(0).GetComponent<Text>().text = "<b><size=21>��ũ��</size></b>  (Workshop)";
            mediumWindow.transform.GetChild(1).GetComponent<Text>().text = "��ũ�� �Ǽ� ����";
            mediumWindow.transform.GetChild(2).GetComponent<Text>().text = "[�ʿ� ���� 0]  <color=white>���� ä�� �� ����ġ ��ȭ�� ���� ��ũ���� ��ġ�մϴ�. </color>";          
        }
        mediumWindow.GetComponent<RectTransform>().anchoredPosition = mediumPos[key];
        mediumWindow.SetActive(true);

    }

    public void SmallInfoOn(int key)
    {

        if (key == 0)
        {
            smallWindow.transform.GetChild(0).GetComponent<Text>().text = "��Ÿ��";
            smallWindow.transform.GetChild(1).GetComponent<Text>().text = "(Run Time)";
            smallWindow.transform.GetChild(2).GetComponent<Text>().text = "���� ��� �ð�";
            smallWindow.transform.GetChild(3).GetComponent<Text>().text = "���� �÷��� ��� �ð��� �����ݴϴ�. �̴� ���� ���� ��, ������ ������ �ݴϴ�.";          
        }
        else if (key == 1)
        {
            smallWindow.transform.GetChild(0).GetComponent<Text>().text = "���̺� Ÿ��";
            smallWindow.transform.GetChild(1).GetComponent<Text>().text = "(Wave Time)";
            smallWindow.transform.GetChild(2).GetComponent<Text>().text = "���� �������� ���̺긦 ǥ���ϴ� ��";
            smallWindow.transform.GetChild(3).GetComponent<Text>().text = "���� �������� ���̺������ ���� �ð��� �����ݴϴ�.";        
        }
        else if (key == 2)
        {
            smallWindow.transform.GetChild(0).GetComponent<Text>().text = "����";
            smallWindow.transform.GetChild(1).GetComponent<Text>().text = "(Essence)";
            smallWindow.transform.GetChild(2).GetComponent<Text>().text = "���� ������ ������";
            smallWindow.transform.GetChild(3).GetComponent<Text>().text = "���� �÷��̾ ���� ���� ������ �����ݴϴ�.";
        }
        else if (key == 3)
        {
            smallWindow.transform.GetChild(0).GetComponent<Text>().text = "ȭ��Ʈ������";
            smallWindow.transform.GetChild(1).GetComponent<Text>().text = "(White Freaks)";
            smallWindow.transform.GetChild(2).GetComponent<Text>().text = "���� ȭ��Ʈ�������� ������";
            smallWindow.transform.GetChild(3).GetComponent<Text>().text = "���� �÷��̾ ������ ���� ȭ��Ʈ�������� ��� ȭ��Ʈ�������� �����ݴϴ�.";
        }


        //////////////////player stat info
        else if (key == 4)
        {
            stringBuilder.Clear();
            stringBuilder.Append("<color=red>");
            stringBuilder.Append(playerStat.PD);
            stringBuilder.Append("</color>");
            
            smallWindow.transform.GetChild(0).GetComponent<Text>().text = "�������ݷ�";
            smallWindow.transform.GetChild(1).GetComponent<Text>().text = "(Physical Damage)";
            smallWindow.transform.GetChild(2).GetComponent<Text>().text = stringBuilder.ToString();
            smallWindow.transform.GetChild(3).GetComponent<Text>().text = "�ش� ����� �⺻ ���ݷ°� �Ϻ� ��ų�� ���ذ� �����մϴ�.";
        }
        else if (key == 5)
        {
            stringBuilder.Clear();
            stringBuilder.Append("<color=blue>");
            stringBuilder.Append(playerStat.ED);
            stringBuilder.Append("</color>");

            smallWindow.transform.GetChild(0).GetComponent<Text>().text = "�������ݷ�";
            smallWindow.transform.GetChild(1).GetComponent<Text>().text = "(Essence Damage)";
            smallWindow.transform.GetChild(2).GetComponent<Text>().text = stringBuilder.ToString();
            smallWindow.transform.GetChild(3).GetComponent<Text>().text = "�ش� ����� �Ϻ� ��ų ���ذ� �����մϴ�.";
        }
        else if (key == 6)
        {
            smallWindow.transform.GetChild(0).GetComponent<Text>().text = "����";
            smallWindow.transform.GetChild(1).GetComponent<Text>().text = "(Defence)";
            smallWindow.transform.GetChild(2).GetComponent<Text>().text = playerStat.ARMOR.ToString();
            //smallWindow.transform.GetChild(2).GetComponent<Text>().text = $"{playerStat.ARMOR}";
            smallWindow.transform.GetChild(3).GetComponent<Text>().text = "�ش� ����� �����κ��� �Դ� ���ط��� �����մϴ�.";
        }
        else if (key == 7)
        {
            smallWindow.transform.GetChild(0).GetComponent<Text>().text = "�̵��ӵ�";
            smallWindow.transform.GetChild(1).GetComponent<Text>().text = "(Moving Speed)";
            smallWindow.transform.GetChild(2).GetComponent<Text>().text = playerStat.MOVE_SPEED.ToString();
            smallWindow.transform.GetChild(3).GetComponent<Text>().text = "�ش� ����� �� �� �̵��� �� �ִ� �Ÿ��� �����ݴϴ�.";
        }
        else if (key == 8)
        {
            smallWindow.transform.GetChild(0).GetComponent<Text>().text = "���ݼӵ�";
            smallWindow.transform.GetChild(1).GetComponent<Text>().text = "(Attack Speed)";
            smallWindow.transform.GetChild(2).GetComponent<Text>().text = playerStat.ATTACK_SPEED.ToString();
            smallWindow.transform.GetChild(3).GetComponent<Text>().text = "�ش� ����� �� �� ������ �� �ִ� Ƚ���� �����ݴϴ�.";
        }
        smallWindow.GetComponent<RectTransform>().anchoredPosition = smallPos[key];
        smallWindow.SetActive(true);
    }





}
