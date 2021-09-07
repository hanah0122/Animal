using UnityEngine;
using UnityEngine.UI;
public class infoCreatePikachu
{

    private int count;
    private Sprite img;
    private Sprite imgClick;
    private Sprite imgIdear;
    private int number;


    public infoCreatePikachu(int count  , Sprite img, Sprite imgClick, Sprite imgIdear, int number)
    {
        this._count = count;
        this._img = img;
        
        this._imgClick = imgClick;
        this._imgIdear = imgIdear;
        this.number = number;
    }

    public int _count { get => count; set => count = value; }
    public Sprite _img { get => img; set => img = value; }

    public Sprite _imgClick { get => imgClick; set => imgClick = value; }
    public Sprite _imgIdear { get => imgIdear; set => imgIdear = value; }
    public int _number { get => number; set => number = value; }
}