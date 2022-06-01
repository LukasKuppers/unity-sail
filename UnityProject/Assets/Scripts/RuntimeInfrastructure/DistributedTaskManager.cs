using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributedTaskManager : MonoBehaviour
{
    [SerializeField]
    private float taskWaitTime = 0.5f;

    private Queue<IQueuableTask> tasks;

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

        StartCoroutine(TaskRunner());
    }

    public void AddTask(IQueuableTask task)
    {
        if (task != null)
            tasks.Enqueue(task);
    }

    private IEnumerator TaskRunner()
    {
        while (tasks != null)
        {
            if (tasks.Count > 0)
            {
                IQueuableTask task = tasks.Dequeue();
                task.RunTask();
                tasks.Enqueue(task);
            }
            yield return new WaitForSeconds(taskWaitTime);
        }
    }
}
