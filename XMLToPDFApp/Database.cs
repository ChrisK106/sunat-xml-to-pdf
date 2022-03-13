using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XMLToPDFApp
{
    public class Database<T>
    {
        public List<T> dataCollection = new();
        public string path;
        public static string databaseDirectory = Path.Combine("..", "data");
        public string imgDirectory = Path.Combine(databaseDirectory, "img");

        public Database(string filename)
        {
            path = Path.Combine(databaseDirectory, filename);

            // Checking if required directories exist
            if (!Directory.Exists(databaseDirectory)) Directory.CreateDirectory(databaseDirectory);
            if (!Directory.Exists(imgDirectory)) Directory.CreateDirectory(imgDirectory);
        }

        public void Save()
        {
            string dataString = JsonConvert.SerializeObject(dataCollection);
            File.WriteAllText(path, dataString);
        }

        public void Load()
        {
            if (File.Exists(path))
            {
                string dataString = File.ReadAllText(path);
                dataCollection = JsonConvert.DeserializeObject<List<T>>(dataString);
            }
            else
            {
                dataCollection = new List<T>();
            }
        }

        public void Insert(T newEntity)
        {
            dataCollection.Add(newEntity);
            Save();
        }

        public List<T> Search(Func<T, bool> query)
        {
            return dataCollection.Where(query).ToList();
        }

        public void Delete(Func<T, bool> query)
        {
            dataCollection = dataCollection.Where(x => !query(x)).ToList();
        }

        public void Update(Func<T, bool> query, T newEntity)
        {
            dataCollection = dataCollection.Select(x =>
            {
                if (query(x)) x = newEntity;
                return x;
            }).ToList();
        }
    }
}
