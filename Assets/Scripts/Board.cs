using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public Piece nextPiece { get; private set; }
    public Vector3Int spawnPosition;
    public Vector3Int previewPosition = new Vector3Int(-20, 12, 0);
    public Vector2Int boardSize = new Vector2Int(20, 20);
    public GameManager gameManager; // Reference to the Game Over UI Panel

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

        nextPiece = gameObject.AddComponent<Piece>();
        nextPiece.enabled = false;

        for (int i = 0; i < tetrominoes.Length; i++)
        {
            tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SetNextPiece();
        SpawnPiece();
    }


    private void SetNextPiece()
    {
        if (nextPiece.cells != null)
        {
            Clear(nextPiece);
        }

        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data = tetrominoes[random];

        nextPiece.Initialize(this, previewPosition, data, true);
        Set(nextPiece);
    }

    public void SpawnPiece()
    {
        activePiece.Initialize(this, spawnPosition, nextPiece.data, true);

        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
        }
        else
        {
            GameOver();
        }

        SetNextPiece();

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
            int numLines = 0;
            int row = bounds.yMin;
            while (row < bounds.yMax)
            {
                if (IsLineFull(row, true))
                {
                    LineClear(row, gravityDir, true);
                    numLines++;
                }
                else
                {
                    row++;
                }
            }
            if (numLines > 0)
            {
                gameManager.IncreaseScore(numLines);
            }
        }
        // check for vertical lines (columns)
        else if (gravityDir == Vector2Int.left || gravityDir == Vector2Int.right)
        {
            int numLines = 0;
            int col = bounds.xMin;
            while (col < bounds.xMax)
            {
                if (IsLineFull(col, false))
                {
                    LineClear(col, gravityDir, false);
                    numLines++;
                }
                else
                {
                    col++;
                }
            }
            if (numLines > 0)
            {
                gameManager.IncreaseScore(numLines);
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
        AudioManager.Instance.PlaySFX("LineClear");
        RectInt bounds = this.Bounds;
        int halfwayPoint;
        ScreenShake.Instance.Shake(0.1f, 0.1f);
        // clear the line
        if (isRow)
        {
            // clear a horizontal line (row)
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, index, 0);
                tilemap.SetTile(position, null);
            }

            // only affect half of the row
            halfwayPoint = bounds.xMin + bounds.width / 2;
        }
        else
        {
            // clear a vertical line (column)
            for (int row = bounds.yMin; row < bounds.yMax; row++)
            {
                Vector3Int position = new Vector3Int(index, row, 0);
                tilemap.SetTile(position, null);
            }

            // only affect half of the column
            halfwayPoint = bounds.yMin + bounds.height / 2;
        }

        // move tiles in the direction of gravity
        if (isRow)
        {
            // for horizontal lines (rows)
            if (gravityDir == Vector2Int.down)
            {
                // move only half of the blocks above the cleared row down
                for (int row = index; row < halfwayPoint - 1; row++)
                {
                    for (int col = bounds.xMin; col < bounds.xMax; col++)
                    {
                        Vector3Int position = new Vector3Int(col, row + 1, 0);
                        TileBase above = tilemap.GetTile(position);

                        position = new Vector3Int(col, row, 0);
                        tilemap.SetTile(position, above);
                    }
                }
            }
            else if (gravityDir == Vector2Int.up)
            {
                // move only half of the blocks below the cleared row up
                for (int row = index; row > halfwayPoint; row--)
                {
                    for (int col = bounds.xMin; col < bounds.xMax; col++)
                    {
                        Vector3Int position = new Vector3Int(col, row - 1, 0);
                        TileBase below = tilemap.GetTile(position);

                        position = new Vector3Int(col, row, 0);
                        tilemap.SetTile(position, below);
                    }
                }

                // clear the moved tiles in the bottom row on the right half
                for (int col = halfwayPoint; col < bounds.xMax; col++)
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
                // move only half of the blocks to the left of the cleared column to the right
                for (int col = index; col > halfwayPoint; col--)
                {
                    for (int row = bounds.yMin; row < halfwayPoint; row++) // Only up to halfway
                    {
                        Vector3Int position = new Vector3Int(col - 1, row, 0);
                        TileBase left = tilemap.GetTile(position);

                        position = new Vector3Int(col, row, 0);
                        tilemap.SetTile(position, left);
                    }
                }
            }
            else if (gravityDir == Vector2Int.left)
            {
                // move only half of the blocks to the right of the cleared column to the left
                for (int col = index; col < halfwayPoint - 1; col++)
                {
                    for (int row = bounds.yMin; row < bounds.yMax; row++)
                    {
                        Vector3Int position = new Vector3Int(col + 1, row, 0);
                        TileBase right = tilemap.GetTile(position);

                        position = new Vector3Int(col, row, 0);
                        tilemap.SetTile(position, right);
                    }
                }
            }
        }
    }

    public Vector3 GridToWorldPosition(Vector3Int gridPos)
    {
        return new Vector3(gridPos.x, gridPos.y, 0); // or whatever your scale/offset is
    }
    public void GameOver()
    {
        AudioManager.Instance.PlaySFX("GameOver");
        // TODO: add game over screen
        this.tilemap.ClearAllTiles();
        gameManager.ShowGameOver(); // Show Game Over UI
    }
}
