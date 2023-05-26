using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Specialized;


namespace YANSMBWE
{
    public static class RecentFilesManager
    {
        static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1);
        static List<RecentFile>? recentFiles = null;
        public static List<RecentFile> RecentFiles { get => GetRecentFiles(); set => SetRecentFiles(value); }

        static List<RecentFile> GetRecentFiles()
        {
            semaphore.Wait();
            try
            {
                if (recentFiles is not null)
                {
                    recentFiles = recentFiles.OrderByDescending(f => f.LastAccessed.Ticks).ToList();
                    return recentFiles;
                }

                if (!File.Exists("recent.json"))
                {
                    recentFiles = new List<RecentFile>();
                    return recentFiles;
                }

                List<RecentFile> files = new();
                JArray recentFilesJson = JArray.Parse(File.ReadAllText("recent.json"));
                
                foreach (JToken file in recentFilesJson)
                {
                    string? name = (string?)file["name"];
                    string? path = (string?)file["path"];
                    long? lastAccessed = (long?)file["lastAccessed"];

                    if (name is null ||
                        path is null ||
                        lastAccessed is null)
                        continue;

                    files.Add(new(name, path, new DateTime(lastAccessed.Value)));
                }

                recentFiles = files.OrderByDescending(f => f.LastAccessed.Ticks).ToList();
                return recentFiles;
            }
            finally
            {
                semaphore.Release();
            }
        }
        static void SetRecentFiles(List<RecentFile> newRecentFiles)
        {
            semaphore.Wait();
            try
            {
                recentFiles = newRecentFiles.OrderByDescending(f => f.LastAccessed.Ticks).ToList();
            }
            finally
            {
                semaphore.Release();
            }
        }

        public static void SaveRecentFiles()
        {
            semaphore.Wait();
            try
            {
                if (recentFiles is null)
                    return;

                JArray recentFilesJson = new();
                foreach (RecentFile file in recentFiles)
                {
                    JToken fileJson = new JObject
                    {
                        ["name"] = file.Name,
                        ["path"] = file.Path,
                        ["lastAccessed"] = file.LastAccessed.Ticks
                    };

                    recentFilesJson.Add(fileJson);
                }

                File.WriteAllText("recent.json", recentFilesJson.ToString());
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
