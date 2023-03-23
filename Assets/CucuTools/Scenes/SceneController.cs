using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CucuTools.Attributes;
using UnityEngine;

namespace CucuTools.Scenes
{
    /// <summary>
    /// Base behaviour for scene controller
    /// </summary>
    public abstract class SceneController : CucuBehaviour
    {
        private CucuArgumentManager CucuArgumentManager => CucuArgumentManager.Instance;

        /// <summary>
        /// Fill fields with <see cref="InjectArgAttribute"/> into object
        /// </summary>
        /// <param name="obj">Target</param>
        /// <param name="poolArgs">Pool of arguments</param>
        private static void InjectArgs(object obj, IReadOnlyCollection<CucuArg> poolArgs)
        {
            if (obj == null) return;

            if (poolArgs == null) return;
            
            // get type of target
            var targetType = obj.GetType();
            
            // get all instance fields
            var fields = targetType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            // get fields with interested attribute
            var argFields = fields.Where(f => f.GetCustomAttribute<InjectArgAttribute>() != null).ToArray();

            // for each field
            foreach (var argField in argFields)
            {
                // get field type
                var fieldType = argField.FieldType;

                // if field type array
                if (fieldType.IsArray)
                {
                    var elementType = fieldType.GetElementType();
                    
                    /*
                    // reset isLocal = true
                    var val = argField.GetValue(obj);
                    if (val != null)
                    {
                        var cucuArgs = val as CucuArg[];
                        foreach (var cucuArg in cucuArgs)
                            cucuArg.IsDefault = true;
                    }
                    */
                    
                    // search in pool args with interested type
                    var args = poolArgs.Where(ca => ca != null && ca.GetType() == elementType).ToArray();

                    if ((args?.Length ?? 0) > 0)
                    {
                        foreach (var arg in args) arg.IsDefault = false;
                        
                        // create array
                        var array = Array.CreateInstance(elementType, args.Length);

                        // fill array
                        Array.Copy(args, array, args.Length);

                        // set array
                        argField.SetValue(obj, array);
                    }
                }
                else
                {
                    /*
                    // reset isLocal = true 
                    var val = argField.GetValue(obj);
                    if (val != null && val is CucuArg cucuArg)
                        cucuArg.IsDefault = true;
                    */
                    
                    // search in pool arg with interested type
                    var arg = poolArgs.FirstOrDefault(ca => ca != null && ca.GetType() == argField.FieldType);
                    
                    if (arg != null)
                    {
                        arg.IsDefault = false;
                        
                        // set field
                        argField.SetValue(obj, Convert.ChangeType(arg, argField.FieldType));
                    }
                }
            }
        }
        
        private void Inject()
        {
            try
            {
                InjectArgs(this, CucuArgumentManager.args);
            }
            catch (Exception exc)
            {
                Debug.LogError($"Injection failed :: {exc}");
            }
        }
        
        protected virtual void OnAwake()
        {
        }

        private void Awake()
        {
            Inject();
            
            OnAwake();
        }
    }
}