using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerChoose : MonoBehaviour
{
    public Transform PlayerChoosePanel;
    public GameObject ChooseButtonGroup;
    public GameObject StartButtonGroup;
    public List<Player> Players;

    public GameManager gameManager;

    public UnityEvent StartGameEvent;

    private int flag = 0;

    // Update is called once per frame
    void Update()
    {
        bool isLeftArrow = Input.GetKeyDown(KeyCode.LeftArrow);
        bool isRightArrow = Input.GetKeyDown(KeyCode.RightArrow);
        bool isSpace = Input.GetKeyDown(KeyCode.Space);
        if(isLeftArrow)
        {
            TurnLeft();
        }
        else if(isRightArrow)
        {
            TurnRight();
        }
        else if(isSpace)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        Players[flag].transform.parent = null;

        Players[flag].SetPlayer();
        gameManager.SetPlayer(Players[flag]);

        StartGameEvent.Invoke();
        ChooseButtonGroup.SetActive(false);
        StartButtonGroup.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void TurnLeft()
    {
        int preFlag = flag;
        flag = flag - 1 < 0 ? 0 : flag - 1;
        float offset = Players[preFlag].transform.localPosition.x - Players[flag].transform.localPosition.x;
        MovePanel(offset);

    }

    public void TurnRight()
    {
        int preFlag = flag;
        flag = flag + 1 >= Players.Count ? Players.Count - 1 : flag + 1;
        float offset = Players[preFlag].transform.localPosition.x - Players[flag].transform.localPosition.x;
        MovePanel(offset);
    }

    public void MovePanel(float offset)
    {
        Vector3 prePosition = PlayerChoosePanel.position;
        PlayerChoosePanel.position = new Vector3(prePosition.x + offset, prePosition.y, prePosition.z);
    }
}
