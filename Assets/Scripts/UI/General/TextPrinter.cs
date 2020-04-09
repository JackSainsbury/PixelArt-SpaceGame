using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPrinter : MonoBehaviour
{
    public float fadeInSpeed = 1f;
    public float printSpeed = .05f;
    public TextMeshPro textMesh;
    public SpriteRenderer panelRenderer;

    public string secondString = "Crikey..";

    private string initialText;

    private float printTimer = 0;
    private int len;

    public int state = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == 0)
        {
            state = 1;
            transform.SetParent(GameObject.FindGameObjectWithTag("Ship").transform);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        initialText = textMesh.text;
        len = initialText.Length;

        textMesh.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(state == 1)
        {
            printTimer += Time.deltaTime * fadeInSpeed;

            panelRenderer.color = new Color(1, 1, 1, printTimer);

            if(printTimer >= 1)
            {
                printTimer = 0;
                state = 2;
            }
        }
        else if (state == 2)
        {
            printTimer += Time.deltaTime * printSpeed;

            int count = Mathf.RoundToInt(Mathf.Clamp(printTimer * len, 0, len));

            string subString = initialText.Substring(0, count);

            textMesh.text = subString;

            if(printTimer >= 1)
            {
                printTimer = 0;
                state = 3;
            }
        }
        else if(state == 3)
        {

        }
        else if(state == 4)
        {
            printTimer += Time.deltaTime * printSpeed * 6;

            int count = Mathf.RoundToInt(Mathf.Clamp(printTimer * secondString.Length, 0, secondString.Length));

            string subString = secondString.Substring(0, count);

            textMesh.text = subString;
        }
    }
}
