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

        public override bool IsComplete => _isComplete;
        private bool _isComplete;

        public override string GenerateText()
        {
            return IsComplete ? $"{Target.DisplayName} collected".Green() : $"Collect {Target.DisplayName}".Red();
        }

        private void _onCollected()
        {
            _isComplete = true;
            RaiseStateUpdated();
        }
    }
}
