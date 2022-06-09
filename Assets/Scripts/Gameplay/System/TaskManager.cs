using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TaskManager : MonoBehaviour
{
    public List<Interactable> tasks;
    public List<Interactable> finishedTasks;
    public event Action<Interactable> taskDone;
    public bool allTaskDone;
    public event Action OnAllTaskDone;

    [SerializeField]
    RectTransform textHolder;
    private void Awake()
    {   
        Initiate();
    }
    private void Update()
    {
        if (finishedTasks.Count == tasks.Count)
        {
            allTaskDone = true;
            this.OnAllTaskDone?.Invoke();
        }
        else
        {
            allTaskDone = false;
        }
    }
    void Initiate()
    {
        finishedTasks.Clear();
        TaskListUpdate();
    }
    public void TaskFinished(Interactable task)
    {
        if (!finishedTasks.Contains(task))
        {
            finishedTasks.Add(task);
            taskDone?.Invoke(task);
        }
        TaskListUpdate();
    }
    public void SabotageTask(Interactable task)
    {
        if (finishedTasks.Contains(task))
        {
            finishedTasks.Remove(task);
        }
        TaskListUpdate();
    }
    public void OnTaskButtonClick()
    {
        this.textHolder.gameObject.SetActive(!this.textHolder.gameObject.activeSelf); //toggle task list visibility
        TaskListUpdate();
    }
    void TaskListUpdate()
    {
        TextMeshProUGUI[] texts = this.textHolder.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI text in texts)
        {
            GameObject.Destroy(text.gameObject);
        }
        List<Interactable> unfinishedTasks = new List<Interactable>();
        foreach (Interactable task in tasks)
        {
            if (!finishedTasks.Contains(task))
            {
                unfinishedTasks.Add(task);
            }
        }
        foreach (Interactable task in unfinishedTasks)
        {
            TextMeshProUGUI currentText = Instantiate(Resources.Load<TextMeshProUGUI>("UI/TaskList/Text"), this.textHolder);
            currentText.text = task.name;
        }
    }
    public void FinishedRandomizeTasks(int finishedTask) 
    {
        finishedTasks.Clear();
        int counter = 0;
        while (counter<finishedTask)
        {
            
            TaskFinished(tasks[UnityEngine.Random.Range(0,tasks.Count)]);
            counter++;
        }
    }
}
