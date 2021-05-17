using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MistakeHUD : PercentHUD
{
    public Image mistakeImage;

    public override void SetPlayer(Player player)
    {
        base.SetPlayer(player);
        Initialize(player.maxAllowMistakeNumber, player.GetAllowMistakeNumber());
        player.mistakeEvent.AddListener(UpdateHUD);
    }

    public override void UpdateHUD()
    {
        curValue = player.GetAllowMistakeNumber();
        mistakeImage.fillAmount = curValue / maxValue;
    }
}
