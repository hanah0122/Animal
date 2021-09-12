using System;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp.Assets.Scripts.BUSS;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//using System.Security.Policy;

public class bussGame : MonoBehaviour
{
    public infoTime _infoTime = new infoTime();
    public infoBuss _infoBus = new infoBuss();
    private bussTimer _bussTimer = new bussTimer();
    private bussLevel _bussLevel = new bussLevel();
    public int timeMenu = 3;
    private bool started = false;
    [SerializeField] public int cols;
    [SerializeField] public int rows;
    [SerializeField] public float paddingPikachu = 4f;
    private int sizePikache; //so hinh se khoi tao
    private int imgPikachu;
    public int winPikachu = 0;

    public int _point;
    private string type_Reward = "reload";
    private bussData dataGame = null;
    private string TypeAdmobIntersitial = "new_game";
    Sprite[] img;
    Sprite[] imgClick;
    Sprite[] imgClick2;
    [SerializeField] public Image PannelWinner;
    [SerializeField] public Sprite[] line;
    [SerializeField] public Button btnPause;
    [SerializeField] public Button btnContinueMenu;
    [SerializeField] public Button btnContinueMenuTop;
    [SerializeField] public Button btnContinue;
    [SerializeField] public Button btnReload;
    [SerializeField] public Button btnIdear;
    [SerializeField] public Button btnViewReload;
    [SerializeField] public Button btnViewIdear;
    [SerializeField] public ParticleSystem ProgressBarParticles;
    [SerializeField] public Slider _slider;
    [SerializeField] Text _txtTime;
    [SerializeField] Text txtPoint;
    [SerializeField] Text txtLevel;
    [SerializeField] Text txtAdmob;
    [SerializeField] Text txtTextContinue;
    [SerializeField] Image _pnLoseHasAdmob;
    [SerializeField] Image _pnLoseNoAdmob;
    private Image _pnLose;
    [SerializeField] Image _pnGame;
    [SerializeField] Image _pnLoadGame;
    [SerializeField] Image _pnNewGame;
    [SerializeField] Image _pnTop;
    [SerializeField] Image _pnMenu;
    [SerializeField] Image _pnViewReload;
    [SerializeField] Image _pnViewIdear;

    [SerializeField] GameObject Audio;

    //matrix
    public infoPikachu[,] _matrix;

    private bussIdear bussIdear;

    //pikachu
    private List<infoCreatePikachu> _infoCreatePikachu = new List<infoCreatePikachu>();

    //check pikachu

    private List<infoPikachu> _checkPiakchu = new List<infoPikachu>();
    private List<infoPikachu> _idearPiakchu = new List<infoPikachu>();

    protected bool HasSave;

    private void Awake()
    {
        imgClick = Resources.LoadAll<Sprite>("Sprites/pokemon_photo_img_click") as Sprite[];
        img = Resources.LoadAll<Sprite>("Sprites/pokemon_photo_img") as Sprite[];
        imgClick2 = Resources.LoadAll<Sprite>("Sprites/pokemon_photo_img_click_2") as Sprite[];
        //bussTimer
        _infoTime._particleSystem = ProgressBarParticles;
        _infoTime._slider = _slider;
        _infoTime._txtTime = _txtTime;
        _infoTime._slider.maxValue = _infoTime._slider.value = _infoTime._gameTime;
        //bussGame
        _infoBus._pnGame = _pnGame;
        txtPoint.text = _point.ToString();
        txtLevel.text = _bussLevel.txtLevel(_infoBus._level);
        btnReload.GetComponentInChildren<Text>().text = _infoBus._countReload.ToString();
        btnIdear.GetComponentInChildren<Text>().text = _infoBus._countIdear.ToString();
        btnContinueMenu.gameObject.SetActive(false);
        btnContinueMenuTop.gameObject.SetActive(false);
    }


    private void config()
    {
        // _infoBus._level = 1;
        if (_infoBus._level < 5)
        {
            rows = 8;
            cols = 14;
        }

        if (_infoBus._level >= 5)
        {
            rows = 8;
            cols = 16;
        }

        if (_infoBus._level >= 10 && _infoBus._level < 15)
        {
            rows = 8;
            cols = 18;
        }

        if (_infoBus._level >= 15 && _infoBus._level < 20)
        {
            rows = 10;
            cols = 18;
        }

        if (_infoBus._level >= 20 && _infoBus._level < 25)
        {
            rows = 10;
            cols = 20;
        }

        if (_infoBus._level >= 25 && _infoBus._level < 30)
        {
            rows = 12;
            cols = 20;
        }

        if (_infoBus._level >= 30 && _infoBus._level < 35)
        {
            rows = 12;
            cols = 20;
        }

        if (_infoBus._level >= 35 && _infoBus._level < 40)
        {
            rows = 12;
            cols = 22;
        }

        if (_infoBus._level >= 40)
        {
            rows = 14;
            cols = 24;
        }

        imgPikachu = 4; // so cap pikachu toi da trong game
        sizePikache = (cols - 2) * (rows - 2) / imgPikachu;
        // Debug.Log(imgPikachu.ToString() + '-' + sizePikache.ToString());
        if (winPikachu == 0)
            winPikachu = (cols - 2) * (rows - 2);
        //matrix
        _matrix = null;
        _matrix = new infoPikachu[cols + 2, rows + 2];
        _infoCreatePikachu.Clear(); //khoi tao so hinh va so khoi cua pikachu
        if (dataGame == null)
        {
            int RandomImage = 0;
            List<int> imgTmp = new List<int>();
            for (int m = 0; m < img.Length; m++)
            {
                imgTmp.Add(m);
            }

            for (int i = 0; i < sizePikache; i++)
            {
                RandomImage = Random.Range(0, imgTmp.Count);
                _infoCreatePikachu.Add(
                    new infoCreatePikachu(imgPikachu, img[imgTmp[RandomImage]],
                        imgClick2[imgTmp[RandomImage]], imgClick[imgTmp[RandomImage]], imgTmp[RandomImage]));

                imgTmp.RemoveAt(RandomImage);
            }

            imgTmp.Clear();
        }
        else
            for (int i = 0; i < img.Length; i++)
                _infoCreatePikachu.Add(new infoCreatePikachu(imgPikachu, img[i], imgClick2[i], imgClick[i], i));
    }

    // Start is called before the first frame update
    void Start()
    {
        _pnLose = GameObject.Find("Admob").GetComponent<AbManager>().getRunAdmob() ? _pnLoseHasAdmob : _pnLoseNoAdmob;
        _infoTime._particleSystem.Pause();
        started = false;
        _pnMenu.gameObject.SetActive(true);
        _pnGame.gameObject.SetActive(false);
        //_pnTop.gameObject.SetActive(false);
        btnReload.interactable = started;
        btnPause.interactable = started;
        btnIdear.interactable = started;
        if (bussSaveLoadData.CheckData())
        {
            btnPause.gameObject.SetActive(started);
            btnContinueMenuTop.gameObject.SetActive(true);
            btnContinueMenuTop.interactable = false;
            btnContinue.gameObject.SetActive(true);
            btnContinueMenu.gameObject.SetActive(false);
            _pnNewGame.gameObject.SetActive(false);
            _pnLoadGame.gameObject.SetActive(true);
            dataGame = bussSaveLoadData.loadGame();
            txtTextContinue.text = "Tiếp tục chơi ở mức Level: " + (dataGame.level + 1).ToString();
            LoadGame();
        }
        else
        {
            _pnTop.gameObject.SetActive(false);
            _pnNewGame.gameObject.SetActive(true);
            _pnLoadGame.gameObject.SetActive(false);
        }
    }

    public void AdmobGameIntersitial()
    {
        gameObject.SetActive(true);
        if (TypeAdmobIntersitial == "win_game")
        {
            started = true;
            PannelWinner.gameObject.SetActive(false);
            Audio.GetComponent<bussSound>().SoundGameWin();
            _infoTime._startTime = Time.time;
            if (_matrix != null)
                for (int i = 0; i < cols + 2; i++)
                for (int j = 0; j < rows + 2; j++)
                    Destroy(_matrix[i, j]._gameObject);

            startGame();
            bussSaveLoadData.saveGame(this, _infoTime._startTime - Time.time, timeMenu);
        }

        if (TypeAdmobIntersitial == "ongame_continue")
        {
            //Audio.gameObject.GetComponent<bussSound>().UnPause();
            started = true;
            _infoTime._particleSystem.Play();
            _infoBus._pnGame.gameObject.SetActive(started);
            btnPause.gameObject.SetActive(started);
            btnReload.interactable = true;
            btnIdear.interactable = true;
            btnContinueMenuTop.gameObject.SetActive(false);
            _pnMenu.gameObject.SetActive(false);
            Audio.GetComponent<bussSound>().SoungBegin();
        }

        if (TypeAdmobIntersitial == "load_game")
        {
            _pnMenu.gameObject.SetActive(false);
            btnContinueMenuTop.gameObject.SetActive(false);
            btnPause.gameObject.SetActive(true);
            _infoTime._gameTime = 60 * timeMenu;
            _infoTime._startTime = dataGame._startTime + Time.time;
            startGame();
            _pnTop.gameObject.SetActive(true);
            started = true;
            btnPause.gameObject.SetActive(started);
        }

        if (TypeAdmobIntersitial == "new_game")
        {
            //Audio.gameObject.GetComponent<bussSound>().UnPause();
            _point = _infoBus._level = 0;
            txtPoint.text = _point.ToString();
            txtLevel.text = "Level: " + (_infoBus._level + 1).ToString();
            _infoBus._countIdear = _infoBus._countReload = 10;
            btnReload.GetComponentInChildren<Text>().text = _infoBus._countReload.ToString();
            btnIdear.GetComponentInChildren<Text>().text = _infoBus._countIdear.ToString();
            _pnMenu.gameObject.SetActive(false);
            btnContinueMenuTop.gameObject.SetActive(false);
            btnPause.gameObject.SetActive(true);
            _infoTime._slider.maxValue = _infoTime._gameTime = 60 * timeMenu;
            _infoTime._startTime = Time.time;
            dataGame = null;
            LoadGame();
            startGame();
            _pnTop.gameObject.SetActive(true);
            started = true;
        }
    }

    public void BtnVua()
    {
        timeMenu = 1;
        AdmobGameIntersitial();
    }

    public void BtnNhanh()
    {
        timeMenu = 2;
        AdmobGameIntersitial();
    }

    public void BtnCham()
    {
        timeMenu = 5;
        AdmobGameIntersitial();
    }

