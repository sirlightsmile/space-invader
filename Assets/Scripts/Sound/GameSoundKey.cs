using System.IO;
using SmileProject.Generic.Audio;
using SmileProject.Generic.Utilities;

namespace SmileProject.SpaceInvader.Sounds
{
    public class MixerGroup : StringEnum<MixerGroup>
    {
        public const string MAIN_MIXER_KEY = "SoundMixer";
        public MixerGroup(string value) : base(value)
        {
        }

        public static readonly MixerGroup BGM = new MixerGroup("BGM");
        public static readonly MixerGroup SoundEffect = new MixerGroup("SoundEffect");
    }

    public sealed class GameSoundKeys : SoundKeys
    {
        private const string ASSET_PATH = "GameplaySounds/";
        public GameSoundKeys(string value, string assetKey, string mixerKey) : base(value, assetKey, mixerKey)
        {
        }

        public override string GetAssetKey()
        {
            string assetKey = base.GetAssetKey();
            string fullPath = Path.Combine(ASSET_PATH, assetKey);
            return fullPath;
        }

        #region BGM
        public static readonly GameSoundKeys GameplayBGM = new GameSoundKeys(nameof(GameplayBGM), "GameplayBGM.mp3", MixerGroup.BGM.ToString());
        #endregion

        #region Sound effects
        public static readonly GameSoundKeys DiveBomb = new GameSoundKeys(nameof(DiveBomb), "DiveBomb.wav", MixerGroup.SoundEffect.ToString());
        public static readonly GameSoundKeys Explosion = new GameSoundKeys(nameof(Explosion), "Explosion.wav", MixerGroup.SoundEffect.ToString());
        public static readonly GameSoundKeys Failed = new GameSoundKeys(nameof(Failed), "Failed.mp3", MixerGroup.SoundEffect.ToString());
        public static readonly GameSoundKeys Shoot = new GameSoundKeys(nameof(Shoot), "LaserShoot.wav", MixerGroup.SoundEffect.ToString());
        public static readonly GameSoundKeys PlayerExplosion = new GameSoundKeys(nameof(PlayerExplosion), "PlayerExplosion.wav", MixerGroup.SoundEffect.ToString());
        public static readonly GameSoundKeys PowerUp = new GameSoundKeys(nameof(PowerUp), "PowerUp.mp3", MixerGroup.SoundEffect.ToString());
        public static readonly GameSoundKeys Succeed = new GameSoundKeys(nameof(Succeed), "Succeed.mp3", MixerGroup.SoundEffect.ToString());
        public static readonly GameSoundKeys Hit = new GameSoundKeys(nameof(Hit), "Hit.wav", MixerGroup.SoundEffect.ToString());
        public static readonly GameSoundKeys Drone = new GameSoundKeys(nameof(Drone), "Drone.wav", MixerGroup.SoundEffect.ToString());
        public static readonly GameSoundKeys Impact = new GameSoundKeys(nameof(Impact), "Impact.wav", MixerGroup.SoundEffect.ToString());
        #endregion
    }
}
