using UnityEngine;

namespace Data
{
    public class Coins : ResourceObject
    {
        public override int DefaultValue => Resources.Load<GameSettings>("Settings/GameSetting").startCoins;

        public override bool Consumes(int amount)
        {
            return base.Consumes(amount);
        }

        public override void ResetResource()
        {
            
        }
    }
}