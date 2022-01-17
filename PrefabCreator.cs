using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabCreator : MonoBehaviour
{
    //1.2
    #region Parameters
    public string parentObjectName;
    public List<GameObject> prefabs;
    public bool shuffle;
    public bool randomGenerate;
    public Vector3 startPosition;
    public bool zValueRange;
    public bool isRandomX;
    public bool isRandomY;

    public float distance, minZ, maxZ;
    public int numberOfObjects;
    public float XValue, minX, maxX;
    public float YValue, minY, maxY;
    #endregion
    #region MonoBehaviour Methods
    #endregion
    #region My Methods
    public void InstantiateObjects()
    {
        if (CheckSafe())
        {
            return;
        }
        GameObject parentObject = new GameObject();
        parentObject.transform.position = Vector3.zero;
        parentObject.name = parentObjectName;

        if (numberOfObjects > 0)
        {
            int k = 0;
            for (int i = 0; i < numberOfObjects; i++)
            {
                if (randomGenerate)
                {
                    k = Random.Range(0, prefabs.Count);
                }
                GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefabs[k], parentObject.transform);
                obj.transform.localPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), minZ * i);
                k++;
                if (k >= prefabs.Count)
                {
                    k = 0;
                }
            }
        }
        else
        {
            int i = 0;
            int k = 0;
            if (randomGenerate)
            {
                k = Random.Range(0, prefabs.Count);
            }
            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(prefabs[k], parentObject.transform);
            obj.transform.localPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), minZ + (distance * i));
            while (obj.transform.localPosition.z + distance < maxZ)
            {
                i++;
                k++;
                if (k >= prefabs.Count)
                {
                    k = 0;
                }
                if (randomGenerate)
                {
                    k = Random.Range(0, prefabs.Count);
                }
                obj = (GameObject)PrefabUtility.InstantiatePrefab(prefabs[k], parentObject.transform);
                obj.transform.localPosition = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), minZ + (distance * i));
            }
        }
    }
    public void Shuffle<T>(List<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
    private bool CheckSafe()
    {
        if (distance <= 0)
        {
            Debug.LogError("Please enter positive number for distance");
            return true;
        }
        else if (minX > maxX)
        {
            Debug.LogError("minX must be less than maxX");
            return true;
        }
        else if (minZ > maxZ)
        {
            Debug.LogError("minZ must be less than maxZ");
            return true;
        }
        else if (minY > maxY)
        {
            Debug.LogError("minY must be less than maxY");
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}

[CustomEditor(typeof(PrefabCreator))]
public class CreatorEditor : Editor
{
    #region Parameters

    #endregion
    #region Editor Methods
    public override void OnInspectorGUI()
    {
        PrefabCreator creator = (PrefabCreator)target;

        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        creator.parentObjectName = EditorGUILayout.TextField("Parent Object Name:", creator.parentObjectName);
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("prefabs"));

        GUILayout.Label("Prefab List Generate Settings", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Suffle List:", EditorStyles.label);
        if (GUILayout.Button("Shuffle"))
        {
            creator.Shuffle(creator.prefabs);
        }
        GUILayout.EndHorizontal();

        creator.randomGenerate = GUILayout.Toggle(creator.randomGenerate, "Random Generate");

        GUILayout.Label("Distance(Z) Value Set", EditorStyles.boldLabel);
        creator.zValueRange = GUILayout.Toggle(creator.zValueRange, "Z Value Range");
        if (creator.zValueRange)
        {
            creator.distance = EditorGUILayout.FloatField("Distance:", creator.distance);
            creator.minZ = EditorGUILayout.FloatField("Min Z:", creator.minZ);
            creator.maxZ = EditorGUILayout.FloatField("Max Z:", creator.maxZ);
            creator.numberOfObjects = 0;
        }
        else
        {
            creator.distance = EditorGUILayout.FloatField("Distance:", creator.distance);
            creator.minZ = creator.distance;
            creator.maxZ = creator.distance;
            creator.numberOfObjects = EditorGUILayout.IntField("Number Of Objects:", creator.numberOfObjects);
        }

        GUILayout.Label("X Value Set", EditorStyles.boldLabel);
        creator.isRandomX = GUILayout.Toggle(creator.isRandomX, "Is Random X");
        if (creator.isRandomX)
        {
            creator.minX = EditorGUILayout.FloatField("Min X:", creator.minX);
            creator.maxX = EditorGUILayout.FloatField("Max X:", creator.maxX);
        }
        else
        {
            creator.XValue = EditorGUILayout.FloatField("X Value:", creator.XValue);
            creator.minX = creator.XValue;
            creator.maxX = creator.XValue;
        }

        GUILayout.Label("Y Value Set", EditorStyles.boldLabel);
        creator.isRandomY = GUILayout.Toggle(creator.isRandomY, "Is Random Y");
        if (creator.isRandomY)
        {
            creator.minY = EditorGUILayout.FloatField("Min Y:", creator.minY);
            creator.maxY = EditorGUILayout.FloatField("Max Y:", creator.maxY);
        }
        else
        {
            creator.YValue = EditorGUILayout.FloatField("Y Value:", creator.YValue);
            creator.minY = creator.YValue;
            creator.maxY = creator.YValue;
        }

        if (GUILayout.Button("Run Creator!"))
        {
            creator.InstantiateObjects();
        }
    }
    #endregion
    #region My Methods

    #endregion
}
