[System.Serializable]
public class bussData
{
    public int level;
    public int point;
    public int countIdear;
    public int countReload;
    public clsMatrix[,] _matrix;
    public int winPikachu;
    public float _startTime;
    public int timeMenu;
    public bussData(bussGame game, float _startTime, int timeMenu)
    {
        this.level = game._infoBus._level;
        this.point = game._point;
        this.countIdear = game._infoBus._countIdear;
        this.countReload = game._infoBus._countReload;
        this.winPikachu = game.winPikachu;
        this._startTime = _startTime;
        this.timeMenu = timeMenu;
        this._matrix = new clsMatrix[game._matrix.GetLength(0), game._matrix.GetLength(1)];

        for (int i = 2; i < game._matrix.GetLength(0); i++)
        {
            for (int j = 2; j < game._matrix.GetLength(1); j++)
            {
                _matrix[i, j] = new clsMatrix(game._matrix[i, j]._i, game._matrix[i, j]._j,
                    game._matrix[i, j]._empty, game._matrix[i, j]._numberImg);

            }
        }
    }

    private int getNumImg(infoCreatePikachu info)
    {
        if (info != null)
            return info._number;
        return -1;
    }

}


[System.Serializable]
public class clsMatrix
{
    public int i;
    public int j;
    public bool empty;
    public int imgNunber;

    public clsMatrix(int i, int j, bool empty, int imgNunber)
    {
        this.i = i;
        this.j = j;
        this.empty = empty;
        this.imgNunber = imgNunber;
    }
}
