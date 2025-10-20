using UnityEngine;
using UnityEngine.EventSystems;

public class UIHover : MonoBehaviour, IPointerEnterHandler
{
    public string actionName;
    
    public GameObject UIBack;
    public GameObject UIUp;
    public GameObject UIDown;
    public GameObject UIJury;
    public cameraManager camManager;
    

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (camManager == null) return;

        switch (actionName)
        {
            case "Down": camManager.lookDown();
                UIDown.SetActive(false);
                UIJury.SetActive(false);
                UIUp.SetActive(true);
                break;

            case "Up": camManager.lookUp();
                UIUp.SetActive(false);
                UIJury.SetActive(true);
                UIDown.SetActive(true);
                break;

            case "Jury": camManager.lookJury();
                UIJury.SetActive(false);
                UIDown.SetActive(false);
                UIBack.SetActive(true);
                break;

            case "Back": camManager.lookBack();
                UIJury.SetActive(true);
                UIDown.SetActive(true);
                UIBack.SetActive(false);
                break;
        }
    }
}
