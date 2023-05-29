using System.Linq;
using Helpers;

namespace Game.Objectives
{
    public class ObjectiveCompound : ObjectiveBase
    {
        public string DisplayName;
        public ObjectiveBase[] SubObjectives;

        public override bool IsComplete => SubObjectives.All(obj => obj.IsComplete);

        public override string GenerateText()
        {
            var name = IsComplete ? DisplayName.Green() : DisplayName.Red();
            return name + "\n\t" + string.Join("\n\t", SubObjectives.Select(obj => obj.GenerateText()));
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
