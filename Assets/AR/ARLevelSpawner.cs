using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace Assets.AR
{
    public class ARLevelSpawner : MonoBehaviour
    {
        [SerializeField] private ARPlaneManager _arPlaneManager;
        [SerializeField] private Transform _placeTargetPrefab;

        [SerializeField] private GameObject _startGameText;
        [SerializeField] private GameObject _selectPlaneText;

        [SerializeField] private GameObject _inGameUI;
        [SerializeField] private GameObject _menuUI;

        [SerializeField] private GameObject _levelPrefab;

        private Vector2 _screenCenter;
        private Camera _camera;
        private Transform _target;
        private Transform _arPlaneTransform;

        private void Start()
        {
            _camera = Camera.main;
            _screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

            _startGameText.SetActive(false);
            _selectPlaneText.SetActive(true);
        }

        private void Update()
        {
            Debug.Log($"[AR] Tracked planes count: {_arPlaneManager.trackables.count}");
            if (_arPlaneManager.trackables.count <= 0) return;

            var ray = _camera.ScreenPointToRay(_screenCenter);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.transform.TryGetComponent(out ARPlane arPlane))
                {
                    AdjustPlacementTarget(hit, arPlane);
                }
            }

            if (_target != null && (Input.GetMouseButtonDown(0) || IfTouchedScreen()))
            {
                PlaceLevel();
                HidePlanes();
                StartGame();
            }
        }

        private bool IfTouchedScreen()
        {
            if (Input.touchCount <= 0) return false;
            return Input.GetTouch(0).phase == TouchPhase.Began;
        }

        private void AdjustPlacementTarget(RaycastHit hit, ARPlane arPlane)
        {
            _arPlaneTransform = arPlane.transform;
            _target ??= Instantiate(_placeTargetPrefab);
            _target.position = hit.point;
            _target.rotation = arPlane.transform.rotation;

            _startGameText.SetActive(true);
            _selectPlaneText.SetActive(false);
        }

        private void PlaceLevel()
        {
            _target.gameObject.SetActive(false);
            _startGameText.SetActive(false);

            _levelPrefab.transform.position = _arPlaneTransform.position;
            _levelPrefab.transform.rotation = _arPlaneTransform.rotation;

            _levelPrefab.SetActive(true);

            Debug.Log($"[AR] Level is placed on plane: {_arPlaneTransform.name}");
        }

        private void StartGame()
        {
            _inGameUI.SetActive(true);
            _menuUI.SetActive(true);
        }

        private void HidePlanes()
        {
            var trackableObjects = _arPlaneManager.trackables;
            if (trackableObjects.count <= 0) 
            { 
                return; 
            }
            foreach(var trackable in trackableObjects)
            {
                trackable.gameObject.SetActive(false);
            }

            enabled = false;
        }
    }
}
