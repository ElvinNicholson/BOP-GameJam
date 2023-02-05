using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterAndMineral : MonoBehaviour
{
    private Vector3Int cell_pos;
    private Vector3Int next_cell_pos;
    private Vector3Int after_cell_pos;

    bool is_spawn_left;
    bool is_spawn_up;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase water;
    //[SerializeField] private TileBase rock;
    [SerializeField] private TileBase mineral;

    [SerializeField] private int minimum_rand_x;
    [SerializeField] private int maximum_rand_x;

    private int minimum_rand_y;
    private int maximum_rand_y;

    [SerializeField] private int first_minimum_rand_y;
    [SerializeField] private int first_maximum_rand_y;    
    
    [SerializeField] private int second_minimum_rand_y;
    [SerializeField] private int second_maximum_rand_y;    
    
    [SerializeField] private int third_minimum_rand_y;
    [SerializeField] private int third_maximum_rand_y;

    [SerializeField] private int rand_distance_x;
    [SerializeField] private int rand_distance_y;

    [SerializeField] private int offset_x;
    [SerializeField] private int offset_y;

    [SerializeField] private int randomPositionX;
    [SerializeField] private int randomPositionY;    
    
    private int nextRandomPositionX;
    private int nextRandomPositionY;

    private int afterRandomPositionX;
    private int afterRandomPositionY;

    private int height = 6;
    private int width = 11;

    private TileBase tileTypeToDraw;

    [SerializeField] private List<int> registered_rand_x;
    [SerializeField] private List<int> registered_rand_y;

    // Start is called before the first frame update
    void Start()
    {
        //DrawOrigin();
        DrawDivider();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("1"))
        {
            GenerateObjectSet(1);
        }

        if (Input.GetKeyDown("2"))
        {
            GenerateObjectSet(2);
        }

        if (Input.GetKeyDown("3"))
        {
            GenerateObjectSet(3);
        }

        if (Input.GetKeyDown("d"))
        {
            DrawSquare(next_cell_pos, 1);
        }

        if (Input.GetKeyDown("f"))
        {
            DrawSquare(after_cell_pos, 0);
        }

        if(Input.GetKeyDown("g"))
        {
            randomPositionX = -53;
            randomPositionY = Random.Range(minimum_rand_y, maximum_rand_y - height);

            cell_pos = new Vector3Int(randomPositionX, randomPositionY);
            DrawSquare(cell_pos, 1);
        }
    }

    private void GenerateObjectSet(int containerNumber)
    {
        switch(containerNumber)
        {
            case 1:
                minimum_rand_y = first_minimum_rand_y + offset_y; //+ height;
                maximum_rand_y = first_maximum_rand_y + offset_y; //- height;
                break;                                           //
            case 2:                                              //
                minimum_rand_y = second_minimum_rand_y + offset_y;// + height;
                maximum_rand_y = second_maximum_rand_y + offset_y; // - height;
                break;                                           //
            case 3:                                              //
                minimum_rand_y = third_minimum_rand_y + offset_y; //+ height;
                maximum_rand_y = third_maximum_rand_y + offset_y; //- height;
                break;
        }

        //minimum_rand_x += offset_x;
        //maximum_rand_x -= offset_x;

        randomPositionX = Random.Range(minimum_rand_x, maximum_rand_x - width);
        randomPositionY = Random.Range(minimum_rand_y, maximum_rand_y - height);

        if (randomPositionX < -53)
        {
            randomPositionX = -53;
        }
        if (randomPositionX > 42)
        {
            randomPositionX = 42;
        }

        cell_pos = new Vector3Int(randomPositionX, randomPositionY);

        // determining second box position
        int leftRight = Random.Range(0, 1);

        if (leftRight == 0)
        {
            // 2nd box will spawn to the left of the first
            nextRandomPositionX = Random.Range(minimum_rand_x, randomPositionX - rand_distance_x - width);
        }
        else
        {
            // 2nd box will spawn to the right of the first
            nextRandomPositionX = Random.Range(randomPositionX + rand_distance_x, maximum_rand_x);
        }

        if (nextRandomPositionX < -53)
        {
            nextRandomPositionX = -39;
        }
        if (nextRandomPositionX > 42)
        {
            nextRandomPositionX = 28;
        }

        if (Random.Range(0, 1) == 0)
        {
            // 2nd box up of the first
            nextRandomPositionY = Random.Range(randomPositionY + rand_distance_y, maximum_rand_y);
        }
        else
        {
            // 2nd box down of the first
            nextRandomPositionY = Random.Range(minimum_rand_y, randomPositionY - rand_distance_y - height);
        }

        next_cell_pos = new Vector3Int(nextRandomPositionX, nextRandomPositionY);

        // determining 3rd box position
        if (leftRight == 0)
        {
            // 3rd box will spawn to the right of the first
            afterRandomPositionX = Random.Range(randomPositionX + rand_distance_x, maximum_rand_x);
        }
        else
        {
            // 3rd box will spawn to the left of the first
            afterRandomPositionX = Random.Range(minimum_rand_x, randomPositionX - rand_distance_x - width);
        }

        if (afterRandomPositionX < -53)
        {
            afterRandomPositionX = -39;
        }
        if (afterRandomPositionX > 42)
        {
            afterRandomPositionX = 28;
        }

        if (Random.Range(0, 1) == 0)
        {
            // third box will spawn up of the first
            afterRandomPositionY = Random.Range(randomPositionY + rand_distance_y, maximum_rand_y);
        }
        else
        {
            // third box will spawn down of the first
            afterRandomPositionY = Random.Range(minimum_rand_y, randomPositionY - rand_distance_y - height);
        }

        after_cell_pos = new Vector3Int(afterRandomPositionX, afterRandomPositionY);

        DrawSquare(cell_pos, 0);
    }

    private void DrawSquare(Vector3 pos, int tileType)
    {
        //Rectangle starts to draw from bottom-left of rectangle.
        cell_pos = tilemap.WorldToCell(pos);
    
        switch(tileType)
        {
            case 0:
                tileTypeToDraw = water;
                break;
           
            case 1:
                tileTypeToDraw = mineral;
                break;

            default:
                break;
        }

        for (int x = 0; x < 11; x++) //3 //-2
        {
            for (int y = 0; y < 6; y++) //3 
            {
                tilemap.SetTile(cell_pos + new Vector3Int(x, y, 0), tileTypeToDraw);
            }
        }

        //tilemap.SetTile(cell_pos, water);
    }

    private void DrawOrigin()
    {
        for(int x = 0; x < 11; x++)
        {
            tilemap.SetTile(new Vector3Int(x, 0, 0), water);
        }

        for (int y = 0; y < 6; y++)
        {
            tilemap.SetTile(cell_pos + new Vector3Int(0, y, 0), water);
        }
    }

    private void DrawDivider()
    {
        for(int x = minimum_rand_x; x < maximum_rand_x; x++)
        {
            tilemap.SetTile(new Vector3Int(x, first_maximum_rand_y - height + offset_y, 0), water);

            tilemap.SetTile(new Vector3Int(x, second_maximum_rand_y - height + offset_y, 0), water);

            tilemap.SetTile(new Vector3Int(x, third_maximum_rand_y - height + offset_y, 0), water);
        }

        for (int x = minimum_rand_x; x < maximum_rand_x; x++)
        {
            tilemap.SetTile(new Vector3Int(x, first_minimum_rand_y + offset_y, 0), mineral);

            tilemap.SetTile(new Vector3Int(x, second_minimum_rand_y + offset_y, 0), mineral);

            tilemap.SetTile(new Vector3Int(x, third_minimum_rand_y + offset_y, 0), mineral);
        }
    }
}
