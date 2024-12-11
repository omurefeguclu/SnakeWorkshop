using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SnakeWorkshop.Scripts.Helpers
{
    public class RuntimeCloningHelper<T> where T : Component
    {
        private readonly T _original;
        private readonly Action<T> _setupAction;
        private readonly List<T> _objects = new List<T>();

        private bool _initialized;
        
        public RuntimeCloningHelper(T original, Action<T> setupAction = null)
        {
            _original = original;
            _setupAction = setupAction;
        }
        
        private void Initialize()
        {
            // Destroy other objects than original
            foreach (Transform obj in _original.transform.parent)
            {
                if (obj == _original.transform)
                {
                    continue;
                }
                
                Object.Destroy(obj.gameObject);
            }
            
            // Add the original object to the list
            _objects.Add(_original);
            
            // Return the original object to pool
            _original.gameObject.SetActive(false);
 
            // Invoke the setup action if it is not null for the original object
            _setupAction?.Invoke(_original);
            
            // Set the initialization flag
            _initialized = true;
        }
        
        public List<T> Populate(int count)
        {
            // Initialize the helper if it is not initialized
            if (!_initialized)
            {
                Initialize();
            }
            
            
            while(count > _objects.Count)
            {
                // Complete the list with new objects
                var newObject = Object.Instantiate(_original, _original.transform.parent);
                
                // Invoke the setup action if it is not null
                _setupAction?.Invoke(newObject);
                
                // Add the new object to the list
                _objects.Add(newObject);
            }
            
            // Activate all the objects
            for (var i = 0; i < count; i++)
            {
                _objects[i].gameObject.SetActive(true);
            }
            
            // Deactivate the rest of the objects
            for (var i = count; i < _objects.Count; i++)
            {
                _objects[i].gameObject.SetActive(false);
            }

            // Return the used span of the list
            return _objects.GetRange(0, count);
        }
    }
}