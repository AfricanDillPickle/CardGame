using NUnit.Framework.Constraints;
using NUnit.Framework.Internal.Builders;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class JuryManager : MonoBehaviour
{
    public int minSways = 11;

    public List<Juror> jurors = new List<Juror>();

    public List<Juror> charityJurors = new List<Juror>();
    private List<Juror> humilityJurors = new List<Juror>();
    private List<Juror> faithJurors = new List<Juror>();
    private List<Juror> justiceJurors = new List<Juror>();

    public GameObject judge;
    public bool check;

    public List<GameObject> inPlay = new List<GameObject>();

    private int sways;
    private bool juryChecked = false;
    private float time;
    private bool win;

    void Start()
    {

        foreach (Juror juror in jurors)
        {
            switch (juror.suit)
            {
                case "CHARITY":
                    charityJurors.Add(juror);
                    break;
                case "HUMILITY":
                    humilityJurors.Add(juror);
                    break;
                case "FAITH":
                    faithJurors.Add(juror);
                    break;
                case "JUSTICE":
                    justiceJurors.Add(juror);
                    break;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (check)
        {
            time += Time.deltaTime;
            if (time >= 4)
            {
                if (!juryChecked)
                {
                    checkJury();
                }

            }

            if (time >= 5)
            {
                if (win)
                {
                    GetComponent<Player>().resetCards();
                } else { GetComponent<Player>().resetCards("LOSE"); }
                
                check = false;
                juryChecked = false;
                time = 0;
            }
        }
    }

    public void addJuror(Juror myJuror)
    {
        jurors.Add(myJuror);
    }

    public void checkJury()
    {
        sways = 0;
        foreach (Juror juror in jurors)
        {
            juror.vote();
            if (juror.swayed == true) sways += 1;
        }
        if (sways >= minSways)
        {
            Debug.Log("WIN");
            win = true;
        }
        else
        {
            Debug.Log("LOSES");
            win = false;
        }
        juryChecked = true;
    }

    public void applyEffect(bool benevolance = false)
    {
        if (!benevolance)
        {
            foreach (GameObject play in inPlay)
            {
                string type = play.GetComponent<Card>().type;
                string suite = play.GetComponent<Card>().suite;
                Debug.Log(play.name);
                if (type == "VIRTUE")
                {
                    for (int j = 0; j < jurors.Count; j++)
                    {
                        if (jurors[j].swayed == false && jurors[j].suit == suite)
                        {
                            jurors[j].currProb += 0.3f;
                            j = jurors.Count;
                        }
                    }
                }
                else if (type == "SIN")
                {
                    if (suite == "MURDER")
                    {
                        foreach (Juror juror in jurors)
                        {
                            if (juror.currProb <= 0.9f)
                            {
                                juror.currProb += 0.1f;
                            }
                        }
                    }
                    if (suite == "STEAL")
                    {
                        foreach (Juror juror in jurors)
                        {
                            if (juror.swayed == false)
                            {
                                juror.sway();
                                break;
                            }
                        }
                    }

                }
            }
        } else
        {
            string suite = judge.GetComponent<Judge>().currentCard.GetComponent<Card>().suite;
            switch (suite)
            {
                case "HEAL":
                    unSway();
                    break;
            }
        }
    }

    public void unSway()
    {
        foreach (Juror juror in jurors)
        {
            if (juror.swayed == true)
            {
                juror.swayed = false;
                juror.currProb = 0;
                break;
            }
        }
    }
}
