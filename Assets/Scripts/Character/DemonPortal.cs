using Buildings;
using Splines;
using UnityEngine;

public class DemonPortal : BuildingFactoryBase
{
    
    //[SerializeField] private Vector3 _exitRotation;
    //[SerializeField] private Vector3 _offset;
    public DemonPortal ExitPortal;
    //private bool _isColliding = false;

    protected override void ExecuteMachineProcessingBehaviour()
    {
        if (_unprocessedDemonContainer.Count > 0 && ContainerHasSpace(ExitPortal._processedDemonContainer))
        {
            ExitPortal.AddDemon(ExitPortal._processedDemonContainer, _unprocessedDemonContainer.Dequeue());
            PlayProcessingAnimation();
        }

    }
    protected override void PlayProcessingAnimation()
    {
        base.PlayProcessingAnimation();
    }
    //protected override void ExecuteMachineSpawningBehaviour()
    //{
    //    SpawnDemon(_exitBoxes[0]);
    //}


    //private void OnTriggerEnter(Collider other)
    //{
    //    _isColliding = true;
    //    if (other.CompareTag("Demon"))
    //    {
    //        other.transform.position = ExitPortal.position + _offset;
    //        other.GetComponent<DemonFear>().SetLayer();
    //    }


    //    else if (!other.CompareTag("Player"))
    //    { return; }
    //    else
    //    {
    //        other.GetComponent<CharacterController>().enabled = false;
    //        other.transform.position = ExitPortal.position + _offset;
    //        other.GetComponent<CharacterController>().enabled = true;
            

    //    }
         

  
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    _isColliding = false;
    //}

    //public bool IsColliding()
    //{
    //    return _isColliding;
    //}
}
