using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets up and displays a grid on the map
public class GridGenerator : MonoBehaviour {
    
    [SerializeField] private GameObject[,] grid;
    [SerializeField] private int rows;
    [SerializeField] private int columns;
    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private GameObject unwalkable_nodePrefab;
    [SerializeField] private Material reset;
    [SerializeField] private Terrain terrain;
    [SerializeField] private LayerMask unwalkableMask;
    
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
                bool walkable = !(Physics.CheckSphere(pos, xSpacing/2,unwalkableMask)); // xSpacing = zSpacing in this terrian
                GameObject node = (walkable) ? Instantiate(nodePrefab): Instantiate(unwalkable_nodePrefab);
                pos.y = terrain.SampleHeight(pos); // get y-pos on terrian
                node.transform.position = pos; // set position of new node
                if( node != null )
                {
                    var myScriptReference = node.GetComponent<Node>();
                    if( myScriptReference != null )
                    {
                        myScriptReference.setNode(walkable,pos,x,z);
                    }
                }

                
                
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

    public GameObject GetNodeByCoor(int x, int z)
    {
        return grid[x,z];
    }
    
    /// <summary>
    /// Gets the cooresponding node on the grid from a coordinate in world space
    /// </summary>
    /// <param name="position">Position in World Space</param>
    /// <returns></returns>
    public GameObject GetNodeFromPosition(Vector3 position)
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
        //int[] values = { x, z };
        return grid[x,z];
    }

    /// <summary>
    /// Gets the the all neighbours node on the grid to chosen node
    /// </summary>
    /// <param name="node">node that need to get all neighbours</param>
    /// <returns></returns>
    public  List<GameObject> GetNodeNeighbours(Node node)
    {
        List<GameObject> neighbours = new List<GameObject>();
//                y            
//                1           
// x        -1   node    1
//               -1  
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    // that is the origin 
                    continue;
                }

                int neighbour_x = x + node.x;
                int neighbour_y = y + node.y;
                
                if (neighbour_x >= 0 && neighbour_x < rows && neighbour_y >= 0 && neighbour_y < columns)
                {
                    // to check the node is eligible
                    GameObject neighbour = grid[neighbour_x, neighbour_y];

                    if (neighbour.GetComponent<Node>().walkable)
                    {
                        // check the node is walkable
                        neighbours.Add(grid[neighbour_x, neighbour_y]);
                    }
                }

            }
        }
//        // add portal connection
//        if (node.x == 20 && node.y == 20)
//        {
//            neighbours.Add(grid[26, 25]);
//        }else  if (node.x == 26 && node.y == 25)
//        {
//            neighbours.Add(grid[20, 20]);
//        }
        
        return neighbours;
    }
    public void wipePath()
    {
        foreach (var gameObject in grid)
        {
            Node node= gameObject.GetComponent<Node>();
            if (node.parent != null)
            {
                gameObject.GetComponent<MeshRenderer>().material = reset;
            }
            node.parent = null;
        }
    }
}

