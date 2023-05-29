using System.Linq;

namespace Game.Objectives
{
    public class ObjectiveCompound : ObjectiveBase
    {
        public ObjectiveBase[] SubObjectives;

        public override bool IsComplete => SubObjectives.All(obj => obj.IsComplete);

        public override string GenerateText()
        {
            return string.Join("\n", SubObjectives.Select(obj => obj.GenerateText()));
        }

        void Start()
        {
            foreach (var obj in SubObjectives)
            {
                obj.StateUpdated += RaiseStateUpdated;
            }
        }
    }
}
