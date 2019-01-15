using GameCore.Enums;

namespace GameCore.Model
{
    public class BacteriumModel
    {
        public BacteriumModel(BacteriumData bacteriumData) : this(bacteriumData.Id, bacteriumData.Owner, bacteriumData.Transform, bacteriumData.VirusCount) {}
        public BacteriumModel(int id, OwnerType owner, Transform transform, int virusCount)
        {
            Id = id;
            Owner = owner;
            Transform = transform;
            VirusCount = virusCount;
        }

        public int Id { get; private set; }
        public Transform Transform { get; private set; }
        public OwnerType Owner { get; set; }
        public int VirusCount { get; set; }

        public BacteriumData Data => new BacteriumData(Id, Owner, Transform, VirusCount);
    }
}