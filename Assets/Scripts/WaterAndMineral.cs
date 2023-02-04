using System.Collections;
using System.Collections.Generic;
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
    //[SerializeField] private TileBase mineral;

    [SerializeField] private int minimum_rand_x;
    [SerializeField] private int maximum_rand_x;

    [SerializeField] private int minimum_rand_y;
    [SerializeField] private int maximum_rand_y;

    [SerializeField] private int rand_distance_x;
    [SerializeField] private int rand_distance_y;

    private int randomPositionX;
    private int randomPositionY;    
    
    private int nextRandomPositionX;
    private int nextRandomPositionY;

    private int afterRandomPositionX;
    private int afterRandomPositionY;

    [SerializeField] private List<int> registered_rand_x;
    [SerializeField] private List<int> registered_rand_y;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("e"))
        {

            randomPositionX = Random.Range(minimum_rand_x, maximum_rand_x);
            randomPositionY = Random.Range(minimum_rand_y, maximum_rand_y);
            cell_pos = new Vector3Int(randomPositionX, randomPositionY);

            if(Random.Range(0, 1) == 0)
            {
                is_spawn_left = true;
            }
            else
            {
                is_spawn_left = false;
            }

            if (Random.Range(0, 1) == 0)
            {
                is_spawn_up = true;
            }
            else
            {
                is_spawn_up = false;
            }

            if (is_spawn_left == true)
            {
                //Gonna spawn somewhere to the Left of initial spawned Water.
                nextRandomPositionX = Random.Range(minimum_rand_x, randomPositionX - rand_distance_x);
            }
            else
            {
                //Gonna spawn Right.
                nextRandomPositionX = Random.Range(randomPositionX + rand_distance_x, maximum_rand_x);
            }

            if(is_spawn_up == true)
            {
                //Gonna spawn Up.
                nextRandomPositionY = Random.Range(randomPositionY + rand_distance_y, maximum_rand_y);
            }
            else
            {
                //Gonna spawn Below.
                nextRandomPositionY = Random.Range(minimum_rand_y, randomPositionY - rand_distance_y);
            }

            nextRandomPositionX = Random.Range(randomPositionX - rand_distance_x, randomPositionX + rand_distance_x);
            nextRandomPositionY = Random.Range(randomPositionY - rand_distance_y, randomPositionY + rand_distance_y);
            next_cell_pos = new Vector3Int(nextRandomPositionX, nextRandomPositionY);

            //afterRandomPositionX = Random.Range(nextRandomPositionX - rand_distance_x, nextRandomPositionX + rand_distance_x);
            //afterRandomPositionY = Random.Range(nextRandomPositionY - rand_distance_y, nextRandomPositionY + rand_distance_y);
            //after_cell_pos = new Vector3Int(afterRandomPositionX, afterRandomPositionY);

            ////Check if list is Empty
            //if (registered_rand_x.Count == 0)
            //{
            //    //List is Empty.
            //    cell_pos = new Vector3Int(randomPositionX, randomPositionY);
            //    registered_rand_x.Add(randomPositionX);
            //    registered_rand_x.Add(randomPositionX + distance_x);
            //    registered_rand_x.Add(randomPositionX - distance_x);
            //    registered_rand_x.Add(randomPositionY);
            //    registered_rand_y.Add(randomPositionY + distance_y);
            //    registered_rand_y.Add(randomPositionY - distance_y);
            //}
            //else
            //{
            //    //List is not Empty.
            //    int id_x = registered_rand_x.IndexOf(randomPositionX);
            //    int id_y = registered_rand_y.IndexOf(randomPositionY);
            //
            //    while
            //
            //}


            DrawSquare(cell_pos);
        }

        if (Input.GetKeyDown("d"))
        {
            DrawSquare(next_cell_pos);
        }

        if (Input.GetKeyDown("f"))
        {
            DrawSquare(after_cell_pos);
        }
    }

    private void DrawSquare(Vector3 pos)
    {
        //Rectangle starts to draw from bottom-left of rectangle.
        cell_pos = tilemap.WorldToCell(pos);

        for (int x = 0; x < 11; x++) //3 //-2
        {
            for (int y = 0; y < 6; y++) //3 
            {
                tilemap.SetTile(cell_pos + new Vector3Int(x, y, 0), water);
            }

        }

        //tilemap.SetTile(cell_pos, water);
    }
}
