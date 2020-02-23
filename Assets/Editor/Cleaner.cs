using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class Cleaner
    {
        [MenuItem("Help/Clean Up")]
        private static void CleanUp()
        {
            RemoveDirs(Path.Combine(Directory.GetCurrentDirectory(), "Assets"));
            
            AssetDatabase.Refresh();
        }
    
        private static void RemoveDirs(string startLocation)
        {
            foreach (var directory in Directory.GetDirectories(startLocation))
            {
                RemoveDirs(directory);
                if (Directory.GetFiles(directory).Length == 0 && 
                    Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }
        }
    }
}
