using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributedTaskManager : MonoBehaviour
{
    [SerializeField]
    private float taskWaitTime = 0.5f;

    private Queue<IQueuableTask> tasks;
    private Dictionary<IQueuableTask, GameObject> objectMap;
    private HashSet<IQueuableTask> standaloneTaskSet;

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
        standaloneTaskSet = new HashSet<IQueuableTask>();

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

    public void AddStandaloneTask(IQueuableTask task)
    {
        if (task != null)
        {
            tasks.Enqueue(task);
            standaloneTaskSet.Add(task);
        }
    }

    public void RemoveStandaloneTask(IQueuableTask task)
    {
        if (standaloneTaskSet.Contains(task))
        {
            standaloneTaskSet.Remove(task);
        }
    }

    private IEnumerator TaskRunner()
    {
        while (tasks != null)
        {
            if (tasks.Count > 0)
            {
                IQueuableTask task = tasks.Dequeue();

                bool isObjectTask = objectMap.ContainsKey(task);
                float deltaTime = (tasks.Count + 1) * taskWaitTime;

                if (isObjectTask)
                {
                    if (objectMap[task] != null)
                    {
                        if (objectMap[task].activeSelf)
                            task.RunTask(deltaTime);

                        tasks.Enqueue(task);
                    }
                    else
                        objectMap.Remove(task);
                }
                else
                {
                    if (standaloneTaskSet.Contains(task))
                    {
                        task.RunTask(deltaTime);
                        tasks.Enqueue(task);
                    }
                }
            }
            yield return new WaitForSeconds(taskWaitTime);
        }
    }
}
