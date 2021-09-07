
using UnityEngine;

public class infoPikachu
{
    private int i;
    private int j;
    private bool empty;
    private GameObject gameObject;
    private infoCreatePikachu infoCreate;
    private int numberImg;

   

    public infoPikachu(int i , int j , infoCreatePikachu _infoCreate, bool empty ,GameObject gameObject, int _numberImg)
    {
        this._i = i;
        this._j = j;
        this._gameObject = gameObject;
        this._infoCreate = _infoCreate; 
        this._empty = empty;
        this.numberImg = _numberImg;
    }

    public int _i { get => i; set => i = value; }
    public bool _empty { get => empty; set => empty = value; }
    public GameObject _gameObject { get => gameObject; set => gameObject = value; }
    public infoCreatePikachu _infoCreate { get => infoCreate; set => infoCreate = value; }
    public int _j { get => j; set => j = value; }
    public int _numberImg { get => numberImg; set => numberImg = value; }
}