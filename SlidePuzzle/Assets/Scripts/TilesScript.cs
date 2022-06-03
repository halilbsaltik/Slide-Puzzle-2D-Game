using UnityEngine;

public class TilesScript : MonoBehaviour
{

    public Vector3 targetPosition;
    public Vector3 correctPosition;
    private SpriteRenderer spriteRenderBox;
    public int number;
    public bool inRightPlace;

    // Start is called before the first frame update
    void Awake()
    {
        targetPosition = transform.position;
        correctPosition = transform.position;
        spriteRenderBox = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.19f);
        if (targetPosition == correctPosition)
        {
            spriteRenderBox.color = Color.green;
            inRightPlace = true;
        }
        else
        {
            spriteRenderBox.color = Color.white;
            inRightPlace = false;
        }
    }
}
