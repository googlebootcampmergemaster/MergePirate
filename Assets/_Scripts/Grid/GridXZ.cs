using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class GridXZ <TGridObject> // Generic class to create grid in XZ axis
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged; // OnGridObjectChangedEventArgs
    public class OnGridObjectChangedEventArgs : EventArgs { // EventArgs is a class that is used to pass desired data to an event.
        public int x;
        public int z;
    }
    
    private int width; // width of the grid in grid units
    private int height; // height of the grid in grid units
    //private float cellSize; // size of a grid unit
    private float cellSizeX;
    private float cellSizeZ;
    private Vector3 originPosition; // the world position of the grid origin (bottom left corner)
    private TGridObject[,] gridArray; // 2D array of grid units - default value is null for every section of array
    
    public GridXZ(int width, int height, /*float cellSize*/float cellSizeX, float cellSizeZ, Vector3 originPosition, Func<GridXZ<TGridObject>, int, int, TGridObject> createGridObject) {
        //Func<GridXZ<TGridObject>, int, int, TGridObject> createGridObject - function that creates a grid object from any type of objects
        this.width = width;
        this.height = height;
        this.cellSizeX = cellSizeX;
        this.cellSizeZ = cellSizeZ;
        //this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++) { // loop through the width of the grid
            for (int z = 0; z < gridArray.GetLength(1); z++) {  // loop through the height of the grid
                gridArray[x, z] = createGridObject(this, x, z); // create a grid object at the current x and z position in order to fill the gridArray so there will be no bugs
            }
        }

        bool showDebug = true; // if it is true, the grid will be drawn in the scene view
        if (showDebug) {
            TextMesh[,] debugTextArray = new TextMesh[width, height]; // 2D array of TextMesh components to display grid units

            for (int x = 0; x < gridArray.GetLength(0); x++) {
                for (int z = 0; z < gridArray.GetLength(1); z++) {
                    debugTextArray[x, z] = UtilsClass.CreateWorldText(gridArray[x, z]?.ToString(), null, GetWorldPosition(x, z) + new Vector3(cellSizeX, 0, cellSizeZ) * .5f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => { // it is like a delegate, it is a function that is called when the event is triggered so we can use it to update the text mesh
                debugTextArray[eventArgs.x, eventArgs.z].text = gridArray[eventArgs.x, eventArgs.z]?.ToString(); //it is only updating the value of the text mesh when the event is triggered
            };
        }
    }
    public int GetWidth() {
        return width;
    }

    public int GetHeight() {
        return height;
    }

    /*public float GetCellSize() {
        return cellSize;
    }*/
    public float GetCellSizeX() {
        return cellSizeX;
    }
    public float GetCellSizeZ() {
        return cellSizeZ;
    }

    public Vector3 GetWorldPosition(int x, int z) { // GetWorldPosition is a function that returns the world position of a grid unit
        return new Vector3(x * cellSizeX, 0, z* cellSizeZ)  + originPosition;
    }

    public Vector3 GetWorldPositionCenterOfGrid(int x, int z)
    {
        return new Vector3(x * cellSizeX + cellSizeX / 2, 0, z * cellSizeZ + cellSizeZ / 2) + originPosition;
    }
    
    public void GetXZ(Vector3 worldPosition, out int x, out int z) { // it is used to get the grid unit coordinates of a world position
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSizeX);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSizeZ);
    }

    public void SetGridObject(int x, int z, TGridObject value) { //setting grid object based on grid coordinates
        if (x >= 0 && z >= 0 && x < width && z < height) { // if the x and z coordinates are within the grid-- check wrong values
            gridArray[x, z] = value;
            TriggerGridObjectChanged(x, z);
        }
    }

    public void TriggerGridObjectChanged(int x, int z) { // trigger the event when the grid object is changed 
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, z = z });
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value) {
        GetXZ(worldPosition, out int x, out int z); //get the grid coodinates of the world position and store them in x and z
        SetGridObject(x, z, value); // set the desired object at the grid coordinates
    }

    public TGridObject GetGridObject(int x, int z) { //return the grid object at the grid coordinates
        if (x >= 0 && z >= 0 && x < width && z < height) {
            return gridArray[x, z];
        } else {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition) { //return the grid object at the world position
        int x, z;
        GetXZ(worldPosition, out x, out z);
        return GetGridObject(x, z);
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridPosition) { 
        return new Vector2Int(
            Mathf.Clamp(gridPosition.x, 0, width - 1),
            Mathf.Clamp(gridPosition.y, 0, height - 1)
        );
    }
}
