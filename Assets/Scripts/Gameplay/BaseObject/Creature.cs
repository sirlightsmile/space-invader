namespace SmileProject.SpaceInvader.Gameplay
{
    public abstract class Creature : SpaceObject
    {
        public int HP { get; private set; }

        public Creature SetHP(int hp)
        {
            HP = hp;
            return this;
        }
    }
}
