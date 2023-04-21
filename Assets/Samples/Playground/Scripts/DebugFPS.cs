using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Profiling;

namespace Examples.Scripts
{
    public class DebugFPS : MonoBehaviour
    {
        [Min(1)]
        public int frequency = 120;
        public float damp = 8;
        
        [Header("FPS")]
        public int fps = 0;
        public int smoothFPS = 0;
        public int averageFPS = 0;
        [Min(1)]
        public int fpsHistoryLength = 120;
        public int fpsGroup = 0;
        
        [Header("Heap Memory")]
        public long monoHeapSize = 0;
        public long totalMemory = 0;
        public int heapMemoryGroup = 1;
        
        [Header("Reserved Memory")]
        public long totalReserved = 0;
        public long unusedReserved = 0;
        public long totalAllocated = 0;
        public int reservedMemoryGroup = 2;
        
        public const string FPS = nameof(FPS);
        public const string SmoothFPS = nameof(SmoothFPS);
        public const string AverageFPS = nameof(AverageFPS);

        private readonly Color _colorFPS = new Color(1, 0.5f, 0.5f);
        private readonly Color _colorSmoothFPS = new Color(1, 1, 0.5f);
        private readonly Color _colorAverageFPS = new Color(0.5f, 1, 0.5f);
        
        private int _fpsIndex = 0;
        private int _fpsSum = 0;
        private int[] _fpsHistory = null;

        private int _maxFPS = 120;
        
        private void GraphFPS()
        {
            if (Time.deltaTime != 0)
            {
                fps = (int)(1 / Time.deltaTime);
            }
            
            if (Time.smoothDeltaTime != 0)
            {
                smoothFPS = (int)(1 / Time.smoothDeltaTime);
            }

            if (_fpsHistory == null || _fpsHistory.Length < fpsHistoryLength)
            {
                _fpsHistory = new int[fpsHistoryLength];
            }

            _fpsHistory[_fpsIndex] = fps;
            _fpsIndex = (_fpsIndex + 1) % fpsHistoryLength;

            _fpsSum = 0;
            for (var i = 0; i < fpsHistoryLength; i++)
            {
                _fpsSum += _fpsHistory[i];
            }
            
            averageFPS = _fpsSum / fpsHistoryLength;
            
            _maxFPS = (int)Mathf.Lerp(_maxFPS, Mathf.Max(2 * averageFPS, 120), damp * Time.deltaTime);
            
            DebugGUI.SetGraphProperties(FPS, "FPS: " + fps, 0, _maxFPS, fpsGroup, _colorFPS, false);
            DebugGUI.Graph(FPS, fps);
            
            DebugGUI.SetGraphProperties(SmoothFPS, "Smooth FPS: " + smoothFPS, 0, _maxFPS, fpsGroup, _colorSmoothFPS, false);
            DebugGUI.Graph(SmoothFPS, smoothFPS);
            
            DebugGUI.SetGraphProperties(AverageFPS, "Average FPS: " + averageFPS, 0, _maxFPS, fpsGroup, _colorAverageFPS, false);
            DebugGUI.Graph(AverageFPS, averageFPS);
        }

        public const string TotalMemory = nameof(TotalMemory);
        public const string MonoHeapSize = nameof(MonoHeapSize);

        private float _maxHeap;
        
        private void GraphHeap()
        {
            monoHeapSize = Profiler.GetMonoHeapSizeLong() / 1024 / 1024;
            totalMemory = GC.GetTotalMemory(false) / 1024 / 1024;

            var max = Mathf.Max(totalMemory, monoHeapSize);
            _maxHeap = (int)Mathf.Lerp(_maxHeap, 1.5f * max, damp * Time.deltaTime);
            
            DebugGUI.SetGraphProperties(MonoHeapSize, $"Mono Heap Size: {monoHeapSize} Mb", 0, _maxHeap, heapMemoryGroup, _colorAverageFPS, false);
            DebugGUI.Graph(MonoHeapSize, monoHeapSize);
            
            DebugGUI.SetGraphProperties(TotalMemory, $"Total Memory: {totalMemory} Mb", 0, _maxHeap, heapMemoryGroup, _colorSmoothFPS, false);
            DebugGUI.Graph(TotalMemory, totalMemory);
        }
        
        public const string ReservedMemory = nameof(ReservedMemory);
        public const string UnusedReserved = nameof(UnusedReserved);
        public const string AllocatedMemory = nameof(AllocatedMemory);

        private float _maxReserved;
        
        private void GraphReservedMemory()
        {
            totalReserved = Profiler.GetTotalReservedMemoryLong() / 1024 / 1024;
            unusedReserved = Profiler.GetTotalUnusedReservedMemoryLong() / 1024 / 1024;
            totalAllocated = Profiler.GetTotalAllocatedMemoryLong() / 1024 / 1024;
            
            var max = Mathf.Max(Mathf.Max(totalReserved, unusedReserved), totalAllocated);
            _maxReserved = (int)Mathf.Lerp(_maxReserved, 1.5f * max, damp * Time.deltaTime);
            
            DebugGUI.SetGraphProperties(ReservedMemory, "Reserved Memory: " + totalReserved, 0, _maxReserved, reservedMemoryGroup, _colorFPS, false);
            DebugGUI.Graph(ReservedMemory, totalReserved);
            
            DebugGUI.SetGraphProperties(UnusedReserved, $"Unused Reserved Memory: {unusedReserved} Mb", 0, _maxReserved, reservedMemoryGroup, _colorSmoothFPS, false);
            DebugGUI.Graph(UnusedReserved, unusedReserved);
            
            DebugGUI.SetGraphProperties(AllocatedMemory, $"Allocated Memory: {totalAllocated} Mb", 0, _maxReserved, reservedMemoryGroup, _colorAverageFPS, false);
            DebugGUI.Graph(AllocatedMemory, totalAllocated);
        }
        
        private IEnumerator _Update()
        {
            while (true)
            {
                GraphFPS();

                GraphHeap();

                GraphReservedMemory();
                
                yield return new WaitForSeconds(1f / frequency);
            }
        }

        private void Start()
        {
            StartCoroutine(_Update());
        }
    }
}
