using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AssemblyCSharp.Assets.Scripts.BUSS
{
    public class bussIdear 
    {
        private infoPikachu[,] _matrix;
        private bool checkMove;
        private int t;
        public bussIdear(infoPikachu[,] _matrix)
        {
            this._matrix = _matrix;
            this.checkMove = false;
            this.t = -1;

        }

        public bool check2Pikachu(infoPikachu node1, infoPikachu node2 )
        {
            for (int i = 0; i < (this._matrix.GetLength(0) - 2); i++)
                for (int j = 0; j < (this._matrix.GetLength(1) - 2); j++)
                    if (i > 1 && j > 1 && i <= (this._matrix.GetLength(0) - 1) && j <= (this._matrix.GetLength(1) - 1))
                        if(!this._matrix[i,j]._empty)
                            this._matrix[i, j]._gameObject.GetComponent<RectTransform>().GetComponent<Button>().GetComponent<Image>().sprite = this._matrix[i, j]._infoCreate._img;
            

            node1._empty = node2._empty = true;
            if (node1._infoCreate._img.ToString().ToLower() == node2._infoCreate._img.ToString().ToLower())
            {

                if(node1._i == node2._i) 
                    this.checkMove = this.checkLineX(node1._j, node2._j, node1._i);
                
                if(node1._j == node2._j)
                    this.checkMove = this.checkLineY(node1._i, node2._i, node1._j);

                if (this.checkMove)
                {
                    node1._empty = node2._empty = false;
                    return checkMove;
                }
                   
                else
                {
                    if(this.t == -1)
                        this.t = this.checkRectX(node1, node2);
                    if (this.t == -1)
                        this.t = this.checkRectY(node1, node2);
                    if (this.t == -1)
                        this.t = this.checkMoreLineX(node1, node2, 1);
                    if (this.t == -1)
                        this.t = this.checkMoreLineX(node1, node2, -1);
                    if (this.t == -1)
                        this.t = this.checkMoreLineY(node1, node2, 1);
                    if (this.t == -1)
                        this.t = this.checkMoreLineY(node1, node2, -1);
                    if(this.t != -1)
                    {
                        //Debug.Log("[" + node1._i + "," + node1._j + "],[" + node2._i + "," + node2._j + "]");
                        node1._empty = node2._empty = false;
                        return true;
                    }

                }
            }
            node1._empty = node2._empty = false;
            return false;
        }

        private bool checkLineX(int y1, int y2, int x)
        {
            int min = Math.Min(y1, y2);
            int max = Math.Max(y1, y2);

           
            for (int y = min; y <= max; y++)
            {
                if (this._matrix[x, y]._empty == false)
                {
                    return false;
                }
            }
            return true;
        }

        private bool checkLineY(int x1, int x2, int y)
        {
            int min = Math.Min(x1, x2);
            int max = Math.Max(x1, x2);
            for (int x = min; x <= max; x++)
            {
                if (this._matrix[x, y]._empty == false)
                {
                    return false;
                }
            }
            return true;
        }
        private int checkRectX(infoPikachu p1, infoPikachu p2)
        {
            infoPikachu pMinY = p1;
            infoPikachu pMaxY = p2;
            if (p1._j > p2._j)
            {
                pMinY = p2;
                pMaxY = p1;
            }
            for (int y = pMinY._j + 1; y < pMaxY._j; y++)
            {
                if (checkLineX(pMinY._j, y, pMinY._i) && checkLineY(pMinY._i, pMaxY._i, y) && checkLineX(y, pMaxY._j, pMaxY._i))
                    return y;
                
            }
            pMinY = pMinY = p1 = p2 = null;
            return -1;
        }

        private int checkRectY(infoPikachu p1, infoPikachu p2)
        {
            infoPikachu pMinX = p1;
            infoPikachu pMaxX = p2;
            if (p1._i > p2._i)
            {
                pMinX = p2;
                pMaxX = p1;
            }
            for (int x = pMinX._i + 1; x < pMaxX._i; x++)
            {
                if (checkLineY(pMinX._i, x, pMinX._j)
                        && checkLineX(pMinX._j, pMaxX._j, x)
                        && checkLineY(x, pMaxX._i, pMaxX._j))
                {
                   
                    pMinX = pMaxX = p1 = p2 = null;
                    return x;
                }
            }

            pMinX = pMaxX = p1 = p2 = null;
            return -1;
        }

        private int checkMoreLineX(infoPikachu p1, infoPikachu p2, int type)
        {
            infoPikachu pMinY = p1;
            infoPikachu pMaxY = p2;
            if (p1._j > p2._j)
            {
                pMinY = p2;
                pMaxY = p1;
            }
            int y = pMaxY._j;
            int row = pMinY._i;
            if (type == -1)
            {
                y = pMinY._j;
                row = pMaxY._i;
            }
 
            if (checkLineX(pMinY._j, pMaxY._j, row))
            {
                while (this._matrix[pMinY._i, y]._empty == true && this._matrix[pMaxY._i, y]._empty == true)
                {
                    if (checkLineY(pMinY._i, pMaxY._i, y))
                    {
                        pMinY = pMaxY = p1 = p2 = null;
                        return y;
                    }
                    y += type;
                }
            }
            pMinY = pMaxY = p1 = p2 = null;
            return -1;
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
                while (this._matrix[x, pMinX._j]._empty == true
                        && this._matrix[x, pMaxX._j]._empty == true)
                {
                    if (checkLineX(pMinX._j, pMaxX._j, x))
                    {
                        pMinX = pMaxX = p1 = p2 = null;
                        return x;
                    }
                    x += type;
                }
            }
            pMinX = pMaxX = p1 = p2 = null;
            return -1;
        }


    }
}
