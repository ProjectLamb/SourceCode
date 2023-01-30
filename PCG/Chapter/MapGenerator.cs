using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    int maxX = 10;
    int maxY = 10;

    [Range(8,10)]
    public int roomAmount; //editable or not
    struct Room
    {
        public Room(int n)
        {
            roomNumber = n;
            type = "normal";
            vacancy = true;
        }
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
        public bool CheckAdjacentStartRoom(Room[] r, int maxX)
        {
            int i = 0;
            if (r[roomNumber - 1].type == "start")
                i++;
            if (r[roomNumber + 1].type == "start")
                i++;
            if (r[roomNumber - maxX].type == "start")
                i++;
            if (r[roomNumber + maxX].type == "start")
                i++;

            if (i == 0)
                return true;
            else
                return false;
        }
        private int roomNumber;	//�� ��ȣ 1 ~ maxRoom
        private bool vacancy;
        private string type;	//start, normal, shop, boss
    };
    
    public GameObject normalRoom;
    public GameObject startRoom;
    public GameObject bossRoom;
    public GameObject shopRoom;
    bool random()
    {
        int n = Random.Range(0, 2);
        if(n == 1)
            return true;
        else
            return false;
    } 
    void GenerateStage(int n)
{
    Room[] room = new Room[maxX * maxY + 1];
	int roomAmount = n;
	int maxRoom = maxX * maxY;

	for (int i = 1; i <= maxRoom; i++)
		room[i].SetRoomNumber(i);

	Queue<int> q = new Queue<int>();
	Queue<int> endQ = new Queue<int>();

	int initRoomNumber = maxRoom / 2 - maxX / 2;

	q.Enqueue(room[initRoomNumber].GetRoomNumber());
	room[initRoomNumber].SetRoomType("start");
	int amount = 0;

	while (amount != roomAmount)
	{
		if (q.Count == 0)
		{
			for (int i = 1; i <= maxRoom; i++)
				room[i].ResetRoom();

			//cout << "Error: Not enough room. Fail to generate" << "\n";	//���� �޼��� ���
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
		if (!room[i].IsVacant() && room[i].CheckEndRoom(room, maxX))
			endQ.Enqueue(i);

	Queue<string> endRoomSet = new Queue<string>();
	endRoomSet.Enqueue("shop");
	endRoomSet.Enqueue("boss");

	while (endRoomSet.Count != 0)
	{
		endQ.Enqueue(endQ.Peek());
		endQ.Dequeue();

		if (!random())
			continue;
		if (endRoomSet.Peek() == "boss" && !room[endQ.Peek()].CheckAdjacentStartRoom(room, maxX))
			continue;
		room[endQ.Peek()].SetRoomType(endRoomSet.Peek());
		endQ.Dequeue();
		endRoomSet.Dequeue();
	}

    int x = 0;
    int z = 0;
    int roomInterval = 10; //room's length

    for(int i = 1; i <= maxX * maxY; i++)
    {
        Vector3 roomPos = new Vector3(x, 0, z);
        if(!room[i].IsVacant())
        {
            if(room[i].GetRoomType() == "start")
            {
                Instantiate(startRoom, roomPos, Quaternion.identity);
            }
            else if (room[i].GetRoomType() == "boss")
            {
                Instantiate(bossRoom, roomPos, Quaternion.identity);
            }
            else if (room[i].GetRoomType() == "shop")
            {
                Instantiate(shopRoom, roomPos, Quaternion.identity);
            }
            else if (room[i].GetRoomType() == "normal")
            {
                Instantiate(normalRoom, roomPos, Quaternion.identity);
            }
        }
        if(i % maxX == 0)
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
    void Start()
    {
        //roomAmount = Random.Range(8, 11);
        GenerateStage(roomAmount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}