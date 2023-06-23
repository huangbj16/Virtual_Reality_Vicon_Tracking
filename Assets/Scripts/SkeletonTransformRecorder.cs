using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
 * this script takes one object as root object, and record transforms of all child objects in each frame.
*/

public class SkeletonTransformRecorder : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform rootTransform;
    [SerializeField] private string fileName = "SkeletonTransformData.txt";
    [SerializeField] private string message = "Hello, world!\n";
    string filePath;
    StreamWriter writer;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(filePath, message);
        writer = File.AppendText(filePath);
        Debug.Log("save to "+filePath);
    }

    // Update is called once per frame
    void Update()
    {
        writer.WriteLine("BEGIN");
        writer.WriteLine(Time.time);
        RecordTransform(rootTransform);
        writer.WriteLine("END");
    }

    void RecordTransform(Transform bone)
    {
        Debug.Log("Record " + bone.name);
        DataObject data = new DataObject(bone.name, bone.position, bone.rotation);
        writer.WriteLine(JsonUtility.ToJson(data));
        for(int i = 0; i < bone.childCount; i++)
        {
            RecordTransform(bone.GetChild(i));
        }
    }

    private void OnDisable()
    {
        writer.Close();
    }

    [System.Serializable]
    public class DataObject
    {
        public string boneName;
        public Vector3 position;
        public Quaternion rotation;

        public DataObject(string _b, Vector3 _p, Quaternion _r)
        {
            boneName = _b;
            position = _p;
            rotation = _r;
        }
    }
}
