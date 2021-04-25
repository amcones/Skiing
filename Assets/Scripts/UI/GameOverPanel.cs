using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
    public Text TitleText;
    public Text ContentText;
    public Text LeftButtonText;
    public Text RightButtonText;

    public void SetTitleText(string title)
    {
        TitleText.text = title;
    }
   
    public void SetContentText(string content)
    {
        ContentText.text = content;
    }

    public void SetLeftButtonText(string leftButton)
    {
        LeftButtonText.text = leftButton;
    }

    public void SetRightButtonText(string rightButton)
    {
        RightButtonText.text = rightButton;
    }

    public void SetText(string title, string content, string leftButton, string rightButton)
    {
        SetTitleText(title);
        SetContentText(content);
        SetLeftButtonText(leftButton);
        SetRightButtonText(rightButton);
    }
}
