using UnityEngine;
using System.Collections.Generic;

public class PersistentObjectsManager : MonoBehaviour
{
    public List<GameObject> persistentObjects;
    private static PersistentObjectsManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            MarkObjectsAsPersistent();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void MarkObjectsAsPersistent()
    {
        foreach (GameObject obj in persistentObjects)
        {
            if (obj != null && !IsObjectAlreadyPersistent(obj))
            {
                DontDestroyOnLoad(obj);
            }
        }
    }

    private bool IsObjectAlreadyPersistent(GameObject obj)
    {
        // Проверяем, существует ли объект с таким же именем и типом в сцене
        GameObject existingObject = GameObject.Find(obj.name);
        return existingObject != null && existingObject != obj;
    }
}