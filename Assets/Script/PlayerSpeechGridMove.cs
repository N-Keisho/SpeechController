using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeechGridMove : MonoBehaviour
{
    // --- components ---
    private SpeechController sc;

    // --- keywords ---
    private string goStraight;
    private string goBack;
    private string goLeft;
    private string goRight;

    // --- grid movement ---
    public Pos2D grid;
    private Pos2D newGrid = null;
    public EDir direction;
    public float speed = 0.9f;
    public float maxPerFrame = 1.67f;
    public float completeFrame;
    private int currentFrame = 0;

    // Start is called before the first frame update
    void Start()
    {
        sc = GameObject.Find("__SpeechController__").GetComponent<SpeechController>();
        completeFrame = maxPerFrame / Time.deltaTime;

        goStraight = sc.words[0];
        goBack = sc.words[1];
        goRight = sc.words[2];
        goLeft = sc.words[3];
    }

    void Update()
    {
        if (currentFrame == 0)
        {
            EDir d = KeyToDir();
            if (d != EDir.Pause)
            {
                direction = d;
                transform.rotation = DirToRotation(direction);
                newGrid = GetNewGrid(grid, direction);
                grid = Move(grid, newGrid, ref currentFrame);
            }
        }
        else
        {
            grid = Move(grid, newGrid, ref currentFrame);
        }
    }

    public EDir KeyToDir()
    {
        if (sc.flags[goStraight] || Input.GetKeyDown(KeyCode.W))
        {
            sc.flags[goStraight] = false;
            return EDir.Up;
        }
        if (sc.flags[goBack] || Input.GetKeyDown(KeyCode.S))
        {
            sc.flags[goBack] = false;
            return EDir.Down;
        }
        if (sc.flags[goLeft] || Input.GetKeyDown(KeyCode.A))
        {
            sc.flags[goLeft] = false;
            return EDir.Left;
        }
        if (sc.flags[goRight] || Input.GetKeyDown(KeyCode.D))
        {
            sc.flags[goRight] = false;
            return EDir.Right;
        }
        return EDir.Pause;
    }

    /**
   * 引数で与えられた向きに対応する回転のベクトルを返す
   */
    public Quaternion DirToRotation(EDir d)
    {
        Quaternion r = transform.rotation;
        switch (d)
        {
            case EDir.Left:
                r = r * Quaternion.Euler(0, 270, 0);
                break;
            case EDir.Up:
                break;
            case EDir.Right:
                r = r * Quaternion.Euler(0, 90, 0);
                break;
            case EDir.Down:
                r = r * Quaternion.Euler(0, 180, 0);
                break;
        }
        return r;
    }

    /**
* グリッド座標をワールド座標に変換
*/
    private float ToWorldX(int xgrid)
    {
        return xgrid * 2;
    }

    private float ToWorldZ(int zgrid)
    {
        return zgrid * 2;
    }

    /**
    * ワールド座標をグリッド座標に変換
*/
    private int ToGridX(float xworld)
    {
        return Mathf.FloorToInt(xworld / 2);
    }

    private int ToGridZ(float zworld)
    {
        return Mathf.FloorToInt(zworld / 2);
    }

    /**
    * 補完で計算して進む
*/
    private Pos2D Move(Pos2D currentPos, Pos2D newPos, ref int frame)
    {
        float px1 = ToWorldX(currentPos.x);
        float pz1 = ToWorldZ(currentPos.z);
        float px2 = ToWorldX(newPos.x);
        float pz2 = ToWorldZ(newPos.z);
        frame += 1;
        float t = frame / completeFrame;
        float newX = px1 + (px2 - px1) * t;
        float newZ = pz1 + (pz2 - pz1) * t;
        transform.position = new Vector3(newX, transform.position.y, newZ);
        if (frame >= completeFrame)
        {
            currentPos = newPos;
            frame = 0;
        }
        return currentPos;
    }

    public Pos2D GetNewGrid(Pos2D position, EDir direction)
    {
        Pos2D newPos = new Pos2D();
        newPos.x = position.x;
        newPos.z = position.z;
        int directionY = (int)transform.rotation.eulerAngles.y;
        // Debug.Log(direction);
        if (direction == EDir.Up)
        {
            switch (directionY)
            {
                case 0:
                    newPos.z += 1; break;
                case 90:
                    newPos.x += 1; break;
                case 180:
                    newPos.z -= 1; break;
                case 270:
                    newPos.x -= 1; break;
            }
        }
        return newPos;
    }
}
