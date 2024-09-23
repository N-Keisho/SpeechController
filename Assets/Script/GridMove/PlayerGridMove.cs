using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGridMove : MonoBehaviour
{
    public int currentFrame = 0;
    private Pos2D newGrid = null;
    public EDir direction = EDir.Up;

    public Pos2D grid = new Pos2D { x = 0, z = 0 };
   public float speed = 0.9f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (currentFrame == 0)
        {
            EDir d = KeyToDir();
            if (d != EDir.Pause)
            {
                transform.rotation = DirToRotation(d);
                newGrid = GetNewGrid(grid, (int)d);
                grid = Move(grid, newGrid, ref currentFrame);
            }
        }
        else
        {
            grid = Move(grid, newGrid, ref currentFrame);
        }
    }

    /**
   * 入力されたキーに対応する向きを返す
   */
    private EDir KeyToDir()
    {
        if (!Input.anyKey)
        {
            Debug.Log("Pause");
            return EDir.Pause;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("Left");
            return EDir.Left;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("Up");
            return EDir.Up;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("Right");
            return EDir.Right;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("Down");
            return EDir.Down;
        }
        return EDir.Pause;
    }

    /**
   * 引数で与えられた向きに対応する回転のベクトルを返す
   */
   private Quaternion DirToRotation(EDir d)
   {
       Quaternion r = Quaternion.Euler(0, 0, 0);
       switch (d)
       {
           case EDir.Left:
               r = Quaternion.Euler(0, 270, 0); break;
           case EDir.Up:
               r = Quaternion.Euler(0, 0, 0); break;
           case EDir.Right:
               r = Quaternion.Euler(0, 90, 0); break;
           case EDir.Down:
               r = Quaternion.Euler(0, 180, 0); break;
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

    private Pos2D Move(Pos2D currentPos, Pos2D newPos, ref int frame)
    {
        float px1 = ToWorldX(currentPos.x);
        float pz1 = ToWorldZ(currentPos.z);
        float px2 = ToWorldX(newPos.x);
        float pz2 = ToWorldZ(newPos.z);
        frame += 1;
        float t = frame / 60.0f;
        float newX = px1 + (px2 - px1) * t;
        float newZ = pz1 + (pz2 - pz1) * t;
        transform.position = new Vector3(newX, 0, newZ);
        if (frame == 60)
        {
            currentPos = newPos;
            frame = 0;
        }
        return currentPos;
    }

    private Pos2D GetNewGrid(Pos2D position, int direction)
    {
        Pos2D newPos = new Pos2D();
        newPos.x = position.x;
        newPos.z = position.z;
        switch (direction)
        {
            case 0:
                newPos.z += 1;
                break;
            case 1:
                newPos.x += 1;
                break;
            case 2:
                newPos.z -= 1;
                break;
            case 3:
                newPos.x -= 1;
                break;
        }
        return newPos;
    }

}
