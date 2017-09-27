using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float speed;
    public GameObject Target, Control;
    public string filename;

    Vector3 zero = new Vector3(0, 0, 5);
    Quaternion rotation;
    Vector3 delta;
    List<Dictionary<string, object>> TrialData;
    int TrialNumber, NumberOfTrials, angle;
    float delay;

    void Start()
    {
        TrialData = CSVReader.Read(filename);
        NumberOfTrials = TrialData.Count - 1;

        StartNextTrial();
    }

    void Update()
    {
        delta = rotation * new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0.0f) * Time.deltaTime * speed;
        StartCoroutine(DelayedInput(delta));
    }

    void StartNextTrial()
    {
        TrialNumber++;

        if (TrialNumber > NumberOfTrials)
        {
            print("Done!");
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
        }

        // Return controlled element to initial position, set target position
        Control.transform.position = zero;
        Target.transform.position = zero + RandomPointOnUnitCircle(10);

        // Set rotation angle
        angle = (int)TrialData[TrialNumber]["Rotation"];
        rotation = Quaternion.Euler(0, 0, angle);

        // Set the time delay
        delay = (float)TrialData[TrialNumber]["Delay"];

        print("Trial: " + TrialNumber + ", Angle: " + angle + ", Delay: " + delay);
    }

    void OnTriggerEnter(Collider other)
    {
        StartNextTrial();
    }

    static Vector3 RandomPointOnUnitCircle(float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Sin(angle) * radius;
        float y = Mathf.Cos(angle) * radius;

        return new Vector3(x, y, 0);
    }

    IEnumerator DelayedInput(Vector3 delta)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }

        Control.transform.position += delta;
    }
}
