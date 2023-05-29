using Helpers;

namespace Game.Objectives
{
    public class ObjectiveBaseCollectObject : ObjectiveBase
    {
        public CollectableObject Target;

        void Start()
        {
            Target.Collected += _onCollected;
        }
        
        public override string GenerateText()
        {
            return IsComplete ? $"{Target.name} collected".Green() : $"Collect {Target.name}".Red();
        }

        private void _onCollected()
        {
            IsComplete = true;
            RaiseStateUpdated();
        }
    }
}
