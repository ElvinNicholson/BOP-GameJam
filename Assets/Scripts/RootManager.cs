using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class RootManager : MonoBehaviour
{
    [SerializeField] private WaterAndMineral map_generation;

    [SerializeField] private Vector3 mouse_pos;
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

    [SerializeField] private Text score;

    public int water_found;

    private void Start()
    {
        ResetTilemap();
    }

    private void Update()
    {
        InputManager();
    }

    private void InputManager()
    {
        if (Input.GetMouseButton(0) && can_draw)
        {
            if (!has_drawn)
            {
                mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                cell_pos = tilemap.WorldToCell(mouse_pos);
                if ((16 < cell_pos.y) && (cell_pos.y < 21) && (-4 < cell_pos.x) && (cell_pos.x < 4))
                {
                    DrawCircle(cell_pos);
                    has_drawn = true;
                }
            }
            else
            {
                mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                cell_pos = tilemap.WorldToCell(mouse_pos);
                if ((cell_pos.y < 21))
                {
                    DrawCircle(cell_pos);
                }
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
                            water_found += 1;
                            score.text = "SCORE: " + water_found;
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
        map_generation.GenerateTiles();
        current_stamina = max_stamina;
        can_draw = true;
        has_drawn = false;
        water_found = 0;
        score.text = "SCORE: " + water_found;
    }
}
