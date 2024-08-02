using System.Linq;
using CucuTools;
using CucuTools.Utils;
using UnityEngine;
using UnityEngine.Profiling;

namespace Samples.Demo.Scripts
{
    public class DemoFPS : Singleton<DemoFPS>
    {
        [Header("FPS")]
        public int currentFPS;
        public int smoothFPS;
        
        [Header("Memory")]
        public long usedSizeBytes;
        public long heapSizeBytes;
        
        private int _index = 0;
        private readonly int[] fpsHistory = new int[FPSHistoryLength];
        private const int FPSHistoryLength = 1000;

        public void Hello()
        {
        }
        
        public long GetUsedSize()
        {
            return Profiler.GetMonoUsedSizeLong();
        }
        
        public long GetHeapSize()
        {
            return Profiler.GetMonoHeapSizeLong();
        }

        public int GetFPS()
        {
            return (int)(1f / Time.deltaTime);
        }
        
        private void UpdateFPS()
        {
            currentFPS = GetFPS();
            
            fpsHistory[_index] = currentFPS;
            _index = (_index + 1) % fpsHistory.Length;

            smoothFPS = fpsHistory.Sum() / fpsHistory.Length;
        }
        
        private void UpdateMemory()
        {
            usedSizeBytes = GetUsedSize();
            
            heapSizeBytes = GetHeapSize();
        }
        
        private void GUIFPS()
        {
            GUILayout.Box($"Smooth  FPS: {smoothFPS}");
            GUILayout.Box($"Current FPS: {currentFPS}");
        }
        
        private void GUIMemory()
        {
            var usedSizeMb = usedSizeBytes.AsMb();
            var heapSizeMb = heapSizeBytes.AsMb();
            
            GUILayout.Box($"{usedSizeMb} / {heapSizeMb} Mb");
        }
        
        private void Update()
        {
            UpdateFPS();

            UpdateMemory();
        }

        private void OnGUI()
        {
            GUIFPS();

            GUIMemory();
        }
    }
    
    public static class StatsExt
    {
        public static long AsMb(this long bytes)
        {
            return bytes / 1000000;
        }
    }
}