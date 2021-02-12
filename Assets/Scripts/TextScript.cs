using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScript : MonoBehaviour
{
    public Text ammoCount;
    public Text score;
    public Text crystals;
    public GameObject underLineAmmoText;
    public GameObject ButtonPause;
    public Image ProgressBar;
    public Text BestScore;
    public Text levelText;
    public Text buttonContinueText;
    public Text buttonStartText;
    public Text buttonShopText;
    public Text buttonExitText;
    public Text GameOverScore;
    public Text GameOverCheckPointScore;
    public Text GameOverDoublescoreText;

    public Button buttonContinue;
    public List<Color> colors = new List<Color>();
    public 
    void Start()
    {
        //StartCoroutine(ChangeColor());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator ChangeColor()
    {
        int i = 0; 
        while (true)
        {
            i+=3;
            if (i > colors.Count) i = 0;
            ammoCount.color = colors[i];
            score.color = colors[i];
            ProgressBar.color = colors[i];
            BestScore.color = colors[i];
            buttonContinueText.color = colors[i];
            buttonExitText.color = colors[i];
            buttonShopText.color = colors[i];
            buttonStartText.color = colors[i];
            yield return new WaitForSeconds(0.1f);
        }
    }

    


}
