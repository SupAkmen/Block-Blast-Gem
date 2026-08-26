using UnityEngine;

namespace Data
{
    [CreateAssetMenu (fileName = "ResourceData", menuName = "ResourceData", order = 1)]
    public class ResourceItem : ResourceObject
    {
        public int defaultValue;
        
        public override int DefaultValue => defaultValue;
        public override void ResetResource()
        {
            
        }
    }
}