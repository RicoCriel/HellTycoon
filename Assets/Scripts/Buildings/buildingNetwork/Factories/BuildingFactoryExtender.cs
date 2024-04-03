namespace Buildings
{
    public class BuildingFactoryExtender: BuildingFactoryBase
    {
        

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
            SpawnDemon(_exitBoxes[0]);
        }
       
    }
}
