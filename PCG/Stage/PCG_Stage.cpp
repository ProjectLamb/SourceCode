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

//현재 버그: 9 * 8 사이즈로 제한해놔서 그 범위를 넘어가면 방이 안 만들어짐

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
	int roomNumber;	//방 번호 1 ~ maxRoom
	bool vacancy = true;	//비어있는지의 여부
	string type;
};

void GenerateStage()
{
	int roomAmount = random(7, 8);	//7 ~ 8개 랜덤
	Room room[maxX * maxY + 1];
	int maxRoom = maxX * maxY;	//방 전체 개수 (빈 방 포함)

	for (int i = 1; i <= maxRoom; i++)
		room[i].roomNumber = i;

	queue<int> q;	//실제로 채울 방의 방 번호가 들어갈 큐

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
			else	//만약 방이 최대 방까지 채워지지 못할 경우 초기화하고 다시 시행
			{
				for (int i = 1; i <= maxRoom; i++)
					room[i].ResetRoom();

				while (!q.empty())
					q.pop();
				cout << "Error: Not enough room. Fail to generate" << endl;	//에러 메세지 출력
				q.push(room[initRoomNumber].roomNumber);
				room[initRoomNumber].type = "start";
				amount = 1;
			}
		}
		
		room[q.front()].OccupyRoom();

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
			if (amount == roomAmount)	//만약 방 개수가 이미 다 찼다면
				continue;
			if (random())	//50% 확률로 포기 -> 다양한 맵 생성
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