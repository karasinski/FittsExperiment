using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float speed, delay;
    private List<int> rotations = new List<int>(new int[] { 45, -45, 0, 90, -90 });
    private Vector3 zero = new Vector3(0, 0, 5);
    private Quaternion rotation;
    private Vector3 delta;
    private List<Dictionary<string, object>> TrialData;

    public GameObject Target, Control;

    private static Vector3 RandomPointOnUnitCircle(float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Sin(angle) * radius;
        float y = Mathf.Cos(angle) * radius;

        return new Vector3(x, y, 0);
    }

    private void SetRotation()
    {
        int choice = rotations[Random.Range(0, rotations.Count)];
        rotation = Quaternion.Euler(0, 0, choice);
        //print(choice);
    }

    private IEnumerator DelayedInput(Vector3 delta)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }

        Control.transform.position += delta;
    }

    private void Start()
    {
        ReadCSV("trials");
    }

    private void Update()
    {
        delta = rotation * new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0.0f) * Time.deltaTime * speed;
        StartCoroutine(DelayedInput(delta));
    }

    private void OnTriggerEnter(Collider other)
    {
        Control.transform.position = zero;
        Target.transform.position = zero + RandomPointOnUnitCircle(10);
        SetRotation();
    }

    private void ReadCSV(string filename)
    {
        TrialData = CSVReader.Read(filename);
        //for (var i = 0; i < TrialData.Count; i++)
        //{
        //    print((i + 1) +
        //          "trial " + TrialData[i]["Trial"] + " " +
        //          "rotation " + TrialData[i]["Rotation"] + " " +
        //          "delay " + TrialData[i]["Delay"]);
        //}
    }
}
