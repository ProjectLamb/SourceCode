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

class Room	//방 클래스
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
	int roomNumber;	//방 번호 1 ~ maxRoom
	bool vacancy = true;	//비어있는지의 여부
	string type;	//start, normal, shop, boss
};

void GenerateStage(int n)
{
	Room room[maxX * maxY + 1];
	int roomAmount = n;	//7 ~ 8개 랜덤
	int maxRoom = maxX * maxY;	//방 전체 개수 (빈 방 포함)

	for (int i = 1; i <= maxRoom; i++)
		room[i].roomNumber = i;

	queue<int> q;	//실제로 채울 방의 방 번호가 들어갈 큐
	queue<int> endQ;	//막다른 방의 방 번호가 들어갈 큐

	int initRoomNumber = maxRoom / 2 - maxX / 2;

	q.push(room[initRoomNumber].roomNumber);
	room[initRoomNumber].type = "start";
	int amount = 0;

	while (amount != roomAmount)
	{
		if (q.empty())	//만약 방 개수가 최대 방개수와 일치하지 않다면
		{
			for (int i = 1; i <= maxRoom; i++)	//초기화 후 다시 실행
				room[i].ResetRoom();

			//cout << "Error: Not enough room. Fail to generate" << "\n";	//에러 메세지 출력
			q.push(room[initRoomNumber].roomNumber);
			room[initRoomNumber].type = "start";
			amount = 0;
		}

		if (q.front() >= 1 && q.front() <= maxRoom && room[q.front()].vacancy)	//방 번호가 1 ~ maxRoom까지이며 중복이 아닐 경우
		{
			room[q.front()].OccupyRoom();
			amount++;
		}

		for (int i = 0; i < 4; i++)	//인접 방 차지 알고리즘, 좌 우 상 하 순으로 4번 실행
		{
			int num = q.front();	//방 번호

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

			if (!room[num].vacancy)	//만약 인접 방이 있다면
				continue;
			if (room[num].CheckAdjacency(room))	//만약 인접 방 자체가 2개 이상의 인접 방이 있을 경우 (순환 방지)
				continue;
			if (amount == roomAmount)	//만약 방 개수가 이미 다 찼다면
				continue;
			if (random())	//50% 확률로 포기 -> 다양한 맵 생성
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
	int roomAmount = random(7, 8);
	int genAmount = 1;
	cout << "Input Generate Amount: ";
	cin >> genAmount;

	for (int i = 1; i <= genAmount; i++)
	{
		cout << "#" << i << " Stage\n";
		cout << "Number of rooms: " << roomAmount << "\n";
		GenerateStage(roomAmount);
	}

	return 0;
}