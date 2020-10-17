using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ResourceManagement : MonoBehaviour
{
    [SerializeField]
    private GameObject lifeOne;
    [SerializeField]
    private GameObject lifeTwo;
    [SerializeField]
    private GameObject lifeThree;
    [SerializeField]
    private Text scoreCounter;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerLives.Lives == 2)
        {
            lifeThree.SetActive(false);
        }
        else if (PlayerLives.Lives == 1)
        {
            lifeThree.SetActive(false);
            lifeTwo.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreCounter.text = Score.score.ToString();
    }
}
