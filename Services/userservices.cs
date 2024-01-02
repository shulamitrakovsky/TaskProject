using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using TaskProject.Interfaces;
using TaskProject.Models;

namespace TaskProject.Services
{

public class userservices : IUserService
{

        List<User> users { get; }
        private IWebHostEnvironment  webHost;
        private string filePath;
        public userservices(IWebHostEnvironment webHost) {
            this.webHost = webHost;
            this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "user.json");
            using (var jsonFile = File.OpenText(filePath))
            {
                users = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }
        private void saveToFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(users));
        }
        public List<User> GetAll() => users?.ToList();

        public User Get(long userId) => users?.FirstOrDefault(p => p.UserId == userId);

        public void Add(User user)
        {
            user.UserId = users.Count() + 1;
            users.Add(user);
            saveToFile();
        }

        public void Delete(long userId,int id)
        {
            var user = Get(userId);
            if (user is null)
                return;

            users.Remove(user);
            saveToFile();
        }

        public void Update(User user)
        {
            var index = users.FindIndex(p => p.UserId == user.UserId);
            if (index == -1)
                return;

            users[index] = user;
            saveToFile();
        }

        public int Count ()=> users.Count();

      
    
    }
}