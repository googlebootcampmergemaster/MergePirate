using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;
using CodeMonkey.Utils;
public class GridBuildingSystem : MonoBehaviour
{
    private GridXZ<GridObject> grid;
    [SerializeField] private GameObject gridObjectPrefab;
    private void Awake()
    {
        int gridWidth = 8;
        int gridHeight = 8;
        float gridCellSize = 10f;
        grid = new GridXZ<GridObject>(gridWidth, gridHeight, gridCellSize, Vector3.zero, (GridXZ<GridObject> grid, int x, int z) => new GridObject(grid, x, z));
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
            return x + " " + z + transform;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {   grid.GetXZ(Mouse3D.GetMouseWorldPosition(),out int x, out int z);
            Debug.Log(x + " " + z);
            GridObject gridObject = grid.GetGridObject(x, z);
            if (gridObject.CanBuild())
            {   
                gridObject.SetTransform(gridObjectPrefab.transform);
            }
            
        }
    }

    
    
    
}
