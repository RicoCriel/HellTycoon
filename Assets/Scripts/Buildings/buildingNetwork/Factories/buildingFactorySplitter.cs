namespace Buildings
{
    public class BuildingFactorySplitter : BuildingFactoryBase
    {
        private int _currentSplitterInt = 0;

        private int _maxAmountOfExitBoxes;

        protected override void ExecuteMachineProcessingBehaviour()
        {
            base.ExecuteMachineProcessingBehaviour();
        }
        protected override void PlayProcessingAnimation()
        {
            base.PlayProcessingAnimation();
        }
        protected override void ExecuteMachineSpawningBehaviour()
        {
            int maxAmountOfExitBoxes = _exitBoxes.Count;

            if (_exitBoxes[_currentSplitterInt].Spline != null)
            {
                SpawnDemon(_exitBoxes[_currentSplitterInt]);
            }
            IncreaseSplitterInt(maxAmountOfExitBoxes);
        }
        private void IncreaseSplitterInt(int maxAmountOfExitBoxes)
        {
            _currentSplitterInt++;
            if (_currentSplitterInt >= maxAmountOfExitBoxes)
            {
                _currentSplitterInt = 0;
            }
        }
    }
}
