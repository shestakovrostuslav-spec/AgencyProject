using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace RealEstateAgency.DataAccess
{

    public class JsonFileStorage<T>
    {
        private readonly string filePath;

        private readonly JsonSerializerOptions options =
            new JsonSerializerOptions
            {
                WriteIndented = true
            };

        public JsonFileStorage(string fileName)
        {
            string directory = Path.Combine(
                AppContext.BaseDirectory, "Data");

            Directory.CreateDirectory(directory);

            filePath = Path.Combine(directory, fileName);
        }

        // Загрузка данных
        public List<T> Load()
        {
            if (!File.Exists(filePath))
            {
                return new List<T>();
            }

            string json = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<List<T>>(
                json, options) ?? new List<T>();
        }

        // Сохранение данных
        public void Save(List<T> data)
        {
            string json = JsonSerializer.Serialize(
                data, options);

            File.WriteAllText(filePath, json);
        }
    }
}
