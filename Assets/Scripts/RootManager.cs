using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RootManager : MonoBehaviour
{
    private Vector3 mouse_pos;
    private Vector3Int cell_pos;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase root;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        InputManager();
    }

    private void InputManager()
    {
        if (Input.GetMouseButton(0))
        {
            mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            DrawCircle(mouse_pos);
        }
    }

    private void DrawCircle(Vector3 pos)
    {
        cell_pos = tilemap.WorldToCell(pos);

        for (int x = -2; x < 3; x++)    
        {
            for (int y = -2; y < 3; y++)
            {
                Vector3Int tile_pos = cell_pos + new Vector3Int(x, y, 0);

                if (!(Mathf.Abs(x) == 2 && Mathf.Abs(y) == 2))
                {
                    tilemap.SetTile(tile_pos, root);
                }
            }
        }
    }
}
