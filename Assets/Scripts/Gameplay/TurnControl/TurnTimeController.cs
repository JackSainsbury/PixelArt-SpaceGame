using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnTimeController : MonoBehaviour
{
    [SerializeField]
    private TurnMode currentTurnTimeMode;

    public Sprite spaceSprite;
    public Sprite timerSprite;


    public Transform barContainer;
    public Image pausePanel;

    private float turnTimer = 0;

    private bool paused = false;

    private float pausedTimer = 25f;
    private float runTimer = 5f;

    // Start is called before the first frame update
    void Start()
    {
        switch (currentTurnTimeMode)
        {
            case TurnMode.NoTurn:
                pausePanel.gameObject.SetActive(false);
                break;
            case TurnMode.OnPlayerSpace:
                pausePanel.sprite = spaceSprite;
                break;
            case TurnMode.Timer:
                barContainer.gameObject.SetActive(true);
                pausePanel.sprite = timerSprite;
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(currentTurnTimeMode == TurnMode.NoTurn)
        {
            paused = false;
            turnTimer = 0;
        }
        else if(currentTurnTimeMode == TurnMode.OnPlayerSpace || currentTurnTimeMode == TurnMode.Timer)
        {
            // Skip either
            if (Input.GetKeyDown(KeyCode.Space))
            {
                turnTimer = 0;
                paused = !paused;

                pausePanel.gameObject.SetActive(paused);
            }

            // Timer logic
            if (currentTurnTimeMode == TurnMode.Timer)
            {
                turnTimer += Time.unscaledDeltaTime;

                if (paused)
                {
                    barContainer.transform.localScale = new Vector3(1 - Mathf.Clamp01(turnTimer / pausedTimer), 1, 1);

                    if (turnTimer >= pausedTimer)
                    {
                        turnTimer = 0;
                        paused = false;
                    }
                }
                else
                {

                    if (turnTimer >= runTimer)
                    {
                        turnTimer = 0;
                        paused = true;
                    }
                }
            }

            pausePanel.gameObject.SetActive(paused);
        }

        Time.timeScale = paused ? 0 : 1;
    }
}
