using TaskProject.Models;
using TaskProject.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace TaskProject.Services
{

public class TaskServices : ITaskService
{

        List<Task> Tasks { get; } 
        private IWebHostEnvironment  webHost;
        private string filePath;
        public TaskServices(IWebHostEnvironment webHost)
        {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "Task.json");
            this.filePath = webHost.ContentRootPath+@"/Data/Task.json";
            using (var jsonFile = File.OpenText(filePath))
            {
                Tasks = JsonSerializer.Deserialize<List<Task>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }
        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(Tasks));
        }
        public List<Task> GetAll(long userId) => Tasks?.Where(t=>t.UserId==userId).ToList<Task>();

        public Task Get(long userId,int id) => Tasks?.FirstOrDefault(p => p.Id == id&&p.UserId==userId);

        public void Add(long userId,Task task)
        {
            task.Id = Tasks.Count() + 1;
            task.UserId=userId;
            Tasks.Add(task);
            saveToFile();
        }

        public void Delete(long userId,int id)
        {
            var task = Get(userId,id);
            if (task is null)
                return;

            Tasks.Remove(task);
            saveToFile();
        }

        public void Update(long userId,Task task)
        {

            var index = Tasks.FindIndex(p => p.Id == task.Id&&p.UserId==userId);
            if (index == -1)
                return;
            task.UserId=userId;
            Tasks[index] = task;
            saveToFile();
        }

        public int Count ()=> Tasks.Count();
    }
}

     
