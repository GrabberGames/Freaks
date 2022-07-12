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


        //스킬 이미지 첨부
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
        stringBuilder.Append("쿨타임: ");
        stringBuilder.Append(csvReader.skillInfos[num + key].coolTime.ToString());
        stringBuilder.Append("초");
        largeWindow.transform.GetChild(1).GetComponent<Text>().text = stringBuilder.ToString();


        if (csvReader.skillInfos[num + key].key == 'p')
            largeWindow.transform.GetChild(2).GetComponent<Text>().text = "패시브";
        else
            largeWindow.transform.GetChild(2).GetComponent<Text>().text = csvReader.skillInfos[num + key].key.ToString();


        largeWindow.transform.GetChild(3).GetComponent<Text>().text = csvReader.skillInfos[num + key].info;


        largeWindow.SetActive(true);
    }
  

    public void MediumInfoOn(int key)
    {

        if (key == 0)
        {
            mediumWindow.transform.GetChild(0).GetComponent<Text>().text = "<b><size=21>상점</size></b>  (Shop)";
            mediumWindow.transform.GetChild(1).GetComponent<Text>().text = "상점창으로 이동";
            mediumWindow.transform.GetChild(2).GetComponent<Text>().text = "<color=white>영웅의 능력치 향상 및 화이트프릭스 생산을 위한 상점창으로 이동합니다. </color>";
            mediumWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(414, 236);
        }
        else if (key == 1)
        {
            mediumWindow.transform.GetChild(0).GetComponent<Text>().text = "<b><size=21>알터</size></b>  (Alter)";
            mediumWindow.transform.GetChild(1).GetComponent<Text>().text = "알터 위치 변경을 위한 건설 진행";
            mediumWindow.transform.GetChild(2).GetComponent<Text>().text = "[필요 정수 600]  <color=white>알터를 기존 위치에서 새로운 위치로 이동시킵니다. </color>";

            mediumWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(479,236);

        }
        else if (key == 2)
        {
            mediumWindow.transform.GetChild(0).GetComponent<Text>().text = "<b><size=21>화이트타워</size></b>  (White Tower)";
            mediumWindow.transform.GetChild(1).GetComponent<Text>().text = "화이트타워 건설 진행";
            mediumWindow.transform.GetChild(2).GetComponent<Text>().text = "[필요 정수 500]  <color=white>접근하는 적을 공격하는 수비 건물인 화이트타워를 건설합니다. </color>";
            mediumWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(582, 236);
        }
        else if (key == 3)
        {
           mediumWindow.transform.GetChild(0).GetComponent<Text>().text = "<b><size=21>워크샵</size></b>  (Workshop)";
            mediumWindow.transform.GetChild(1).GetComponent<Text>().text = "워크샵 건설 진행";
            mediumWindow.transform.GetChild(2).GetComponent<Text>().text = "[필요 정수 0]  <color=white>정수 채취 및 스위치 정화를 위한 워크샵을 설치합니다. </color>";
            mediumWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(689, 236);
        }
        mediumWindow.SetActive(true);

    }

    public void SmallInfoOn(int key)
    {

        if (key == 0)
        {
            smallWindow.transform.GetChild(0).GetComponent<Text>().text = "런타임";
            smallWindow.transform.GetChild(1).GetComponent<Text>().text = "(Run Time)";
            smallWindow.transform.GetChild(2).GetComponent<Text>().text = "게임 경과 시간";
            smallWindow.transform.GetChild(3).GetComponent<Text>().text = "게임 플레이 경과 시간을 보여줍니다. 이는 게임 종료 시, 점수에 영향을 줍니다.";

            smallWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(251, 928);
        }
        else if (key == 1)
        {
            smallWindow.transform.GetChild(0).GetComponent<Text>().text = "웨이브 타임";
            smallWindow.transform.GetChild(1).GetComponent<Text>().text = "(Wave Time)";
            smallWindow.transform.GetChild(2).GetComponent<Text>().text = "다음 블랙프릭스 웨이브를 표기하는 바";
            smallWindow.transform.GetChild(3).GetComponent<Text>().text = "다음 블랙프릭스 웨이브까지의 남은 시간을 보여줍니다.";

            smallWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(251, 871);
        }
        else if (key == 2)
        {
            smallWindow.transform.GetChild(0).GetComponent<Text>().text = "정수";
            smallWindow.transform.GetChild(1).GetComponent<Text>().text = "(Essence)";
            smallWindow.transform.GetChild(2).GetComponent<Text>().text = "보유 정수를 보여줌";
            smallWindow.transform.GetChild(3).GetComponent<Text>().text = "현재 플레이어가 보유 중인 정수를 보여줍니다.";

            smallWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(414, 903);
        }
        else if (key == 3)
        {
            smallWindow.transform.GetChild(0).GetComponent<Text>().text = "화이트프릭스";
            smallWindow.transform.GetChild(1).GetComponent<Text>().text = "(White Freaks)";
            smallWindow.transform.GetChild(2).GetComponent<Text>().text = "보유 화이트프릭스를 보여줌";
            smallWindow.transform.GetChild(3).GetComponent<Text>().text = "현재 플레이어가 보유한 여유 화이트프릭스와 모든 화이트프릭스를 보여줍니다.";

            smallWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(719, 903);
        }
        smallWindow.SetActive(true);
    }





}
