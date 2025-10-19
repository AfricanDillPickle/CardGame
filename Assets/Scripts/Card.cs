using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Card : MonoBehaviour
{
    public Vector3 homePosition;
    public bool selected = false;
    public string description = "";

    private float time = 0;
    private bool kill;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideText();
    }

    // Update is called once per frame
    void Update()
    {
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
        GetComponent<SpriteRenderer>().DOFade(0, 1);
        kill = true;
    }
}
