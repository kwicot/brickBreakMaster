using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public TextScript textScript;
    
    
    public GameObject[] linePrefab;
    public GameObject[] linesPrefabs;
    public GameObject[] blocksPrefabs;


    public GameObject startPos;
    public GameObject startPosMain;
    public GameObject startPosNext;
    public GameObject point;
    public GameObject ammoPrefab;
    public GameObject canvas;
    public GameObject panelMenu;
    public GameObject buttonReturn;
    public GameObject checkPointText;
    public GameObject panelGameOver;
    public GameObject buttonWachAddCheck;
    public GameObject buttonWachAddDouble;
    public GameObject topLine;
    public GameObject bottomLine;





    public LineRenderer lineRenderer;


    List<GameObject> LatestLines = new List<GameObject>();
    List<GameObject> ChekPoint = new List<GameObject>();
    List<GameObject> lDestroy = new List<GameObject>();
    List<GameObject> lAmmo = new List<GameObject>();
    List<GameObject> lLine = new List<GameObject>();
    List<LineScript> lAllLine = new List<LineScript>();


    List<BlockScript> lBlock = new List<BlockScript>();
    List<BonusScript> lBonus = new List<BonusScript>();


   public List<Color> colors = new List<Color>();


    float _score;
    float _bestScore;
    float _crystals;
    float _crystalsForGame;
    float _experience;
    float _experienceBonuses;
    float _scorebonuses;
    float _timescalegame;
    float _maxAmmo;
    float _timerr;
    float _touchTime = 0;
    float _checkScore;
    float _checkAmmo;

    float _firstSpawnY;
    float _firstSpawnX;

    Vector3 _firstSpawnPosition;


    public float experienceToLevelUp;
    public float experienceToLevelUpStep;
    public float experienceForDestroyedBlock;
    public float experienceBonusAdded;
    public float ammoSpawnInterval;
    public float ammoSpeed;
    public float blockSizeX;
    public float chanceToBonus;
    public float chanceToBlock;



    int _blockHealth;
    int _blockId;
    int _bonusId;
    int _accountLevel;
    int _checkBlockHealth;


    public int blockMaxCount;
    public int bonusMaxCount;


    Vector2 _ammoDirect;
    Vector2 _mousePosition;


    bool _cantime;
    bool _first;
    bool _canShoot;


    public bool isDebug;















    void Start()
    {
        if (isDebug) PlayerPrefs.DeleteAll();
        float step = 0.03f;
        Color newcolor;
        for(float i = 0; i< 1; i += step)
        {
            newcolor = new Color(1, i, 0, 1);
            colors.Add(newcolor);
        }
        for(float i = 1; i > 0; i-= step)
        {
            newcolor = new Color(i, 1, 0, 1);
            colors.Add(newcolor);
        }
        for(float i = 0; i < 1; i+= step)
        {
            newcolor = new Color(0, 1, i, 1);
            colors.Add(newcolor);
        }
        for(float i= 1; i > 0; i-= step)
        {
            newcolor = new Color(0, i, 1, 1);
            colors.Add(newcolor);
        }
        for (float i = 0; i < 1; i+= step)
        {
            newcolor = new Color(i, 0, 1, 1);
            colors.Add(newcolor);
        }
        for(float i = 1;i> 0; i-= step)
        {
            newcolor = new Color(1, 0, i, 1);
            colors.Add(newcolor);
        }
        textScript.colors = colors;
        blockSizeX = Screen.width / 7;
        _firstSpawnPosition = Camera.main.WorldToScreenPoint(topLine.transform.position);
        _firstSpawnPosition.x = blockSizeX / 2;
        Debug.Log(blockSizeX / 2);
        _firstSpawnPosition.y -= blockSizeX / 2;
        _firstSpawnPosition.y -= 5;
        _firstSpawnY = _firstSpawnPosition.y;
        _firstSpawnX = _firstSpawnPosition.x;
        startPosMain.GetComponent<RectTransform>().sizeDelta = new Vector3(blockSizeX, blockSizeX, 0);
        startPos.GetComponent<RectTransform>().sizeDelta = new Vector3(blockSizeX, blockSizeX, 0);
        startPosNext.GetComponent<RectTransform>().sizeDelta = new Vector3(blockSizeX, blockSizeX, 0);
        EnteringGame();

        /*
        string adUnitId = "ca-app-pub-3940256099942544/5224354917";
        this.rewardedAd = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
        */


    }


    void Update()
    {
        //if (Input.GetMouseButton(0))
        if (panelMenu.activeSelf == false)
        {
            InputManager();

            if (_cantime)
            {
                _timerr += Time.deltaTime;
                if (_timerr >= 5)
                {
                    buttonReturn.SetActive(true);
                    Time.timeScale = 2;
                    _timerr = 0;
                    _cantime = false;
                }
            }
        }
    }
    void InputManager()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                _touchTime = 0;
                
            }
            if(touch.phase == TouchPhase.Moved)
            {
                if (_canShoot)
                {
                    _touchTime += Time.deltaTime;
                    Vector2 sPos = startPos.transform.position;
                    _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 newDir = _mousePosition - sPos;
                    float angle = Vector2.Angle(startPosMain.transform.up, Input.mousePosition);
                    _ammoDirect = newDir.normalized;
                    if (startPos.activeSelf)
                    {
                        startPos.transform.LookAt(_mousePosition, new Vector3(0, 0, 1));
                        CalcTraectory(newDir);
                    }
                }
            }
            //if (Input.GetMouseButtonUp(0))
            if (touch.phase == TouchPhase.Ended)
            {
                Debug.Log("Touch end");
                if (_canShoot && _touchTime >= 0.2f)
                    MakeShoot();
                lineRenderer.SetPosition(0, Vector3.zero);
                lineRenderer.SetPosition(1, Vector3.zero);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                _touchTime = 0;
                Debug.Log("Touch began");

            }
            if (Input.GetMouseButton(0))
            {
                if (_canShoot)
                {
                    _touchTime += Time.deltaTime;
                    Vector2 sPos = startPos.transform.position;
                    _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 newDir = _mousePosition - sPos;
                    float angle = Vector2.Angle(startPosMain.transform.up, Input.mousePosition);
                    _ammoDirect = newDir.normalized;
                    if (startPos.activeSelf)
                    {
                        startPos.transform.LookAt(_mousePosition, new Vector3(0, 0, 1));
                        CalcTraectory(newDir);
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Touch end");
                if(_canShoot && _touchTime >= 0.2f)
                MakeShoot();
                lineRenderer.SetPosition(0, Vector3.zero);
                lineRenderer.SetPosition(1, Vector3.zero);
            }
        }
    }

    void CalcTraectory(Vector2 direct)
    {
        Vector2 dir = direct;
        RaycastHit2D hit1;
        RaycastHit2D hit2;
        RaycastHit2D hit3;
        RaycastHit2D hit4;

        hit1 = Physics2D.Raycast(startPos.transform.position, dir,100);
        dir = Vector2.Reflect(dir, hit1.normal);
        hit2 = Physics2D.Raycast(hit1.point, dir,100);
        dir = Vector2.Reflect(dir, hit2.normal);
        hit3 = Physics2D.Raycast(hit2.point, dir,100);
        dir = Vector2.Reflect(dir, hit3.normal);
        hit4 = Physics2D.Raycast(hit3.point, dir,100);


        lineRenderer.SetPosition(0, startPos.transform.position);
        lineRenderer.SetPosition(1, hit1.point);


        //lineRenderer.SetPosition(2, hit3.point);
        //lineRenderer.SetPosition(3, hit4.point);

    }

    IEnumerator Shoot()
    {
        if (_canShoot)
        {
            _cantime = true;
            _timerr = 0;
            _canShoot = false;
            float curammo = _maxAmmo;
            int ID = 0;
            while (curammo > 0)
            {
                Debug.Log(startPos.transform.position);
                GameObject obj = Instantiate(ammoPrefab, startPos.transform.position, Quaternion.identity,canvas.transform);
                obj.GetComponent<AmmoScript>().setParram(_ammoDirect, ammoSpeed, ID);
                obj.GetComponent<AmmoScript>().gameManager = this;
                obj.GetComponent<AmmoScript>().SetSize(blockSizeX);
                curammo--;
                lAmmo.Add(obj);
                if (curammo == 0)
                {
                    startPos.SetActive(false);
                    textScript.ammoCount.gameObject.SetActive(false);
                    textScript.underLineAmmoText.SetActive(false);
                }
                ID++;
                textScript.ammoCount.text = curammo.ToString();
                Debug.Log("Shooted");
                yield return new WaitForSeconds(ammoSpawnInterval);
            }
        }
    }

    public void DestroyedAmmo(Vector3 pos, int id)
    {
        //for(int i = 0; i < lAmmo.Count; i++)
        //{
        //    if(lAmmo[i] == obj)
        //    {
        //        GameObject obj1 = lAmmo[i];
        //        lAmmo.RemoveAt(i);
        //        Destroy(obj1);
        //    }
        //}
        Debug.Log("destroyed ammo start");

        if (_first && id<10000)
        {
            Debug.Log("if first");

            _first = false;
            SettingsSpawnPoints(pos);
            Debug.Log("set spawn point");

        }
        if (lAmmo.Count > 0)
        {
            Debug.Log("if ammo > 0");

            for (int i = 0; i < lAmmo.Count; i++)
            {

                if (lAmmo[i].GetComponent<AmmoScript>().ID == id)
                {
                    Debug.Log("if id == id");

                    GameObject obj = lAmmo[i];
                    lAmmo.RemoveAt(i);
                    Destroy(obj);
                }
            }
        }
        if (lAmmo.Count == 0)
        {
            Debug.Log("if ammo 0");

            bool can = true;
            bool deathLine = false;
            for(int i = 0; i < lBlock.Count; i++)
            {
                Vector3 screenposition = Camera.main.WorldToScreenPoint(lBlock[i].gameObject.transform.position);
                screenposition.y -= blockSizeX;
                if (screenposition.y <= 200)
                {
                    can = false;
                    break;
                }
                else if(screenposition.y <= 200 + blockSizeX && deathLine == false)
                {
                    bottomLine.GetComponent<Image>().color = Color.red;
                    deathLine = true;
                }
                else if ( deathLine == false)
                    bottomLine.GetComponent<Image>().color = Color.white;
            }
            if (can)
            {
                Debug.Log("if can");

                MoveWave();
                textScript.ammoCount.text = _maxAmmo.ToString();
            }
            else
            {
                Debug.Log("else can");

                GameOver();
                Debug.Log("destroyed ammo ended");

            }
        }

    }
    public void DestroyedBlock(GameObject obj)
    {
        Debug.Log("DestroyedBlock");
        int id = obj.GetComponent<BlockScript>().ID;
        Destroy(obj);
        for(int i = 0; i< lBlock.Count; i++)
        {
            if(lBlock[i].ID == id)
            {
                Debug.Log("Find");
                lBlock.RemoveAt(i);
                _score += 100 * 0.3f * _scorebonuses;
                _scorebonuses+=0.1f;
                addExperience();
            }
        }
        if(lBlock.Count == 0)
        {
        }
        textScript.score.text = "Score- " + Mathf.RoundToInt( _score).ToString();
    }

    public void TakedBonus(int id,GameObject obj)
    {
        switch (id)
        {
            case 0:                 //Дополнительный шар
                {
                    _maxAmmo++;
                    break;
                }
            case 1:                 //Щит
                {
                   Destroy(obj);
                    break;
                }
            case 2:                 //Горизонтальный лазер
                {
                    lDestroy.Add(obj);

                    break;
                }
            case 3:                 //Вертикальный лазер
                {
                    lDestroy.Add(obj);

                    break;
                }
            case 4:
                {   
                    lDestroy.Add(obj);
                    //Горизонтальный + вертикальный лазер
                    break;
                }
            case 5:
                {
                    //Кристал
                    _crystals++;
                    _crystalsForGame++;
                    textScript.crystals.text = _crystals.ToString();
                    break;
                }
            case 6:
                {
                    //Двойные шары
                    break;
                }
            case 7:
                {
                    //Рандомное направление
                    break;
                }
        }
        for(int i = 0; i < lBonus.Count; i++)
        {
            if (lBonus[i] == null) lBonus.RemoveAt(i);
            else if (lBonus[i].gameObject == obj) lBonus.RemoveAt(i);
        }
        Destroy(obj);
    }
    void DestroyLines()
    {
        if (lLine.Count > 0) 
        {
            for (int i = 0; i < lLine.Count; i++)
            {
                GameObject obj = lLine[i].gameObject;
                //if (obj != null) 
                //Destroy(obj);
            }
            lLine.Clear();
        }
        if (lBlock.Count > 0)
        {
            for (int i = 0; i < lBlock.Count; i++)
            {
                GameObject obj = lBlock[i].gameObject;
                Destroy(obj);
            }
            lBlock.Clear();
        }
        if (lBonus.Count > 0)
        {
            for (int i = 0; i < lBonus.Count; i++)
            {
                GameObject obj = lBonus[i].gameObject;
                Destroy(obj);
            }
            lBonus.Clear();
        }
    }
    public void LoadCheckPoint()
    {
        
        lBlock.Clear();
        lBonus.Clear();
        _maxAmmo = _checkAmmo;
        _blockHealth = _checkBlockHealth;
        _score = _checkScore;
        DestroyLines();
        startPos.SetActive(true);
        startPosMain.transform.position = startPosNext.transform.position;
        startPosNext.SetActive(false);
        _first = true;

        Vector3 pos = new Vector3(0, 570, 0);
        
        GameObject obj = Instantiate(ChekPoint[0], canvas.transform);
        obj.transform.localPosition = pos;
        BlockScript[] blockScript = obj.GetComponentsInChildren<BlockScript>();
        BonusScript[] bonusScript = obj.GetComponentsInChildren<BonusScript>();
        for (int i = 0; i < blockScript.Length; i++)
        {
            blockScript[i].setParram(_blockHealth, _blockId, this);
            blockScript[i].step = blockSizeX;
            blockScript[i].movePosition();
            lBlock.Add(blockScript[i]);
            _blockId++;
        }
        for (int i = 0; i < bonusScript.Length; i++)
        {
            bonusScript[i].setParram(this);
            bonusScript[i].step = blockSizeX;
            bonusScript[i].movePosition();
            lBonus.Add(bonusScript[i]);
        }
        lLine.Add(obj);
        _canShoot = true;
        _blockHealth++;
        textScript.ammoCount.gameObject.SetActive(true);
        textScript.underLineAmmoText.SetActive(true);
        textScript.ammoCount.text = _maxAmmo.ToString();
        textScript.score.text = Mathf.RoundToInt(_score).ToString();
        textScript.BestScore.text = "Best- " + Mathf.RoundToInt( _bestScore).ToString();
    }

    void SettingsSpawnPoints(Vector3 pos)
    {
        Vector3 newposs = Camera.main.WorldToScreenPoint(bottomLine.transform.position);
        startPosNext.SetActive(true);
        Vector3 newpos = Camera.main.WorldToScreenPoint(pos);
        newpos.y = newposs.y + blockSizeX /4.5f ;
        newpos = Camera.main.ScreenToWorldPoint(newpos);
        startPosNext.transform.position = newpos;
        
    }


    void NewWave()
    {
        startPos.SetActive(true);
        startPosMain.transform.position = startPosNext.transform.position;
        startPosNext.SetActive(false);

        _first = true;

        GameObject newLine = RandomizeLine();
        LineScript script = newLine.GetComponent<LineScript>();
        LineScript copy = script;
        copy._blockHealth = _blockHealth;
        copy._crystalsForGame = _crystalsForGame;
        copy._maxAmmo = _maxAmmo;
        copy._saveScore = _score;
        lAllLine.Add(copy);
        GameObject obj;
        for(int i = 0; i < 7; i++)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(_firstSpawnPosition);
            int id = script.blocksId[i];
            if (id != 0)
                obj = Instantiate(blocksPrefabs[id], canvas.transform);
            else obj = null;
            if (obj != null)
            {
                if (obj.GetComponent<BlockScript>() != null)
                {
                    BlockScript blockScript = obj.GetComponent<BlockScript>();
                    blockScript.setParram(_blockHealth,pos, _blockId, this,blockSizeX);
                    blockScript.step = blockSizeX;
                    blockScript.movePosition();
                    blockScript.SetSize(blockSizeX);
                    lBlock.Add(blockScript);
                    _blockId++;
                }
                else
                {
                    BonusScript bonusScript = obj.GetComponent<BonusScript>();
                    bonusScript.setParram(this, _bonusId,pos, blockSizeX);
                    bonusScript.step = blockSizeX;
                    bonusScript.movePosition();
                    bonusScript.SetSize(blockSizeX);
                    lBonus.Add(bonusScript);
                    _bonusId++;
                }
            }

            _firstSpawnPosition.x += blockSizeX;


           //blockScript[i].setParram(_blockHealth, _blockId, this);
            //blockScript[i].step = blockSizeX;
            //blockScript[i].movePosition();
            //blockScript[i].SetSize(blockSizeX);
            //lBlock.Add(blockScript[i]);
            //_blockId++;
        }
        //for(int i = 0; i < bonusScript.Length; i++)
        //{
        //    bonusScript[i].setParram(this,_bonusId);
        //    bonusScript[i].step = blockSizeX;
        //    bonusScript[i].movePosition();
        //    bonusScript[i].SetSize(blockSizeX);
        //    lBonus.Add(bonusScript[i]);
        //    _bonusId++;
        //}
        // MovePositionsBlocks();

        _firstSpawnPosition.y = _firstSpawnY;
        _firstSpawnPosition.x = _firstSpawnX;
        lLine.Add(newLine);
        _canShoot = true;
        _blockHealth++ ;
        textScript.ammoCount.gameObject.SetActive(true);
        textScript.underLineAmmoText.SetActive(true);
        textScript.ammoCount.text = _maxAmmo.ToString();
        textScript.score.text = Mathf.RoundToInt(_score).ToString();
        textScript.BestScore.text = "Best- " + Mathf.RoundToInt(_bestScore).ToString();

    }

    //void NewWave()
    //{
    //    Debug.Log("New wave");
    //    startPos.SetActive(true);
    //    startPosMain.transform.position = startPosNext.transform.position;
    //    startPosNext.SetActive(false);
    //    _first = true;

    //    Vector3 pos = new Vector3(0, topLine.transform.localPosition.y + blockSizeX / 2, 0);
    //    GameObject newLine = RandomizeLine();
    //    GameObject obj = Instantiate(newLine, canvas.transform);
    //    obj.transform.localPosition = pos;
    //    BlockScript[] blockScript = obj.GetComponentsInChildren<BlockScript>();
    //    BonusScript[] bonusScript = obj.GetComponentsInChildren<BonusScript>();
    //    for (int i = 0; i < blockScript.Length; i++)
    //    {
    //        blockScript[i].setParram(_blockHealth, _blockId, this);
    //        blockScript[i].step = blockSizeX;
    //        blockScript[i].movePosition();
    //        blockScript[i].SetSize(blockSizeX);
    //        lBlock.Add(blockScript[i]);
    //        _blockId++;
    //    }
    //    for (int i = 0; i < bonusScript.Length; i++)
    //    {
    //        bonusScript[i].setParram(this, _bonusId);
    //        bonusScript[i].step = blockSizeX;
    //        bonusScript[i].movePosition();
    //        bonusScript[i].SetSize(blockSizeX);
    //        lBonus.Add(bonusScript[i]);
    //        _bonusId++;
    //    }
    //    MovePositionsBlocks();

    //    lLine.Add(obj);
    //    _canShoot = true;
    //    _blockHealth++;
    //    textScript.ammoCount.gameObject.SetActive(true);
    //    textScript.underLineAmmoText.SetActive(true);
    //    textScript.ammoCount.text = _maxAmmo.ToString();
    //    textScript.score.text = Mathf.RoundToInt(_score).ToString();
    //    textScript.BestScore.text = "Best- " + Mathf.RoundToInt(_bestScore).ToString();
    //}

    //void MovePositionsBlocks(){
    //    Vector3 raystartpos = new Vector3(0,topLine.transform.position.y,0);
    //    Vector3 a = Camera.main.WorldToScreenPoint(raystartpos);
    //    a.x = 0;
    //    a.y -= blockSizeX / 2;
    //    raystartpos = Camera.main.ScreenToWorldPoint(a);
    //    RaycastHit2D[] hits2D = Physics2D.RaycastAll(raystartpos,Vector2.right * 1000);
    //    float pos = blockSizeX / 2;
    //    for(int i = 0;i < hits2D.Length ; i++){
    //        if(hits2D[i].transform.gameObject.GetComponent<BlockScript>() != null || hits2D[i].transform.gameObject.GetComponent<BonusScript>() != null)
    //        {
    //            Vector3 screenposition = Camera.main.WorldToScreenPoint(transform.position);
    //            screenposition.x = pos;
    //            hits2D[i].transform.position = Camera.main.ScreenToWorldPoint(screenposition);
    //        }
    //        pos += blockSizeX;
    //    }
    //    Debug.DrawRay(raystartpos,Vector3.right* 10000,Color.red,10000);
    //}
    GameObject RandomizeLine()
    {
        Debug.Log("Старт метода рандомной волны");

        Debug.Log("создание пустышки");
        GameObject newline;
        Debug.Log("рандом префаб из листа");
        newline = linesPrefabs[UnityEngine.Random.Range(0, linesPrefabs.Length - 1)];
        Debug.Log("цикл проверки");
        for (int i = 0; i < LatestLines.Count; i++)
        {
        Debug.Log("проверка на сходство");
            if (newline.GetComponent<LineScript>().ID == LatestLines[i].GetComponent<LineScript>().ID)
            {
        Debug.Log("Замена префаба");
                newline = linesPrefabs[UnityEngine.Random.Range(0, linesPrefabs.Length - 1)];
                i = -1;
            }
        }
        Debug.Log("Конец цикла");
        Debug.Log("Добавление префаба в лист");
        LatestLines.Add(newline);
        Debug.Log("проверка количества списка последних линий");
        if (LatestLines.Count > 10)
            LatestLines.RemoveAt(0);
        Debug.Log("возврат линии");

        return newline;
    }


    //GameObject RandomizeLine()
    //{
    //    GameObject newline;
    //    newline = linePrefab[UnityEngine.Random.Range(0, linePrefab.Length - 1)];
    //    for(int i = 0; i< LatestLines.Count; i++)
    //    {
    //        if (newline.name == LatestLines[i].name)
    //        {
    //            newline = linePrefab[UnityEngine.Random.Range(0, linePrefab.Length - 1)];
    //            i = -1;
    //        }
    //    }
    //    LatestLines.Add(newline);
    //    if(LatestLines.Count > 10)
    //    LatestLines.RemoveAt(0);

    //    return newline;
    //}


    void MoveWave()
    {
        _cantime = false;
        _timerr = 0;
        Debug.Log("Move wave");
        if (lBlock.Count > 0)
        {
            for (int i = 0; i < lBlock.Count; i++)
            {
                lBlock[i].movePosition();
                Debug.Log("Move block");

            }
        }
        Debug.Log("Blocks moved");

        if (lBonus.Count > 0)
        {
            for (int i = 0; i < lBonus.Count; i++)
            {
                lBonus[i].movePosition();
                Debug.Log("Move bonus");

            }
        }
        Debug.Log("Bonuses moved");

        buttonReturn.SetActive(false);
        Time.timeScale = 1;
        Physics2D.IgnoreLayerCollision(8, 9, false);
        _experienceBonuses = 0.1f;
        _scorebonuses = 0.1f;
        if (_bestScore < _score) _bestScore = _score;
        if (lDestroy.Count > 0)
        {
            for (int i = 0; i < lDestroy.Count; i++)
            {
                Destroy(lDestroy[i]);
            }
        }

        lDestroy.Clear();
        SaveGame();
        Debug.Log("Game saved");

        NewWave();
        Debug.Log("New wave");
        Debug.Log("Move wave Ended");


    }

    int checkpointline;
    public void GameOver()
    {
        Debug.Log("Game over");
        if (_score > _bestScore) _bestScore = _score;
        textScript.BestScore.text = "Best- " + Mathf.RoundToInt(_bestScore).ToString();
        SaveGame();
        decimal random = (decimal)UnityEngine.Random.Range(0.7f, 0.8f);
        checkpointline = (int)(_blockHealth * random);
        panelGameOver.SetActive(true);
        buttonWachAddCheck.SetActive(true);
        //buttonWachAddDouble.SetActive(false);
        textScript.GameOverCheckPointScore.text = "Check point score- " + lAllLine[checkpointline].GetComponent<LineScript>()._saveScore.ToString();
        //if (ChekPoint.Count == 0)
        //{
        //    buttonWachAddCheck.SetActive(false);
        //    buttonWachAddDouble.SetActive(true);
        //    textScript.GameOverCheckPointScore.text = "No check points, wood you like wach add for double crystals?";

        //}
        //else
        //{
        //    buttonWachAddCheck.SetActive(true);
        //    buttonWachAddDouble.SetActive(false);

        //    textScript.GameOverCheckPointScore.text = "Check point score- " + _checkScore.ToString();
        //}
        //textScript.GameOverScore.text = "Score- " + Mathf.RoundToInt(_score).ToString();
        DestroyLines();
    }

    public void wachAdd()
    {
        ////add properties
        //LoadCheckPoint();
        //panelGameOver.SetActive(false);
        /*
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
        */
    }
    public void addRewardLoad()
    {
        LineScript load = lAllLine[checkpointline];
        NewGame(load._blockHealth, load._saveScore, load._maxAmmo, load._crystalsForGame);
    }
    public void wachAddDouble()
    {
        //add properties
        _crystals += _crystalsForGame;
        panelGameOver.SetActive(false);
    }

    public void MakeShoot()
    {
        if (_canShoot)
        StartCoroutine(Shoot());
    }

    public void Return()
    {
        Vector2 newdir;
        Vector2 pos = Camera.main.WorldToScreenPoint(startPos.transform.position);
        Vector2 ammpos;
        for(int i = 0;i<lAmmo.Count; i++)
        {
            ammpos = Camera.main.WorldToScreenPoint(lAmmo[i].transform.position);
            newdir = pos - ammpos;
            newdir = Camera.main.ScreenToWorldPoint(newdir);
            lAmmo[i].GetComponent<AmmoScript>().toHome(newdir.normalized);
        }
    }

    public void NewGame()
    {
        _crystalsForGame = 0;
        _maxAmmo = 1;
        _blockHealth = 1;
        _score = 0;
        _blockId = 0;
        _bonusId = 0;
        _ammoDirect = new Vector2(0, -1);
        textScript.buttonContinue.interactable = true;
        //NewWave();
        DestroyLines();
        NewWave();
    }
    public void NewGame(int blockHealth, float score,float maxAmmo, float crystalsforgame)
    {
        panelGameOver.SetActive(false);
        Physics2D.IgnoreLayerCollision(8, 9, false);
        Time.timeScale = 1;
        _crystalsForGame = crystalsforgame;
        _maxAmmo = maxAmmo;
        _blockHealth = blockHealth;
        _score = score;
        _blockId = 0;
        _bonusId = 0;
        _ammoDirect = new Vector2(0, -1);
        textScript.buttonContinue.interactable = true;
        //NewWave();
        DestroyLines();
        NewWave();
    }
    public void RestartGame()
    {
        panelGameOver.SetActive(false);
        NewGame();
    }
    public void Home()
    {
        panelGameOver.SetActive(false);
        panelMenu.SetActive(true);
        textScript.score.gameObject.SetActive(false);
        textScript.ButtonPause.SetActive(false);
        textScript.buttonContinue.interactable = false;
        _timescalegame = 1;
    }

    void addExperience()
    {
        _experience += experienceForDestroyedBlock * experienceBonusAdded * _experienceBonuses; 
        if(_experience >= experienceToLevelUp)
        {
            LevelUp();
            _experience = _experience - experienceToLevelUp;
            experienceToLevelUp += experienceToLevelUpStep;
        }
        _experienceBonuses+=0.1f;
        textScript.ProgressBar.fillAmount = _experience / experienceToLevelUp;
    }

    void LevelUp()
    {
        _accountLevel++;
        textScript.levelText.text = _accountLevel.ToString();
        switch (_accountLevel)
        {
            case 5:
                {
                    Debug.Log("level- 5 ");
                    break;
                }
            case 10:
                {
                    break;
                }
            case 15:
                {

                    break;
                }
            case 20:
                {

                    break;
                }
            case 25:
                {

                    break;
                }
            case 30:
                {

                    break;
                }
            case 35:
                {

                    break;
                }
            case 40:
                {

                    break;
                }
            case 45:
                {

                    break;
                }
            case 50:
                {

                    break;
                }
            case 55:
                {

                    break;
                }
            case 60:
                {

                    break;
                }
            case 65:
                {

                    break;
                }
            case 70:
                {

                    break;
                }
            case 75:
                {

                    break;
                }
            case 80:
                {

                    break;
                }
            case 85:
                {

                    break;
                }
            case 90:
                {

                    break;
                }
            case 95:
                {

                    break;
                }
            case 100:
                {

                    break;
                }

        }
    }

    public void PauseGame()
    {
        _timescalegame = Time.timeScale;
        textScript.score.gameObject.SetActive(false);
        textScript.ButtonPause.SetActive(false);
        Time.timeScale = 0;
    }
    public void UnpauseGame()
    {
        Time.timeScale = _timescalegame;
        textScript.ButtonPause.SetActive(true);

        textScript.score.gameObject.SetActive(true);

    }


    void EnteringGame()
    {
        loadGame();
        //PauseGame();
        textScript.score.gameObject.SetActive(false);
        textScript.ButtonPause.SetActive(false);
        textScript.buttonContinue.interactable = false;
        _timescalegame = 1;
    }
    void SaveGame()
    {
        PlayerPrefs.SetFloat("crystals", _crystals);
        PlayerPrefs.SetFloat("bestscore", _bestScore);
        PlayerPrefs.SetFloat("experience", _experience);
        PlayerPrefs.SetInt("lvl", _accountLevel);
        PlayerPrefs.Save();
    }
    public void loadGame()
    {
        if (PlayerPrefs.HasKey("bestscore"))
        {
            _bestScore = PlayerPrefs.GetFloat("bestscore");
        }
        else
        {
            _bestScore = 0;
        }

        if (PlayerPrefs.HasKey("crystals"))
        {
            _crystals = PlayerPrefs.GetFloat("crystals");
        }
        else
        {
            _crystals = 0;
        }

        if (PlayerPrefs.HasKey("lvl"))
        {
            _accountLevel = PlayerPrefs.GetInt("lvl");
        }
        else
        {
            _accountLevel = 0;
        }

        if (PlayerPrefs.HasKey("experience"))
        {
            _experience = PlayerPrefs.GetFloat("experience");
        }
        else
        {
            _experience = 0;
        }
        textScript.BestScore.text = "Best- "+ Mathf.RoundToInt(_bestScore).ToString();
        textScript.crystals.text = _crystals.ToString();
        textScript.ProgressBar.fillAmount = _experience / experienceToLevelUp;
        textScript.levelText.text = _accountLevel.ToString();
    }


    public void HideButtons()
    {

    }
    public void ShowButtons()
    {

    }
    public void AddAmmo(GameObject obj)
    {
        lAmmo.Add(obj);
    }

    
    /*
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        LineScript load = lAllLine[checkpointline];
        NewGame(load._blockHealth, load._saveScore, load._maxAmmo, load._crystalsForGame);
        
    }
    */
}