    public void MenuNewGame()
    {
        Audio.GetComponent<bussSound>().SoundButton();
        TypeAdmobIntersitial = "new_game";
        if (_matrix != null)
            for (int i = 0; i < cols + 2; i++)
            for (int j = 0; j < rows + 2; j++)
                Destroy(_matrix[i, j]._gameObject);
        _pnNewGame.gameObject.SetActive(true);
        _pnLoadGame.gameObject.SetActive(false);
    }

    public void MenuContinueGame(bool puase = false)
    {
        TypeAdmobIntersitial = "load_game";
        if (GameObject.Find("Admob").GetComponent<AbManager>().getRunAdmob() && Random.Range(0, 3) == 0 &&
            puase == false)
        {
            // GameObject.Find("Canvas").gameObject.SetActive(false);
            GameObject.Find("Admob").GetComponent<AbManager>().ShowIntersitialAds();
        }
        else
            AdmobGameIntersitial();
    }


    // void OnApplicationFocus(bool hasFocus)
    // {
    //     if (!hasFocus)
    //     {
    //         if (started)
    //         {
    //             pauseGame();
    //             _pnLoadGame.gameObject.SetActive(false);
    //             bussSaveLoadData.saveGame(this, _infoTime._startTime - Time.time, timeMenu);
    //         }
    //     }
    //     else
    //     {
    //         if (_matrix != null)
    //         {
    //             for (int i = 0; i < cols + 2; i++)
    //             for (int j = 0; j < rows + 2; j++)
    //                 Destroy(_matrix[i, j]._gameObject);
    //
    //             dataGame = bussSaveLoadData.loadGame();
    //             LoadGame();
    //             MenuContinueGame(true);
    //             //Audio.gameObject.GetComponent<bussSound>().UnPause();
    //         }
    //     }
    // }

    //void OnApplicationQuit()
    //{
    //    bussSaveLoadData.saveGame(this, _infoTime._startTime - Time.time, timeMenu);
    //}
    void Update()
    {
        if (GameObject.Find("Admob").GetComponent<AbManager>().getRunAdmob())
        {
            if (_infoBus._countReload == 0)
                btnReload.GetComponentInChildren<Text>().text = "Thêm";
            if (_infoBus._countIdear == 0)
                btnIdear.GetComponentInChildren<Text>().text = "Thêm";
        }
        else
        {
            btnIdear.GetComponentInChildren<Text>().text = _infoBus._countIdear.ToString();
            btnReload.GetComponentInChildren<Text>().text = _infoBus._countReload.ToString();
        }

        if (started)
        {
            _infoTime._timer = Time.time - _infoTime._startTime;

            _infoTime._coutDown = _infoTime._gameTime - _infoTime._timer;
            if (_infoTime._coutDown >= 0)
            {
                try
                {
                    _infoTime._txtTime.text = string.Format("{0:0}:{1:00}", Mathf.FloorToInt(_infoTime._coutDown / 60),
                        Mathf.FloorToInt(_infoTime._coutDown - Mathf.FloorToInt(_infoTime._coutDown / 60) * 60));
                    _infoTime._slider.value = _infoTime._coutDown;
                }
                catch
                {
                }
            }
            else
            {
                try
                {
                    if (_infoBus._pnGame && _pnLose)
                    {
                        started = false;
                        _infoTime._particleSystem.Pause();
                        if (_infoBus._level >= 1 && GameObject.Find("Admob").GetComponent<AbManager>().getRunAdmob())
                        {
                            txtAdmob.text = "Xem quảng cáo để chơi tiếp Level: " + (_infoBus._level + 1).ToString();
                            btnReload.interactable = false;
                            btnIdear.interactable = false;
                            btnPause.interactable = false;
                            _infoBus._pnGame.gameObject.SetActive(false);
                            bussSaveLoadData.saveGame(this, _infoTime._startTime - Time.time, timeMenu);
                        }
                        else
                        {
                            _pnLose = _pnLoseNoAdmob;
                            NoViewAdmob();
                        }

                        Audio.GetComponent<bussSound>().SoundGameLose();
                        _pnLose.gameObject.SetActive(true);
                        _pnGame.gameObject.SetActive(false);
                    }
                }
                catch
                {
                }
            }
        }
        else
            _infoTime._startTime = Time.time - _infoTime._timer;
    }

    public void UserViewAdmob()
    {
        this.type_Reward = "level";
        if (GameObject.Find("Admob").GetComponent<AbManager>().getRunAdmob())
        {
            GameObject.Find("Admob").GetComponent<AbManager>().LoadRewardedVideoAds();
        }
    }

    public void NoViewAdmob(bool noview = true)
    {
        started = true;
        btnReload.interactable = true;
        btnIdear.interactable = true;
        btnPause.interactable = true;
        _infoTime._startTime = Time.time;
        _point = 0;
        txtPoint.text = _point.ToString();
        dataGame = null;
        winPikachu = (cols - 2) * (rows - 2);
        if (_infoBus._level > 0 && noview == true)
        {
            _infoBus._level -= 1;
            txtLevel.text = _bussLevel.txtLevel(_infoBus._level);
        }

        if (GameObject.Find("Admob").GetComponent<AbManager>().getRunAdmob() && _infoBus._level >= 1)
        {
            _pnLose.gameObject.SetActive(false);
            _infoTime._startTime = Time.time;
            if (_matrix != null)
                for (int i = 0; i < cols + 2; i++)
                for (int j = 0; j < rows + 2; j++)
                    Destroy(_matrix[i, j]._gameObject);

            startGame();
            bussSaveLoadData.saveGame(this, _infoTime._startTime - Time.time, timeMenu);
        }
        else
            this.StartCoroutine(this.LoadGameNoHasAdmob());
    }

