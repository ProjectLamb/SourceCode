#include <iostream>
#include <random>
#include <queue>

#define maxX 10
#define maxY 10

using namespace std;

int random(int n, int m)	//n ~ m
{
	random_device rd;
	mt19937 gen(rd());

	uniform_int_distribution<int> dis(n, m);

	return dis(gen);
}
int random(int n)	//0 ~ n
{
	random_device rd;
	mt19937 gen(rd());

	uniform_int_distribution<int> dis(0, n);

	return dis(gen);
}
int random()	//0 or 1
{
	random_device rd;
	mt19937 gen(rd());

	uniform_int_distribution<int> dis(0, 1);

	return dis(gen);
}

class Room	//�� Ŭ����
{
public:
	Room()
	{
		roomNumber = 0;
		type = "normal";
		vacancy = true;
	}
	void OccupyRoom()
	{
		vacancy = false;
	}
	void ResetRoom()
	{
		type = "normal";
		vacancy = true;
	}
	void SetRoomType(string s)
	{
		type = s;
	}
	bool CheckAdjacency(Room* r)
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
	bool CheckEndRoom(Room* r)
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
	bool CheckAdjacentStartRoom(Room* r)
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
	int roomNumber;	//�� ��ȣ 1 ~ maxRoom
	bool vacancy = true;	//����ִ����� ����
	string type;	//start, normal, shop, boss
};

void GenerateStage(int n)
{
	Room room[maxX * maxY + 1];
	int roomAmount = n;	//7 ~ 8�� ����
	int maxRoom = maxX * maxY;	//�� ��ü ���� (�� �� ����)

	for (int i = 1; i <= maxRoom; i++)
		room[i].roomNumber = i;

	queue<int> q;	//������ ä�� ���� �� ��ȣ�� �� ť
	queue<int> endQ;	//���ٸ� ���� �� ��ȣ�� �� ť

	int initRoomNumber = maxRoom / 2 - maxX / 2;

	q.push(room[initRoomNumber].roomNumber);
	room[initRoomNumber].type = "start";
	int amount = 0;

	while (amount != roomAmount)
	{
		if (q.empty())	//���� �� ������ �ִ� �氳���� ��ġ���� �ʴٸ�
		{
			for (int i = 1; i <= maxRoom; i++)	//�ʱ�ȭ �� �ٽ� ����
				room[i].ResetRoom();

			//cout << "Error: Not enough room. Fail to generate" << "\n";	//���� �޼��� ���
			q.push(room[initRoomNumber].roomNumber);
			room[initRoomNumber].type = "start";
			amount = 0;
		}

		if (q.front() >= 1 && q.front() <= maxRoom && room[q.front()].vacancy)	//�� ��ȣ�� 1 ~ maxRoom�����̸� �ߺ��� �ƴ� ���
		{
			room[q.front()].OccupyRoom();
			amount++;
		}

		for (int i = 0; i < 4; i++)	//���� �� ���� �˰�����, �� �� �� �� ������ 4�� ����
		{
			int num = q.front();	//�� ��ȣ

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

			if (!room[num].vacancy)	//���� ���� ���� �ִٸ�
				continue;
			if (room[num].CheckAdjacency(room))	//���� ���� �� ��ü�� 2�� �̻��� ���� ���� ���� ��� (��ȯ ����)
				continue;
			if (amount == roomAmount)	//���� �� ������ �̹� �� á�ٸ�
				continue;
			if (random())	//50% Ȯ���� ���� -> �پ��� �� ����
				continue;

			if (num >= 1 && num <= maxRoom && room[num].vacancy)
			{
				room[num].OccupyRoom();
				amount++;
			}
			q.push(num);
		}
		q.pop();
	}

	for (int i = 1; i <= maxRoom; i++)
		if (!room[i].vacancy && room[i].CheckEndRoom(room))
			endQ.push(i);

	queue<string> endRoomSet;
	endRoomSet.push("shop");
	endRoomSet.push("boss");

	while (!endRoomSet.empty())
	{
		endQ.push(endQ.front());
		endQ.pop();

		if (!random())
			continue;
		if (endRoomSet.front() == "boss" && !room[endQ.front()].CheckAdjacentStartRoom(room))
			continue;
		room[endQ.front()].type = endRoomSet.front();
		endQ.pop();
		endRoomSet.pop();
	}

	cout << "====================" << "\n";
	for (int i = 1; i <= maxX * maxY; i++)
	{
		if (!room[i].vacancy)
		{
			if (room[i].type == "start")
				cout << "S";
			else if (room[i].type == "boss")
				cout << "B";
			else if (room[i].type == "shop")
				cout << "$";
			else if (room[i].type == "normal")
				cout << "O";
		}
		else
			cout << " ";
		//cout << room[i].roomNumber;
		if (i % maxX == 0)
			cout << "\n";
	}
	cout << "====================" << "\n";
}

int main()
{
	int genAmount = 1;
	cout << "Input Generate Amount: ";
	cin >> genAmount;

	for (int i = 1; i <= genAmount; i++)
	{
		int roomAmount = random(8, 10);
		cout << "#" << i << " Stage\n";
		cout << "Number of rooms: " << roomAmount << "\n";
		GenerateStage(roomAmount);
	}

	return 0;
}