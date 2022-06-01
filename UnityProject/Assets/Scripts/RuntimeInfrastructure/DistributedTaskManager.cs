using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributedTaskManager : MonoBehaviour
{
    [SerializeField]
    private float taskWaitTime = 0.5f;

    private Queue<IQueuableTask> tasks;
    private Dictionary<IQueuableTask, GameObject> objectMap;

    private static DistributedTaskManager instance;

    public static DistributedTaskManager GetInstance()
    {
        if (instance == null)
            instance = FindObjectOfType<DistributedTaskManager>();

        return instance;
    }

    private void Start()
    {
        tasks = new Queue<IQueuableTask>();
        objectMap = new Dictionary<IQueuableTask, GameObject>();

        StartCoroutine(TaskRunner());
    }

    public void AddTask(GameObject taskObject)
    {
        if (taskObject != null)
        {
            IQueuableTask task = taskObject.GetComponent<IQueuableTask>();
            tasks.Enqueue(task);
            objectMap.Add(task, taskObject);
        }
    }

    private IEnumerator TaskRunner()
    {
        while (tasks != null)
        {
            if (tasks.Count > 0)
            {
                IQueuableTask task = tasks.Dequeue();
                if (objectMap[task] != null)
                {
                    task.RunTask();
                    tasks.Enqueue(task);
                }
                else
                {
                    objectMap.Remove(task);
                }
            }
            yield return new WaitForSeconds(taskWaitTime);
        }
    }
}
