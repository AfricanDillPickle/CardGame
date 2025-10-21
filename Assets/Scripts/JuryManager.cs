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
                    if (suite == "CUSS")
                    {
                        List<Juror> availableJurors = jurors.FindAll(j => !j.swayed);
                        int swayedCount = 0;

                        for (int i = 0; i < 2 && availableJurors.Count > 0; i++)
                        {
                            if (Random.value <= 0.5f)
                            {
                                Juror randomJuror = availableJurors[Random.Range(0, availableJurors.Count)];
                                randomJuror.sway();
                                availableJurors.Remove(randomJuror);
                                swayedCount++;
                            }
                        }
                    }
                    if (suite == "BLASPHEMY")
                    {
                        List<Juror> availableJurors = new List<Juror>(jurors); // all jurors
                        for (int i = 0; i < 3 && availableJurors.Count > 0; i++)
                        {
                            Juror randomJuror = availableJurors[Random.Range(0, availableJurors.Count)];
                            randomJuror.currProb = Mathf.Min(1f, randomJuror.currProb + 0.5f);
                            availableJurors.Remove(randomJuror);
                        }
                    }


                }
            }
        } else
        {
            string suite = judge.GetComponent<Judge>().currentCard.GetComponent<Card>().suite;
            List<Juror> availableJurors = new List<Juror>(jurors); // all jurors

            switch (suite)
            {
                case "HEAL": // Case 0
                    unSway();
                    break;

                case "CHARITY": // Case 1
                    for (int i = 0; i < 3 && availableJurors.Count > 0; i++)
                    {
                        Juror juror = availableJurors[Random.Range(0, availableJurors.Count)];
                        juror.currProb = Mathf.Max(0, juror.currProb - 0.15f);
                        availableJurors.Remove(juror);
                    }
                    break;

                case "HUMILITY": // Case 2
                    for (int i = 0; i < 2 && availableJurors.Count > 0; i++)
                    {
                        if (Random.value <= 0.5f)
                        {
                            Juror juror = availableJurors[Random.Range(0, availableJurors.Count)];
                            juror.currProb = Mathf.Max(0, juror.currProb - 0.25f);
                            availableJurors.Remove(juror);
                        }
                    }
                    break;

                case "JUSTICE": // Case 3
                    for (int i = 0; i < 10 && availableJurors.Count > 0; i++)
                    {
                        Juror juror = availableJurors[Random.Range(0, availableJurors.Count)];
                        juror.currProb = Mathf.Max(0, juror.currProb - 0.05f);
                        availableJurors.Remove(juror);
                    }
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
