using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Beer.Core
{
    public class FileManager
    {
        private const string Folder = "Files";
        public static readonly string AppliactionFilesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Folder);

        private readonly string _basePath;

        public static readonly FileManager Shared = new FileManager(FindSharedFilesPath());
        public static readonly FileManager ForApp = new FileManager(AppliactionFilesPath);


        public FileManager(string basePath)
        {
            _basePath = basePath;
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
        }

        public FileManager() : this(AppliactionFilesPath)
        {
        }

        private static string FindSharedFilesPath()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;

            var directory = new DirectoryInfo(path);
            while (directory != null)
            {
                if (directory.EnumerateFiles("*.sln").Any())
                {
                    return Path.Combine(directory.FullName, Folder);
                }
                directory = directory.Parent;
            }
            throw new ApplicationException("Could not find solution folder.");
        }

        public void SaveJson(object item, string filename = null, bool indented = false, string subFolderName = null)
        {
            if (item == null)
            {
                return;
            }
            var path = filename == null ? GetPathFor(item.GetType()) : GetPathFor(filename, subFolderName);
            File.WriteAllText(path, JsonConvert.SerializeObject(item, indented ? Formatting.Indented : Formatting.None));
        }
        
        public void SaveText(string filename, string text)
        {
            var path = GetPathFor(filename);
            File.WriteAllText(path, text);
        }

        public string LoadText(string filename)
        {
            var path = GetPathFor(filename);
            return File.Exists(path) ? File.ReadAllText(path) : null;
        }

        public T LoadJson<T>(string filename = null)
        {
            var path = filename == null ? GetPathFor<T>() : GetPathFor(filename);
            return File.Exists(path)
                ? JsonConvert.DeserializeObject<T>(File.ReadAllText(path))
                : default(T);
        }
        public T LoadJsonForExactPath<T>(string path)
        {
            return File.Exists(path)
                ? JsonConvert.DeserializeObject<T>(File.ReadAllText(path))
                : default(T);
        }

        public List<JsonFile<T>> LoadFiles<T>(string subfolder = null)
        {
            var path = GetPathFor(subfolder);
            if (Directory.Exists(path))
            {
                var ret = new List<JsonFile<T>>();

                foreach (var file in Directory.GetFiles(path))
                {
                    ret.Add(new JsonFile<T>
                    {
                        FileName = file, 
                        Thing = LoadJson<T>(file)
                    });
                }
            }

                ? JsonConvert.DeserializeObject<T>(File.ReadAllText(path))
                : default(T);
        }

        public T LoadJsonOrDefault<T>(T defaultValue = default(T))
        {
            try
            {
                return LoadJson<T>();
            }
            catch
            {
                return defaultValue;
            }
        }

        private string GetPathFor<T>()
        {
            return GetPathFor(typeof(T));
        }

        private string GetPathFor(Type type)
        {
            var filename = $"{type.Name}.json";
            return GetPathFor(filename);
        }

        private string GetPathFor(string filename, string subfolder = null)
        {
            var path = Path.Combine(_basePath, filename);
            if (!string.IsNullOrWhiteSpace(subfolder))
            {
                path = Path.Combine(_basePath, subfolder, filename);
            }
            return path;
        }
    }


    public class JsonFile<T>
    {
        public string FileName { get; set; }
        public T Thing { get; set; }
    }
}
