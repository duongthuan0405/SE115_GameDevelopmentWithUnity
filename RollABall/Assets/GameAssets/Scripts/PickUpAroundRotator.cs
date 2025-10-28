using UnityEngine;

public class Around : MonoBehaviour
{
    [SerializeField] float radius = 1f;   
    [SerializeField] float speed = 50f;   
    [SerializeField] Vector3 axis = Vector3.up; 

    private Vector3 centerPos; 

    void Start()
    {
        centerPos = transform.position;
        transform.position = centerPos + new Vector3(radius, 0, 0);
    }

    void Update()
    {
        RotateAroundForPickUp();
    }

    private void RotateAroundForPickUp()
    {
        transform.RotateAround(centerPos, axis, speed * Time.deltaTime);
    }    
}
