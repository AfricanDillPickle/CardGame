using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Juror : MonoBehaviour
{
    public Juror self;
    public float defaultProb = 0.2f;
    private float currProb;

    public bool swayed = false;
    public string suit;


    private GameObject targetChild;
    public bool swayTest = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        JuryManager juryManager = FindFirstObjectByType<JuryManager>();
        juryManager.addJuror(this);
        targetChild = transform.Find("swayed").gameObject;
        currProb = defaultProb;

        if (swayTest) sway();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void vote()
    {
        float randFloat = Random.value;
        if (randFloat <= currProb) sway();
    }

    public void sway()
    {
        if (swayed != true)
        {
            swayed = true;
            targetChild.SetActive(true);

            swayTest = false;
        }
        else Debug.Log("ERROR: already swayed");

        swayTest = false;

    }
}
