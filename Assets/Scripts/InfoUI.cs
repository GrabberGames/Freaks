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

   

    private void Start()
    {
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
            largeWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-445, 302);
        }
        else if (key == 1)
        {
            skillImage.sprite = skillImages[1].sprite;
            largeWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-350, 302);
        }
        else if (key == 2)
        {
            skillImage.sprite = skillImages[2].sprite;
            largeWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-251, 302);
        }
        else if (key == 3)
        {
            skillImage.sprite = skillImages[3].sprite;
            largeWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-152, 302);
        }
        else if (key == 4)
        {
            skillImage.sprite = skillImages[4].sprite;
            largeWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, 302);
        }


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


        largeWindow.transform.GetChild(3).GetComponent<Text>().text = csvReader.skillInfos[num + key].info;


        largeWindow.SetActive(true);
    }
  

    public void MediumInfoOn(int key)
    {

        if (key == 0)
        {
            mediumWindow.transform.GetChild(0).GetComponent<Text>().text = "<b><size=21>����</size></b>  (Shop)";
            mediumWindow.transform.GetChild(1).GetComponent<Text>().text = "����â���� �̵�";
            mediumWindow.transform.GetChild(2).GetComponent<Text>().text = "<color=white>������ �ɷ�ġ ��� �� ȭ��Ʈ������ ������ ���� ����â���� �̵��մϴ�. </color>";
            mediumWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(414, 236);
        }
        else if (key == 1)
        {
            mediumWindow.transform.GetChild(0).GetComponent<Text>().text = "<b><size=21>����</size></b>  (Alter)";
            mediumWindow.transform.GetChild(1).GetComponent<Text>().text = "���� ��ġ ������ ���� �Ǽ� ����";
            mediumWindow.transform.GetChild(2).GetComponent<Text>().text = "[�ʿ� ���� 600]  <color=white>���͸� ���� ��ġ���� ���ο� ��ġ�� �̵���ŵ�ϴ�. </color>";

            mediumWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(479,236);

        }
        else if (key == 2)
        {
            mediumWindow.transform.GetChild(0).GetComponent<Text>().text = "<b><size=21>ȭ��ƮŸ��</size></b>  (White Tower)";
            mediumWindow.transform.GetChild(1).GetComponent<Text>().text = "ȭ��ƮŸ�� �Ǽ� ����";
            mediumWindow.transform.GetChild(2).GetComponent<Text>().text = "[�ʿ� ���� 500]  <color=white>�����ϴ� ���� �����ϴ� ���� �ǹ��� ȭ��ƮŸ���� �Ǽ��մϴ�. </color>";
            mediumWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(582, 236);
        }
        else if (key == 3)
        {
           mediumWindow.transform.GetChild(0).GetComponent<Text>().text = "<b><size=21>��ũ��</size></b>  (Workshop)";
            mediumWindow.transform.GetChild(1).GetComponent<Text>().text = "��ũ�� �Ǽ� ����";
            mediumWindow.transform.GetChild(2).GetComponent<Text>().text = "[�ʿ� ���� 0]  <color=white>���� ä�� �� ����ġ ��ȭ�� ���� ��ũ���� ��ġ�մϴ�. </color>";
            mediumWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(689, 236);
        }
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

            smallWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(251, 928);
        }
        else if (key == 1)
        {
            smallWindow.transform.GetChild(0).GetComponent<Text>().text = "���̺� Ÿ��";
            smallWindow.transform.GetChild(1).GetComponent<Text>().text = "(Wave Time)";
            smallWindow.transform.GetChild(2).GetComponent<Text>().text = "���� �������� ���̺긦 ǥ���ϴ� ��";
            smallWindow.transform.GetChild(3).GetComponent<Text>().text = "���� �������� ���̺������ ���� �ð��� �����ݴϴ�.";

            smallWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(251, 871);
        }
        else if (key == 2)
        {
            smallWindow.transform.GetChild(0).GetComponent<Text>().text = "����";
            smallWindow.transform.GetChild(1).GetComponent<Text>().text = "(Essence)";
            smallWindow.transform.GetChild(2).GetComponent<Text>().text = "���� ������ ������";
            smallWindow.transform.GetChild(3).GetComponent<Text>().text = "���� �÷��̾ ���� ���� ������ �����ݴϴ�.";

            smallWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(414, 903);
        }
        else if (key == 3)
        {
            smallWindow.transform.GetChild(0).GetComponent<Text>().text = "ȭ��Ʈ������";
            smallWindow.transform.GetChild(1).GetComponent<Text>().text = "(White Freaks)";
            smallWindow.transform.GetChild(2).GetComponent<Text>().text = "���� ȭ��Ʈ�������� ������";
            smallWindow.transform.GetChild(3).GetComponent<Text>().text = "���� �÷��̾ ������ ���� ȭ��Ʈ�������� ��� ȭ��Ʈ�������� �����ݴϴ�.";

            smallWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(719, 903);
        }
        smallWindow.SetActive(true);
    }





}
