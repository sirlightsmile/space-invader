using UnityEngine;

namespace SmileProject.SpaceInvader.Config
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ScriptableObjects/GameConfig", order = 1)]
    public class GameConfig : ScriptableObject
    {
        public int TotalTime { get { return _totalTime; } }
        public int ExtraBonusScore { get { return _extraBonusScore; } }
        public int ShieldDurability { get { return _shieldDurability; } }

        public PlayerConfig PlayerConfig { get { return _playerConfig; } }
        public EnemyConfig EnemyConfig { get { return _enemyConfig; } }

        [Header("Game")]
        [SerializeField]
        [Tooltip("Total game time")]
        private int _totalTime;

        [SerializeField]
        [Tooltip("Extra bonus score rate that will get if clear before time up")]
        private int _extraBonusScore;

        [Header("Shield")]
        [SerializeField]
        [Tooltip("Shield durability in battle scene")]
        private int _shieldDurability;

        [Header("Player")]
        [SerializeField]
        private PlayerConfig _playerConfig;

        [Header("Enemy")]
        [SerializeField]
        private EnemyConfig _enemyConfig;
    }

    #region Player
    [System.Serializable]
    public class PlayerConfig
    {
        public string PlayerSpaceshipID { get { return _playerSpaceshipId; } }
        public int PlayerFireRate { get { return _playerFireRate; } }

        [Tooltip("Player initial spaceship id")]
        [SerializeField]
        private string _playerSpaceshipId;
        [SerializeField]
        [Tooltip("Player fire rate per second")]
        private int _playerFireRate;
    }
    #endregion

    #region Enemy
    [System.Serializable]
    public class EnemyConfig
    {
        public float MovementSpeed { get { return _movementSpeed; } }
        public float RandomShootChance { get { return _randomShootChance; } }
        public float TriggerShootInterval { get { return _triggerShootInterval; } }
        public float ShootAsyncInterval { get { return _shootAsyncInterval; } }

        [SerializeField]
        [Tooltip("Enemies movement speed")]
        private float _movementSpeed;

        [SerializeField]
        [Tooltip("Rate that enemy will shoot in percent")]
        [Range(0.0f, 1.0f)]
        private float _randomShootChance;

        [SerializeField]
        [Tooltip("Time interval for trigger random spaceships shoot (seconds)")]
        private float _triggerShootInterval;

        [SerializeField]
        [Tooltip("Random time before release bullet")]
        private float _shootAsyncInterval;
    }
    #endregion
}
