using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets up and displays a grid on the map
public class GridGenerator : MonoBehaviour {
    
    [SerializeField] private GameObject[,] grid;
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Terrain terrain;
    
    void Start ()
    {
        // instantiate grid
        grid = new GameObject[rows, columns];

        //fill grid with nodes
        GenerateGrid();
    }
	
	void Update ()
    {
	}

    
    // HELPER METHODS-------------------------------------------------------------------------

    /// <summary>
    /// Generates the grid nodes for the influence map
    /// </summary>
    private void GenerateGrid()
    {
        // get spacing between nodes according to rows/columns and size of terrain, xz-plane
        float xSpacing = terrain.terrainData.size.x / rows;
        float zSpacing = terrain.terrainData.size.z / columns;
        
        // node position
        Vector3 pos = new Vector3(0, 0, 0);
        pos.x = terrain.transform.position.x + (xSpacing / 2); // offset to start in corners
        pos.z = terrain.transform.position.z + (zSpacing / 2); 

        // fill with nodes
        for (int x = 0; x < rows; x++)
        {
            for (int z = 0; z < columns; z++)
            {
                // create the node
                GameObject node = Instantiate(nodePrefab);
                pos.y = terrain.SampleHeight(pos); // get y-pos on terrian
                node.transform.position = pos; // set position of new node
                
                // add to matrix
                grid[x, z] = node;

                // shift z for next
                pos.z += zSpacing;
            }

            // shift x for next
            pos.x += xSpacing;

            // reset pos z for next row
            pos.z = terrain.transform.position.z + (zSpacing / 2);
        }
    }

    /// <summary>
    /// Gets the cooresponding node on the grid from a coordinate in world space
    /// </summary>
    /// <param name="position">Position in World Space</param>
    /// <returns></returns>
    public int[] GetNodeFromPosition(Vector3 position)
    {
        //find spread of nodes
        float xSpacing = terrain.terrainData.size.x / rows;
        float zSpacing = terrain.terrainData.size.z / columns;

        //remove inital offset from position
        position.x -= terrain.transform.position.x;
        position.z -= terrain.transform.position.z;

        //divide xSpacing and zSpacing out of position
        position.x = position.x / xSpacing;
        position.z = position.z / zSpacing;

        //convert position values to ints
        int x = (int)position.x;
        int z = (int)position.z;

        //give back the node that covers that area
        int[] values = { x, z };
        return values;
    }
}
