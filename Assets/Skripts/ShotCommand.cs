using System.Collections;
using UnityEngine;

[RequireComponent(typeof (Rigidbody))]

public class ShotCommand : MonoBehaviour, ICommand
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float damage = 4f;

    private Unit unit;

    private Vector3 shotPoint = Vector3.zero;
    private bool isShotDirectionSet = false;

    private void Start()
    {
        unit = GetComponent<Unit>();
    }

    private void LateUpdate()
    {
        if(unit.isSelected && Input.GetMouseButtonDown(1))
        {
            SetShotDirection();
        }
    }

    private void SetShotDirection()
    {
        shotPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        shotPoint.z = unit.transform.position.z;
        isShotDirectionSet = true;

        LineDrawer.DrawLine(lineRenderer, unit.transform.position, shotPoint);
    }

    public void Execute()
    {
        if (!isShotDirectionSet)
        {
            return;
        }

        Vector3 unitPos = unit.transform.position;
        Vector3 direction = shotPoint - unitPos;

        if (!Physics.Raycast(unitPos, direction, out RaycastHit hit))
        {
            return;
        }

        if (hit.point != null)
        {
            LineDrawer.DrawLine(lineRenderer, unitPos, hit.point);
        }
        else
        {
            LineDrawer.DrawLine(lineRenderer, unitPos, unitPos + direction.normalized * 50);
        }

        Unit hittedUnit;
        if(hit.transform.TryGetComponent<Unit>(out hittedUnit))
        {
            hittedUnit.GetDamage(damage);
            Debug.LogWarning(hittedUnit.name);
        }

        isShotDirectionSet = false;
        StartCoroutine(WaitSomeTimeAndClearLines());
    }

    private IEnumerator WaitSomeTimeAndClearLines()
    {
        yield return new WaitForSeconds(4);
        LineDrawer.ClearLines(lineRenderer);
    }

}
