using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical"; // �յ� �������� ���� �Է��� �̸�
    public string rotateAxisName = "Horizontal"; // �¿� ȸ���� ���� �Է��� �̸�

    // �� �Ҵ��� ���ο����� ����
    public float move { get; private set; } // ������ ������ �Է°�
    public float rotate { get; private set; } // ������ ȸ�� �Է°�

    // �������� ����� �Է��� ����
    private void Update()
    {
        // ���ӿ��� ���¿����� ����� �Է��� �������� �ʴ´�
       /* if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            move = 0;
            rotate = 0;
            return;
        } */

        // move�� ���� �Է� ����
        move = Input.GetAxis(moveAxisName);
        // rotate�� ���� �Է� ����
        rotate = Input.GetAxis(rotateAxisName);
    }
}