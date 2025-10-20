using UnityEngine;
using UnityEngine.XR;
using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
public class Judge : MonoBehaviour
{

    public GameObject card;
    public GameObject currentCard;
    public GameObject juryManager;
    public List<int> requirment = new List<int>();

    private GameObject CHARITY;
    private GameObject HUMILITY;
    private GameObject FAITH;
    private GameObject JUSTICE;

    private void Awake()
    {
        CHARITY = transform.Find("CHARITY").gameObject;
        HUMILITY = transform.Find("HUMILITY").gameObject;
        FAITH = transform.Find("FAITH").gameObject;
        JUSTICE = transform.Find("JUSTICE").gameObject;
        HideIndicators(0);
        for (int i = 0; i < 4; i++)
        {
            requirment.Add(0);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowIndicators(2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayCard()
    {
        currentCard = Instantiate(card);
        currentCard.transform.position = new Vector3(10, -10);
        currentCard.transform.DOMove(new Vector3(0, 3), 1);
        currentCard.GetComponent<Card>().homePosition = new Vector3(0, 3);
        currentCard.GetComponent<SpriteRenderer>().color = Color.yellow;
        currentCard.GetComponent<Card>().Benevolance();
        currentCard.name = "Judge Card";
    }

    public void DestroyCard()
    {
        currentCard.GetComponent<Card>().DestroyCard();
        currentCard = null;
    }

    public void HideIndicators(float time)
    {
        Color tmp;
        tmp = CHARITY.transform.GetComponent<SpriteRenderer>().color;
        tmp.a = 0f;
        CHARITY.transform.GetComponent<SpriteRenderer>().color = tmp;
        tmp = HUMILITY.transform.GetComponent<SpriteRenderer>().color;
        tmp.a = 0f;
        HUMILITY.transform.GetComponent<SpriteRenderer>().color = tmp;
        tmp = FAITH.transform.GetComponent<SpriteRenderer>().color;
        tmp.a = 0f;
        FAITH.transform.GetComponent<SpriteRenderer>().color = tmp;
        tmp = JUSTICE.transform.GetComponent<SpriteRenderer>().color;
        tmp.a = 0f;
        JUSTICE.transform.GetComponent<SpriteRenderer>().color = tmp;
    }

    public void ShowIndicators(float time)
    {
        Color tmp;
        int newRequirment = Random.Range(0,4);
        for (int i = 0; i < requirment.Count; i++)
        {
            requirment[i] = 0;
        }
        requirment[newRequirment] = 1;

        switch(newRequirment)
        {
            case 0:
                tmp = CHARITY.transform.GetComponent<SpriteRenderer>().color;
                tmp.a = 1f;
                CHARITY.transform.GetComponent<SpriteRenderer>().color = tmp;
                break;
            case 1:
                tmp = HUMILITY.transform.GetComponent<SpriteRenderer>().color;
                tmp.a = 1f;
                HUMILITY.transform.GetComponent<SpriteRenderer>().color = tmp;
                break;
            case 2:
                tmp = FAITH.transform.GetComponent<SpriteRenderer>().color;
                tmp.a = 1f;
                FAITH.transform.GetComponent<SpriteRenderer>().color = tmp;
                break;
            case 3:
                tmp = JUSTICE.transform.GetComponent<SpriteRenderer>().color;
                tmp.a = 1f;
                JUSTICE.transform.GetComponent<SpriteRenderer>().color = tmp;
                break;
        }
    }

    public void ApplyEffects()
    {
        currentCard.GetComponent<Card>().ShowText();
        juryManager.GetComponent<JuryManager>().applyEffect(true);
        currentCard.GetComponent<SpriteRenderer>().color = Color.green;
    }
}
