using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Juror : MonoBehaviour
{
    public float defaultProb = 0.2f;
    public float currProb;

    public bool swayed = false;
    public string suit;


    private GameObject targetChild;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        targetChild = transform.Find("swayed").gameObject;
        targetChild.SetActive(true);
    }
    void Start()
    {
        JuryManager juryManager = FindFirstObjectByType<JuryManager>();
        juryManager.addJuror(this);
        currProb = defaultProb;

    }

    // Update is called once per frame
    void Update()
    {
        if (currProb >= 1f)
        {
            sway();
            currProb = 0.99f;
        }
        targetChild.GetComponent<SpriteRenderer>().color = new Color(0f, 255f, 0f, currProb);
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
            
        }
        else Debug.Log("ERROR: already swayed");


    }
}
