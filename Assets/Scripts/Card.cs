using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Card : MonoBehaviour
{
    public Vector3 homePosition;
    public bool selected = false;
    public string description = "";
    public string type = "";
    public string suite = "";
    public Color defaultColor = Color.white;
    public List<int> requirment = new List<int>();
    public GameObject player;

    private float time = 0;
    private bool kill;
    private bool shake = false;

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            requirment.Add(0);
        }
        player = GameObject.Find("Player");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideText();
    }

    // Update is called once per frame
    void Update()
    {
        if (type == "SIN" && selected == true)
        {
            if (checkRequirments())
            {
                if (!DOTween.IsTweening(GetInstanceID(), true))
                {
                    transform.position = new Vector3(homePosition.x, homePosition.y+1);
                }
                shake = false;
                player.GetComponent<AudioManager>().StopPlaying("SIN");
            } else
            {
                if (shake == false)
                {
                    player.GetComponent<AudioManager>().Play("SIN",2);
                    Debug.Log("Play");
                }
                shake = true;
            }
        }
        if (shake == true && selected == false)
        {
            player.GetComponent<AudioManager>().StopPlaying("SIN");
            shake = false;
        }
        if (shake == true && !DOTween.IsTweening(GetInstanceID(), true) && selected == true) 
        {
            transform.position = new Vector3(homePosition.x, homePosition.y + 1 + Random.Range(0f, 0.1f));
        }
        if (kill)
        {
            time += Time.deltaTime;
            if (time >= 1)
            {
                Destroy(gameObject);
            }
        }
    }

    public void ShowText()
    {
        TextMeshPro textBox = transform.GetChild(0).GetComponent<TextMeshPro>();
        textBox.text = description;
    }
    public void HideText()
    {
        TextMeshPro textBox = transform.GetChild(0).GetComponent<TextMeshPro>();
        textBox.text = "";
    }

    public void DestroyCard()
    {
        HideText();
        GetComponent<SpriteRenderer>().DOFade(0, 1);
        kill = true;
    }

    public void DetermineType()
    {
        int chance = Random.Range(0,10);
        if (chance >= 3 && chance <= 10)
        {
            type = "VIRTUE";
            chance = Random.Range(0,4);
            defaultColor = Color.white;
            switch (chance)
            {
                case 0:
                    suite = "CHARITY";
                    description = suite;
                    GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(suite);
                    break;
                case 1:
                    suite = "HUMILITY";
                    description = suite;
                    GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(suite);
                    break;
                case 2:
                    suite = "FAITH";
                    description = suite;
                    GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(suite);
                    break;
                case 3:
                    suite = "JUSTICE";
                    description = suite;
                    GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(suite);
                    break;
            }
        } else
        {
            type = "SIN";
            //defaultColor = Color.red;
            chance = Random.Range(0, 4);

            switch (chance)
            {
                case 0:
                    suite = "HARM";
                    description = "HARM: 10% INCREASE ON ALL JURORS";
                    requirment[3] = 1;
                    break;
                case 1:
                    suite = "STEAL";
                    description = "STEAL: SWAYS AN EXTRA RANDOM JUROR";
                    requirment[0] = 1;
                    break;
                case 2:
                    suite = "CUSS";
                    description = "CUSS: 50% CHANCE TO SWAY TWO JURORS";
                    requirment[1] = 1;
                    break;
                case 3:
                    suite = "BLASPHEMY";
                    description = "BLASPHEMY 50% INCREASE ON 3 RANDOM JURORS";
                    requirment[2] = 1;
                    break;
                default:
                    suite = "UNKNOWN";
                    break;
            }
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(suite);

        }
        GetComponent<SpriteRenderer>().color = defaultColor;
    }

    public bool checkRequirments()
    {
        Player playerScript = player.GetComponent<Player>();
        if (playerScript.selectedCards[0] >= requirment[0] && playerScript.selectedCards[1] >= requirment[1] && playerScript.selectedCards[2] >= requirment[2] && playerScript.selectedCards[3] >= requirment[3])
        {
            return true;
        }
        return false;
    }

    public void infect()
    {
        type = "SIN";
        defaultColor = Color.red;
        GetComponent<SpriteRenderer>().color = defaultColor;
        switch (suite)
        {
            case "CHARITY":
                requirment[0] = 1;
                description = "REQUIRES CHARITY";
                break;
            case "HUMILITY":
                requirment[1] = 1;
                description = "REQUIRES HUMILITY";
                break;
            case "FAITH":
                requirment[2] = 1;
                description = "REQUIRES FAITH";
                break;
            case "JUSTICE":
                requirment[3] = 1;
                description = "REQUIRES JUSTICE";
                break;
            default:
                break;
        }
        suite = "EMPTY SIN";
    }

    public void Benevolance()
    {
        int chance = Random.Range(0, 4);
        type = "BENEVOLANCE";
        description = suite;
        switch (chance)
        {
            case 0:
                description = "UNSWAYS ONE JUROR";
                suite = "HEAL";
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(suite);
                break;
            case 1:
                description = "REDUCES 3 JURORS BY 15%";
                suite = "CHARITY";
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("HEAL");
                break;
            case 2:
                description = "50% TO REDUCE 2 JURORS BY 25%";
                suite = "HUMILITY";
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("HEAL");
                break;
            case 3:
                description = "DECREASES 10 JURORS BY 5%";
                suite = "JUSTICE";
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("HEAL");
                break;
        }
    }
}
