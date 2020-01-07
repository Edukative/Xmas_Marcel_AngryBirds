using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ScoreGM : MonoBehaviour
{
    Text Score;
    Text Victory;
    Text GameOver;

    public int score = 0;
    public GameObject[] birdList;
    public int BirdsInLvl = -1;
    public bool AllPigsEliminated;
    //private bool AllBirdsEliminated;
    private bool scoreDone;
    public GameObject AllPigs;
    public Transform GCamera;

    Scene CurrentLvl;
    int numberLvl;

    public int DelayAnotherBird = 0;


    // Start is called before the first frame update
    void Start()
    {

        
        GameObject canvas = GameObject.Find("Canvas");
        
        Score = canvas.transform.Find("Text (TMP)").GetComponent<Text>();
        Victory = canvas.transform.Find("Text_Victory").GetComponent<Text>();
        GameOver = canvas.transform.Find("Text_GameOver").GetComponent<Text>();
        
        Victory.enabled = false;
        GameOver.enabled = false;

        CurrentLvl = SceneManager.GetActiveScene();
        numberLvl = CurrentLvl.buildIndex;

        AllPigs = GameObject.Find("Enemies");
        GCamera = GameObject.Find("Main Camera").GetComponent<Transform>();


        //Debug.Log("level number:" + numberLvl);
        //Debug.Log(SceneManager.sceneCountInBuildSettings - 1);

    }

    // Update is called once per frame
    void Update()
    {

        Score.text = "Score: " + score.ToString();

        AllPigsEliminatedFuncion();

        if (GameObject.FindGameObjectWithTag("Bird") != true && BirdsInLvl <= birdList.Length && !AllPigsEliminated)
        {

            PutAnotherBird();

        }else if(GameObject.FindGameObjectWithTag("Bird") == null && BirdsInLvl >= birdList.Length && !AllPigsEliminated)
        {

            GameOver.enabled = true;
            //AllBirdsEliminated = true;
            
        }

        if (scoreDone)
        {

            if (Input.GetMouseButton(0))
            {

                if (numberLvl == SceneManager.sceneCountInBuildSettings - 1)
                {

                    SceneManager.LoadScene(0);
                    score = 0;

                }
                else
                {

                    SceneManager.LoadScene(numberLvl + 1);
                    score = 0;
                    AllPigsEliminated = false;

                }
            }
        }
    }

    void PutAnotherBird()
    {

        DelayAnotherBird += -1;

        if (DelayAnotherBird <= 0)
        {

            BirdsInLvl += 1;
            Instantiate(birdList[BirdsInLvl], birdList[BirdsInLvl].transform.position, Quaternion.identity);
            GCamera.transform.position = new Vector3(0, 0, -10);
        }

    }

    void AllPigsEliminatedFuncion()
    {

        if(AllPigs.transform.childCount <= 0)
        {

            AllPigsEliminated = true;

            if(AllPigsEliminated && !scoreDone)
            {

                int bird;

                for(bird = BirdsInLvl; bird <= birdList.Length; bird++)
                {

                    score += 10000;

                }

                Victory.enabled = true;
                scoreDone = true;
            }
        }
    }
}
