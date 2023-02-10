using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject map;
    public Image img;

    public int offsetX;
    public int offsetY;

    public void Print()
    {
        map = GameObject.Find("MapGenerator");
        int interval = 10;
        int x = 0;
        int y = 0;
        for(int i = 1; i <= 15 * 15; i++)
        {
            Vector2 pos = new Vector2(x,y);
            if(!map.GetComponent<MapGenerator>().room[i].IsVacant())
            {
                Image tmp;
                tmp = Instantiate(img, pos, Quaternion.identity, transform);
                if(map.GetComponent<MapGenerator>().room[i].GetRoomType() == "start")
                    tmp.color = Color.green;
                else if (map.GetComponent<MapGenerator>().room[i].GetRoomType() == "boss")
                    tmp.color = Color.red;
                else if (map.GetComponent<MapGenerator>().room[i].GetRoomType() == "shop")
                    tmp.color = Color.blue;
            }

            if(i % 15 == 0)
            {
                x = 0;
                y += interval;
            }
            else
            {
                x += interval;
            }
        }
        transform.rotation = Quaternion.Euler(0, 0, 135f);
        transform.position = new Vector2(offsetX, offsetY);
    }

    void Start()
    {
        Print();
    }
}
