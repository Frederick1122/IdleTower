using Configs;
using UnityEngine;

namespace Core
{
    public static class BaseDataHandler
    {
        private const string COINS = "COINS";
        
        public static void SaveCoins(float coins) => SaveFloat(COINS, coins);

        public static float GetCoins() => GetFloat(COINS);

        public static void SaveUpgrade(int level, UpgradesType type) => SaveInt(type.ToString(), level);

        public static int GetUpgrade(UpgradesType type) => GetInt(type.ToString());

        private static int GetInt(string key)
        {
            if (PlayerPrefs.HasKey(key))
                return PlayerPrefs.GetInt(key);
            
            PlayerPrefs.SetInt(key, 1);
            PlayerPrefs.Save();
            return 1;
        }

        private static void SaveInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }
        
        private static float GetFloat(string key)
        {
            if (PlayerPrefs.HasKey(key))
                return PlayerPrefs.GetFloat(key);
            
            PlayerPrefs.SetFloat(key, 0f);
            PlayerPrefs.Save();
            return 0f;
        }

        private static void SaveFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
            PlayerPrefs.Save();
        }
    }
}