using UnityEngine;
using TMPro;
public class NextDirectionUI : MonoBehaviour
{
     public RectTransform arrowTransform;

    public void ShowNextGravity(Vector2Int next)
    {
        float angle = GetRotationAngle(next);
        arrowTransform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private float GetRotationAngle(Vector2Int dir)
    {
        if (dir == Vector2Int.right) return 0f;
        if (dir == Vector2Int.down) return 90f;
        if (dir == Vector2Int.left) return 180f;
        if (dir == Vector2Int.up) return -90f;

        return 0f; // default
    }
}
