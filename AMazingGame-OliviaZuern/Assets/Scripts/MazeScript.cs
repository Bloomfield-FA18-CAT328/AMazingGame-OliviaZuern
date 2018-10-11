using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeScript{
    // using primm's algorythm
    public byte[,] mapArray; //maze map.
    public List<Vector2> walList;
    public  int wall;
    public byte mapHeight = 10;
    public byte mapWidth = 5;

    public string mapString;
    public bool check;

   // private int i;
    //private int j;

    private int XX;
    public float cellX;
    public float cellY;
    public void MazeGen()
    {
        ResizeMap();
        mapArray = new byte[mapHeight, mapWidth];

        // note: 0=path, 1=wall, 2=untouched

        //1)Full grid of walls (false)
        for (int i = 0; i < mapWidth - 1; i++)
        {
            for (int j = 0; j < mapHeight - 1; j++)
            {
                mapArray[j, i] = 2;
            }
        }
        // DebugMap();

        //2)Pick cell, mark as part of maze, add cell walls to list
        cellX = Mathf.FloorToInt(Random.Range(1, (mapWidth - 2)));
        cellY = Mathf.FloorToInt(Random.Range(1, (mapHeight - 2)));

        mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY)] = 0;

        if (cellY >= 1){ walList.Add(new Vector2(cellX, cellY - 1)); }
        if (cellY <= mapHeight - 2) { walList.Add(new Vector2(cellX, cellY + 1)); }
        if (cellX >= 1) { walList.Add(new Vector2(cellX - 1, cellY)); }
        if(cellX <= mapWidth - 2) { walList.Add(new Vector2(cellX + 1, cellY)); }

      //  Debug.Log(mapArray[1, 0]);
        //3) while there are 3+ walls on list:
        while (walList.Count > 3)
        {

            // pick random wall from list. if only one of the two cells it divides is visited (0):
            // remove wall (true) and mark unvisited as part of maze  

            wall = Mathf.FloorToInt(Random.Range(0, walList.Count));

            cellX = walList[wall].x;
            cellY = walList[wall].y;
            //int visited = 0;
            check = false;
            if (check == false && cellX <= mapWidth - 2 && cellX >= 1)
            {
                if (mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY)] == 0 && mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY)] != 0)
                {
                    check = true;
                    mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY)] = 0;
                    mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY)] = 1;
                }
            }
            if (check == false && cellX >= 1 && cellX <= mapWidth - 2)
            {
                if (mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY)] == 0 && mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY)] != 0)
                {
                    check = true;
                    mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY)] = 0;
                    mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY)] = 1;
                }
            }
            if (check == false && cellY >= 1 && cellY <= mapHeight - 2)
            {
                if (mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY - 1)] == 0 && mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY + 1)] != 0)
                {
                    check = true;
                    mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY)] = 0;
                    mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY + 1)] = 1;
                }
            }
            if (check == false && cellY <= mapHeight - 2 && cellY >= 1)
            {
                if ( mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY + 1)] == 0 && mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY - 1)] != 0)
                {
                    check = true;
                    mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY)] = 0;
                    mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY - 1)] = 1;
                }
            }
            // add neighboring walls of cell to list
            if (check == true)
            {
                if (cellX >= 1 && cellX <= mapWidth - 2)
                {
                    if (mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY)] == 2) { walList.Add(new Vector2(cellX + 1, cellY)); }
                    if (mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY)] == 2) { walList.Add(new Vector2(cellX - 1, cellY)); }
                }
                if (cellY >= 1 && cellY <= mapHeight - 2)
                {
                    if (mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY - 1)] == 2) { walList.Add(new Vector2(cellX, cellY - 1)); }
                    if (mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY + 1)] == 2) { walList.Add(new Vector2(cellX, cellY + 1)); }
                }
            }


            /*  if (mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY + 1)] == 0) { visited += 1; }
              if (mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY + 1)] == 0) { visited += 1; }
              if (mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY - 1)] == 0) { visited += 1; }
              if (mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY - 1)] == 0) { visited += 1; }


              if (visited <= 1) {

                  mapArray[Mathf.FloorToInt(cellX), Mathf.FloorToInt(cellY)] = 0;

               if (mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY + 1)] != 0)
                        { mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY + 1)] = 1; }

                  if (mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY + 1)] != 0)
                        { mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY + 1)] = 1; }

                  if (mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY - 1)] != 0)
                        { mapArray[Mathf.FloorToInt(cellX + 1), Mathf.FloorToInt(cellY - 1)] = 1; }

                  if (mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY - 1)] != 0)
                         { mapArray[Mathf.FloorToInt(cellX - 1), Mathf.FloorToInt(cellY - 1)] = 1; }

      */


        // remove wall from list
        walList.Remove(walList[wall]);
    }
    }

    public void ResizeMap (){
        // height/width limits: Must be odd. (divisible by 2.) Min. 4x4
        if (Mathf.FloorToInt(mapHeight / 2) * 2 == mapHeight) {
            mapHeight += 1;
        }
        if (Mathf.FloorToInt(mapWidth / 2) * 2 == mapWidth) {
            mapWidth += 1;
        }
        if ( mapHeight < 7) {
            mapHeight = 7;
        }
        if (mapWidth < 7) {
            mapWidth = 7;
        }
       
        
    }
   public void DebugMap(){
        
       for (int i = 1; i <= mapWidth-1; i++)
        {
            for (int j = 1; j <= mapHeight-1; j++)
            {
               mapString += mapArray[j,i];
            }
            
           mapString += System.Environment.NewLine;
            //https://stackoverflow.com/questions/12826760/printing-2d-array-in-matrix-format
        }
        Debug.Log(mapString);
       
    }


}
