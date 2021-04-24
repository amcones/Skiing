using System.Text;

public class ScoreData
{
    private long score;
    private long overflow;

    public long Score => score;
    public long Overflow
    {
        get => overflow;
        set
        {
            if (overflow > 0)
                this.overflow = value;
        }
    }

    public ScoreData()
    {
        score = 0;
        overflow = 1000;
    }

    public void AddScore(long score)
    {
        this.score += score;
    }

    public void DecreaseScore(long score)
    {
        this.score -= score;
    }

    public long GetHeadNumber()
    {
        return score / overflow;
    }

    public long GetLastNumber()
    {
        return score % overflow;
    }
}
