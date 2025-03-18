using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
    public int rotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float lockTime;
    private static Vector2Int currentGravityDirection = Vector2Int.down;
    
    // Add public getter for currentGravityDirection
    public static Vector2Int CurrentGravityDirection => currentGravityDirection;

    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;
        this.rotationIndex = 0;

        stepTime = Time.time + this.stepDelay;
        lockTime = 0f;

        this.cells ??= new Vector3Int[data.cells.Length];

        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    private void Update()
    {
        this.board.Clear(this);

        // check if the piece can move in the gravity direction
        Vector3Int gravityPosition = this.position + new Vector3Int(currentGravityDirection.x, currentGravityDirection.y, 0);
        if (!board.IsValidPosition(this, gravityPosition))
        {
            lockTime += Time.deltaTime;
        }
        else
        {
            lockTime = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1);
        }

        
        Vector2Int oppositeDirection = new Vector2Int(-currentGravityDirection.x, -currentGravityDirection.y);

        // ignore the opposite direction of gravity
        if (Input.GetKeyDown(KeyCode.A) && Vector2Int.left != oppositeDirection)
        {
            Move(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) && Vector2Int.right != oppositeDirection)
        {
            Move(Vector2Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.W) && Vector2Int.up != oppositeDirection)
        {
            Move(Vector2Int.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) && Vector2Int.down != oppositeDirection)
        {
            Move(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }

        if (Time.time >= stepTime)
        {
            Step();
        }

        this.board.Set(this);
    }

    private void Step()
    {
        stepTime = Time.time + stepDelay;

        // move in the current gravity direction
        Move(currentGravityDirection);

        if (lockTime >= lockDelay)
        {
            Lock();
        }
    }

    private void Lock()
    {
        AudioManager.Instance.PlaySFX("Lock");
        this.board.Set(this);
        this.board.ClearLines();
        
        // choose a new random gravity direction for the next piece
        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };
        
        int randomIndex = Random.Range(0, directions.Length);
        currentGravityDirection = directions[randomIndex];
        
        this.board.SpawnPiece();
    }

    private void HardDrop()
    {
        // hard drop in the current gravity direction
        while (Move(currentGravityDirection))
        {
            continue;
        }

        Lock();
    }

    private bool Move(Vector2Int direction)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += direction.x;
        newPosition.y += direction.y;

        bool valid = board.IsValidPosition(this, newPosition);
        if (valid)
        {
            this.position = newPosition;
            lockTime = 0f;
        }

        return valid;
    }

    private void Rotate(int direction)
    {
        int originalRotationIndex = rotationIndex;
        rotationIndex = Wrap(rotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction);

        if (!TestWallKick(rotationIndex, direction))
        {
            rotationIndex = originalRotationIndex;
            ApplyRotationMatrix(-direction);
        }        
    }

    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3 cell = cells[i];
            
            int x, y;

            switch (data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;  // shift before rotation to center the tiles for I and O
                    cell.y -= 0.5f;
                    // apply rotation matrix
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKick(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = data.wallKicks[wallKickIndex, i];

            if (Move(translation))
            {
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0) 
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, data.wallKicks.GetLength(0));
    }

    private int Wrap(int input, int min, int max)
    {
        if (input < min) {
            return max - (min - input) % (max - min);
        } else {
            return min + (input - min) % (max - min);
        }
    }
}
