using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RootManager : MonoBehaviour
{
    private Vector3 mouse_pos;
    [SerializeField] private Vector3Int cell_pos;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase root;
    [SerializeField] private TileBase water;
    [SerializeField] private TileBase mineral;

    [SerializeField] private StaminaBar stamina;

    [SerializeField] private int max_stamina;
    private int current_stamina;

    private void Start()
    {
        current_stamina = max_stamina;
    }

    private void Update()
    {
        InputManager();
    }

    private void InputManager()
    {
        if (Input.GetMouseButton(0))
        {
            mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cell_pos = tilemap.WorldToCell(mouse_pos);
            if (cell_pos.y < 21)
            {
                DrawCircle(cell_pos);
            }
        }
    }

    private void DrawCircle(Vector3Int pos)
    {
        for (int x = -2; x < 3; x++)
        {
            for (int y = -2; y < 3; y++)
            {
                Vector3Int tile_pos = pos + new Vector3Int(x, y, 0);

                if (!(Mathf.Abs(x) == 2 && Mathf.Abs(y) == 2) && current_stamina > 0)
                {
                    TileBase current_tile = tilemap.GetTile(tile_pos);
                    if (!(current_tile == root))
                    {
                        if (current_tile == water)
                        {
                            Debug.Log("FOUND WATER");
                        }
                        else if (current_tile == mineral)
                        {
                            Debug.Log("FOUND MINERAL");
                        }

                        tilemap.SetTile(tile_pos, root);

                        current_stamina -= 1;
                        if (current_stamina > 0)
                        {
                            stamina.SetStamina((float)current_stamina / max_stamina);
                        }
                    }
                }
            }
        }
    }

    private void ReplaceAdjacentTilesAs(Vector3Int tile_pos, TileBase old_tile, TileBase new_tile)
    {

    }
}
