using UnityEngine;
using TMPro;
using System.Collections;

public class Announcer : MonoBehaviour
{
    [System.Serializable]
    public struct AnnouncerData
    {
        public string name;
        public string voiceline;
        public string audioName;
        public float cooldown;
    }

    public AnnouncerData[] announcerData;
    public TextMeshProUGUI textDisplay;
    public RadiusController PointA;
    public RadiusController PointB;
    public RadiusController PointC;
    public HealthSystem redTank;
    public HealthSystem blueTank;

    public int redScore;
    public int blueScore;

    private bool announcementPlaying = false;

    void Update()
    {
        foreach (AnnouncerData data in announcerData)
        {
            if (data.cooldown == 10f && ExecuteAnnouncement(data.name))
            {
                if (!announcementPlaying)
                {
                    StartCoroutine(PlayAnnouncement(data));
                }
            }
        }

        UpdateScores();
    }

    bool ExecuteAnnouncement(string name)
    {
        switch (name)
        {
            case "OnePointBlue":
                return OnePointBlue();
            case "OnePointRed":
                return OnePointRed();
            case "isBlueGonnaWin":
                return isBlueGonnaWin();
            case "isRedGonnaWin":
                return isRedGonnaWin();
            case "RedWins":
                return RedWins();
            case "BlueWins":
                return BlueWins();
            case "tankStartFight":
                return tankStartFight();
            default:
                return false;
        }
    }

    bool OnePointBlue()
    {
        return blueScore == 2;
    }

    bool isBlueGonnaWin()
    {
        if(PointA.blueProgress > 3 && PointB.blueProgress > 3 && PointB.blueProgress > 3)
        {
            return true;
        }
        return false;
    }

    bool isRedGonnaWin()
    {
        if(PointA.redProgress > 3 && PointB.redProgress > 3 && PointB.redProgress > 3)
        {
            return true;
        }
        return false;
    }

    bool OnePointRed()
    {
        return redScore == 2;
    }

    bool RedWins()
    {
        if(blueTank.Destroyed)
        {
            return true;
        }
        return false;
    }

    bool BlueWins()
    {
        if(redTank.Destroyed)
        {
            return true;
        }
        return false;
    }

    bool tankStartFight()
    {
        if(redTank.TookDamage||blueTank.TookDamage)
        {
            return true;
        }
        return false;
    }
    void UpdateScores()
    {
        UpdateScore(PointA);
        UpdateScore(PointB);
        UpdateScore(PointC);

        redScore = Mathf.Clamp(redScore, 0, 3);
        blueScore = Mathf.Clamp(blueScore, 0, 3);
    }

    void UpdateScore(RadiusController point)
    {
        if (point.redPoint && point.captured)
        {
            redScore++;
            blueScore--;
        }
        else if (point.bluePoint && point.captured)
        {
            blueScore++;
            redScore--;
        }
    }

    IEnumerator PlayAnnouncement(AnnouncerData data)
    {
        announcementPlaying = true;

        textDisplay.text = data.voiceline;
        FindObjectOfType<AudioManager>().Play(data.audioName);
        Debug.LogWarning("Announcement played");

        yield return new WaitForSeconds(data.cooldown);

        announcementPlaying = false;
    }
}
