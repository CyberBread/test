using UnityEngine;

public class UnitPicker : MonoBehaviour
{
    public Unit selectedUnit;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PickUnit();
        }
    }

    private void PickUnit()
    {
        UnPickUnit();

        Ray ray =Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            return;
        }

        Unit pickedUnit;
        if (hit.transform.TryGetComponent<Unit>(out pickedUnit))
        {
            if (pickedUnit.isMine)
            {
                pickedUnit.Select();
                selectedUnit = pickedUnit;
            }
        }
    }

    private void UnPickUnit()
    {
        if(selectedUnit != null)
        {
            if (selectedUnit.isMine)
            {
                selectedUnit.UnSelect();
                selectedUnit = null;
            }
        }
    }
}
