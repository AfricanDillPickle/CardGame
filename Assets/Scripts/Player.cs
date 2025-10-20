using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using DG.Tweening;
using UnityEngine.Lumin;
using UnityEngine.XR;
using System.Linq;
using TMPro;

public class Player : MonoBehaviour
{
    public GameObject card;
    public List<GameObject> hand = new List<GameObject>();
    public List<GameObject> inPlay = new List<GameObject>();
    public GameObject mainCamera;
    public GameObject play;
    public GameObject Judge;
    public GameObject handCountText;
    public GameObject UiJury;
    public GameObject CameraManager;
    public GameObject resultText;

    public int handCount = 1;

    public List<int> selectedCards = new List<int>();

    public Transform currentCard, previousCard;

    public string state = "HAND";

    private float time = 0;
    private bool infected = false;
    private bool judged = false;

    Ray ray;
    RaycastHit2D hit;

    Vector3 mousePosition;

    void Start()
    {
        GetComponent<AudioManager>().Play("AMBIENCE", 0.8f);
        handCountText.GetComponent<TextMeshPro>().text = "Hand: " + handCount;
        for (int i = 0; i < 4; i++)
        {
            selectedCards.Add(0);
        }
        //Draw First Cards
        for (int i = 0; i < 7; i++)
        {
            hand.Add(Instantiate(card));
            hand[i].transform.position = new Vector3(10, -10);
            hand[i].transform.DOMove(new Vector3((7f) - (i * (2.33f)), 0), 1);
            hand[i].GetComponent<Card>().homePosition = new Vector3((7f) - (i * (2.33f)), 0, 0);
            hand[i].GetComponent<Card>().DetermineType();
            hand[i].name = "Card " + i;
        }
    }

