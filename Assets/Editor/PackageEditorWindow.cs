using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class PackageEditorWindow : EditorWindow
    {
        [MenuItem("Tools/Package Settings")]
        private static void WindowShow()
        {
            var window = (PackageEditorWindow)GetWindow(typeof(PackageEditorWindow));
            window.titleContent = new GUIContent("Package Settings");
            window.package = GetPackage();
            window.Show();
        }

        private static string pathPackageDirectory => Path.Combine(Application.dataPath, "CucuTools");
        private static string packageFileName => "package.json";
        private static string packagePath => Path.Combine(pathPackageDirectory, packageFileName);
        
        private static bool TryLoadPackage(out PackageData package)
        {
            package = default;

            if (File.Exists(packagePath))
            {
                var content = File.ReadAllText(packagePath);

                var contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy
                    {
                        ProcessDictionaryKeys = false
                    }
                };

                package = JsonConvert.DeserializeObject<PackageData>(content, new JsonSerializerSettings
                {
                    ContractResolver = contractResolver,
                    Formatting = Formatting.Indented
                });

                return true;
            }

            return false;
        }

        private static void TrySavePackage(PackageData package)
        {
            var content = JsonConvert.SerializeObject(package);
            
            File.WriteAllText(packagePath, content);
        }
        
        private static PackageData GetPackage()
        {
            if (TryLoadPackage(out var package))
            {
                return package;
            }

            package = new PackageData();

            package.displayName = PlayerSettings.productName;
            package.version = PlayerSettings.bundleVersion;
            
            return package;
        }

        private static void SetPackage(PackageData package)
        {
            TrySavePackage(package);
            
            PlayerSettings.productName = package.displayName;
            PlayerSettings.bundleVersion = package.version;
            
            AssetDatabase.SaveAssets();
        }
        
                private static void DeleteDirectory(string sourceDir, bool recursive = true)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            if (dir.Exists)
            {
                dir.Delete(recursive);
                Debug.Log($"Directory \"{dir.FullName}\" was deleted");
            }
        }
        
        private static void CopyDirectory(string sourceDir, string destinationDir, bool recursive = true)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
            {
                Debug.LogError($"Directory \"{dir.FullName}\" does not exist");
                return;
            }

            if (Directory.Exists(destinationDir))
            {
                Directory.Delete(destinationDir, recursive);
            }
            
            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            Debug.Log($"Directory \"{destinationDir}\" was created");
            
            // Get the files in the source directory and copy to the destination directory
            foreach (var file in dir.GetFiles())
            {
                var targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
                
                Debug.Log($"File \"{targetFilePath}\" was copied");
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                // Cache directories before we start copying
                var dirs = dir.GetDirectories();
                
                foreach (var subDir in dirs)
                {
                    var newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }
        
        private static string pathSourceSamples => Path.Combine(Application.dataPath, "Samples");
        private static string pathTargetSamples => Path.Combine(pathPackageDirectory, "Samples~");
        
        private PackageData package = default;
        private bool packageLoaded = false;

        private const int SpaceBlock = 16;

        private Vector2 scroll = Vector2.zero;

        private void OnGUI()
        {
            packageLoaded = package != null;

            scroll = GUILayout.BeginScrollView(scroll);
            
            GUILayout.Space(SpaceBlock);
            
            GUILoadAndSave();

            GUILayout.Space(SpaceBlock);
            
            GUILayout.BeginHorizontal();
            
            GUILayout.BeginVertical();
            
            GUIName();
            
            GUIVersion();

            GUIDisplayName();

            GUILayout.EndVertical();
            
            GUIVersionEditor();
            
            GUILayout.EndHorizontal();
            
            GUILayout.Space(SpaceBlock);
            
            GUIDescription();

            GUILayout.Space(SpaceBlock);
            
            GUIUnity();
            
            GUIDependencies();

            GUILayout.Space(SpaceBlock);
            
            GUIKeywords();

            GUILayout.Space(SpaceBlock);
            
            GUIAuthor();
            
            GUILayout.Space(SpaceBlock);
            
            GUISamples();
            
            GUILayout.EndScrollView();
        }

        private void GUILoadAndSave()
        {
            if (!packageLoaded)
            {
                package = GetPackage();
            }
            
            GUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Reset Package"))
            {
                package = GetPackage();
            }
                
            if (GUILayout.Button("Update Package"))
            {
                SetPackage(package);
            }
            
            GUILayout.EndHorizontal();
        }
        
        private void GUIName()
        {
            GUILayout.Box($"{package.name}", GUILayout.ExpandWidth(true));
        }
        
        private void GUIVersion()
        {
            GUILayout.Box($"{package.version}", GUILayout.ExpandWidth(true));
        }
        
        private void GUIDisplayName()
        {
            GUILayout.Box($"{package.displayName}", GUILayout.ExpandWidth(true));
        }

        private void GUIVersionEditor()
        {
            var numbers = package.version.Split(".").Select(int.Parse).ToArray();

            GUILayout.BeginHorizontal();
            
            for (var i = 0; i < numbers.Length; i++)
            {
                GUILayout.BeginVertical();
                
                if (GUILayout.Button("+"))
                {
                    numbers[i] += 1;
                }
                
                GUILayout.Box(numbers[i].ToString(), GUILayout.ExpandWidth(true));
                
                if (GUILayout.Button("-"))
                {
                    numbers[i] -= 1;
                }
                
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
            
            package.version = string.Join(".", numbers.Select(n => n.ToString()));
        }
        
        private void GUIDescription()
        {
            GUILayout.Box($"Description", GUILayout.ExpandWidth(true));
            GUILayout.Label(package.description);
        }

        private void GUIUnity()
        {
            GUILayout.Box($"Unity {package.unity}", GUILayout.ExpandWidth(true));
        }
        
        private void GUIDependencies()
        {
            GUILayout.Box($"Dependencies", GUILayout.ExpandWidth(true));
            
            GUILayout.BeginHorizontal();
            
            foreach (var pair in package.dependencies)
            {
                GUILayout.Label($"{pair.Key}: {pair.Value}", GUILayout.ExpandWidth(false));
            }
            
            GUILayout.EndHorizontal();
        }
        
        private void GUIKeywords()
        {
            GUILayout.Box($"Keywords", GUILayout.ExpandWidth(true));
            
            GUILayout.BeginHorizontal();
            
            foreach (var keyword in package.keywords)
            {
                GUILayout.Label(keyword, GUILayout.ExpandWidth(false));
            }
            
            GUILayout.EndHorizontal();
        }

        private void GUIAuthor()
        {
            GUILayout.Box($"Author", GUILayout.ExpandWidth(true));
            
            foreach (var pair in package.author)
            {
                GUILayout.Label($"{pair.Key}: {pair.Value}", GUILayout.ExpandWidth(false));
            }
        }

        private SampleData GUISample(SampleData sample, out bool remove)
        {
            remove = false;
            
            var sourceDir = Path.Combine(pathSourceSamples, sample.displayName);
            var targetDir = Path.Combine(pathTargetSamples, sample.displayName);

            var existSource = Directory.Exists(sourceDir) && !string.IsNullOrWhiteSpace(sample.displayName);
            var existTarget = Directory.Exists(targetDir) && !string.IsNullOrWhiteSpace(sample.displayName);
                
            GUILayout.BeginHorizontal();
                
            if (existTarget)
            {
                if (GUILayout.Button("Delete Sample"))
                {
                    DeleteDirectory(targetDir);
                }
            }
            else
            {
                if (GUILayout.Button("Remove Sample"))
                {
                    remove = true;
                }
            }
                
            if (existSource)
            {
                var buttonName = existTarget ? "Update Sample" : "Copy Sample";
                if (GUILayout.Button(buttonName))
                {
                    CopyDirectory(sourceDir, targetDir);
                }
            }
            else
            {
                GUILayout.Box("Copy Sample", GUILayout.ExpandWidth(true));
            }
                
            GUILayout.EndHorizontal();
                
            sample.displayName = GUILayout.TextField(sample.displayName);
            sample.description = GUILayout.TextField(sample.description);
                
            sample.path = Path.Combine("Samples~", sample.displayName);

            GUILayout.Label(sample.path);

            return sample;
        }
        
        private void GUISamples()
        {
            if (!Directory.Exists(pathTargetSamples))
            {
                Directory.CreateDirectory(pathTargetSamples);
            }
            
            GUILayout.Box($"Samples", GUILayout.ExpandWidth(true));

            var removeList = new List<SampleData>();
            for (var i = 0; i < package.samples.Count; i++)
            {
                if (i > 0) GUILayout.Space(16);
                package.samples[i] = GUISample(package.samples[i], out var remove);
                if (remove) removeList.Add(package.samples[i]);
            }
            removeList.ForEach(r => package.samples.Remove(r));
            
            var sourceFolder = new DirectoryInfo(pathSourceSamples);
            var sources = sourceFolder.GetDirectories();
            
            foreach (var source in sources)
            {
                if (package.samples.Any(s => string.Equals(s.displayName, source.Name))) continue;

                if (GUILayout.Button($"+ {source.Name}"))
                {
                    package.samples.Add(new SampleData() { displayName = source.Name });
                }
            }
        }

        [Serializable]
        internal class PackageData
        {
            public string name = string.Empty;
            public string version = string.Empty;
            public string displayName = string.Empty;
            public string description = string.Empty;
            public string unity = string.Empty;
            public readonly Dictionary<string, string> dependencies = new Dictionary<string, string>();
            public readonly List<string> keywords = new List<string>();
            public readonly Dictionary<string, string> author = new Dictionary<string, string>();
            public readonly List<SampleData> samples = new List<SampleData>();
        }

        [Serializable]
        internal struct SampleData
        {
            public string displayName;
            public string description;
            public string path;
        }
    }
}
