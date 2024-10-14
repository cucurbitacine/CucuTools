using System.Collections.Generic;
using CucuTools.StateMachines;
using UnityEngine;

namespace CucuTools
{
    public static class Cucu
    {
        public const string Root = "CucuTools";
        public const string CreateAsset = Root + "/";
        public const string DamageGroup = "Damage System/";

        #region Extensions

        public static bool CompareLayer(this LayerMask layerMask, int layer)
        {
            return (layerMask.value & (1 << layer)) > 0;
        }
        
        public static bool CompareLayer(this GameObject gameObject, LayerMask layerMask)
        {
            return layerMask.CompareLayer(gameObject.layer);
        }

        public static bool CompareLayer(this Component component, LayerMask layerMask)
        {
            return component.gameObject.CompareLayer(layerMask);
        }

        public static bool TrySetContext(this object holder, object context)
        {
            const string methodName = nameof(IContextHolder<object>.SetContext);

            var method = holder.GetType().GetMethod(methodName);
            if (method == null) return false;
            
            var parameters = method.GetParameters();
            if (parameters.Length != 1) return false;

            if (!parameters[0].ParameterType.IsInstanceOfType(context)) return false;
            
            method.Invoke(holder, new object[] { context });
            return true;
        }

        public static void LinSpace(float a, float b, float[] array)
        {
            var resolution = array.Length;

            if (resolution == 0) return;
            if (resolution == 1)
            {
                array[0] = (a + b) * 0.5f;
                return;
            }

            if (resolution == 2)
            {
                array[0] = a;
                array[1] = b;
                return;
            }

            var step = (b - a) / (resolution - 1);
            for (var i = 0; i < resolution; i++)
            {
                array[i] = a + i * step;
            }
        }

        public static void LinSpace(float[] array)
        {
            LinSpace(0f, 1f, array);
        }

        public static float[] LinSpace(float a, float b, int resolution = 8)
        {
            var array = new float[resolution];

            LinSpace(a, b, array);

            return array;
        }

        public static float[] LinSpace(int resolution = 8)
        {
            return LinSpace(0f, 1f, resolution);
        }

        #endregion

        #region Colors

        public static Color R(this Color color, float r)
        {
            return new Color(Mathf.Clamp01(r), color.g, color.b, color.a);
        }
        
        public static Color G(this Color color, float g)
        {
            return new Color(color.r, Mathf.Clamp01(g), color.b, color.a);
        }
        
        public static Color B(this Color color, float b)
        {
            return new Color(color.r, color.g, Mathf.Clamp01(b), color.a);
        }
        
        public static Color Alpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, Mathf.Clamp01(alpha));
        }
        
        public static Color Hue(this Color color, float hue)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            
            return Color.HSVToRGB(Mathf.Repeat(hue, 1f), s, v);
        }
        
        public static Color Saturation(this Color color, float saturation)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            
            return Color.HSVToRGB(h, Mathf.Clamp01(saturation), v);
        }

        public static Color Value(this Color color, float value)
        {
            Color.RGBToHSV(color, out var h, out var s, out var v);
            
            return Color.HSVToRGB(h, s, Mathf.Clamp01(value));
        }

        public static Color AsColor(this Vector3 vector3, float alpha = 1f)
        {
            return new Color(vector3.x, vector3.y, vector3.z, alpha);
        }

        public static Color AsColor(this Vector4 vector4)
        {
            return new Color(vector4.x, vector4.y, vector4.z, vector4.w);
        }

        public static Vector3 AsVector3(this Color color)
        {
            return new Vector3(color.r, color.g, color.b);
        }

        public static Vector4 AsVector4(this Color color)
        {
            return new Vector4(color.r, color.g, color.b, color.a);
        }
        
        #endregion
        
        #region ParticleSystem

        public static void PlaySafe(this ParticleSystem particleSystem, bool play)
        {
            if (play)
            {
                if (!particleSystem.isPlaying)
                {
                    particleSystem.Play();
                }
            }
            else
            {
                if (particleSystem.isPlaying)
                {
                    particleSystem.Stop();
                }
            }
        }

        #endregion
        
        #region AudioSource

        public static void PlaySafe(this AudioSource audioSource, bool play)
        {
            if (play)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }
        
        public static void PlayOneShot(this AudioSource audio, params AudioClip[] clips)
        {
            if (clips != null && clips.Length > 0)
            {
                audio.PlayOneShot(clips[Random.Range(0, clips.Length)]);
            }
        }
        
        public static void PlayOneShot(this GameObject gameObject, params AudioClip[] clips)
        {
            if (gameObject.TryGetComponent<AudioSource>(out var audio))
            {
                audio.PlayOneShot(clips);
            }
        }
        
        public static void PlayOneShot(this AudioSource audio, List<AudioClip> clips)
        {
            if (clips != null && clips.Count > 0)
            {
                audio.PlayOneShot(clips[Random.Range(0, clips.Count)]);
            }
        }

        public static void PlayOneShot(this GameObject gameObject, List<AudioClip> clips)
        {
            if (gameObject.TryGetComponent<AudioSource>(out var audio))
            {
                audio.PlayOneShot(clips);
            }
        }
        
        public static void PlayOneShot(this GameObject gameObject, AudioClip clip)
        {
            if (gameObject.TryGetComponent<AudioSource>(out var audio))
            {
                audio.PlayOneShot(clip);
            }
        }

        #endregion
        
        public static void PlaySafe(this GameObject gameObject, bool value)
        {
            if (gameObject.TryGetComponent<ParticleSystem>(out var particle))
            {
                particle.PlaySafe(value);
            }

            if (gameObject.TryGetComponent<AudioSource>(out var audio))
            {
                audio.PlaySafe(value);
            }
        }

        public static void PlaySafe(this GameObject gameObject)
        {
            gameObject.PlaySafe(true);
        }

        public static void StopSafe(this GameObject gameObject)
        {
            gameObject.PlaySafe(false);
        }
    }
}