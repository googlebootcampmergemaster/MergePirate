using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using CodeMonkey.Utils;
public class GridBuildingSystem : MonoBehaviour
{
    private GridXZ<GridObject> grid;
    [SerializeField] private Transform gridObjectPrefab;
    private void Awake()
    {
        int gridWidth = 8;
        int gridHeight = 8;
        float cellSizeX = 4f;
        float cellSizeZ = 8f;
        //float gridCellSize = 10f;
        grid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSizeX,cellSizeZ, Vector3.zero, (GridXZ<GridObject> grid, int x, int z) => new GridObject(grid, x, z));
    }

    public class GridObject //encapsulates the grid object
    {
        private GridXZ<GridObject> grid;
        private int x;
        private int z;
        
        private Transform transform;


        public GridObject(GridXZ<GridObject> grid, int x, int z)
        {   
            
            this.grid = grid;
            this.x = x;
            this.z = z;
        }
        public void SetTransform(Transform transform)
        {
            this.transform = transform;
            grid.TriggerGridObjectChanged(x,z);
        }
        
        public void ClearTransform()
        {
            this.transform = null;
            grid.TriggerGridObjectChanged(x,z);
        }

        public bool CanBuild()
        {
            return transform == null;
        }
        
        public Transform GetTransform()
        {
            return transform;
        }
        
        public override string ToString()
        {
            return x + "," + z + "\n" + transform;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {   
            grid.GetXZ(Mouse3D.GetMouseWorldPosition(),out int x, out int z);
            GridObject gridObject = grid.GetGridObject(x, z);
            if (gridObject.CanBuild())
            {   Transform buildTransform = Instantiate(gridObjectPrefab, grid.GetWorldPositionCenterOfGrid(x,z),Quaternion.identity);
                gridObject.SetTransform(buildTransform);
            }
            else
            {
                UtilsClass.CreateWorldTextPopup("Cannot merge!", Mouse3D.GetMouseWorldPosition());
            }
            
        }
    }

    private void InstantiateGridObjectRandomly(GridXZ<GridObject> gridObject, Transform gridObjectPrefab)
    {   bool end = false;
        while (!end)
        {
            for (int x = 0; x < gridObject.GetWidth() ; x++)
            {
                for (int z = 0; z < gridObject.GetHeight(); z++)
                {
                    if (grid.GetGridObject(x,z).CanBuild())
                    {
                        Transform buildTransform = Instantiate(gridObjectPrefab, gridObject.GetWorldPositionCenterOfGrid(x,z),Quaternion.identity);
                        gridObject.GetGridObject(x,z).SetTransform(buildTransform);
                        end = true;
                    }
                    
                }
                
            }
        }
        
        
    }
    
    
}
