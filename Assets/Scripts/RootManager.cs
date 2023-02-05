using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RootManager : MonoBehaviour
{
    private Vector3 old_mouse_pos;
    private Vector3 mouse_pos;
    [SerializeField] private Vector3Int cell_pos;

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase root;
    [SerializeField] private TileBase water;
    [SerializeField] private TileBase water_claimed;
    [SerializeField] private TileBase mineral;
    [SerializeField] private TileBase mineral_claimed;

    [SerializeField] private StaminaBar stamina;

    [SerializeField] private int max_stamina;
    private int current_stamina;
    private bool can_draw;
    [HideInInspector] public bool has_drawn;

    private void Start()
    {
        current_stamina = max_stamina;
        old_mouse_pos = tilemap.CellToWorld(new Vector3Int(0, 21, 0));
        can_draw = true;
        has_drawn = false;
    }

    private void Update()
    {
        InputManager();
    }

    private void InputManager()
    {
        if (Input.GetMouseButton(0) && can_draw)
        {
            mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cell_pos = tilemap.WorldToCell(mouse_pos);
            if ((cell_pos.y < 21) && (mouse_pos.y < old_mouse_pos.y) && (Mathf.Abs(Mathf.Abs(old_mouse_pos.x) - Mathf.Abs(mouse_pos.x)) < 5))
            {
                DrawCircle(cell_pos);
                old_mouse_pos = mouse_pos;
                has_drawn = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (has_drawn)
            {
                can_draw = false;
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
                            ReplaceAdjacentTilesAs(tile_pos, water, water_claimed);
                            Debug.Log("FOUND WATER");
                        }
                        else if (current_tile == mineral)
                        {
                            ReplaceAdjacentTilesAs(tile_pos, mineral, mineral_claimed);
                            current_stamina += 100;
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
        List<Vector3Int> tile_list = new List<Vector3Int>();
        tile_list.Add(tile_pos + new Vector3Int(0, 1, 0));
        tile_list.Add(tile_pos + new Vector3Int(1, 0, 0));
        tile_list.Add(tile_pos + new Vector3Int(0, -1, 0));
        tile_list.Add(tile_pos + new Vector3Int(-1, 0, 0));

        foreach (Vector3Int new_pos in tile_list)
        {
            if (tilemap.GetTile(new_pos) == old_tile)
            {
                tilemap.SetTile(new_pos, new_tile);
                ReplaceAdjacentTilesAs(new_pos, old_tile, new_tile);
            }
        }
    }

    public void ResetTilemap()
    {
        tilemap.ClearAllTiles();
        current_stamina = max_stamina;
        old_mouse_pos = tilemap.CellToWorld(new Vector3Int(0, 21, 0));
        can_draw = true;
        has_drawn = false;
    }
}
