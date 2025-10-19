using UnityEngine;
using UnityEngine.XR;
using DG.Tweening;
public class Judge : MonoBehaviour
{

    public GameObject card;
    public GameObject currentCard;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        currentCard.name = "Judge Card";
    }

    public void DestroyCard()
    {
        currentCard.GetComponent<Card>().DestroyCard();
        currentCard = null;
    }
}
