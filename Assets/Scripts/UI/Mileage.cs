using UnityEngine;

public class Mileage : MonoBehaviour
{
    public Flag MileageFlag;

    public float genFlagMileage = 1000.0f;
    public float appendFlagMileage = 1000.0f;
    public float multiplyFlagMileage = 0.1f;
    public float genFlagOffset = 200.0f;

    Player player;

    public void SetPlayer(Player player)
    {
        this.player = player;
        MileageFlag.SetPlay(player);
    }

    // Update is called once per frame
    public void MileageUpdate()
    {
        if (player != null)
        {
            float offset = genFlagMileage - Mathf.Abs(player.transform.position.y);
            if (offset <= genFlagOffset && offset > 0 && MileageFlag.IsReach)
            {
                MileageFlag.Initialize(-genFlagMileage);
                genFlagMileage += appendFlagMileage;
                appendFlagMileage += appendFlagMileage * multiplyFlagMileage;
            }
        }
    }
}
