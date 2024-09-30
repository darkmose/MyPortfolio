using Core.Buildings;
using Core.GameLogic;
using Core.UI;
using Core.Units;
using System.Collections.Generic;
using UnityEngine;

public class TargetingService : MonoBehaviour
{
    [SerializeField] private Transform _root;
    [SerializeField] private UITargetArrow _arrowPrefab;
    [SerializeField] private UITargetSquare _squarePrefab;
    [SerializeField] private Color _allyColor;
    [SerializeField] private Color _enemyColor;
    private bool _enabled;
    private Camera _camera;
    private Dictionary<IUnit, UITargetArrow> _unitTargetsDict;
    private Dictionary<IUnit, UITargetSquare> _targetSquaresDict;
    private Dictionary<IBuilding, UITargetArrow> _buildingTargetsDict;
    private Dictionary<IBuilding, UITargetSquare> _buildingSquaresDict;

    private void ClearTargetingService()
    {
        _unitTargetsDict.Clear();
        _targetSquaresDict.Clear();
        _buildingTargetsDict.Clear();
        _buildingSquaresDict.Clear();
        _root.ClearAllChild();
    }

    public void InitTargets(List<IUnit> units, List<IBuilding> buildings)
    {
        PrepareDictionary();
        ClearTargetingService();
        PrepareCamera();

        foreach (IUnit unit in units)
        {
            var arrowInstance = Instantiate<UITargetArrow>(_arrowPrefab, _root, false);
            var squareInstance = Instantiate<UITargetSquare>(_squarePrefab, _root, false);

            arrowInstance.SetColor(unit.UnitFraction == UnitFraction.Ally ? _allyColor : _enemyColor);
            squareInstance.SetColor(unit.UnitFraction == UnitFraction.Ally ? _allyColor : _enemyColor);

            _unitTargetsDict.Add(unit, arrowInstance);
            _targetSquaresDict.Add(unit, squareInstance);

            arrowInstance.gameObject.SetActive(false);
            squareInstance.gameObject.SetActive(false);

            unit.ObjectDestroyed.AddListener(OnTargetDestroyed);
            unit.ObjectDamaged.AddListener(OnTargetDamaged);
        }

        InitTargetBuildings(buildings);
    }

    private void InitTargetBuildings(List<IBuilding> buildings) 
    {
        if (buildings == null || buildings.Count == 0)
        {
            return;
        }
        foreach (IBuilding building in buildings)
        {
            var arrowInstance = Instantiate<UITargetArrow>(_arrowPrefab, _root, false);
            var squareInstance = Instantiate<UITargetSquare>(_squarePrefab, _root, false);

            arrowInstance.SetColor(building.UnitFraction == UnitFraction.Ally ? _allyColor : _enemyColor);
            squareInstance.SetColor(building.UnitFraction == UnitFraction.Ally ? _allyColor : _enemyColor);

            _buildingTargetsDict.Add(building, arrowInstance);
            _buildingSquaresDict.Add(building, squareInstance);

            arrowInstance.gameObject.SetActive(false);
            squareInstance.gameObject.SetActive(false);

            building.ObjectDestroyed.AddListener(OnTargetDestroyed);
            building.ObjectDamaged.AddListener(OnTargetDamaged);
        }
    }

    private void OnTargetDamaged(IDamagableObject damagable)
    {
        UITargetSquare square = null;

        if (damagable is IUnit unit)
        {
            square = _targetSquaresDict[unit];
        }
        else if (damagable is IBuilding building)
        {
            square = _buildingSquaresDict[building];
        }

        float normalizedHealth = damagable.NormalizedHealth.Value;
        if (normalizedHealth < 1f)
        {
            square.SetActiveHealth(true);
            square.SetHealthNormalized(normalizedHealth);
        }
    }

