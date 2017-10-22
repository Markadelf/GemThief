using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManagerScript : MonoBehaviour {
    public static int score;
    int levelNum;
    int kills = 0;
    public GameObject bar;
    public GameObject[] hearts;
    public TextMesh timeText;
    float levelStartTime;
    float currentTime;
    static bool active;
	// Use this for initialization
	void Start () {
        active = true;
	}

    // Update is called once per frame
    void Update() {
        if (PlayerContolScript.player != null) {
            bar.transform.localScale = new Vector3(PlayerContolScript.player.GetComponent<PlayerContolScript>().momentum / 180f, .05f, .1f);
            for(int i = 0; i < hearts.Length; i++)
            {
                if(PlayerContolScript.player.GetComponent<DamagableScript>().health > i)
                    hearts[i].GetComponent<SpriteRenderer>().color = new Color(1f, .25f,.25f);
                else
                    hearts[i].GetComponent<SpriteRenderer>().color = Color.black;
            }
            if (active)
                currentTime = Time.timeSinceLevelLoad;
            timeText.text = "" + Mathf.Floor((currentTime - levelStartTime) * 100) /100f;

        }
    }

    void NextLevel()
    {
        float time = Time.timeSinceLevelLoad - levelStartTime;
        int levelScore = 100000 - kills * 100 - (int)(time * 333);
        if (levelScore < 0)
            levelScore = 0;
        score += levelScore;
        levelNum++;
        RestartLevel();
    }

    void RestartLevel()
    {
        levelStartTime = Time.timeSinceLevelLoad;
        SceneManager.LoadScene(levelNum);
        active = true;
    }

    void RegisterKill()
    {
        kills++;
    }

    public static void Freeze()
    {
        active = false;
    }




}
