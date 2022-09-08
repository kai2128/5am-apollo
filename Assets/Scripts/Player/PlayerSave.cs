using UnityEngine;

namespace Player
{
    [System.Serializable]
    public class PlayerSave
    {
        [System.Serializable]
        public struct Data
        {
            public int level;
            public float currentXP;
            public bool unlockedSword;
            public bool unlockedFly;
            public bool unlockedGrowShrink;
        }

        public Data data;

        public string ToJson()
        {
            return JsonUtility.ToJson(this);
        }

        public static PlayerSave LoadFromJson(string json)
        {
            var playerSave = new PlayerSave();
            JsonUtility.FromJsonOverwrite(json, playerSave);
            return playerSave;
        }
    }
}