using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level-", menuName = "Cytris/LevelDesign")]
public class LevelDesignSO : ScriptableObject
{
    public SpawnObject[] CollectableObjects;
    public SpawnObject[] StaticObjects;
}

[System.Serializable]
public class SpawnObject
{
    public Vector3 ObjectPosition;
    public Vector3 ObjectRotation;
}