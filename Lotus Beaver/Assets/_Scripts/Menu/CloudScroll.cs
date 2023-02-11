using UnityEngine;

public class CloudScroll : MonoBehaviour
{
    [SerializeField] public float scrollSpeed = 90f;

    private Transform _tranform;
    private float _startX;

    private void Awake()
    {
        _tranform = transform;
        _startX = _tranform.position.x;
    }

    private void OnEnable()
    {
        Vector3 finalPosition = _tranform.position;
        finalPosition.x = _startX;
        _tranform.position = finalPosition;
    }

    private void Update()
    {
        Vector3 finalPosition = _tranform.position;

        finalPosition += Vector3.right * scrollSpeed * Time.unscaledDeltaTime;

        if (finalPosition.x > 1280f + _startX)
        {
            finalPosition = new Vector3(-1280f + _startX, finalPosition.y, finalPosition.z);
        }

        _tranform.position = finalPosition;
    }
}