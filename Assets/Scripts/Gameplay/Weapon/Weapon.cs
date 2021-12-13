using System.Threading.Tasks;
using SmileProject.Generic.Audio;
using SmileProject.SpaceInvader.GameData;
using SmileProject.SpaceInvader.Gameplay;
using UnityEngine;

namespace SmileProject.SpaceInvader.Weapon
{
    public abstract class Weapon<T> where T : WeaponModel
    {
        private const int WeaponInitialLevel = 1;
        protected Transform AttackPoint { get { return _attackPointTransform; } }
        protected T Model { get { return _model; } }
        protected int Damage { get { return _damage; } }
        protected float AttackSpeed { get { return _attackSpeed; } }

        private int _level;
        private int _maxLevel;
        private int _damage;
        private float _attackSpeed;
        private Transform _attackPointTransform;
        private T _model;

        #region Sounds
        private AudioManager _audioManager;
        private SoundKeys _attackSound;
        #endregion

        public Weapon(T model)
        {
            _model = model;
            _level = WeaponInitialLevel;
            _maxLevel = model.MaxLevel;
            _damage = model.BaseDamage;
            _attackSpeed = model.BaseSpeed;
        }

        /// <summary>
        /// Set attack initiate point
        /// /// </summary>
        /// <param name="transform">Transform which represent to attack initiate point</param>
        public void SetAttackPointTransform(Transform transform)
        {
            _attackPointTransform = transform;
        }

        /// <summary>
        /// Invoke weapon attack
        /// </summary>
        /// <param name="attacker"></param>
        public abstract void Attack(Spaceship attacker);

        /// <summary>
        /// Prepare weapon for use. Setting up or loading required assets
        /// </summary>
        public abstract Task Setup();

        /// <summary>
        /// Level up weapon then automatic update status
        /// </summary>
        /// <param name="addLevel">adding level</param>
        public void LevelUp(int addLevel = 1)
        {
            int addedLevel = _level + addLevel;
            int targetLevel = Mathf.Clamp(addedLevel, _level, _maxLevel);
            if (_level != targetLevel)
            {
                _level = targetLevel;
                UpdateStatus();
            }
        }

        /// <summary>
        /// Inject sound system to this weapon
        /// </summary>
        /// <param name="audioManager">Audio manager reference</param>
        /// <param name="attackSound">Attack sound key</param>
        public void SetSound(AudioManager audioManager, SoundKeys attackSound)
        {
            _audioManager = audioManager;
            _attackSound = attackSound;
        }

        protected async Task PlayAttackSound()
        {
            if (_audioManager != null)
            {
                await _audioManager.PlaySound(_attackSound);
            }
        }


        /// <summary>
        /// Update status match with current level
        /// </summary>
        private void UpdateStatus()
        {
            int currentLevel = _level;
            int newDamage = _model.BaseDamage + (currentLevel * _model.DamageIncrement);
            int newAttackSpeed = _model.BaseSpeed + (currentLevel * _model.SpeedIncrement);
            _damage = newDamage;
            _attackSpeed = newAttackSpeed;
        }
    }
}