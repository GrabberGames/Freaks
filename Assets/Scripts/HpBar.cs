using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HpBar : MonoBehaviour
{
    public enum targets
    {
        Alter,
        WhiteTower,
        balckTower,
        Workshop,
        Freaks
    }
    public targets what;

     Camera uiCamera;
    [SerializeField]
    private Canvas barCanvas;
    private RectTransform rectParent;
    private RectTransform rectHp;

    [HideInInspector] public Vector3 offset = new Vector3(0, 10f, 0);
    
    [SerializeField] public GameObject target;
    Transform targetTr;
  
    void Start()
    {

        barCanvas = GetComponentInParent<Canvas>();
        uiCamera = barCanvas.worldCamera;
        rectParent = barCanvas.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();
        targetTr = target.transform;
      
    }
 

    Vector3 screenPos;
    Vector2 localPos;

   
    
    private void LateUpdate()
    {
         screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);

        if (screenPos.z < 0.0f)
            screenPos *= -1;

         localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);

        if (what.Equals(targets.Alter))
        {
            localPos.y += 80f;
            rectHp.localPosition = localPos;
        }    
        else if(what.Equals(targets.Freaks))
        {
            localPos.y += 40f;
            rectHp.localPosition = localPos;
        }
        else if (what.Equals(targets.Workshop))
        {
            localPos.y += 40f;
            rectHp.localPosition = localPos;
        }
        else if(what.Equals(targets.WhiteTower))
        {
            localPos.y += 100f;
            rectHp.localPosition = localPos;
        }
        else if (what.Equals(targets.balckTower))
        {
            localPos.y += 100f;
            rectHp.localPosition = localPos;
        }
    }

}
