using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MapGenerator : MonoBehaviour
{
    int maxX = 15;
    int maxY = 15;

    [Tooltip("Amount is random from N to N + 2")]

    public int roomAmount; //editable or not
    public int minimumDistanceOfEndRoom;
    public List<GameObject> roomArray = new List<GameObject>();
    public GameObject roomObject;
    public struct Room
    {
        // public Room(int n)
        // {
        //     roomNumber = n;
        //     type = "normal";
        //     vacancy = true;
        //     distanceFromStart = 0;
        // }
        public int GetRoomNumber()
        {
            return roomNumber;
        }
        public void SetRoomNumber(int n)
        {
            roomNumber = n;
        }
        public void SetRoomType(string s)
        {
            type = s;
        }
        public string GetRoomType()
        {
            return type;
        }
        public bool IsVacant()
        {
            return vacancy;
        }
        public void OccupyRoom()
        {
            vacancy = false;
        }
        public void ResetRoom()
        {
            type = "normal";
            vacancy = true;
            distanceFromStart = 0;
        }
        public bool CheckAdjacency(Room[] r, int maxX)
        {
            int i = 0;
            if (!r[roomNumber - 1].vacancy)
                i++;
            if (!r[roomNumber + 1].vacancy)
                i++;
            if (!r[roomNumber - maxX].vacancy)
                i++;
            if (!r[roomNumber + maxX].vacancy)
                i++;

            if (i > 1)
                return true;
            else
                return false;
        }
        public bool CheckEndRoom(Room[] r, int maxX)
        {
            if (r[roomNumber].type == "start")
                return false;

            int i = 0;
            if (!r[roomNumber - 1].vacancy)
                i++;
            if (!r[roomNumber + 1].vacancy)
                i++;
            if (!r[roomNumber - maxX].vacancy)
                i++;
            if (!r[roomNumber + maxX].vacancy)
                i++;

            if (i == 1)
                return true;
            else
                return false;
        }
        public void SetDistanceFromStart(int init, int maxX)
        {
            int distance = 0;
            int initLine = init - (maxX / 2);
            int tmp = roomNumber;
            if(tmp < init)
            {
                while(tmp < initLine || tmp > initLine + maxX - 1)
                {
                    tmp += maxX;
                    distance++;
                }
                if(tmp < init)
                    distance += init - tmp;
                else
                    distance += tmp - init;
            }
            else
            {
                while(tmp < initLine || tmp > initLine + maxX - 1)
                {
                    tmp -= maxX;
                    distance++;
                }
                if(tmp < init)
                    distance += init - tmp;
                else
                    distance += tmp - init;
            }
            distanceFromStart = distance;
        }

        public int GetDistanceFromStart()
        {
            return distanceFromStart;
        }
        private int roomNumber;	//�� ��ȣ 1 ~ maxRoom
        private bool vacancy;
        private int distanceFromStart;
        private string type;	//start, normal, shop, boss, boundary
    };

    public Room[] room;
    bool random()
    {
        int n = Random.Range(0, 2);
        if (n == 1)
            return true;
        else
            return false;
    }
    void GenerateStage(int n)
    {
        InfiniteLoopDetector.Run();
        room = new Room[maxX * maxY + 1];
        int roomAmount = n;
        int maxRoom = maxX * maxY;

        for (int i = 1; i <= maxRoom; i++)
        {
            room[i].SetRoomNumber(i);
            if((i >= 1 && i <= maxX) || i % maxX == 1 || i % maxX == 0 || (i >= (maxRoom - maxX + 1) && i <= maxRoom))
            {
                room[i].SetRoomType("boundary");
            }
        }

        Queue<int> q = new Queue<int>();
        Queue<int> endQ = new Queue<int>();

        int initRoomNumber = 1 + (maxX / 2) + (maxY / 2 * maxX);

        q.Enqueue(room[initRoomNumber].GetRoomNumber());
        room[initRoomNumber].SetRoomType("start");
        int amount = 0;

        while (amount != roomAmount)
        {
            if (q.Count == 0)
            {
                for (int i = 1; i <= maxRoom; i++)
                    room[i].ResetRoom();
                q.Enqueue(room[initRoomNumber].GetRoomNumber());
                room[initRoomNumber].SetRoomType("start");
                amount = 0;
            }

            if (q.Peek() >= 1 && q.Peek() <= maxRoom && room[q.Peek()].IsVacant())	//�� ��ȣ�� 1 ~ maxRoom�����̸� �ߺ��� �ƴ� ���
            {
                room[q.Peek()].OccupyRoom();
                amount++;
            }

            for (int i = 0; i < 4; i++)	//���� �� ���� �˰�����, �� �� �� �� ������ 4�� ����
            {
                int num = q.Peek();	//�� ��ȣ

                switch (i)
                {
                case 0:	//Left
                    num--;
                    break;
                case 1:	//Right
                    num++;
                    break;
                case 2:	//Up
                    num -= maxX;
                    break;
                case 3:	//Down
                    num += maxX;
                    break;
                }

                if (room[num].GetRoomType() == "boundary")
                    continue;
                if (!room[num].IsVacant())	//���� ���� ���� �ִٸ�
                    continue;
                if (room[num].CheckAdjacency(room, maxX))	//���� ���� �� ��ü�� 2�� �̻��� ���� ���� ���� ��� (��ȯ ����)
                    continue;
                if (amount == roomAmount)	//���� �� ������ �̹� �� á�ٸ�
                    continue;
                if (random())	//50% Ȯ���� ���� -> �پ��� �� ����
                    continue;

                if (num >= 1 && num <= maxRoom && room[num].IsVacant())
                {
                    room[num].OccupyRoom();
                    amount++;
                }
                q.Enqueue(num);
            }
            q.Dequeue();
        }

        for (int i = 1; i <= maxRoom; i++)
            if (!room[i].IsVacant())
            {
                room[i].SetDistanceFromStart(initRoomNumber, maxX);
                if (room[i].CheckEndRoom(room, maxX) && room[i].GetDistanceFromStart() >= minimumDistanceOfEndRoom)
                    endQ.Enqueue(i);
            }
        Queue<string> endRoomSet = new Queue<string>();
        endRoomSet.Enqueue("boss");
        endRoomSet.Enqueue("shop");

        if(endQ.Count < endRoomSet.Count)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        while (endRoomSet.Count != 0)
        {
            InfiniteLoopDetector.Run();
            endQ.Enqueue(endQ.Peek());
            endQ.Dequeue();

            if (room[endQ.Peek()].GetDistanceFromStart() < minimumDistanceOfEndRoom)  //minimum distance of endRooms (in this case 2)
                continue;
            if (!random())
                continue;
            room[endQ.Peek()].SetRoomType(endRoomSet.Peek());
            endQ.Dequeue();
            endRoomSet.Dequeue();
        }

        int x = 0;
        int z = 0;
        int roomInterval = roomObject.GetComponent<RoomGenerator>().GetMaxSize(); //room gameobect's width

        for (int i = 1; i <= maxX * maxY; i++)
        {
            Vector3 roomPos = new Vector3(x, 0, z);
            if (!room[i].IsVacant())
            {
                GameObject instance;
                bool[] portal = new bool[4]; //east, west, south, north
                instance = Instantiate(roomObject, roomPos, Quaternion.identity);
                instance.GetComponent<RoomGenerator>().SetRoomType(room[i].GetRoomType());
                instance.GetComponent<RoomGenerator>().SetRoomLocation(roomPos.x, roomPos.z);
                if (!room[i - 1].IsVacant())
                    portal[0] = true;
                if (!room[i + 1].IsVacant())
                    portal[1] = true;
                if (!room[i + maxX].IsVacant())
                    portal[2] = true;
                if (!room[i - maxX].IsVacant())
                    portal[3] = true;
                instance.GetComponent<RoomGenerator>().SetPortal(portal[0], portal[1], portal[2], portal[3]);
                instance.transform.parent = transform;
                roomArray.Add(instance);
            }
            if (i % maxX == 0)
            {
                x = 0;
                z += roomInterval;
            }
            else
            {
                x += roomInterval;
            }
        }
    }
    void Awake()
    {
        roomAmount = Random.Range(roomAmount - 1, roomAmount + 3);
        Debug.Log("Generated amount of stages: " + roomAmount);
        GenerateStage(roomAmount);
    }
}