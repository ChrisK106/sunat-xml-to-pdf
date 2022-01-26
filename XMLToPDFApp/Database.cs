using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace XMLToPDFApp
{
    public class Database<T>
    {
        public List<T> dataCollection = new();
        public string path;

        public Database(string filePath)
        {
            path = filePath;
        }

        public void Save()
        {
            string texto = JsonConvert.SerializeObject(dataCollection);
            File.WriteAllText(path, texto);
        }

        public void Load()
        {
            try
            {
                string archivo = File.ReadAllText(path);
                dataCollection = JsonConvert.DeserializeObject<List<T>>(archivo);
            }
            catch (Exception)
            {

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
