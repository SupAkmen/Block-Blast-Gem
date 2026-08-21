using System;
using System.Threading.Tasks;
using UnityEngine;

    [Serializable]
    public class ResourceValue
    {
        
    }
    public abstract class ResourceObject : ScriptableObject
    {
        public ResourceValue value;

        // Name of the resource
        public string ResourceName => name;
        
        public abstract int DefaultValue { get; }
        
        // Value of the resource
        private int Resource;

        public AudioClip sound;

        public delegate void ResourceUpdate(int count);
        
        // Event for resource update
        public event ResourceUpdate OnResourceUpdate;

        private async Task OnEnable()
        {
            await Task.Delay(1000); 
            LoadPrefs();
        }

        public void LoadPrefs()
        {
            Resource = LoadResource();
        }
        
        public int LoadResource()
        {
            return PlayerPrefs.GetInt(ResourceName,DefaultValue);
        }

        // Add amount to resource and save to player prefs
        public void Add(int amount)
        {
            Resource += amount;
            PlayerPrefs.SetInt(ResourceName, Resource);
            PlayerPrefs.Save();
            OnResourceChanged();
        }
        
        //Set resource to amount and save to player prefs
        public void Set(int amount)
        {
            Resource = amount;
            PlayerPrefs.SetInt(ResourceName, Resource);
            PlayerPrefs.Save();
            OnResourceChanged();
        }
        
        //Consumes amount from resource and save to playerprefs if there is enough
        public virtual bool Consumes(int amount)
        {
            if (IsEnough(amount))
            {
                Resource -= amount;
                PlayerPrefs.SetInt(ResourceName, Resource);
                PlayerPrefs.Save();
                OnResourceChanged();
                return true;
            }
            
            return false;
        }


        // callback for ui elements
        private void OnResourceChanged()
        {
            OnResourceUpdate?.Invoke(Resource);
        }

        // get the resource
        public int GetValue()
        {
            return Resource;
        }

        // Check if the resource is enough
        public bool IsEnough(int targetAmount)
        {
            if (GetValue() < targetAmount)
            {
                Debug.Log("Not engough" + ResourceName);
            }
            
            return GetValue() >= targetAmount;
        }
        
        public abstract void ResetResource();
        
        
        
    }


