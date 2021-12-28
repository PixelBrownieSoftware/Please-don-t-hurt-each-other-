using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ObjectPooler : s_Singleton<S_ObjectPooler>
{
    public Dictionary<string, Queue<GameObject>> objectPoolList = new Dictionary<string, Queue<GameObject>>();
    public List<GameObject> objPoolDatabase = new List<GameObject>();
    public GameObject objectPoolerObj;

    private new void Awake()
    {
        base.Awake();
        SetList();
    }

    public void SetList()
    {
        if (objectPoolList.Count == 0)
        {
            for (int i = 0; i < objPoolDatabase.Count; i++)
            {
                Queue<GameObject> objque = new Queue<GameObject>();
                GameObject obj = objPoolDatabase[i];
                GameObject newobj = Instantiate(obj);
                if (newobj == null)
                {
                    Debug.LogWarning("Fault at " + obj.name);
                    continue;
                }
                if (newobj.TryGetComponent(out I_Spawnable spawnable))
                {
                    spawnable = newobj.GetComponent<I_Spawnable>();
                    newobj.transform.SetParent(objectPoolerObj.transform);
                    objque.Enqueue(newobj);
                    newobj.gameObject.SetActive(false);
                    objectPoolList.Add(obj.name, objque);
                }
                else
                {

                    Debug.LogWarning("I_Spawnable not implemented at " + obj.name);
                    continue;
                }

            }
        }
    }
    public GameObject SpawnObject(string objstr, Vector3 pos, Quaternion quant)
    {
        GameObject ob = null;
        GameObject pd = objPoolDatabase.Find(x => x.name == objstr);
        if (pd == null)
        {
            Debug.LogError("Object: " + pd.name + " dosen't exist!");
            return null;
        }
        if (objectPoolList[objstr].Count < 1)
        {
            ob = Instantiate(pd, pos, quant);
            ob.gameObject.SetActive(true);
            ob.transform.SetParent(objectPoolerObj.transform);
            return ob;
        }
        ob = objectPoolList[objstr].Dequeue();
        ob.gameObject.transform.localRotation = quant;
        ob.transform.position = pos;
        ob.gameObject.SetActive(true);
        ob.transform.SetParent(objectPoolerObj.transform);
        if (ob.TryGetComponent(out I_Spawnable sp))
        {
            sp.OnSpawn();
        }
        return ob;
    }
    /*
    public T SpawnObject<T>(string objstr, Vector3 pos) where T : s_object
    {
        T ob = null;
        if (!objectPoolList.ContainsKey(objstr))
        {
            Queue<s_object> objque = new Queue<s_object>();
            GameObject obj = objPoolDatabase.Find(x => x.name == objstr);
            s_object newobj = Instantiate(obj).GetComponent<s_object>();
            if (objectPoolerObj != null)
                newobj.transform.SetParent(objectPoolerObj.transform);
            else
                print("No object pooler set");
            objque.Enqueue(newobj);
            newobj.gameObject.SetActive(false);
            objectPoolList.Add(obj.name, objque);
        }
        GameObject pd = objPoolDatabase.Find(x => x.name == objstr);

        if (objectPoolList[objstr].Count < 1)
        {
            ob = Instantiate(pd, pos, Quaternion.identity).GetComponent<T>();
            ob.gameObject.SetActive(true);
            ob.ID = objstr;
            ob.transform.SetParent(objectPoolerObj.transform);
            return ob;
        }

        ob = objectPoolList[objstr].Dequeue().GetComponent<T>();
        ob.gameObject.transform.localRotation = Quaternion.identity;
        ob.transform.position = pos;
        ob.gameObject.SetActive(true);
        ob.transform.SetParent(objectPoolerObj.transform);
        ob.ID = objstr;
        return ob;
    }
    */

    public void DespawnObject(GameObject obj)
    {
        obj.transform.parent = null;
        obj.gameObject.SetActive(false);
        if (objectPoolerObj != null)
            obj.transform.SetParent(objectPoolerObj.transform);
        /*
        if (objectPoolList.ContainsKey(obj.ID))
            objectPoolList[obj.ID].Enqueue(obj);
        else
            print("No object pooler set");
        */
    }
}

