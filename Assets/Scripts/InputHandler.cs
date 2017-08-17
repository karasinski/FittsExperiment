using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float speed;
    private List<int> rotations = new List<int>(new int[] { 45, -45, 0, 90, -90 });
    private Vector3 zero = new Vector3(0, 0, 5);
    private Quaternion rotation;

    public GameObject Target, Control;

    public static Vector3 RandomPointOnUnitCircle(float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Sin(angle) * radius;
        float y = Mathf.Cos(angle) * radius;

        return new Vector3(x, y, 0);
    }

    void setRotation()
    {
        rotation = Quaternion.Euler(0, 0, rotations[Random.Range(0, rotations.Count)]);
        print(rotation);
    }

    void Update()
    {
        Vector3 delta = rotation * new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0.0f) * Time.deltaTime * speed;
        Control.transform.position += delta;
    }

    void OnTriggerEnter(Collider other)
    {
        Control.transform.position = zero;
        Target.transform.position = zero + RandomPointOnUnitCircle(10);
        setRotation();
    }
}
