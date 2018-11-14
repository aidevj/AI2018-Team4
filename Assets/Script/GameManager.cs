
using UnityEngine;
using System.Collections;

//add using System.Collections.Generic; to use the generic array format
using System.Collections.Generic;
using UnityEngine.UI;

// This script manages overall variables and operations otherwise outside the scope of the vehicle script


public class GameManager : MonoBehaviour
{
    RaycastHit hit;
    Ray mouseray;
    public int rows;
    public int columns;
    public GameObject black;
    public GameObject yellow;
    public GameObject blue;
    public GameObject white;
    [SerializeField] private UnityEngine.UI.Text uitext;

    public int blackStrength = 4;
    public int yellowStrength = 3;
    public int blueStrength = 2;
    public int whiteStrength = 1;

    private string currentColor = "black";
    private Team currentTeam = Team.Green;
    private GameObject hoveredNode;
    private bool[,] units;

    void Start() // Initialize the scene
    {
        units = new bool[rows,columns];
    }

    void Update() // Update the scene variables, called once per frame
    {
        if (Input.GetKeyDown(KeyCode.R)) currentTeam = Team.Red;
        if (Input.GetKeyDown(KeyCode.G)) currentTeam = Team.Green;
        if (Input.GetKeyDown(KeyCode.Alpha1)) currentColor = "white";
        if (Input.GetKeyDown(KeyCode.Alpha2)) currentColor = "blue";
        if (Input.GetKeyDown(KeyCode.Alpha3)) currentColor = "yellow";
        if (Input.GetKeyDown(KeyCode.Alpha4)) currentColor = "black";
        string text = uitext.text;
        int index = text.LastIndexOf("Current Team");
        if (index > 0)
            text = text.Substring(0, index);
        text += "Current Team: " + currentTeam;
        text += "\nCurrent Color: " + currentColor;
        uitext.text = text;

        Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction.normalized * 300f, Color.red);

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 300f, 1<<10)) // Raycast from cursor, check against any interactables
        {
            if (hoveredNode) hoveredNode.transform.localScale = new Vector3(3, 1, 3);
            hit.transform.localScale = new Vector3(4, 1, 4);
            hoveredNode = hit.transform.gameObject;
            var nodeToCheck = hit.transform.gameObject.GetComponent<Node>();
            if (Input.GetMouseButtonDown(0)&&!units[nodeToCheck.x,nodeToCheck.y])
            {
                int currentStrength = 0;
                //update influence map
                GameObject newObject;
                switch (currentColor)
                {
                    case "white":
                        newObject = Instantiate(white, hit.transform.position, Quaternion.identity);
                        //hit.transform.gameObject.GetComponent<Node>().containedUnit = whiteStrength;
                        hit.transform.gameObject.GetComponent<Node>().addInfluence(whiteStrength,currentTeam);
                        currentStrength = whiteStrength;
                        break;
                    case "blue":
                        newObject = Instantiate(blue, hit.transform.position, Quaternion.identity);
                        //hit.transform.gameObject.GetComponent<Node>().containedUnit = blueStrength;
                        hit.transform.gameObject.GetComponent<Node>().addInfluence(blueStrength,currentTeam);
                        currentStrength = blueStrength;
                        break;
                    case "yellow":
                        newObject = Instantiate(yellow, hit.transform.position, Quaternion.identity);
                        //hit.transform.gameObject.GetComponent<Node>().containedUnit = yellowStrength;
                        hit.transform.gameObject.GetComponent<Node>().addInfluence(yellowStrength,currentTeam);
                        currentStrength = yellowStrength;
                        break;
                    case "black":
                        newObject = Instantiate(black, hit.transform.position, Quaternion.identity);
                        Debug.Log("fuck "+newObject.transform.position);//wow, language
                        //hit.transform.gameObject.GetComponent<Node>().containedUnit = blackStrength;
                        hit.transform.gameObject.GetComponent<Node>().addInfluence(blackStrength,currentTeam);
                        currentStrength = blackStrength;
                        break;
                    default:
                        newObject = Instantiate(black, hit.transform.position, Quaternion.identity);
                        hit.transform.gameObject.GetComponent<Node>().addInfluence(blackStrength,currentTeam);
                        currentStrength = blackStrength;
                        break;
                }
                hit.transform.gameObject.GetComponent<Node>().currentTeam = currentTeam;
                var temp = hit.transform.gameObject.GetComponent<Node>();
                units[temp.x, temp.y] = true; // allocated
                temp.Refresh();
                GridGenerator._instance.RangeUpdate(temp,currentStrength,currentTeam);
            }
        }
        else if (hoveredNode) hoveredNode.transform.localScale = new Vector3(3, 1, 3);
    }
}
