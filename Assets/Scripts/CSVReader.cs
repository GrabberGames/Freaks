using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class CSVReader : MonoBehaviour
{

    

   public struct characterSkill
    {
        public int character;
        public char key;
        public string name_kr;
        public string name_en;
        public int coolTime;
        public string info;
        public string info2;
        public int cal1;
        public float cal2;
        public string cal3;

    };



    public characterSkill[] skillInfos;
    

    // Start is called before the first frame update
    void Start()
    {       
        test();
    }
    
    void test()
    {

      skillInfos = new characterSkill[20];

        StreamReader sr = new StreamReader(Application.dataPath + "/Resources/" + "skillInfo.csv");
        int j = 0;
        bool endOfFile = false;
        while (!endOfFile)
        {
            string data_String = sr.ReadLine();
            if (data_String == null)
            {
                endOfFile = true;
                break;
            }
            var data_values = data_String.Split(',');
           
            if (j > 0)
            {
                skillInfos[j - 1].character = int.Parse(data_values[0]);
                skillInfos[j - 1].key = char.Parse(data_values[1]);
                skillInfos[j - 1].name_kr = data_values[2].ToString();
                skillInfos[j - 1].name_en = data_values[3].ToString();
                skillInfos[j - 1].coolTime = int.Parse(data_values[4]);
                skillInfos[j - 1].info = data_values[5].ToString();
                skillInfos[j - 1].info2 = data_values[6].ToString();
                skillInfos[j - 1].cal1 = int.Parse(data_values[7]);
                skillInfos[j - 1].cal2 = float.Parse(data_values[8]);
                skillInfos[j - 1].cal3 = data_values[9].ToString();
            }
            j++;
        }

        /*
        for (int i = 0; i < skillInfos.Length; i++)
        {
            Debug.Log(skillInfos[i].name_kr);
        }*/
    }
    
}
