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
    public class VersionEditorWindow : EditorWindow
    {
        [MenuItem("Tools/Package Settings")]
        private static void WindowShow()
        {
            var window = (VersionEditorWindow)GetWindow(typeof(VersionEditorWindow));
            window.titleContent = new GUIContent("Version Editor Window");
            window.package = GetPackage();
            window.lastPackage = window.package;
            window.Show();
        }

        private static string directoryPath => Path.Combine(Application.dataPath, "CucuTools");
        private static string fileName => "package.json";
        private static string packagePath => Path.Combine(directoryPath, fileName);
        
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
        
        private PackageData lastPackage = default;
        private PackageData package = default;
        private bool packageLoaded = false;
        private bool packageChanged = false;
        
        private void OnGUI()
        {
            packageLoaded = !package.Equals(default);
            
            GUILoadAndSave();

            GUIName();
            
            GUIVersion();

            GUIDisplayName();

            GUIDescription();

            GUIUnity();
            
            GUIDependencies();

            GUIKeywords();
        }

        private void GUILoadAndSave()
        {
            if (!packageLoaded)
            {
                package = GetPackage();
                lastPackage = package;
            }
            
            packageChanged = !package.Equals(lastPackage);
            
            GUILayout.BeginHorizontal();
            
            if (packageChanged)
            {
                if (GUILayout.Button("Reset Package"))
                {
                    package = GetPackage();
                    lastPackage = package;
                }
                
                if (GUILayout.Button("Update Package"))
                {
                    SetPackage(package);
                    lastPackage = package;
                }
            }
            else
            {
                if (GUILayout.Button("Reset Package"))
                {
                    package = GetPackage();
                    lastPackage = package;
                }
            }
            
            GUILayout.EndHorizontal();
        }
        
        private void GUIName()
        {
            GUILayout.Box($"{package.name}", GUILayout.ExpandWidth(true));
        }
        
        private void GUIVersion()
        {
            GUILayout.Box($"{package.version}{(packageChanged ? "*" : "")}", GUILayout.ExpandWidth(true));

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
        
        private void GUIDisplayName()
        {
            GUILayout.Box($"{package.displayName}", GUILayout.ExpandWidth(true));
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
        
        [Serializable]
        internal struct PackageData
        {
            public string name;
            public string version;
            public string displayName;
            public string description;
            public string unity;
            public Dictionary<string, string> dependencies;
            public string[] keywords;
            public Dictionary<string, string> author;
        }
    }
}