    void Update()
    {
        mousePosition = Mouse.current.position.ReadValue();
        ray = Camera.main.ScreenPointToRay(mousePosition);
        previousCard = currentCard;
        hit = Physics2D.Raycast(ray.origin, ray.direction);
        currentCard = hit ? hit.collider.transform : null;
        //currentCard = hit.collider.GetComponent<GameObject>();
        //
        //hit.transform.position = new Vector3(0f, 0f);
        if (state == "HAND")
        {
            //When the player is looking at their hand
            if (currentCard && currentCard.GetComponent<Card>())
            {
                if (currentCard != previousCard)
                {
                    GetComponent<AudioManager>().Play("CLICK", 2);
                }
                bool selected = currentCard.GetComponent<Card>().selected;
                Vector3 currentPostion = currentCard.GetComponent<Card>().homePosition;
                currentCard.GetComponent<Card>().ShowText();
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    GetComponent<AudioManager>().Play("CLICK", 1);
                    if (!currentCard.GetComponent<Card>().selected)
                    {
                        //Select the card
                        currentCard.GetComponent<SpriteRenderer>().color = Color.green;
                        if (currentCard.GetComponent<Card>().type == "SIN")
                        {
                            for (int i = 0; i < hand.Count(); i++)
                            {
                                if (hand[i].GetComponent<Card>().type == "SIN" && hand[i].GetComponent<Card>().selected == true)
                                {
                                    hand[i].GetComponent<SpriteRenderer>().color = currentCard.GetComponent<Card>().defaultColor;
                                    hand[i].GetComponent<Card>().selected = false;
                                    manageCardSelect(hand[i].GetComponent<Card>().suite, "SUBTRACT");
                                    hand[i].transform.DOKill();
                                    hand[i].transform.DOMove(new Vector3(hand[i].GetComponent<Card>().homePosition.x, hand[i].GetComponent<Card>().homePosition.y), 0.5f);
                                }
                            }
                        }
                        currentCard.GetComponent<Card>().selected = true;
                        manageCardSelect(currentCard.GetComponent<Card>().suite, "ADD");
                        currentCard.DOKill();
                        currentCard.DOMove(new Vector3(currentPostion.x, currentPostion.y + 1f), 0.5f);
                    }
                    else
                    {
                        //Deselect the card
                        currentCard.GetComponent<SpriteRenderer>().color = currentCard.GetComponent<Card>().defaultColor;
                        currentCard.GetComponent<Card>().selected = false;
                        manageCardSelect(currentCard.GetComponent<Card>().suite, "SUBTRACT");
                        currentCard.DOKill();
                        currentCard.DOMove(new Vector3(currentPostion.x, currentPostion.y), 0.5f);
                    }
                }
                else
                {
                    if (!currentCard.GetComponent<Card>().selected)
                    {
                        if (previousCard)
                        {
                            if (previousCard.GetInstanceID() != currentCard.GetInstanceID())
                            {
                                currentCard.GetComponent<SpriteRenderer>().color = Color.grey;
                                currentCard.DOKill();
                                currentCard.DOMove(new Vector3(currentPostion.x, currentPostion.y + 0.5f), 0.3f);
                            }
                        }
                        else
                        {
                            currentCard.GetComponent<SpriteRenderer>().color = Color.grey;
                            currentCard.DOKill();
                            currentCard.DOMove(new Vector3(currentPostion.x, currentPostion.y + 0.5f), 0.3f);
                        }
                    }
                }
                //Debug.Log(currentCard);
                // && currentCard && previousCard.GetInstanceID() != currentCard.GetInstanceID()
            }
            else if (previousCard && previousCard.GetComponent<Card>())
            {
                previousCard.GetComponent<Card>().HideText();
                bool selected = previousCard.GetComponent<Card>().selected;
                if (selected != true)
                {
                    previousCard.GetComponent<SpriteRenderer>().color = previousCard.GetComponent<Card>().defaultColor;
                    Vector3 previousPostion = previousCard.GetComponent<Card>().homePosition;
                    previousCard.DOMove(new Vector3(previousPostion.x, previousPostion.y), 0.5f);
                    //Debug.Log(previousCard);
                }
            }
            else if (currentCard && currentCard.GetComponent<Play>())
            {
                currentCard.GetComponent<SpriteRenderer>().color = Color.grey;
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    CameraManager.GetComponent<cameraManager>().lookDown();
                    UiJury.SetActive(false);
                    currentCard.GetComponent<SpriteRenderer>().color = Color.white;
                    currentCard.transform.DOKill();
                    currentCard.transform.DOMove(new Vector3(0, -10), 1);
                    for (int i = 0; i < hand.Count; i++)
                    {
                        Vector3 currentPostion = hand[i].GetComponent<Card>().homePosition;
                        hand[i].GetComponent<SpriteRenderer>().color = hand[i].GetComponent<Card>().defaultColor;
                        hand[i].GetComponent<Card>().HideText();
                        if (hand[i].GetComponent<Card>().selected)
                        {
                            //print("Select" + hand[i].name);
                            hand[i].GetComponent<Card>().selected = false;
                            hand[i].transform.DOKill();
                            inPlay.Add(hand[i]);
                            hand.RemoveAt(i);
                            i--;
                        }
                        else
                        {
                            //print("Not" + hand[i].name);
                            hand[i].transform.DOKill();
                            //Debug.Log("Move Cards");
                            hand[i].transform.DOMove(new Vector3(currentPostion.x, currentPostion.y - 10f), 1f);
                        }
                    }
                    for (int i = 0; i < inPlay.Count; i++)
                    {
                        inPlay[i].transform.DOMove(new Vector3((7f) - (i * (2.33f)), -2), Random.Range(0.6f, 1f));
                    }
                    Judge.GetComponent<Judge>().HideIndicators(0.5f);
                    state = "PLAY";
                    GetComponent<AudioManager>().Play("CLICK", 0.5f);
                    time = 0;
                    Judge.GetComponent<Judge>().PlayCard();
                }
            }
            else if (previousCard && previousCard.GetComponent<Play>())
            {
                previousCard.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        else if (state == "PLAY")
        {
            time += Time.deltaTime;
            if(time >= 2)
            {
                for (int i = 0; i < Judge.GetComponent<Judge>().requirment.Count; i++)
                {
                    if (Judge.GetComponent<Judge>().requirment[i] > selectedCards[i])
                    {
                        Judge.GetComponent<Judge>().ApplyEffects();
                    }
                }
                judged = true;
            }
            if (time >= 4)
            {
                Judge.GetComponent<Judge>().DestroyCard();
                GetComponent<JuryManager>().inPlay.Clear();
                for (int i = 0; i < inPlay.Count; i++)
                {
                    if (inPlay[i].GetComponent<Card>().type == "SIN")
                    {
                        if (!inPlay[i].GetComponent<Card>().checkRequirments())
                        {
                            Time.timeScale = 0;
                        }
                    }
                    GetComponent<JuryManager>().inPlay.Add(inPlay[i]);

                    inPlay[i].GetComponent<Card>().DestroyCard();
                    inPlay.RemoveAt(i);
                    i--;
                }
                GetComponent<JuryManager>().applyEffect();
                if (handCount <= 2)
                {
                    resetCards();
                    handCount++;
                    handCountText.GetComponent<TextMeshPro>().text = "Hand: " + handCount;
                }
                else
                {
                    CameraManager.GetComponent<cameraManager>().lookJury();
                    mainCamera.transform.DOMove(new Vector3(-18, 0, -10), 2);
                    handCount = 1;
                    handCountText.GetComponent<TextMeshPro>().text = "Hand: " + handCount;
                    GetComponent<JuryManager>().check = true;
                    state = "JUDGE";
                }
                judged = false;
                time = 0;
            }
        }
        else if (state == "TRANSITION TO HAND")
        {
            time += Time.deltaTime;
            if (time >= 1.5f)
            {
                UiJury.SetActive(true);
                Judge.GetComponent<Judge>().ShowIndicators(1f);
                state = "HAND";

            }
        }
        else if (state == "INFECT")
        {
            
            time += Time.deltaTime;
            resultText.GetComponent<TextMeshPro>().text = "INFECT";
            if (time >= 2f && infected == false)
            {
                for (int i = 0; i < hand.Count; i++)
                {
                    if (hand[i].GetComponent<Card>().type == "VIRTUE")
                    {
                        hand[i].GetComponent<Card>().infect();
                        infected = true;
                        i = hand.Count;
                        break;
                    }
                }
                infected = true;
            }
            if (time >= 3f)
            {
                resultText.GetComponent<TextMeshPro>().text = "";
                UiJury.SetActive(true);
                Judge.GetComponent<Judge>().ShowIndicators(1f);
                state = "HAND";
            }
        }
    }
    public void manageCardSelect(string type, string action)
    {
        switch (type) {
            case "CHARITY":
                if (action == "ADD") { selectedCards[0]++;}
                else if (action == "SUBTRACT") { selectedCards[0]--;}
                break;
            case "HUMILITY":
                if (action == "ADD") { selectedCards[1]++; }
                else if (action == "SUBTRACT") { selectedCards[1]--;}
                break;
            case "FAITH":
                if (action == "ADD") { selectedCards[2]++; }
                else if (action == "SUBTRACT") { selectedCards[2]--;}
                break;
            case "JUSTICE":
                if (action == "ADD") { selectedCards[3]++; }
                else if (action == "SUBTRACT") { selectedCards[3]--;}
                break;
            default:
                break;
        }
    }

