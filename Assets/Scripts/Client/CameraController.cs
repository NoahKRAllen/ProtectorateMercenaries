using Unity.Cinemachine;
using Unity.Entities;
using UnityEngine;

namespace TMG.NFE_Tutorial
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera cinemachineCamera;
        
        [Header("Move Settings")]
        [SerializeField] private bool drawBounds;
        [SerializeField] private Bounds cameraBounds;
        [SerializeField] private float camSpeed;
        [SerializeField] private Vector2 screenPercentageDetection;

        [Header("Zoom Settings")]
        [SerializeField] private float minZoomDistance;
        [SerializeField] private float maxZoomDistance;
        [SerializeField] private float zoomSpeed;

        [Header("Camera Start Positions")] 
        [SerializeField] private Vector3 redTeamPosition = new(120f, 0f, 120f);
        [SerializeField] private Vector3 blueTeamPosition = new(-120f, 0f, -120f);
        [SerializeField] private Vector3 spectatorPosition = new(0f, 0f, 0f);
        
        private Vector2 _normalScreenPercentage;
        private Vector2 NormalMousePos => new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        private bool InScreenLeft => NormalMousePos.x < _normalScreenPercentage.x  && Application.isFocused;
        private bool InScreenRight => NormalMousePos.x > 1 - _normalScreenPercentage.x  && Application.isFocused;
        private bool InScreenTop => NormalMousePos.y < _normalScreenPercentage.y  && Application.isFocused;
        private bool InScreenBottom => NormalMousePos.y > 1 - _normalScreenPercentage.y  && Application.isFocused;

        private CinemachinePositionComposer _positionComposer;
        private EntityManager _entityManager;
        private EntityQuery _teamControllerQuery;
        private EntityQuery _localChampQuery;
        private bool _cameraSet;
        
        private void Awake()
        {
            _normalScreenPercentage = screenPercentageDetection * 0.01f;
            _positionComposer = cinemachineCamera.GetComponent<CinemachinePositionComposer>();

            if (!_positionComposer)
            {
                Debug.Log("First attempt to get Position Composer failed. Trying to get it from children.");
                _positionComposer = cinemachineCamera.GetComponentInChildren<CinemachinePositionComposer>();
            }
            
        }

        /*private void Start()
        {
            if (World.DefaultGameObjectInjectionWorld == null) return;
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            _teamControllerQuery = _entityManager.CreateEntityQuery(typeof(ClientTeamRequest));
            _localChampQuery = _entityManager.CreateEntityQuery(typeof(OwnerChampTag));

            // Move the camera to the base corresponding to the team the player is on.
            // Spectators' cameras will start in the center of the map
            if (_teamControllerQuery.TryGetSingleton<ClientTeamRequest>(out var requestedTeam))
            {
                var team = requestedTeam.Value;
                var cameraPosition = team switch
                {
                    TeamType.Blue => _blueTeamPosition,
                    TeamType.Red => _redTeamPosition,
                    _ => _spectatorPosition
                };
                transform.position = cameraPosition;

                if (team != TeamType.AutoAssign)
                {
                    _cameraSet = true;
                }
            }
        }*/

        private void OnValidate()
        {
            _normalScreenPercentage = screenPercentageDetection * 0.01f;
        }

        private void Update()
        {
            // SetCameraForAutoAssignTeam();
            MoveCamera();
            ZoomCamera();
        }

        private void MoveCamera()
        {
            if (InScreenLeft)
            {
                transform.position += Vector3.left * (camSpeed * Time.deltaTime);
            }

            if (InScreenRight)
            {
                transform.position += Vector3.right * (camSpeed * Time.deltaTime);
            }

            if (InScreenTop)
            {
                transform.position += Vector3.back * (camSpeed * Time.deltaTime);
            }

            if (InScreenBottom)
            {
                transform.position += Vector3.forward * (camSpeed * Time.deltaTime);
            }
            
            if (!cameraBounds.Contains(transform.position))
            {
                transform.position = cameraBounds.ClosestPoint(transform.position);
            }
        }

        private void ZoomCamera()
        {
            if (Mathf.Abs(Input.mouseScrollDelta.y) > float.Epsilon)
            {
                _positionComposer.CameraDistance -= Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime;
                _positionComposer.CameraDistance =
                    Mathf.Clamp(_positionComposer.CameraDistance, minZoomDistance, maxZoomDistance);
            }
        }

        /*private void SetCameraForAutoAssignTeam()
        {
            if (!_cameraSet)
            {
                if (_localChampQuery.TryGetSingletonEntity<OwnerChampTag>(out var localChamp))
                {
                    var team = _entityManager.GetComponentData<MobaTeam>(localChamp).Value;
                    var cameraPosition = team switch
                    {
                        TeamType.Blue => _blueTeamPosition,
                        TeamType.Red => _redTeamPosition,
                        _ => _spectatorPosition
                    };
                    transform.position = cameraPosition;
                    _cameraSet = true;
                }
            }
        }*/

        private void OnDrawGizmos()
        {
            if (!drawBounds) return;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(cameraBounds.center, cameraBounds.size);
        }
    }
}