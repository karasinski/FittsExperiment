using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public float Gain;
    public GameObject Target, Control;
    public string filename;

    Vector3 InitialPosition = new Vector3(0, 0, 5);
    Quaternion Rotation;
    Vector3 Delta;
    List<Dictionary<string, object>> TrialData;
    int TrialNumber, NumberOfTrials, Angle, Trial;
    float Delay, x, y;

    private StreamWriter Writer;
    private String Output;
    private float startTime;

    void Start()
    {
        TrialData = CSVReader.Read(filename);
        NumberOfTrials = TrialData.Count - 1;

        StartNextTrial();
    }

    void Update()
    {
        x = Input.GetAxisRaw("Mouse X");
        y = Input.GetAxisRaw("Mouse Y");
        Delta = Rotation * new Vector3(x, y, 0.0f) * Time.deltaTime * Gain;
        StartCoroutine(DelayedInput(Delta));
    }

    void FixedUpdate() {
        var r0 = Control.transform.position.ToString() + ", ";
        var input1 = x.ToString() + ", ";
        var input2 = y.ToString() + ", ";
        var r2 = Angle.ToString() + ", ";
        var r3 = Delay.ToString();
        var r = r0 + input1 + input2 + r2 + r3;

        Log(r, Writer);
    }

    void StartNextTrial()
    {
        TrialNumber++;

        if (TrialNumber > NumberOfTrials)
        {
            print("Done!");
            Application.Quit();
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }

        // Start a new data log
        Output = String.Format("logs/trial_{0}_log_{1}.csv", TrialNumber, DateTime.UtcNow.ToLocalTime().ToString("yyy_MM_dd_hh_mm_ss"));
        Writer = File.AppendText(Output);
        startTime = Time.time;

        // Return controlled element to initial position, set target position
        Control.transform.position = InitialPosition;
        Target.transform.position = InitialPosition + RandomPointOnUnitCircle(10);

        // Set current trial
        Trial = (int)TrialData[TrialNumber]["Trial"];

        // Set rotation angle
        Angle = (int)TrialData[TrialNumber]["Rotation"];
        Rotation = Quaternion.Euler(0, 0, Angle);

        // Set the time delay
        Delay = (float)TrialData[TrialNumber]["Delay"];

        print("Trial: " + Trial + ", Angle: " + Angle + ", Delay: " + Delay);
    }

    void OnTriggerEnter(Collider other)
    {
        // Close the file
        Writer.Close();

        // Discard all remaining delayed inputs
        StopAllCoroutines();

        // Start next trial
        StartNextTrial();
    }

    static Vector3 RandomPointOnUnitCircle(float radius)
    {
        float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Sin(angle) * radius;
        float y = Mathf.Cos(angle) * radius;

        return new Vector3(x, y, 0);
    }

    IEnumerator DelayedInput(Vector3 delta)
    {
        if (Delay > 0)
        {
            yield return new WaitForSeconds(Delay);
        }

        Control.transform.position += delta;
    }

    void Log(string logMessage, TextWriter w)
    {
        var time = Time.time - startTime;
        w.WriteLine("{0}, {1}", time, logMessage);
    }
}
