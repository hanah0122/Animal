using UnityEngine;
using UnityEngine.UI;
public class infoTime
{
    private Text TxtTime;
    private float GameTime;
    private Slider Slider;
    private ParticleSystem ParticleSystem;

    private float startTime;
    private float timer;
    private float coutDown;



    public Text _txtTime { get => TxtTime; set => TxtTime = value; }
    public float _gameTime { get => GameTime; set => GameTime = value; }
    public Slider _slider { get => Slider; set => Slider = value; }
    public ParticleSystem _particleSystem { get => ParticleSystem; set => ParticleSystem = value; }

    public float _startTime { get => startTime; set => startTime = value; }
    public float _timer { get => timer; set => timer = value; }
    public float _coutDown { get => coutDown; set => coutDown = value; }

    public infoTime()
    {
        this._gameTime = 60;
        this.startTime = 0;
    }
}