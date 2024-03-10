using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Game : MonoBehaviour
{
    public Labyrinth[] labyrinths;

    private byte[] labyrinth;
    private byte length;
    private List<int> positions;

    private byte playerTurn = 0;

    public void CreateLabyrinth()
    {
        labyrinth = new byte[length * length];
        for (int i = 0; i < labyrinth.Length; i++)
            labyrinth[i] = 3;
        int nowCell = Random.Range(0, length * length);
        int cellsCount = 1;
        bool[] cells = new bool[length * length];
        cells[nowCell] = true;

        while (cellsCount < length * length)
        {
            byte rotation = (byte)Random.Range(0, 4);
            switch (rotation)
            {
                case 0:
                    if (nowCell >= length)
                    {
                        if (!cells[nowCell - length])
                        {
                            labyrinth[nowCell - length] -= 2;
                            cells[nowCell - length] = true;
                            cellsCount++;
                        }
                        nowCell -= length;
                    }
                    break;
                case 1:
                    if (nowCell % length != length - 1)
                    {
                        if (!cells[nowCell + 1])
                        {
                            labyrinth[nowCell] -= 1;
                            cells[nowCell + 1] = true;
                            cellsCount++;
                        }
                        nowCell += 1;
                    }
                    break;
                case 2:
                    if (nowCell < length * length - length)
                    {
                        if (!cells[nowCell + length])
                        {
                            labyrinth[nowCell] -= 2;
                            cells[nowCell + length] = true;
                            cellsCount++;
                        }
                        nowCell += length;
                    }
                    break;
                default:
                    if (nowCell % length != 0)
                    {
                        if (!cells[nowCell - 1])
                        {
                            labyrinth[nowCell - 1] -= 1;
                            cells[nowCell - 1] = true;
                            cellsCount++;
                        }
                        nowCell -= 1;
                    }
                    break;
            }
        }
    }

    private void SetPlayers()
    {
        byte players = 3;
        positions = new List<int>(players);
        int position;
        for (int i = 0; i < players; i++)
        {
            do position = Random.Range(0, length * length);
            while (positions.IndexOf(position) != -1);
            positions.Add(position);
        }
    }

    private bool Move(byte player, byte rotation)
    {
        int position = positions[player];
        int newPosition = position;
        switch (rotation)
        {
            case 0:
                if (position >= length && labyrinth[position - length] < 2)
                    newPosition = position - length;
                break;
            case 1:
                if ((position + 1) % length != 0 && labyrinth[position] % 2 == 0)
                    newPosition = position + 1;
                break;
            case 2:
                if (position < length * length - length && labyrinth[position] < 2)
                    newPosition = position + length;
                break;
            default:
                if (position % length != 0 && labyrinth[position - 1] % 2 == 0)
                    newPosition = position - 1;
                break;
        }
        //Debug.Log(newPosition);
        if (newPosition == position)
            return false;

        positions[player] = newPosition;
            return true;
    }

    private void Start()
    {
        length = 4;
        CreateLabyrinth();
        SetPlayers();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
            AssertMove(playerTurn, 0);
        else if (Input.GetKeyUp(KeyCode.D))
            AssertMove(playerTurn, 1);
        else if (Input.GetKeyUp(KeyCode.S))
            AssertMove(playerTurn, 2);
        else if (Input.GetKeyUp(KeyCode.A))
            AssertMove(playerTurn, 3);
    }

    public void AssertMove(byte player, byte rotation)
    {
        //if (player != playerTurn)
        //    return 0;
        //Debug.Log(playerTurn);

        if (!Move(player, rotation))
            labyrinths[playerTurn].Assert(1, rotation);
        else
            labyrinths[playerTurn].Assert(2, rotation);

        playerTurn = (byte)(playerTurn == 0 ? 1 : 0);
    }
}
