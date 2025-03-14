using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(20, 20);

    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(
                -this.boardSize.x / 2,
                -this.boardSize.y / 2
            );
            return new RectInt(position, this.boardSize);
        }
    }

    public TetrominoData[] tetrominoes;

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int randomIndex = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[randomIndex];

        this.activePiece.Initialize(this, this.spawnPosition, data);

        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
        }
        else
        {
            GameOver();
        }
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                return false;
            }

            if (this.tilemap.HasTile(tilePosition))
            {
                return false;
            }
        }

        return true;
    }

    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        Vector2Int gravityDir = Piece.CurrentGravityDirection;
        
        // check for horizontal lines (rows)
        if (gravityDir == Vector2Int.up || gravityDir == Vector2Int.down)
        {
            int row = bounds.yMin;
            while (row < bounds.yMax)
            {
                if (IsLineFull(row, true))
                {
                    LineClear(row, gravityDir, true);
                }
                else
                {
                    row++;
                }
            }
        }
        // check for vertical lines (columns)
        else if (gravityDir == Vector2Int.left || gravityDir == Vector2Int.right)
        {
            int col = bounds.xMin;
            while (col < bounds.xMax)
            {
                if (IsLineFull(col, false))
                {
                    LineClear(col, gravityDir, false);
                }
                else
                {
                    col++;
                }
            }
        }
    }

    private bool IsLineFull(int index, bool isRow)
    {
        RectInt bounds = this.Bounds;

        if (isRow)
        {
            // check if a horizontal line (row) is full
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, index, 0);
                
                if (!this.tilemap.HasTile(position))
                {
                    return false;
                }
            }
        }
        else
        {
            // check if a vertical line (column) is full
            for (int row = bounds.yMin; row < bounds.yMax; row++)
            {
                Vector3Int position = new Vector3Int(index, row, 0);
                
                if (!this.tilemap.HasTile(position))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void LineClear(int index, Vector2Int gravityDir, bool isRow)
    {
        RectInt bounds = this.Bounds;

        // clear the line
        if (isRow)
        {
            // clear a horizontal line (row)
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, index, 0);
                tilemap.SetTile(position, null);
            }
        }
        else
        {
            // clear a vertical line (column)
            for (int row = bounds.yMin; row < bounds.yMax; row++)
            {
                Vector3Int position = new Vector3Int(index, row, 0);
                tilemap.SetTile(position, null);
            }
        }

        // move tiles in the direction of gravity
        if (isRow)
        {
            // for horizontal lines (rows)
            if (gravityDir == Vector2Int.down)
            {
                // move everything above the cleared row down
                for (int row = index; row < bounds.yMax - 1; row++)
                {
                    for (int col = bounds.xMin; col < bounds.xMax; col++)
                    {
                        Vector3Int position = new Vector3Int(col, row + 1, 0);
                        TileBase above = tilemap.GetTile(position);

                        position = new Vector3Int(col, row, 0);
                        tilemap.SetTile(position, above);
                    }
                }
                
                // clear the top row
                for (int col = bounds.xMin; col < bounds.xMax; col++)
                {
                    Vector3Int position = new Vector3Int(col, bounds.yMax - 1, 0);
                    tilemap.SetTile(position, null);
                }
            }
            else if (gravityDir == Vector2Int.up)
            {
                // move everything below the cleared row up
                for (int row = index; row > bounds.yMin; row--)
                {
                    for (int col = bounds.xMin; col < bounds.xMax; col++)
                    {
                        Vector3Int position = new Vector3Int(col, row - 1, 0);
                        TileBase below = tilemap.GetTile(position);

                        position = new Vector3Int(col, row, 0);
                        tilemap.SetTile(position, below);
                    }
                }
                
                // clear the bottom row
                for (int col = bounds.xMin; col < bounds.xMax; col++)
                {
                    Vector3Int position = new Vector3Int(col, bounds.yMin, 0);
                    tilemap.SetTile(position, null);
                }
            }
        }
        else
        {
            // for vertical lines (columns)
            if (gravityDir == Vector2Int.right)
            {
                // move everything to the left of the cleared column to the right
                for (int col = index; col > bounds.xMin; col--)
                {
                    for (int row = bounds.yMin; row < bounds.yMax; row++)
                    {
                        Vector3Int position = new Vector3Int(col - 1, row, 0);
                        TileBase left = tilemap.GetTile(position);

                        position = new Vector3Int(col, row, 0);
                        tilemap.SetTile(position, left);
                    }
                }
                
                // clear the leftmost column
                for (int row = bounds.yMin; row < bounds.yMax; row++)
                {
                    Vector3Int position = new Vector3Int(bounds.xMin, row, 0);
                    tilemap.SetTile(position, null);
                }
            }
            else if (gravityDir == Vector2Int.left)
            {
                // move everything to the right of the cleared column to the left
                for (int col = index; col < bounds.xMax - 1; col++)
                {
                    for (int row = bounds.yMin; row < bounds.yMax; row++)
                    {
                        Vector3Int position = new Vector3Int(col + 1, row, 0);
                        TileBase right = tilemap.GetTile(position);

                        position = new Vector3Int(col, row, 0);
                        tilemap.SetTile(position, right);
                    }
                }
                
                // clear the rightmost column
                for (int row = bounds.yMin; row < bounds.yMax; row++)
                {
                    Vector3Int position = new Vector3Int(bounds.xMax - 1, row, 0);
                    tilemap.SetTile(position, null);
                }
            }
        }
    }

    public void GameOver()
    {
        // TODO: add game over screen
        this.tilemap.ClearAllTiles();
    }
}
