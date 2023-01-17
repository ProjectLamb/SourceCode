#include <iostream>
#include <random>
#include <queue>

#define maxX 9
#define maxY 8

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

//���� ����: 9 * 8 ������� �����س��� �� ������ �Ѿ�� ���� �� �������

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
	int roomNumber;	//�� ��ȣ 1 ~ maxRoom
	bool vacancy = true;	//����ִ����� ����
	string type;
};

void GenerateStage()
{
	int roomAmount = random(7, 8);	//7 ~ 8�� ����
	Room room[maxX * maxY + 1];
	int maxRoom = maxX * maxY;	//�� ��ü ���� (�� �� ����)

	for (int i = 1; i <= maxRoom; i++)
		room[i].roomNumber = i;

	queue<int> q;	//������ ä�� ���� �� ��ȣ�� �� ť

	int initRoomNumber = maxRoom / 2 - maxX / 2;

	q.push(room[initRoomNumber].roomNumber);
	room[initRoomNumber].type = "start";
	int amount = 1;

	while (1)
	{
		if (q.empty())
		{
			if (amount == roomAmount)
				break;
			else	//���� ���� �ִ� ����� ä������ ���� ��� �ʱ�ȭ�ϰ� �ٽ� ����
			{
				for (int i = 1; i <= maxRoom; i++)
					room[i].ResetRoom();

				while (!q.empty())
					q.pop();
				cout << "Error: Not enough room. Fail to generate" << endl;	//���� �޼��� ���
				q.push(room[initRoomNumber].roomNumber);
				room[initRoomNumber].type = "start";
				amount = 1;
			}
		}
		
		room[q.front()].OccupyRoom();

		for (int i = 0; i < 4; i++)	//���� �� ���� �˰���, �� �� �� �� ������ 4�� ����
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
			if (amount == roomAmount)	//���� �� ������ �̹� �� á�ٸ�
				continue;
			if (random())	//50% Ȯ���� ���� -> �پ��� �� ����
				continue;

			q.push(num);
			amount++;
		}
		q.pop();
		//cout << amount << endl;
	}

	cout << "====================" << endl;
	cout << "Number of rooms: " << roomAmount << endl;
	cout << "====================" << endl;

	for (int i = 1; i <= maxRoom; i++)
	{
		if (!room[i].vacancy)
			if (room[i].type == "start")
				cout << "S";
			else
				cout << "O";
		else
			cout << " ";
		//cout << room[i].roomNumber;
		if (i % maxX == 0)
			cout << "\n";
	}
}

int main()
{
	int genAmount = 1;
	cout << "Input Generate Amount: ";
	cin >> genAmount;

	for (int i = 0; i < genAmount; i++)
		GenerateStage();

	return 0;
}