    public void resetCards(string newState = "NONE")
    {
        CameraManager.GetComponent<cameraManager>().lookBack();
        time = 0;
        if (mainCamera.transform.position != new Vector3(0, 0, -10))
        {
            mainCamera.transform.DOMove(new Vector3(0, 0, -10), 2);
        }
        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].transform.DOKill();
            hand[i].transform.DOMove(new Vector3((7f) - (i * (2.33f)), 0), Random.Range(0.8f, 1.5f));
            hand[i].GetComponent<Card>().homePosition = new Vector3((7f) - (i * (2.33f)), 0);
            hand[i].GetComponent<Card>().selected = false;

        }
        int currentCount = hand.Count;
        if (hand.Count < 7)
        {
            // Draw cards again after play
            while (hand.Count != 7)
            {
                hand.Add(Instantiate(card));
            }
            for (int i = currentCount; i < hand.Count; i++)
            {
                hand[i].transform.position = new Vector3(10, -10);
                hand[i].transform.DOMove(new Vector3((7f) - (i * (2.33f)), 0), Random.Range(1f, 2f));
                hand[i].GetComponent<Card>().homePosition = new Vector3((7f) - (i * (2.33f)), 0);
                hand[i].name = "Card " + i;
                hand[i].GetComponent<Card>().DetermineType();
                hand[i].GetComponent<Card>().selected = false;
            }
        }
        for (int i = 0; i < 4; i++)
        {
            selectedCards[i] = 0;
        }
        play.transform.DOKill();
        play.transform.DOMove(new Vector3(0, -3.85f), 1);
        if (newState == "LOSE") {
            state = "INFECT";
        } else { state = "TRANSITION TO HAND"; }
    }
}