    private void OnTargetDestroyed(IDamagableObject damagable)
    {
        damagable.ObjectDestroyed.RemoveListener(OnTargetDestroyed);

        if (damagable is IUnit unit)
        {
            var targetArrow = _unitTargetsDict[unit];
            var targetSquare = _targetSquaresDict[unit];

            targetArrow.gameObject.SetActive(false);
            targetSquare.gameObject.SetActive(false);

            _unitTargetsDict.Remove(unit);
            _targetSquaresDict.Remove(unit);
        }
        else if (damagable is IBuilding building)
        {
            var targetArrow = _buildingTargetsDict[building];
            var targetSquare = _buildingSquaresDict[building];

            targetArrow.gameObject.SetActive(false);
            targetSquare.gameObject.SetActive(false);

            _buildingTargetsDict.Remove(building);
            _buildingSquaresDict.Remove(building);
        }
    }

    private void PrepareDictionary()
    {
        if (_targetSquaresDict == null)
        {
            _targetSquaresDict = new Dictionary<IUnit, UITargetSquare>();
        }
        if (_unitTargetsDict == null)
        {
            _unitTargetsDict = new Dictionary<IUnit, UITargetArrow>();
        }
        if (_buildingTargetsDict == null)
        {
            _buildingTargetsDict = new Dictionary<IBuilding, UITargetArrow>();
        }
        if (_buildingSquaresDict == null)
        {
            _buildingSquaresDict = new Dictionary<IBuilding, UITargetSquare>();
        }
    }

    private void PrepareCamera()
    {
        _camera = Camera.main;  
    }

    public void EnableTargeting()
    {
        _enabled = true;
    }

    public void DisableTargeting() 
    {
        _enabled = false;
    }

    private Vector3 CalculateScreenEdgePosition(Vector2 targetScreenPos)
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        targetScreenPos.x = Mathf.Clamp(targetScreenPos.x, 32, screenWidth - 32);
        targetScreenPos.y = Mathf.Clamp(targetScreenPos.y, 32, screenHeight - 64);

        return targetScreenPos;
    }

    private void UpdateTargetOfUnit(IUnit unit)
    {
        var arrowTarget = _unitTargetsDict[unit];
        var squareTarget = _targetSquaresDict[unit];
        var isVisible = unit.UnitView.UnitSystems.Renderer.isVisible;
        UpdateTarget(unit.UnitView.transform, arrowTarget, squareTarget, isVisible);
    }

    private void UpdateTargetOfBuilding(IBuilding building)
    {
        var arrowTarget = _buildingTargetsDict[building];
        var squareTarget = _buildingSquaresDict[building];
        var isVisible = building.BuildingView.BuildingSystems.Renderer.isVisible;
        UpdateTarget(building.BuildingView.transform, arrowTarget, squareTarget, isVisible);
    }

    private void UpdateTarget(Transform objTransform, UITargetArrow uITargetArrow, UITargetSquare uITargetSquare, bool isVisibleInCamera)
    {
        if (!isVisibleInCamera)
        {
            uITargetSquare.gameObject.SetActive(false);
            uITargetArrow.gameObject.SetActive(true);

            var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            var upVector = Vector2.up;
            var targetPos = (Vector2)_camera.WorldToScreenPoint(objTransform.position);
            var direction = (targetPos - screenCenter).normalized;
            var d_angle = Vector3.SignedAngle(upVector, direction, -Vector3.forward);
            Vector3 screenPosition = CalculateScreenEdgePosition(targetPos);

            uITargetArrow.Rect.position = screenPosition;
            uITargetArrow.Rect.rotation = Quaternion.Euler(0, 0, -d_angle);
        }
        else
        {
            uITargetSquare.gameObject.SetActive(true);
            uITargetArrow.gameObject.SetActive(false);

            var unitPos = objTransform.position;
            uITargetSquare.Rect.position = _camera.WorldToScreenPoint(unitPos);
        }
    }

    private void FixedUpdate()
    {
        if (_enabled) 
        {
            foreach (var unit in _unitTargetsDict.Keys)
            {
                UpdateTargetOfUnit(unit);
            }
            foreach (var building in _buildingTargetsDict.Keys)
            {
                UpdateTargetOfBuilding(building);
            }
        }
    }
}
