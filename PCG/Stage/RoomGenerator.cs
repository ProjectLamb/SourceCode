using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public void SetRoomType(string s)
    {
        this.type = s;
    }
    public void SetPortal(bool e, bool w, bool s, bool n)
    {
        if(e)
        portalE = true;
        if(w)
        portalW = true;
        if(s)
        portalS = true;
        if(n)
        portalN = true;
    }

    public void SetRoomLocation(float x, float z)
    {
        this.x = x;
        this.z = z;
    }
    public int GetMaxSize()
    {
        return (15 * 3) * (15 * 3);
    }
    
    public Vector3 GetRoomLocation()
    {
        Vector3 roomPos = new Vector3(this.x, 0, this.z);
        return roomPos;
    }

    public int GetWidth()
    {
        return width;
    }
    int width = 10;
    int height;
    [SerializeField]
    int size;
    int maxSize;
    float x;
    float z;
    int roomSizeRandom;
    [SerializeField]
    string type;
    int mobAmount;
    [SerializeField]
    private bool portalE = false;
    [SerializeField]
    private bool portalW = false;
    [SerializeField]
    private bool portalS = false;
    [SerializeField]
    private bool portalN = false;
    public GameObject tile;
    public GameObject[,] tileArray;
    GameObject RoomCamera;

    void InstantiateTile(int r, int c)
    {
        float interval = 10f;   //tile interval
        float x = transform.position.x - interval * (r / 2);
        float z = transform.position.z - interval * (c / 2);
        for(int i = 1; i <= r; i++)
        {
            for(int j = 1; j <= c; j++)
            {
                GameObject instance;
                Vector3 pos = new Vector3(x + interval * i, 0, z + interval * j);
                instance = Instantiate(tile, pos, Quaternion.identity);
                instance.transform.parent = transform;
                if(j == 1 || i == width)
                    instance.GetComponent<Renderer>().material.color = Color.black; //instantiate wall
                else
                {
                    if(type == "shop")
                        instance.GetComponent<Renderer>().material.color = Color.blue;
                    else if(type == "start")
                        instance.GetComponent<Renderer>().material.color = Color.green;
                    else if (type == "boss")
                        instance.GetComponent<Renderer>().material.color = Color.red;
                }
                tileArray[i,j] = instance;
            }
        }
    }
    
    void InstantiatePortal()
    {
        if(portalE)
        {
            tileArray[1, height/2 + 1].GetComponent<Renderer>().material.color = Color.cyan;
            tileArray[1, height/2 + 1].tag = "Portal";
            tileArray[1, height/2 + 1].GetComponent<Tile>().SetPortalType("east");
        }
        if(portalW)
        {
            tileArray[width,height/2 + 1].GetComponent<Renderer>().material.color = Color.cyan;
            tileArray[width,height/2 + 1].tag = "Portal";
            tileArray[width, height/2 + 1].GetComponent<Tile>().SetPortalType("west");
        }
        if(portalN)
        {
            tileArray[width/2 + 1,1].GetComponent<Renderer>().material.color = Color.cyan;
            tileArray[width/2 + 1,1].tag = "Portal";
            tileArray[width/2 + 1,1].GetComponent<Tile>().SetPortalType("north");
        }
        if(portalS)
        {
            tileArray[width/2 + 1,height].GetComponent<Renderer>().material.color = Color.cyan;
            tileArray[width/2 + 1,height].tag = "Portal";
            tileArray[width/2 + 1,height].GetComponent<Tile>().SetPortalType("south");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        RoomCamera = transform.GetChild(0).gameObject;
        if(type == "normal")
            roomSizeRandom = Random.Range(1,4);
        else
            roomSizeRandom = 1;
        width += 5 * roomSizeRandom;
        height = width;
        tileArray = new GameObject[width + 1, height + 1];
        //RoomCamera.transform.position = new Vector3(RoomCamera.transform.position.x - 15 * (roomSizeRandom - 1), RoomCamera.transform.position.y, RoomCamera.transform.position.z + 15 * (roomSizeRandom - 1));
        InstantiateTile(width,height);
        InstantiatePortal();
        size = width * height;
        if(type == "start")
        {
            GameObject character = GameObject.Find("Character");
            character.transform.position = new Vector3(transform.localPosition.x, GameObject.Find("Character").transform.localScale.y, transform.localPosition.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
