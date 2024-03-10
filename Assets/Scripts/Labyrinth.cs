using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Labyrinth : MonoBehaviour
{
    public GameObject player;
    public GameObject way;
    public GameObject wall;
    
    public Game game;
    public sbyte leftRight;
    public sbyte downUp;

    public byte index;

    private GameObject playerObject;

    private List<byte> labyrinth = new();

    private Vector2 position;
    //public List<GameObject> walls;

    private void Start()
    {
        position = transform.position;
        playerObject = Instantiate(player, transform);
        labyrinth.Add(0);
    }

    private void DrawWay(byte rotation)
    {
        leftRight += (sbyte)((-rotation + 2) % 2);
        downUp += (sbyte)((-rotation + 1) % 2);
        Vector2 shift = new Vector2((-rotation + 2) % 2,
            (-rotation + 1) % 2) * transform.lossyScale.x;
        position += shift * 2;
        Instantiate(way, position - shift, 
            Quaternion.Euler(0, 0, rotation % 2 == 0 ? 90 : 0), transform);
        playerObject.transform.position = position;
    }

    private void DrawWall(byte rotation)
    {
        Instantiate(wall, new Vector2(position.x + (rotation % 2 == 1 ? -rotation + 2 : 0) * transform.lossyScale.x,
            position.y + (rotation % 2 == 0 ? -rotation + 1 : 0) * transform.lossyScale.x),
            Quaternion.Euler(0, 0, rotation % 2 == 0 ? 0 : 90), transform);
    }

    /*
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
            Assert(game.AssertMove(index, 0), 0);
        else if (Input.GetKeyUp(KeyCode.D))
            Assert(game.AssertMove(index, 1), 1);
        else if (Input.GetKeyUp(KeyCode.S))
            Assert(game.AssertMove(index, 2), 2);
        else if (Input.GetKeyUp(KeyCode.A))
            Assert(game.AssertMove(index, 3), 3);
    }*/

    public void Assert(byte result, byte rotation)
    {
        if (result == 0)
            return;
        if (result == 1)
            DrawWall(rotation);
        else
            DrawWay(rotation);
    }
}
