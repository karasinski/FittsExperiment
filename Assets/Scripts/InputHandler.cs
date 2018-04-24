using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float speed, delay;
    private List<int> rotations = new List<int>(new int[] { 30, -30, 60, -60 });
    //private List<int> rotations = new List<int>(new int[] { 45 });
    //public int angle;
    private Vector3 zero = new Vector3(0, 0, 5);
    private Quaternion rotation;
    private Vector3 delta;

    public GameObject Target, Control;

    private static Vector3 RandomPointOnUnitCircle(float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Sin(angle) * radius;
        float y = Mathf.Cos(angle) * radius;

        return new Vector3(x, y, 0);
    }

    private void setRotation()
    {
        int choice = rotations[Random.Range(0, rotations.Count)];
        rotation = Quaternion.Euler(0, 0, choice);
        print(choice);
    }

    private IEnumerator delayedInput(Vector3 delta)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }

        Control.transform.position += delta;
    }

    private void Update()
    {
        delta = rotation * new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0.0F) * Time.deltaTime * speed;
        //delta = rotation * new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Twist")) * Time.deltaTime * speed;
        StartCoroutine(delayedInput(delta));
    }

    private void OnTriggerEnter(Collider other)
    {
        Control.transform.position = zero;
        Target.transform.position = zero + RandomPointOnUnitCircle(10);
        setRotation();
        System.Threading.Thread.Sleep(1000);
    }
}
