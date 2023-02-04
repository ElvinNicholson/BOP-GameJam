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
    [SerializeField] private StaminaBar stamina;

    [SerializeField] private int max_stamina;
    private int current_stamina;

    // Start is called before the first frame update
    private void Start()
    {
        current_stamina = max_stamina;
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
                    if (!(tilemap.GetTile(tile_pos) == root))
                    {
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
}
