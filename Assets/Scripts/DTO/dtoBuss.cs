using UnityEngine.UI;
public class infoBuss
{

    private Image pnGame;
    private int level;
    private int countReload;
    private int countIdear;

    public infoBuss()
    {
        this._level = 0;
    }

    public Image _pnGame { get => pnGame; set => pnGame = value; }
    public int _level { get => level; set => level = value; }
    public int _countIdear { get => countIdear; set => countIdear = value; }
    public int _countReload { get => countReload; set => countReload = value; }
}
