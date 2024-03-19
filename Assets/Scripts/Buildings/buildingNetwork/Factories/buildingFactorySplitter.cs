namespace Buildings
{
    public class buildingFactorySplitter : BuildingFactoryBase
    {
        private int CurrentSplitterInt = 0;

        private int MaxAmountOfExitBoxes;

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
            int MaxAmountOfExitBoxes = _exitBoxes.Count;

            if (_exitBoxes[CurrentSplitterInt].Spline != null)
            {
                SpawnDemon(_exitBoxes[CurrentSplitterInt]);
            }
            IncreasesplitterInt(MaxAmountOfExitBoxes);
        }
        private void IncreasesplitterInt(int MaxAmountOfExitBoxes)
        {
            CurrentSplitterInt++;
            if (CurrentSplitterInt >= MaxAmountOfExitBoxes)
            {
                CurrentSplitterInt = 0;
            }
        }
    }
}
