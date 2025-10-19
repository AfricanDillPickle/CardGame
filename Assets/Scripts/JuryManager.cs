using System.Collections.Generic;
using UnityEngine;

public class JuryManager : MonoBehaviour
{
    public int minSways = 11;

    public List<Juror> jurors = new List<Juror>();

    public List<Juror> charityJurors = new List<Juror>();
    private List<Juror> humilityJurors = new List<Juror>();
    private List<Juror> faithJurors = new List<Juror>();
    private List<Juror> justiceJurors = new List<Juror>();

    private int sways;

    public bool testCheckJury = false;

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

        if (testCheckJury) checkJury();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addJuror(Juror myJuror)
    {
        jurors.Add(myJuror);
    }

    public void checkJury()
    {
        sways = 0;
        foreach(Juror juror in jurors)
        {
            juror.vote();
            if (juror.swayed == true) sways += 1;
        }
        if (sways >= minSways) Debug.Log("round win");
        else Debug.Log("round lose");
    }

}
