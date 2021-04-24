using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MistakeHUD : MonoBehaviour
{
    public GameObject mistakeIconObject;

    private List<Image> mistakeImage;
    private int flag = -1;
    public void InitializeMistakePanel(int mistakeNumber)
    {
        if(mistakeImage != null && mistakeImage.Count > 0)
        {
            foreach(var image in mistakeImage)
            {
                Destroy(image.gameObject);
            }
        }

        mistakeImage = new List<Image>();

        while(mistakeNumber > 0)
        {
            mistakeImage.Add(
                Instantiate(mistakeIconObject, this.transform)
                .GetComponent<Image>());

            mistakeNumber--;
        }
    }

    public void CancelOneMistake()
    {
        flag++;
        if(flag >= 0 && flag < mistakeImage.Count)
        {
            mistakeImage[flag].color = Color.grey;
        }
    }

    public void EnableOneMistake()
    {
        flag--;
        if (flag >= 0 && flag < mistakeImage.Count)
        {
            mistakeImage[flag].color = Color.white;
        }
    }
}