    private IEnumerator LoadGameNoHasAdmob()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(reloadGame(false));
        _pnLose.gameObject.SetActive(false);
    }

    public void pauseGame()
    {
        Audio.GetComponent<bussSound>().SoundButton();
        //Audio.gameObject.GetComponent<bussSound>().Pause();
        //bussSaveLoadData.saveGame(this, _infoTime._startTime - Time.time, timeMenu);
        started = false;
        _infoTime._particleSystem.Pause();
        _pnMenu.gameObject.SetActive(true);
        _pnGame.gameObject.SetActive(false);
        _pnLoadGame.gameObject.SetActive(true);
        _pnNewGame.gameObject.SetActive(false);
        btnReload.interactable = started;
        btnIdear.interactable = started;
        btnContinueMenuTop.gameObject.SetActive(true);
        btnContinue.interactable = true;
        btnContinueMenu.gameObject.SetActive(true);
        btnContinue.gameObject.SetActive(false);
        btnPause.gameObject.SetActive(false);
    }

    public void continueGame()
    {
        TypeAdmobIntersitial = "ongame_continue";
        if (GameObject.Find("Admob").GetComponent<AbManager>().getRunAdmob() && Random.Range(0, 3) == 0)
        {
            // GameObject.Find("Canvas").gameObject.SetActive(false);
            GameObject.Find("Admob").GetComponent<AbManager>().ShowIntersitialAds();
        }
        else
            AdmobGameIntersitial();
    }

    public void LoadGame()
    {
        //if (!NewGame)

        if (dataGame == null)
        {
            _point = _infoBus._level = 0;
            _infoBus._countIdear = _infoBus._countReload = 10;
            winPikachu = 0;
        }
        else
        {
            _point = dataGame.point;
            _infoBus._level = dataGame.level;
            _infoBus._countIdear = dataGame.countIdear;
            _infoBus._countReload = dataGame.countReload;
            winPikachu = dataGame.winPikachu;
            timeMenu = dataGame.timeMenu;
            _infoTime._startTime = dataGame._startTime;
            txtPoint.text = dataGame.point.ToString();
            dataGame.level += 1;
            txtLevel.text = "Level: " + dataGame.level.ToString();
            btnIdear.GetComponentInChildren<Text>().text = dataGame.countIdear.ToString();
            btnReload.GetComponentInChildren<Text>().text = dataGame.countReload.ToString();
            _infoTime._coutDown = 60 * dataGame.timeMenu + dataGame._startTime;
            if (GameObject.Find("Admob").GetComponent<AbManager>().getRunAdmob() && dataGame.countReload == 0)
            {
                btnReload.GetComponentInChildren<Text>().text = "Thêm";
            }

            if (GameObject.Find("Admob").GetComponent<AbManager>().getRunAdmob() && dataGame.countIdear == 0)
            {
                btnIdear.GetComponentInChildren<Text>().text = "Thêm";
            }

            try
            {
                if (_infoTime._coutDown < 0)
                    _infoTime._coutDown = 0;
                _infoTime._txtTime.text = string.Format("{0:0}:{1:00}", Mathf.FloorToInt(_infoTime._coutDown / 60),
                    Mathf.FloorToInt(_infoTime._coutDown - Mathf.FloorToInt(_infoTime._coutDown / 60) * 60));
                _infoTime._slider.maxValue = 60 * timeMenu;
                _infoTime._slider.value = (int) _infoTime._coutDown;
            }
            catch
            {
            }
        }
    }

    private IEnumerator SoundIdeaButton()
    {
        yield return new WaitForSeconds(0.3f);
        Audio.GetComponent<bussSound>().SoundIdea();
    }

    public void btnIdea()
    {
        if (_idearPiakchu.Count >= 1 && _infoBus._countIdear > 0)
        {
            _infoBus._countIdear -= 1;
            btnIdear.GetComponentInChildren<Text>().text =
                (_infoBus._countIdear == 0 && GameObject.Find("Admob").GetComponent<AbManager>().RunAdmob)
                    ? "Thêm"
                    : _infoBus._countIdear.ToString();
            _matrix[_idearPiakchu[0]._i, _idearPiakchu[0]._j]._gameObject.GetComponent<RectTransform>()
                    .GetComponent<Button>().GetComponent<Image>().sprite =
                _matrix[_idearPiakchu[0]._i, _idearPiakchu[0]._j]._infoCreate._imgIdear;
            _matrix[_idearPiakchu[1]._i, _idearPiakchu[1]._j]._gameObject.GetComponent<RectTransform>()
                    .GetComponent<Button>().GetComponent<Image>().sprite =
                _matrix[_idearPiakchu[1]._i, _idearPiakchu[1]._j]._infoCreate._imgIdear;
            this.StartCoroutine(this.SoundIdeaButton());
        }
        else
        {
            if (_idearPiakchu.Count >= 1 && GameObject.Find("Admob").GetComponent<AbManager>().RunAdmob)
            {
                this.type_Reward = "idear";
                this.started = false;
                this._pnGame.gameObject.SetActive(false);
                this._pnViewIdear.gameObject.SetActive(true);
                bussSaveLoadData.saveGame(this, _infoTime._startTime - Time.time, timeMenu);
            }
        }
    }

    public void LoadRewadAdIdear()
    {
        this.btnViewIdear.GetComponentInChildren<Text>().text = "Đang tải ...";
        GameObject.Find("Admob").GetComponent<AbManager>().LoadRewardedVideoAds();
    }


    public void startGame()
    {
        if (_infoTime._particleSystem.isPaused)
            _infoTime._particleSystem.Play();

        if (!btnReload.interactable)
            btnReload.interactable = true;
        if (!btnPause.interactable)
            btnPause.interactable = true;
        if (!btnIdear.interactable)
            btnIdear.interactable = true;
        config();
        int _flag = 0;
        if (!_infoBus._pnGame.IsActive())
            _infoBus._pnGame.gameObject.SetActive(true);
        for (int i = 0; i < cols + 2; i++)
        {
            for (int j = 0; j < rows + 2; j++)
            {
                GameObject _gameObject = new GameObject("pikachu_" + i + "_" + j);
                if (i > 1 && j > 1 && i <= cols - 1 && j <= rows - 1)
                {
                    RectTransform rectTransform = _gameObject.AddComponent<RectTransform>();
                    rectTransform.anchorMin = new Vector2(0, 1);
                    rectTransform.anchorMax = new Vector2(0, 1);

                    rectTransform.sizeDelta =
                        new Vector2(
                            (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols) -
                                     paddingPikachu),
                            (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows) -
                                     paddingPikachu));
                    rectTransform.transform.position = new Vector2(
                        (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols / 2) +
                                 (float) ((float) _infoBus._pnGame.rectTransform.rect.width / cols * i -
                                          (float) (_infoBus._pnGame.rectTransform.rect.width / cols)) + paddingPikachu),
                        (float) ((float) -(float) (_infoBus._pnGame.rectTransform.rect.height / rows / 2) -
                                 (float) ((float) (_infoBus._pnGame.rectTransform.rect.height / rows * j) -
                                          (float) _infoBus._pnGame.rectTransform.rect.height / rows)) + paddingPikachu);
                    _gameObject.transform.SetParent(_infoBus._pnGame.transform, false);
                    rectTransform.gameObject.AddComponent<Button>();

                    _flag = dataGame == null
                        ? Random.Range(0, _infoCreatePikachu.Count)
                        : dataGame._matrix[i, j].imgNunber;


                    _infoCreatePikachu[_flag]._count -= 1;
                    rectTransform.GetComponent<Button>().gameObject.AddComponent<Image>().sprite =
                        _infoCreatePikachu[_flag]._img;

                    int x = i;
                    int y = j;

                    _matrix[x, y] = new infoPikachu(x, y, _infoCreatePikachu[_flag],
                        dataGame == null ? false : dataGame._matrix[x, y].empty, _gameObject,
                        _infoCreatePikachu[_flag]._number);
                    _gameObject.SetActive(dataGame == null ? true : !dataGame._matrix[x, y].empty);

                    rectTransform.GetComponent<Button>().onClick.AddListener(delegate() { clikPikachu(x, y); });
                    // Debug.Log(_infoCreatePikachu[_flag]._img.ToString() + '_' +
                    //          _infoCreatePikachu[_flag]._count.ToString() + '_'+
                    //          _flag.ToString()
                    //           );
                    if (dataGame == null && _infoCreatePikachu[_flag]._count <= 0)
                        _infoCreatePikachu.RemoveAt(_flag);
                }
                else
                {
                    if (i >= 1 && j >= 1 && i <= cols && j <= rows)
                    {
                        #region

                        RectTransform rectTransform = _gameObject.AddComponent<RectTransform>();
                        rectTransform.anchorMin = new Vector2(0, 1);
                        rectTransform.anchorMax = new Vector2(0, 1);

                        rectTransform.sizeDelta = new Vector2(
                            (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols) -
                                     paddingPikachu),
                            (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows) -
                                     paddingPikachu));
                        rectTransform.transform.position = new Vector2(
                            (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols / 2) +
                                     (float) ((float) _infoBus._pnGame.rectTransform.rect.width / cols * i -
                                              (float) (_infoBus._pnGame.rectTransform.rect.width / cols)) +
                                     paddingPikachu),
                            (float) ((float) -(float) (_infoBus._pnGame.rectTransform.rect.height / rows / 2) -
                                     (float) ((float) (_infoBus._pnGame.rectTransform.rect.height / rows * j) -
                                              (float) _infoBus._pnGame.rectTransform.rect.height / rows)) +
                            paddingPikachu);
                        _gameObject.transform.SetParent(_infoBus._pnGame.transform, false);
                        rectTransform.gameObject.AddComponent<Button>();
                        rectTransform.GetComponent<Button>().gameObject.AddComponent<Image>().sprite = line[0];
                        _gameObject.SetActive(false);

                        #endregion

                        _matrix[i, j] = new infoPikachu(i, j, null, true, _gameObject, -1);
                    }
                    else
                    {
                        #region

                        //GameObject _gameObject = new GameObject("pikachu_false_" + i + "_" + j);
                        //RectTransform rectTransform = _gameObject.AddComponent<RectTransform>();
                        //rectTransform.anchorMin = new Vector2(0, 1);
                        //rectTransform.anchorMax = new Vector2(0, 1);

                        //rectTransform.sizeDelta = new Vector2((float)(_infoBus._pnGame.rectTransform.rect.width / cols - paddingPikachu), (float)(_infoBus._pnGame.rectTransform.rect.height / rows - paddingPikachu));
                        //rectTransform.transform.position = new Vector2(
                        //    paddingPikachu + (float)((float)(_infoBus._pnGame.rectTransform.rect.width / cols / 2) + (float)(_infoBus._pnGame.rectTransform.rect.width / cols * i - _infoBus._pnGame.rectTransform.rect.width / cols)),
                        //    paddingPikachu + (float)(-(float)(_infoBus._pnGame.rectTransform.rect.height / rows / 2) - (float)(_infoBus._pnGame.rectTransform.rect.height / rows * j - _infoBus._pnGame.rectTransform.rect.height / rows)));
                        //_gameObject.transform.SetParent(_infoBus._pnGame.transform, false);

                        #endregion

                        Destroy(_gameObject);
                        _matrix[i, j] = new infoPikachu(i, j, null, false, null, -1);
                    }
                }

                _gameObject = null;
            }
        }

        // Debug.Log(_infoCreatePikachu.Count);
        _infoCreatePikachu.Clear();
        aiIDear();
        Audio.GetComponent<bussSound>().SoungBegin();
    }


    #region Move

    public void clikPikachu(int i, int j)
    {
        _matrix[i, j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>().GetComponent<Image>().sprite =
            _matrix[i, j]._infoCreate._imgClick;

        if (_checkPiakchu.Count < 1)
            _checkPiakchu.Add(_matrix[i, j]);
        else
        {
            if (_checkPiakchu[0]._i != i || _checkPiakchu[0]._j != j)
            {
                _checkPiakchu.Add(_matrix[i, j]);

                if (_checkPiakchu[0]._infoCreate._img.ToString().ToLower() ==
                    _checkPiakchu[1]._infoCreate._img.ToString().ToLower())
                {
                    _checkPiakchu[0]._empty = true;
                    _checkPiakchu[1]._empty = true;
                    bool checkMove = false;
                    if (_checkPiakchu[0]._i == _checkPiakchu[1]._i)
                    {
                        checkMove = checkLineX(_checkPiakchu[0]._j, _checkPiakchu[1]._j, _checkPiakchu[0]._i);
                        if (checkMove)
                        {
                            List<infoPikachu> infoPikachu = new List<infoPikachu>();
                            int min = Math.Min(_checkPiakchu[0]._j, _checkPiakchu[1]._j);
                            int max = Math.Max(_checkPiakchu[0]._j, _checkPiakchu[1]._j);
                            for (int tmp = min; tmp <= max; tmp++)
                            {
                                _matrix[_checkPiakchu[0]._i, tmp]._gameObject.SetActive(true);
                                infoPikachu.Add(_matrix[_checkPiakchu[0]._i, tmp]);
                            }

                            if (infoPikachu.Count > 2)
                            {
                                _checkPiakchu[0]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                                    .GetComponent<Image>().sprite = _checkPiakchu[0]._infoCreate._img;
                                _checkPiakchu[1]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                                    .GetComponent<Image>().sprite = _checkPiakchu[1]._infoCreate._img;
                            }

                            StartCoroutine(lineMove(infoPikachu, 2));
                        }
                    }

                    if (_checkPiakchu[0]._j == _checkPiakchu[1]._j)
                    {
                        checkMove = checkLineY(_checkPiakchu[0]._i, _checkPiakchu[1]._i, _checkPiakchu[0]._j);

                        if (checkMove)
                        {
                            List<infoPikachu> infoPikachu = new List<infoPikachu>();
                            int min = Math.Min(_checkPiakchu[0]._i, _checkPiakchu[1]._i);
                            int max = Math.Max(_checkPiakchu[0]._i, _checkPiakchu[1]._i);
                            for (int tmp = min; tmp <= max; tmp++)
                            {
                                _matrix[tmp, _checkPiakchu[0]._j]._gameObject.SetActive(true);
                                infoPikachu.Add(_matrix[tmp, _checkPiakchu[0]._j]);
                            }

                            if (infoPikachu.Count > 2)
                            {
                                _checkPiakchu[0]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                                    .GetComponent<Image>().sprite = _checkPiakchu[0]._infoCreate._img;
                                _checkPiakchu[1]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                                    .GetComponent<Image>().sprite = _checkPiakchu[1]._infoCreate._img;
                            }

                            StartCoroutine(lineMove(infoPikachu, 1));
                        }
                    }

                    if (checkMove)
                    {
                        List<infoPikachu> _pikachuMoveSuccess = new List<infoPikachu>();
                        _pikachuMoveSuccess.Add(_matrix[_checkPiakchu[0]._i, _checkPiakchu[0]._j]);
                        _pikachuMoveSuccess.Add(_matrix[_checkPiakchu[1]._i, _checkPiakchu[1]._j]);
                        moveSuccess(_pikachuMoveSuccess);
                        _checkPiakchu.Clear();
                    }

                    else
                    {
                        int t = -1; // t is column find

                        // check in rectangle with x
                        if (t == -1)
                            t = checkRectX(_checkPiakchu[0], _checkPiakchu[1]);
                        // check in rectangle with y
                        if (t == -1)
                            t = checkRectY(_checkPiakchu[0], _checkPiakchu[1]);
                        // check more right
                        if (t == -1)
                            t = checkMoreLineX(_checkPiakchu[0], _checkPiakchu[1], 1);

                        // check more left - chu U nguoc

                        if (t == -1)
                            t = checkMoreLineX(_checkPiakchu[0], _checkPiakchu[1], -1);

                        // check more down
                        if (t == -1)
                            t = checkMoreLineY(_checkPiakchu[0], _checkPiakchu[1], 1);

                        // check more up
                        if (t == -1)
                            t = checkMoreLineY(_checkPiakchu[0], _checkPiakchu[1], -1);


                        if (t != -1)
                        {
                            _matrix[_checkPiakchu[0]._i, _checkPiakchu[0]._j]._empty =
                                _matrix[_checkPiakchu[1]._i, _checkPiakchu[1]._j]._empty = true;
                            List<infoPikachu> _pikachuMoveSuccess = new List<infoPikachu>();
                            _pikachuMoveSuccess.Add(_matrix[_checkPiakchu[0]._i, _checkPiakchu[0]._j]);
                            _pikachuMoveSuccess.Add(_matrix[_checkPiakchu[1]._i, _checkPiakchu[1]._j]);
                            moveSuccess(_pikachuMoveSuccess);
                            _checkPiakchu.Clear();
                        }
                        else
                        {
                            Audio.GetComponent<bussSound>().SoundMoveFlase();
                            _matrix[_checkPiakchu[0]._i, _checkPiakchu[0]._j]._empty =
                                _matrix[_checkPiakchu[1]._i, _checkPiakchu[1]._j]._empty = false;

                            List<infoPikachu> _checkPiakchuHidden = new List<infoPikachu>();
                            _checkPiakchuHidden.Add(_matrix[_checkPiakchu[0]._i, _checkPiakchu[0]._j]);
                            _checkPiakchuHidden.Add(_matrix[_checkPiakchu[1]._i, _checkPiakchu[1]._j]);
                            _checkPiakchu.Clear();
                            StartCoroutine(hideImgClickFalse(_checkPiakchuHidden));
                        }
                    }
                }
                else
                {
                    Audio.GetComponent<bussSound>().SoundMoveFlase();
                    List<infoPikachu> _checkPiakchuHidden = new List<infoPikachu>();
                    _checkPiakchuHidden.Add(_matrix[_checkPiakchu[0]._i, _checkPiakchu[0]._j]);
                    _checkPiakchuHidden.Add(_matrix[_checkPiakchu[1]._i, _checkPiakchu[1]._j]);
                    _checkPiakchu.Clear();
                    StartCoroutine(hideImgClickFalse(_checkPiakchuHidden));
                }
            }
        }
    }

    private IEnumerator reloadGame(bool isUpdate)
    {
        yield return new WaitForSeconds(0.5f);
        if (!isUpdate)
            _infoTime._startTime = Time.time;
        if (_matrix != null)
            for (int i = 0; i < cols + 2; i++)
            for (int j = 0; j < rows + 2; j++)
                Destroy(_matrix[i, j]._gameObject);

        startGame();
        if (!GameObject.Find("Admob").GetComponent<AbManager>().getRunAdmob())
        {
            if (_infoBus._countIdear == 0)
            {
                _infoBus._countIdear = 1;
                btnIdear.GetComponentInChildren<Text>().text = _infoBus._countIdear.ToString();
            }

            if (_infoBus._countReload == 0)
            {
                _infoBus._countReload = 1;
                btnReload.GetComponentInChildren<Text>().text = _infoBus._countReload.ToString();
            }
        }

        txtPoint.text = _point.ToString();
        yield return new WaitForSeconds(0.1f);
        bussSaveLoadData.saveGame(this, _infoTime._startTime - Time.time, timeMenu);
    }

    private IEnumerator WinGame()
    {
        TypeAdmobIntersitial = "win_game";
        PannelWinner.gameObject.SetActive(true);
        dataGame = null;
        _infoBus._level += 1;
        started = false;
        txtLevel.text = _bussLevel.txtLevel(_infoBus._level);
        if (GameObject.Find("Admob").GetComponent<AbManager>().getRunAdmob() && Random.Range(0, 3) != 10)
        {
            GameObject.Find("Admob").GetComponent<AbManager>().ShowIntersitialAds();
        }
        else
        {
            started = true;
            Audio.GetComponent<bussSound>().SoundGameWin();
            yield return new WaitForSeconds(1f);
            PannelWinner.gameObject.SetActive(false);
            this.StartCoroutine(this.reloadGame(false));
        }
    }


    private void moveSuccess(List<infoPikachu> _pikachuMoveSuccess)
    {
        winPikachu -= 2;
        _point += 2;
        txtPoint.text = _point.ToString();
        if (winPikachu == 0)
            this.StartCoroutine(this.WinGame());
        else
        {
            Audio.GetComponent<bussSound>().SoundMoveTrue();
            if (_infoBus._level > 15)
            {
                if (Random.Range(0, 10) < 8)
                {
                    switch (Random.Range(1, 4))
                    {
                        case 1:
                            StartCoroutine(moveLeft(_pikachuMoveSuccess));
                            break;
                        case 2:
                            StartCoroutine(moveRight(_pikachuMoveSuccess));
                            break;
                        case 3:
                            StartCoroutine(moveUp(_pikachuMoveSuccess));
                            break;
                        case 4:
                            StartCoroutine(moveDown(_pikachuMoveSuccess));
                            break;
                    }
                }
                else
                {
                    this.aiIDear();
                }
            }
            else
            {
                bool hasHoanVi = false;

                if (_infoBus._level == 1 || _infoBus._level == 6 || _infoBus._level == 11)
                {
                    StartCoroutine(moveLeft(_pikachuMoveSuccess));
                    hasHoanVi = true;
                }

                if (_infoBus._level == 2 || _infoBus._level == 7 || _infoBus._level == 12)
                {
                    StartCoroutine(moveRight(_pikachuMoveSuccess));
                    hasHoanVi = true;
                }

                if (_infoBus._level == 3 || _infoBus._level == 8 || _infoBus._level == 13)
                {
                    StartCoroutine(moveUp(_pikachuMoveSuccess));
                    hasHoanVi = true;
                }

                if (_infoBus._level == 4 || _infoBus._level == 9 || _infoBus._level == 14)
                {
                    StartCoroutine(moveDown(_pikachuMoveSuccess));
                    hasHoanVi = true;
                }

                if (!hasHoanVi)
                    aiIDear();
            }
        }
    }


    private IEnumerator moveDown(List<infoPikachu> _pikachuMoveSuccess)
    {
        yield return new WaitForSeconds(0.6f);

        if ((Math.Max(_pikachuMoveSuccess[0]._j, _pikachuMoveSuccess[1]._j) -
             Math.Min(_pikachuMoveSuccess[0]._j, _pikachuMoveSuccess[1]._j)) != 1 &&
            _pikachuMoveSuccess[0]._i != _pikachuMoveSuccess[1]._i)
        {
            hoanviDown(0, _pikachuMoveSuccess);
            hoanviDown(1, _pikachuMoveSuccess);
        }
        else
            hoanviDown(0, _pikachuMoveSuccess);
        //if (_infoBus._level > 15)
        //{
        //    if (UnityEngine.Random.Range(0, 10) % 2 == 0)
        //    {
        //        hoanViMoveLeft(0, _pikachuMoveSuccess);
        //        hoanViMoveLeft(1, _pikachuMoveSuccess);
        //    }
        //    else
        //    {
        //        hoanViMoveRight(0, _pikachuMoveSuccess);
        //        hoanViMoveRight(1, _pikachuMoveSuccess);
        //    }
        //}


        aiIDear();
    }

    private void hoanviDown(int tmp, List<infoPikachu> _pikachuMoveSuccess)
    {
        infoCreatePikachu infoCreatePikachu = null;
        int i = _pikachuMoveSuccess[tmp]._i;
        bool empty = false;
        int imgNunber = 0;
        for (int j = _pikachuMoveSuccess[tmp]._j; j > 1; j--)
        {
            if (j > 2)
            {
                empty = _matrix[i, j]._empty;
                imgNunber = _matrix[i, j]._numberImg;
                infoCreatePikachu = _matrix[i, j]._infoCreate;

                _matrix[i, j]._infoCreate = _matrix[i, j - 1]._infoCreate;
                _matrix[i, j]._empty = _matrix[i, j - 1]._empty;
                _matrix[i, j]._numberImg = _matrix[i, j - 1]._numberImg;
                _matrix[i, j]._gameObject.SetActive(!_matrix[i, j]._empty);
                _matrix[i, j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>().GetComponent<Image>()
                    .sprite = _matrix[i, j - 1]._infoCreate._img;
                _matrix[i, j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols) - paddingPikachu),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows) - paddingPikachu));


                _matrix[i, j - 1]._empty = empty;
                _matrix[i, j - 1]._gameObject.SetActive(!_matrix[i, j - 1]._empty);
                _matrix[i, j - 1]._numberImg = imgNunber;
                _matrix[i, j - 1]._infoCreate = infoCreatePikachu;
                _matrix[i, j - 1]._gameObject.GetComponent<RectTransform>().GetComponent<Button>().GetComponent<Image>()
                    .sprite = _matrix[i, j]._infoCreate._img;
                _matrix[i, j - 1]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols) - paddingPikachu),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows) - paddingPikachu));
            }
            else
            {
                _matrix[i, j]._empty = true;
                _matrix[i, j]._gameObject.SetActive(false);
            }
        }

        infoCreatePikachu = null;
    }

    private IEnumerator moveUp(List<infoPikachu> _pikachuMoveSuccess)
    {
        yield return new WaitForSeconds(0.6f);

        if ((Math.Max(_pikachuMoveSuccess[0]._j, _pikachuMoveSuccess[1]._j) -
             Math.Min(_pikachuMoveSuccess[0]._j, _pikachuMoveSuccess[1]._j)) != 1 &&
            _pikachuMoveSuccess[0]._i != _pikachuMoveSuccess[1]._i)
        {
            hoanViMoveUp(0, _pikachuMoveSuccess);
            hoanViMoveUp(1, _pikachuMoveSuccess);
        }
        else
            hoanViMoveUp(0, _pikachuMoveSuccess);
        //if (_infoBus._level > 15)
        //{
        //    if (UnityEngine.Random.Range(0, 10) % 2 == 0)
        //    {
        //        hoanViMoveLeft(0, _pikachuMoveSuccess);
        //        hoanViMoveLeft(1, _pikachuMoveSuccess);
        //    }
        //    else
        //    {
        //        hoanViMoveRight(0, _pikachuMoveSuccess);
        //        hoanViMoveRight(1, _pikachuMoveSuccess);
        //    }
        //}


        aiIDear();
    }

    private void hoanViMoveUp(int tmp, List<infoPikachu> _pikachuMoveSuccess)
    {
        infoCreatePikachu infoCreatePikachu = null;
        int i = _pikachuMoveSuccess[tmp]._i;
        bool empty = false;
        int imgNunber = 0;
        for (int j = _pikachuMoveSuccess[tmp]._j; j < rows; j++)
        {
            if (j < rows - 1)
            {
                empty = _matrix[i, j]._empty;
                imgNunber = _matrix[i, j]._numberImg;
                infoCreatePikachu = _matrix[i, j]._infoCreate;

                _matrix[i, j]._infoCreate = _matrix[i, j + 1]._infoCreate;
                _matrix[i, j]._empty = _matrix[i, j + 1]._empty;
                _matrix[i, j]._numberImg = _matrix[i, j + 1]._numberImg;
                _matrix[i, j]._gameObject.SetActive(!_matrix[i, j]._empty);
                _matrix[i, j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>().GetComponent<Image>()
                    .sprite = _matrix[i, j + 1]._infoCreate._img;
                _matrix[i, j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols) - paddingPikachu),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows) - paddingPikachu));


                _matrix[i, j + 1]._empty = empty;
                _matrix[i, j + 1]._numberImg = imgNunber;
                _matrix[i, j + 1]._gameObject.SetActive(!_matrix[i, j + 1]._empty);

                _matrix[i, j + 1]._infoCreate = infoCreatePikachu;
                _matrix[i, j + 1]._gameObject.GetComponent<RectTransform>().GetComponent<Button>().GetComponent<Image>()
                    .sprite = _matrix[i, j]._infoCreate._img;
                _matrix[i, j + 1]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols) - paddingPikachu),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows) - paddingPikachu));
            }
            else
            {
                _matrix[i, j]._empty = true;
                _matrix[i, j]._gameObject.SetActive(false);
            }
        }

        infoCreatePikachu = null;
    }

    private IEnumerator moveRight(List<infoPikachu> _pikachuMoveSuccess)
    {
        yield return new WaitForSeconds(0.6f);

        if ((Math.Max(_pikachuMoveSuccess[0]._i, _pikachuMoveSuccess[1]._i) -
             Math.Min(_pikachuMoveSuccess[0]._i, _pikachuMoveSuccess[1]._i)) != 1 &&
            _pikachuMoveSuccess[0]._j != _pikachuMoveSuccess[1]._j)
        {
            hoanViMoveRight(0, _pikachuMoveSuccess);
            hoanViMoveRight(1, _pikachuMoveSuccess);
        }
        else
        {
            hoanViMoveRight(0, _pikachuMoveSuccess);
            //if(_infoBus._level > 15)
            //{
            //    if (UnityEngine.Random.Range(0, 10) % 2 == 0)
            //    {
            //        hoanViMoveUp(0, _pikachuMoveSuccess);
            //        hoanViMoveUp(1, _pikachuMoveSuccess);
            //    }
            //    else
            //    {
            //        hoanviDown(0, _pikachuMoveSuccess);
            //        hoanviDown(1, _pikachuMoveSuccess);
            //    }
            //}
        }

        aiIDear();
    }

    private void hoanViMoveRight(int tmp, List<infoPikachu> _pikachuMoveSuccess)
    {
        infoCreatePikachu infoCreatePikachu = null;
        int j = _pikachuMoveSuccess[tmp]._j;
        bool empty = false;
        int imgNunber = 0;
        for (int i = _pikachuMoveSuccess[tmp]._i; i < cols; i++)
        {
            if (i < cols - 1)
            {
                empty = _matrix[i, j]._empty;
                imgNunber = _matrix[i, j]._numberImg;
                infoCreatePikachu = _matrix[i, j]._infoCreate;

                _matrix[i, j]._infoCreate = _matrix[i + 1, j]._infoCreate;
                _matrix[i, j]._numberImg = _matrix[i + 1, j]._numberImg;
                _matrix[i, j]._empty = _matrix[i + 1, j]._empty;
                _matrix[i, j]._gameObject.SetActive(!_matrix[i, j]._empty);
                _matrix[i, j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>().GetComponent<Image>()
                    .sprite = _matrix[i + 1, j]._infoCreate._img;

                _matrix[i, j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols) - paddingPikachu),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows) - paddingPikachu));

                _matrix[i + 1, j]._empty = empty;
                _matrix[i + 1, j]._numberImg = imgNunber;
                _matrix[i + 1, j]._gameObject.SetActive(!_matrix[i + 1, j]._empty);

                _matrix[i + 1, j]._infoCreate = infoCreatePikachu;
                _matrix[i + 1, j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>().GetComponent<Image>()
                    .sprite = _matrix[i, j]._infoCreate._img;
                _matrix[i + 1, j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols) - paddingPikachu),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows) - paddingPikachu));
            }
            else
            {
                _matrix[i, j]._empty = true;
                _matrix[i, j]._gameObject.SetActive(false);
            }
        }

        infoCreatePikachu = null;
    }

    private IEnumerator moveLeft(List<infoPikachu> _pikachuMoveSuccess)
    {
        yield return new WaitForSeconds(0.6f);

        if ((Math.Max(_pikachuMoveSuccess[0]._i, _pikachuMoveSuccess[1]._i) -
             Math.Min(_pikachuMoveSuccess[0]._i, _pikachuMoveSuccess[1]._i)) != 1 &&
            _pikachuMoveSuccess[0]._j != _pikachuMoveSuccess[1]._j)
        {
            hoanViMoveLeft(0, _pikachuMoveSuccess);
            hoanViMoveLeft(1, _pikachuMoveSuccess);
        }
        else
        {
            hoanViMoveLeft(0, _pikachuMoveSuccess);
            //if (_infoBus._level > 15)
            //{
            //    if (UnityEngine.Random.Range(0, 10) % 2 == 0)
            //    {
            //        hoanViMoveUp(0, _pikachuMoveSuccess);
            //        hoanViMoveUp(1, _pikachuMoveSuccess);
            //    }
            //    else
            //    {
            //        hoanviDown(0, _pikachuMoveSuccess);
            //        hoanviDown(1, _pikachuMoveSuccess);
            //    }
            //}
        }

        aiIDear();
    }

    private void hoanViMoveLeft(int tmp, List<infoPikachu> _pikachuMoveSuccess)
    {
        infoCreatePikachu infoCreatePikachu = null;
        int j = _pikachuMoveSuccess[tmp]._j;
        bool empty = false;
        int imgNunber = 0;
        for (int i = _pikachuMoveSuccess[tmp]._i; i > 1; i--)
        {
            if (i > 2)
            {
                empty = _matrix[i, j]._empty;
                imgNunber = _matrix[i, j]._numberImg;
                infoCreatePikachu = _matrix[i, j]._infoCreate;

                _matrix[i, j]._infoCreate = _matrix[i - 1, j]._infoCreate;
                _matrix[i, j]._empty = _matrix[i - 1, j]._empty;
                _matrix[i, j]._numberImg = _matrix[i - 1, j]._numberImg;
                _matrix[i, j]._gameObject.SetActive(!_matrix[i, j]._empty);
                _matrix[i, j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>().GetComponent<Image>()
                    .sprite = _matrix[i - 1, j]._infoCreate._img;

                _matrix[i, j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols) - paddingPikachu),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows) - paddingPikachu));

                _matrix[i - 1, j]._empty = empty;
                _matrix[i - 1, j]._numberImg = imgNunber;
                _matrix[i - 1, j]._gameObject.SetActive(!_matrix[i - 1, j]._empty);

                _matrix[i - 1, j]._infoCreate = infoCreatePikachu;
                _matrix[i - 1, j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>().GetComponent<Image>()
                    .sprite = _matrix[i, j]._infoCreate._img;
                _matrix[i - 1, j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols) - paddingPikachu),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows) - paddingPikachu));
            }
            else
            {
                _matrix[i, j]._empty = true;

                _matrix[i, j]._gameObject.SetActive(false);
            }
        }

        infoCreatePikachu = null;
    }

    private IEnumerator lineMove(List<infoPikachu> infoPikachus, int line)
    {
        if (infoPikachus.Count > 2)
        {
            for (int i = 1; i < infoPikachus.Count - 1; i++)
            {
                infoPikachus[i]._gameObject.SetActive(true);
                infoPikachus[i]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));
                infoPikachus[i]._gameObject.GetComponent<RectTransform>().GetComponent<Button>().GetComponent<Image>()
                    .sprite = this.line[line];
            }

            yield return new WaitForSeconds(0.2f);
        }

        if (infoPikachus.Count <= 2)
        {
            for (int i = 0; i < infoPikachus.Count; i++)
            {
                infoPikachus[i]._gameObject.GetComponent<RectTransform>().GetComponent<Button>().GetComponent<Image>()
                    .sprite = infoPikachus[i]._infoCreate._imgClick;
            }

            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < infoPikachus.Count; i++)
            {
                infoPikachus[i]._gameObject.GetComponent<RectTransform>().GetComponent<Button>().GetComponent<Image>()
                    .sprite = infoPikachus[i]._infoCreate._img;
            }

            yield return new WaitForSeconds(0.1f);
        }


        for (int i = 0; i < infoPikachus.Count; i++)
        {
            infoPikachus[i]._gameObject.SetActive(false);
        }

        infoPikachus.Clear();
        infoPikachus = null;
    }

    private IEnumerator hideImgClickFalse(List<infoPikachu> _checkPiakchuHidde)
    {
        for (int i = 0; i < _checkPiakchuHidde.Count; i++)
        {
            _checkPiakchuHidde[i]._gameObject.GetComponent<RectTransform>().GetComponent<Button>().GetComponent<Image>()
                .sprite = _checkPiakchuHidde[i]._infoCreate._img;
        }

        yield return new WaitForSeconds(0.1f);


        for (int i = 0; i < _checkPiakchuHidde.Count; i++)
        {
            _checkPiakchuHidde[i]._gameObject.GetComponent<RectTransform>().GetComponent<Button>().GetComponent<Image>()
                .sprite = _checkPiakchuHidde[i]._infoCreate._imgClick;
        }

        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < _checkPiakchuHidde.Count; i++)
        {
            _checkPiakchuHidde[i]._gameObject.GetComponent<RectTransform>().GetComponent<Button>().GetComponent<Image>()
                .sprite = _checkPiakchuHidde[i]._infoCreate._img;
        }

        aiIDear();
    }

    // check with line x, from column y1 to y2
    private bool checkLineX(int y1, int y2, int x)
    {
        // find point have column max and min
        int min = Math.Min(y1, y2);
        int max = Math.Max(y1, y2);
        //Debug.Log(min + "-" + max + "-" + x);
        // run column
        for (int y = min; y <= max; y++)
        {
            if (_matrix[x, y]._empty == false)
            {
                // if see barrier then die
                //Debug.Log("die: " + x + "" + y);
                return false;
            }
            //Debug.Log("doc:" + x + "_" + y);
        }

        // not die -> success
        return true;
    }

    private bool checkLineY(int x1, int x2, int y)
    {
        int min = Math.Min(x1, x2);
        int max = Math.Max(x1, x2);
        for (int x = min; x <= max; x++)
        {
            if (_matrix[x, y]._empty == false)
            {
                //  Debug.Log("die: " + x + "" + y);
                return false;
            }
            //   Debug.Log("ngang: " + x + "_" + y);
        }

        return true;
    }

    // check in rectangle
    private int checkRectX(infoPikachu p1, infoPikachu p2)
    {
        // find point have y min and max
        infoPikachu pMinY = p1;
        infoPikachu pMaxY = p2;
        if (p1._j > p2._j)
        {
            pMinY = p2;
            pMaxY = p1;
        }

        for (int y = pMinY._j + 1; y < pMaxY._j; y++)
        {
            // check three line
            if (checkLineX(pMinY._j, y, pMinY._i) && checkLineY(pMinY._i, pMaxY._i, y) &&
                checkLineX(y, pMaxY._j, pMaxY._i))
            {
                //Debug.Log("Rect x");
                //Debug.Log("(" + pMinY._i + "," + pMinY._j + ") -> ("
                //        + pMinY._i + "," + y + ") -> (" + pMaxY._i + "," + y
                //        + ") -> (" + pMaxY._i + "," + pMaxY._j + ")");

                List<infoPikachu> listMove = new List<infoPikachu>();
                listMove.Add(_matrix[pMinY._i, pMinY._j]);
                listMove.Add(_matrix[pMinY._i, y]);
                listMove.Add(_matrix[pMaxY._i, y]);
                listMove.Add(_matrix[pMaxY._i, pMaxY._j]);
                _matrix[p1._i, p1._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                        .GetComponent<Image>().sprite =
                    _matrix[p1._i, p1._j]._infoCreate._img;
                _matrix[p2._i, p2._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                        .GetComponent<Image>().sprite =
                    _matrix[p2._i, p2._j]._infoCreate._img;
                StartCoroutine(lineRectX(listMove));
                pMinY = pMinY = p1 = p2 = null;

                // if three line is true return column y
                return y;
            }
        }

        // have a line in three line not true then return -1
        pMinY = pMinY = p1 = p2 = null;
        return -1;
    }

    private IEnumerator lineRectX(List<infoPikachu> listMove)
    {
        for (int j = listMove[0]._j + 1; j <= listMove[1]._j; j++)
        {
            _matrix[listMove[0]._i, j]._gameObject.SetActive(true);
            _matrix[listMove[0]._i, j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));

            _matrix[listMove[0]._i, j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                .GetComponent<Image>().sprite = line[2];

            if (listMove[0]._i == listMove[1]._i && j == listMove[1]._j)
            {
                _matrix[listMove[0]._i, j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                    .GetComponent<Image>().sprite = line[
                    listMove[1]._i > listMove[2]._i ? 6 : 5];
            }
        }

        if (listMove[1]._i > listMove[2]._i)
        {
            for (int x = listMove[1]._i - 1; x >= listMove[2]._i; x--)
            {
                _matrix[x, listMove[1]._j]._gameObject.SetActive(true);
                _matrix[x, listMove[1]._j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));
                _matrix[x, listMove[1]._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                    .GetComponent<Image>().sprite = line[1];
                if (listMove[1]._j == listMove[2]._j && x == listMove[2]._i)
                {
                    _matrix[x, listMove[1]._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                        .GetComponent<Image>().sprite = line[3];
                }
            }
        }
        else
        {
            for (int x = listMove[1]._i + 1; x <= listMove[2]._i; x++)
            {
                _matrix[x, listMove[1]._j]._gameObject.SetActive(true);
                _matrix[x, listMove[1]._j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));
                _matrix[x, listMove[1]._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                    .GetComponent<Image>().sprite = line[1];
                if (listMove[1]._j == listMove[2]._j && x == listMove[2]._i)
                {
                    _matrix[x, listMove[1]._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                        .GetComponent<Image>().sprite = line[4];
                }
            }
        }


        for (int x_ = listMove[2]._j + 1; x_ < listMove[3]._j; x_++)
        {
            _matrix[listMove[3]._i, x_]._gameObject.SetActive(true);
            _matrix[listMove[3]._i, x_]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));
            _matrix[listMove[3]._i, x_]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                .GetComponent<Image>().sprite = line[2];
        }

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < listMove.Count; i++)
        {
            listMove[i]._gameObject.SetActive(false);
        }

        for (int j = listMove[0]._j + 1; j <= listMove[1]._j; j++)
        {
            _matrix[listMove[0]._i, j]._gameObject.SetActive(false);
        }

        if (listMove[1]._i > listMove[2]._i)
        {
            for (int x = listMove[1]._i - 1; x >= listMove[2]._i; x--)
            {
                _matrix[x, listMove[1]._j]._gameObject.SetActive(false);
            }
        }
        else
        {
            for (int x = listMove[1]._i + 1; x <= listMove[2]._i; x++)
            {
                _matrix[x, listMove[1]._j]._gameObject.SetActive(false);
            }
        }

        for (int x_ = listMove[2]._j + 1; x_ < listMove[3]._j; x_++)
        {
            _matrix[listMove[3]._i, x_]._gameObject.SetActive(false);
        }

        listMove.Clear();
        listMove = null;
    }

    private int checkRectY(infoPikachu p1, infoPikachu p2)
    {
        // find point have y min
        infoPikachu pMinX = p1;
        infoPikachu pMaxX = p2;
        if (p1._i > p2._i)
        {
            pMinX = p2;
            pMaxX = p1;
        }

        // find line and y begin
        for (int x = pMinX._i + 1; x < pMaxX._i; x++)
        {
            if (checkLineY(pMinX._i, x, pMinX._j)
                && checkLineX(pMinX._j, pMaxX._j, x)
                && checkLineY(x, pMaxX._i, pMaxX._j))
            {
                //Debug.Log("Rect y");
                //Debug.Log("(" + pMinX._i + "," + pMinX._j + ") -> (" + x
                //        + "," + pMinX._j + ") -> (" + x + "," + pMaxX._j
                //        + ") -> (" + pMaxX._i + "," + pMaxX._j + ")");
                List<infoPikachu> listMove = new List<infoPikachu>();
                listMove.Add(_matrix[pMinX._i, pMinX._j]);
                listMove.Add(_matrix[x, pMinX._j]);
                listMove.Add(_matrix[x, pMaxX._j]);
                listMove.Add(_matrix[pMaxX._i, pMaxX._j]);
                _matrix[p1._i, p1._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                        .GetComponent<Image>().sprite =
                    _matrix[p1._i, p1._j]._infoCreate._img;
                _matrix[p2._i, p2._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                        .GetComponent<Image>().sprite =
                    _matrix[p2._i, p2._j]._infoCreate._img;
                StartCoroutine(lineRectY(listMove));
                pMinX = pMaxX = p1 = p2 = null;
                return x;
            }
        }

        pMinX = pMaxX = p1 = p2 = null;
        return -1;
    }

    private IEnumerator lineRectY(List<infoPikachu> listMove)
    {
        for (int i = listMove[0]._i + 1; i <= listMove[1]._i; i++)
        {
            _matrix[i, listMove[0]._j]._gameObject.SetActive(true);
            _matrix[i, listMove[0]._j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));

            _matrix[i, listMove[0]._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                .GetComponent<Image>().sprite = line[1];

            if (listMove[0]._j == listMove[1]._j && i == listMove[1]._i)
            {
                _matrix[i, listMove[0]._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                    .GetComponent<Image>().sprite = line[
                    listMove[1]._j < listMove[2]._j ? 4 : 6
                ];
            }
        }

        // (8, 3)-> (9, 3)-> (9, 6)-> (11, 6)
        //(7, 7)-> (9, 7)-> (9, 4)-> (rows, 4)
        if (listMove[1]._j < listMove[2]._j)
        {
            for (int y = listMove[1]._j + 1; y <= listMove[2]._j; y++)
            {
                _matrix[listMove[1]._i, y]._gameObject.SetActive(true);
                _matrix[listMove[1]._i, y]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));
                _matrix[listMove[1]._i, y]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                    .GetComponent<Image>().sprite = line[2];
                if (listMove[1]._i == listMove[2]._i && y == listMove[2]._j)
                {
                    _matrix[listMove[1]._i, y]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                        .GetComponent<Image>().sprite = line[5];
                }
            }
        }
        else
        {
            for (int y_ = listMove[1]._j - 1; y_ >= listMove[2]._j; y_--)
            {
                _matrix[listMove[1]._i, y_]._gameObject.SetActive(true);
                _matrix[listMove[1]._i, y_]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));
                _matrix[listMove[1]._i, y_]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                    .GetComponent<Image>().sprite = line[2];
                if (listMove[1]._i == listMove[2]._i && y_ == listMove[2]._j)
                {
                    _matrix[listMove[1]._i, y_]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                        .GetComponent<Image>().sprite = line[3];
                }
            }
        }


        for (int x_ = listMove[2]._i + 1; x_ < listMove[3]._i; x_++)
        {
            _matrix[x_, listMove[3]._j]._gameObject.SetActive(true);
            _matrix[x_, listMove[3]._j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));
            _matrix[x_, listMove[3]._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                .GetComponent<Image>().sprite = line[1];
        }

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < listMove.Count; i++)
        {
            listMove[i]._gameObject.SetActive(false);
        }

        for (int i = listMove[0]._i + 1; i <= listMove[1]._i; i++)
        {
            _matrix[i, listMove[0]._j]._gameObject.SetActive(false);
        }

        if (listMove[1]._j < listMove[2]._j)
        {
            for (int y = listMove[1]._j + 1; y <= listMove[2]._j; y++)
            {
                _matrix[listMove[1]._i, y]._gameObject.SetActive(false);
            }
        }
        else
        {
            for (int y_ = listMove[1]._j - 1; y_ >= listMove[2]._j; y_--)
            {
                _matrix[listMove[1]._i, y_]._gameObject.SetActive(false);
            }
        }

        for (int x_ = listMove[2]._i + 1; x_ < listMove[3]._i; x_++)
        {
            _matrix[x_, listMove[3]._j]._gameObject.SetActive(false);
        }

        listMove.Clear();
        listMove = null;
    }


    private int checkMoreLineX(infoPikachu p1, infoPikachu p2, int type)
    {
        // find point have y min
        infoPikachu pMinY = p1;
        infoPikachu pMaxY = p2;
        if (p1._j > p2._j)
        {
            pMinY = p2;
            pMaxY = p1;
        }

        // find line and y begin
        int y = pMaxY._j;
        int row = pMinY._i;
        if (type == -1)
        {
            y = pMinY._j;
            row = pMaxY._i;
        }

        // check more
        if (checkLineX(pMinY._j, pMaxY._j, row))
        {
            while (_matrix[pMinY._i, y]._empty == true && _matrix[pMaxY._i, y]._empty == true)
            {
                if (checkLineY(pMinY._i, pMaxY._i, y))
                {
                    //Debug.Log("TH X " + type);
                    //Debug.Log("(" + pMinY._i + "," + pMinY._j + ") -> ("
                    //        + pMinY._i + "," + y + ") -> (" + pMaxY._i + "," + y
                    //        + ") -> (" + pMaxY._i + "," + pMaxY._j + ")");

                    List<infoPikachu> listMove = new List<infoPikachu>();
                    if (_matrix[pMinY._i, pMinY._j]._i <= _matrix[pMaxY._i, pMaxY._j]._i)
                    {
                        listMove.Add(_matrix[pMinY._i, pMinY._j]);
                        listMove.Add(_matrix[pMinY._i, y]);
                        listMove.Add(_matrix[pMaxY._i, y]);
                        listMove.Add(_matrix[pMaxY._i, pMaxY._j]);
                    }
                    else
                    {
                        listMove.Add(_matrix[pMaxY._i, pMaxY._j]);
                        listMove.Add(_matrix[pMaxY._i, y]);
                        listMove.Add(_matrix[pMinY._i, y]);
                        listMove.Add(_matrix[pMinY._i, pMinY._j]);
                    }

                    _matrix[listMove[0]._i, listMove[0]._j]._gameObject.GetComponent<RectTransform>()
                            .GetComponent<Button>().GetComponent<Image>().sprite =
                        _matrix[listMove[0]._i, listMove[0]._j]._infoCreate._img;
                    _matrix[listMove[3]._i, listMove[3]._j]._gameObject.GetComponent<RectTransform>()
                            .GetComponent<Button>().GetComponent<Image>().sprite =
                        _matrix[listMove[3]._i, listMove[3]._j]._infoCreate._img;
                    StartCoroutine(lineMoveX(type, listMove));

                    pMinY = pMaxY = p1 = p2 = null;
                    return y;
                }

                y += type;
            }
        }

        pMinY = pMaxY = p1 = p2 = null;
        return -1;
    }


    private IEnumerator lineMoveX(int type, List<infoPikachu> listMove)
    {
        if (type == -1)
        {
            //Debug.Log("TH X " + type);
            //Debug.Log("(" + listMove[0]._i + "," + listMove[0]._j + ") -> ("
            //        + listMove[1]._i + "," + listMove[1]._j + ") -> (" + listMove[2]._i + "," + listMove[2]._j
            //        + ") -> (" + listMove[3]._i + "," + listMove[3]._j + ")");
            for (int j = listMove[0]._j - 1; j >= listMove[1]._j; j--)
            {
                _matrix[listMove[0]._i, j]._gameObject.SetActive(true);
                _matrix[listMove[0]._i, j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));

                _matrix[listMove[0]._i, j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                    .GetComponent<Image>().sprite = line[2];

                if (listMove[0]._i == listMove[1]._i && j == listMove[1]._j)
                {
                    _matrix[listMove[0]._i, j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                        .GetComponent<Image>().sprite = line[3];
                }
            }

            for (int i = listMove[1]._i + 1; i < listMove[2]._i; i++)
            {
                _matrix[i, listMove[1]._j]._gameObject.SetActive(true);
                _matrix[i, listMove[1]._j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));
                _matrix[i, listMove[1]._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                    .GetComponent<Image>().sprite = line[1];
            }

            for (int j = listMove[3]._j - 1; j >= listMove[2]._j; j--)
            {
                _matrix[listMove[2]._i, j]._gameObject.SetActive(true);
                _matrix[listMove[2]._i, j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));
                _matrix[listMove[2]._i, j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                    .GetComponent<Image>().sprite = line[2];


                if (listMove[3]._i == listMove[2]._i && j == listMove[2]._j)
                {
                    _matrix[listMove[2]._i, j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                        .GetComponent<Image>().sprite = line[4];
                }
            }


            yield return new WaitForSeconds(0.2f);

            for (int i = 0; i < listMove.Count; i++)
            {
                listMove[i]._gameObject.SetActive(false);
            }

            for (int j = listMove[0]._j - 1; j >= listMove[1]._j; j--)
            {
                _matrix[listMove[0]._i, j]._gameObject.SetActive(false);
            }

            for (int i = listMove[1]._i + 1; i < listMove[2]._i; i++)
            {
                _matrix[i, listMove[1]._j]._gameObject.SetActive(false);
            }

            for (int j = listMove[3]._j - 1; j >= listMove[2]._j; j--)
            {
                _matrix[listMove[2]._i, j]._gameObject.SetActive(false);
            }

            listMove.Clear();
            listMove = null;
        }
        else
        {
            //Debug.Log("TH X " + type);
            //Debug.Log("(" + listMove[0]._i + "," + listMove[0]._j + ") -> ("
            //        + listMove[1]._i + "," + listMove[1]._j + ") -> (" + listMove[2]._i + "," + listMove[2]._j
            //        + ") -> (" + listMove[3]._i + "," + listMove[3]._j + ")");

            for (int y = (listMove[0]._j + 1); y <= listMove[1]._j; y++)
            {
                _matrix[listMove[0]._i, y]._gameObject.SetActive(true);
                _matrix[listMove[0]._i, y]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));

                _matrix[listMove[0]._i, y]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                    .GetComponent<Image>().sprite = line[2];

                if (listMove[0]._i == listMove[1]._i && y == listMove[1]._j)
                {
                    _matrix[listMove[0]._i, y]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                        .GetComponent<Image>().sprite = line[5];
                }
            }

            for (int x = listMove[1]._i + 1; x < listMove[2]._i; x++)
            {
                _matrix[x, listMove[1]._j]._gameObject.SetActive(true);
                _matrix[x, listMove[1]._j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));
                _matrix[x, listMove[1]._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                    .GetComponent<Image>().sprite = line[1];
            }

            for (int y_ = listMove[2]._j; y_ > listMove[3]._j; y_--)
            {
                _matrix[listMove[2]._i, y_]._gameObject.SetActive(true);
                _matrix[listMove[2]._i, y_]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));
                _matrix[listMove[2]._i, y_]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                    .GetComponent<Image>().sprite = line[2];


                if (listMove[3]._i == listMove[2]._i && y_ == listMove[2]._j)
                {
                    _matrix[listMove[2]._i, y_]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                        .GetComponent<Image>().sprite = line[6];
                }
            }

            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < listMove.Count; i++)
            {
                listMove[i]._gameObject.SetActive(false);
            }

            for (int y = (listMove[0]._j + 1); y <= listMove[1]._j; y++)
            {
                _matrix[listMove[0]._i, y]._gameObject.SetActive(false);
            }

            for (int x = listMove[1]._i + 1; x < listMove[2]._i; x++)
            {
                _matrix[x, listMove[1]._j]._gameObject.SetActive(false);
            }

            for (int y_ = listMove[2]._j; y_ > listMove[3]._j; y_--)
            {
                _matrix[listMove[2]._i, y_]._gameObject.SetActive(false);
            }

            listMove.Clear();
            listMove = null;
        }
    }

    private int checkMoreLineY(infoPikachu p1, infoPikachu p2, int type)
    {
        infoPikachu pMinX = p1;
        infoPikachu pMaxX = p2;
        if (p1._i > p2._i)
        {
            pMinX = p2;
            pMaxX = p1;
        }

        int x = pMaxX._i;
        int col = pMinX._j;
        if (type == -1)
        {
            x = pMinX._i;
            col = pMaxX._j;
        }

        if (checkLineY(pMinX._i, pMaxX._i, col))
        {
            while (_matrix[x, pMinX._j]._empty == true
                   && _matrix[x, pMaxX._j]._empty == true)
            {
                if (checkLineX(pMinX._j, pMaxX._j, x))
                {
                    //Debug.Log("TH Y " + type);
                    //Debug.Log("(" + pMinX._i + "," + pMinX._j + ") -> ("
                    //        + x + "," + pMinX._j + ") -> (" + x + "," + pMaxX._j
                    //        + ") -> (" + pMaxX._i + "," + pMaxX._j + ")");

                    List<infoPikachu> listMoveY = new List<infoPikachu>();
                    if (pMaxX._j < pMinX._j)
                    {
                        listMoveY.Add(_matrix[pMaxX._i, pMaxX._j]);
                        listMoveY.Add(_matrix[x, pMaxX._j]);
                        listMoveY.Add(_matrix[x, pMinX._j]);
                        listMoveY.Add(_matrix[pMinX._i, pMinX._j]);
                    }
                    else
                    {
                        listMoveY.Add(_matrix[pMinX._i, pMinX._j]);
                        listMoveY.Add(_matrix[x, pMinX._j]);
                        listMoveY.Add(_matrix[x, pMaxX._j]);
                        listMoveY.Add(_matrix[pMaxX._i, pMaxX._j]);
                    }

                    _matrix[p1._i, p1._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                            .GetComponent<Image>().sprite =
                        _matrix[p1._i, p1._j]._infoCreate._img;
                    _matrix[p2._i, p2._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                            .GetComponent<Image>().sprite =
                        _matrix[p2._i, p2._j]._infoCreate._img;
                    StartCoroutine(lineMoveY(type, listMoveY));
                    pMinX = pMaxX = p1 = p2 = null;
                    return x;
                }

                x += type;
            }
        }

        pMinX = pMaxX = p1 = p2 = null;
        return -1;
    }

    private IEnumerator lineMoveY(int type, List<infoPikachu> listMove)
    {
        if (type == 1)
        {
            for (int i = listMove[0]._i + 1; i <= listMove[1]._i; i++)
            {
                _matrix[i, listMove[0]._j]._gameObject.SetActive(true);
                _matrix[i, listMove[0]._j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));
                _matrix[i, listMove[0]._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                    .GetComponent<Image>().sprite = line[1];

                if (listMove[0]._j == listMove[1]._j && i == listMove[1]._i)
                {
                    _matrix[i, listMove[0]._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                        .GetComponent<Image>().sprite = line[4];
                }
            }

            for (int x = listMove[1]._j + 1; x < listMove[2]._j; x++)
            {
                _matrix[listMove[1]._i, x]._gameObject.SetActive(true);
                _matrix[listMove[1]._i, x]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));
                _matrix[listMove[1]._i, x]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                    .GetComponent<Image>().sprite = line[2];
            }

            for (int x_ = listMove[2]._i; x_ > listMove[3]._i; x_--)
            {
                _matrix[x_, listMove[2]._j]._gameObject.SetActive(true);
                _matrix[x_, listMove[2]._j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));
                _matrix[x_, listMove[2]._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                    .GetComponent<Image>().sprite = line[1];


                if (listMove[3]._j == listMove[2]._j && x_ == listMove[2]._i)
                {
                    _matrix[x_, listMove[2]._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                        .GetComponent<Image>().sprite = line[6];
                }
            }

            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < listMove.Count; i++)
            {
                listMove[i]._gameObject.SetActive(false);
            }

            for (int i = listMove[0]._i + 1; i <= listMove[1]._i; i++)
            {
                _matrix[i, listMove[0]._j]._gameObject.SetActive(false);
            }

            for (int x = listMove[1]._j + 1; x < listMove[2]._j; x++)
            {
                _matrix[listMove[1]._i, x]._gameObject.SetActive(false);
            }

            for (int x_ = listMove[2]._i; x_ > listMove[3]._i; x_--)
            {
                _matrix[x_, listMove[2]._j]._gameObject.SetActive(false);
            }

            listMove.Clear();
            listMove = null;
        }
        else
        {
            for (int i = listMove[0]._i - 1; i >= listMove[1]._i; i--)
            {
                _matrix[i, listMove[0]._j]._gameObject.SetActive(true);
                _matrix[i, listMove[0]._j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));
                _matrix[i, listMove[0]._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                    .GetComponent<Image>().sprite = line[1];

                if (listMove[0]._j == listMove[1]._j && i == listMove[1]._i)
                {
                    _matrix[i, listMove[0]._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                        .GetComponent<Image>().sprite = line[3];
                }
            }

            for (int x = listMove[1]._j + 1; x < listMove[2]._j; x++)
            {
                _matrix[listMove[1]._i, x]._gameObject.SetActive(true);
                _matrix[listMove[1]._i, x]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));
                _matrix[listMove[1]._i, x]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                    .GetComponent<Image>().sprite = line[2];
            }

            for (int x_ = listMove[2]._i; x_ < listMove[3]._i; x_++)
            {
                _matrix[x_, listMove[2]._j]._gameObject.SetActive(true);
                _matrix[x_, listMove[2]._j]._gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.width / cols)),
                    (float) ((float) (float) (_infoBus._pnGame.rectTransform.rect.height / rows)));
                _matrix[x_, listMove[2]._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                    .GetComponent<Image>().sprite = line[1];


                if (listMove[3]._j == listMove[2]._j && x_ == listMove[2]._i)
                {
                    _matrix[x_, listMove[2]._j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>()
                        .GetComponent<Image>().sprite = line[5];
                }
            }

            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < listMove.Count; i++)
            {
                listMove[i]._gameObject.SetActive(false);
            }

            for (int i = listMove[0]._i - 1; i >= listMove[1]._i; i--)
            {
                _matrix[i, listMove[0]._j]._gameObject.SetActive(false);
            }

            for (int x = listMove[1]._j + 1; x < listMove[2]._j; x++)
            {
                _matrix[listMove[1]._i, x]._gameObject.SetActive(false);
            }

            for (int x_ = listMove[2]._i; x_ < listMove[3]._i; x_++)
            {
                _matrix[x_, listMove[2]._j]._gameObject.SetActive(false);
            }

            listMove.Clear();
            listMove = null;
        }
    }


    public void reLoadRamdom()
    {
        //Audio.GetComponent<bussSound>().SoundButton();
        if (_infoBus._countReload > 0)
        {
            Audio.GetComponent<bussSound>().SoungBegin();
            btnReload.interactable = false;
            if (_checkPiakchu.Count > 0)
                _checkPiakchu.Clear();
            _infoBus._countReload -= 1;
            btnReload.GetComponentInChildren<Text>().text =
                (_infoBus._countReload == 0 && GameObject.Find("Admob").GetComponent<AbManager>().RunAdmob)
                    ? "Thêm"
                    : _infoBus._countReload.ToString();

            reload();
        }
        else
        {
            if (GameObject.Find("Admob").GetComponent<AbManager>().RunAdmob)
            {
                this.type_Reward = "reload";
                this.started = false;
                this._pnGame.gameObject.SetActive(false);
                this._pnViewReload.gameObject.SetActive(true);
                bussSaveLoadData.saveGame(this, _infoTime._startTime - Time.time, timeMenu);
            }
        }
    }

    public void RewardCallback()
    {
        Audio.GetComponent<bussSound>().SoundAdmob();
        if (this.type_Reward == "level")
        {
            this.NoViewAdmob(false);
        }

        if (this.type_Reward == "idear")
        {
            this._infoBus._countIdear += 1;
            btnIdear.GetComponentInChildren<Text>().text = _infoBus._countIdear.ToString();
            this.btnViewReload.GetComponentInChildren<Text>().text = "Có";
            this.NoViewAddCount();
        }

        if (this.type_Reward == "reload")
        {
            this._infoBus._countReload += 1;
            btnReload.GetComponentInChildren<Text>().text = _infoBus._countReload.ToString();
            this.btnViewReload.GetComponentInChildren<Text>().text = "Có";
            this.NoViewAddCount();
        }

        bussSaveLoadData.saveGame(this, _infoTime._startTime - Time.time, timeMenu);
    }

    public void NoViewAddCount()
    {
        this.started = true;
        this._pnGame.gameObject.SetActive(true);
        this._pnViewReload.gameObject.SetActive(false);
        this._pnViewIdear.gameObject.SetActive(false);
    }


    public void LoadRewadAdRandom()
    {
        this.btnViewReload.GetComponentInChildren<Text>().text = "Đang tải ...";
        GameObject.Find("Admob").GetComponent<AbManager>().LoadRewardedVideoAds();
    }


    private void reload()
    {
        infoCreatePikachu infoCreatePikachu = null;
        int numberImg = 0;
        //int jT = 0;
        //int iT = 0;
        //bool _empty = false;
        for (int i = 2; i < cols; i++)
        {
            for (int j = 2; j < cols; j++)
            {
                for (int k = 2; k < rows; k++)
                {
                    for (int l = 2; l < rows; l++)
                    {
                        if (Random.Range(-1, 1) == 0 && _matrix[i, k]._empty == false && _matrix[j, l]._empty == false)
                        {
                            infoCreatePikachu = _matrix[i, k]._infoCreate;
                            numberImg = _matrix[i, k]._numberImg;
                            //iT = _matrix[i, k]._i;
                            //jT = _matrix[i, k]._j;
                            //_empty = _matrix[i, k]._empty;

                            _matrix[i, k]._infoCreate = _matrix[j, l]._infoCreate;
                            //_matrix[i, k]._i = _matrix[j, l]._i;
                            //_matrix[i, k]._j = _matrix[j, l]._j;
                            //_matrix[i, k]._empty = _matrix[j, l]._empty;
                            _matrix[i, k]._numberImg = _matrix[j, l]._numberImg;


                            _matrix[j, l]._infoCreate = infoCreatePikachu;
                            _matrix[j, l]._numberImg = numberImg;
                            //_matrix[j, l]._i = iT;
                            //_matrix[j, l]._j = jT;
                            //_matrix[j, l]._empty = _empty;


                            //infoCreatePikachu = null;
                        }
                    }
                }
            }
        }

        aiIDear();
    }


    private void aiIDear()
    {
        bussIdear = new bussIdear(_matrix);
        _idearPiakchu.Clear();

        for (int i = 2; i < cols; i++)
        {
            for (int j = 2; j < cols; j++)
            {
                for (int n = 2; n < rows; n++)
                {
                    for (int k = 2; k < rows; k++)
                    {
                        if (_matrix[i, n]._empty == false && _matrix[j, k]._empty == false)
                        {
                            if ((_matrix[i, n] != _matrix[j, k] &&
                                 bussIdear.check2Pikachu(_matrix[i, n], _matrix[j, k])))
                            {
                                _idearPiakchu.Add(_matrix[i, n]);
                                _idearPiakchu.Add(_matrix[j, k]);

                                break;
                            }
                        }
                    }

                    if (_idearPiakchu.Count > 0) break;
                }

                if (_idearPiakchu.Count > 0) break;
            }

            if (_idearPiakchu.Count > 0) break;
        }

        if (_idearPiakchu.Count == 0)
            reload();
        else
        {
            _matrix[_idearPiakchu[0]._i, _idearPiakchu[0]._j]._gameObject.GetComponent<RectTransform>()
                    .GetComponent<Button>().GetComponent<Image>().sprite
                = _matrix[_idearPiakchu[0]._i, _idearPiakchu[0]._j]._infoCreate._imgIdear;
            _matrix[_idearPiakchu[1]._i, _idearPiakchu[1]._j]._gameObject.GetComponent<RectTransform>()
                    .GetComponent<Button>().GetComponent<Image>().sprite
                = _matrix[_idearPiakchu[1]._i, _idearPiakchu[1]._j]._infoCreate._imgIdear;
        }


        bussIdear = null;
        if (!btnReload.interactable) btnReload.interactable = true;
        //bussSaveLoadData.saveGame(this, _infoTime._startTime - Time.time, timeMenu);
    }

    #endregion
}