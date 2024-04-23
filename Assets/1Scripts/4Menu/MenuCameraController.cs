using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraController : MonoBehaviour
{
    [SerializeField] private CameraStageSlot[] _cameraSlots;


    private void Start()
    {
        EventsProvider.OnGameStateChange += MoveCamera;
    }

    private void MoveCamera(Enums.GameStage stage)
    {
        foreach(CameraStageSlot slot in _cameraSlots)
        {
            if(slot.stage != stage)
            {
                slot.camera.SetActive(false);
            }
            else
            {
                slot.camera.SetActive(true);
            }
        }
    }

    private void OnDestroy()
    {
        EventsProvider.OnGameStateChange -= MoveCamera;
    }

}

[System.Serializable]
public class CameraStageSlot
{
    public GameObject camera;
    public Enums.GameStage stage;
}
