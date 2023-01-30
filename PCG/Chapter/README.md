# PCG/Chapter
> 절차적 챕터 생성기

### 개요
- 아이작의 챕터 생성과 유사한 방식으로 절차적 챕터 생성을 구현한 코드
- 스테이지의 종류는 4가지로, 노말, 보스, 스타트, 상점이 존재함.
- 스테이지의 생성 개수 조절 가능.

[참고 사이트](https://www.boristhebrave.com/2020/09/12/dungeon-generation-in-binding-of-isaac/)

---
### 용어
> 소피아 기획안의 용어 정리에 맞춰 서술하기 때문에 정확한 의미를 설명함.

- 챕터
	- **스테이지**들을 구성하고 있는 단위.
	- **맵, 지도**와 동일한 개념.
	- 인게임 UI인 **미니맵**을 통해 챕터의 **전체적인 윤곽을 가시적으로 확인**할 수 있음. (확정 X)
- 스테이지
	- 플레이어가 게임 세계에서 **활동할 수 있는 유한한 공간** 단위.
	- 현실 세계의 **방**과 동일한 개념.
	- **벽**으로 둘러싸여 있으며, 인접 형태에 따라 동서남북에 **문**이 존재함. 문을 통해 스테이지간 **출입**이 가능함.
---
### 1. Chapter.cpp
- C++언어로 논리적으로 구현한 소스 코드
- Room을 Class로 선언.
- 챕터는 콘솔창에서 텍스트 형식으로 출력됨.


##### 사용법
- 생성하고 싶은 만큼의 챕터 수 **N**을 입력하면 끝
```
S: start방
O: 일반방
$: 상점방
B: 보스방
```

##### 결과창
<img src="https://github.com/ProjectLamb/SourceCode/blob/neoskyclad/PCG/Chapter/_image/Chapter.gif?raw=true"/>

- 용어의 혼선이 있다. 스테이지가 아닌 **챕터**를 10개 생성한 모습이다.

---
### 2. MapGenerator.cs
- C# 언어로 유니티에서 구현한 소스코드
- Room이 구조체로 선언, 코드내에서 논리적으로 구현.
- 실제 인게임에선 지정된 스테이지 Prefab을 사용하여 Instantiate함.


##### 사용법
- 빈 GameObject에 MapGenerator.cs 스크립트를 넣고 게임을 실행하면 자동 생성됨.
- RoomAmount를 public으로 선언하였기 때문에 8 ~ 10의 정수 범위내에서 스테이지 생성값을 정할 수 있음.
```
초록색: start방
흰색: 일반방
파란색: 상점방
빨간색: 보스
```

##### 결과창
<img src="https://github.com/ProjectLamb/SourceCode/blob/neoskyclad/PCG/Chapter/_image/MapGenerator.gif?raw=true"/>

- 실행할 때마다 다른 형태의 챕터가 생성되는 모습.
