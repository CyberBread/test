using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class JumpCommand : MonoBehaviour, ICommand
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float maxDistance = 5f;
    [SerializeField] float maxSliderLength = 5f;

    private Unit unit;
    private Rigidbody rigidbody;

    private Vector3 jumpDirection = Vector3.zero;
    private Vector3 startingJumpPoint = Vector3.zero;
    private bool isJumped = false;
    private bool isJumpDirectionSet = false;

    private void Start()
    {
        unit = GetComponent<Unit>();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (isJumped)
        {
            CheckJumpDistance();
        }
    }

    private void LateUpdate()
    {
        if (unit.isSelected && Input.GetMouseButton(0))
        {
            Vector3 mousePos = MousePositionToWorldPoint();
            DrawSlider(mousePos);
            SetJumpDirecton(mousePos);
        }
    }

    public void Execute()
    {
        if (!isJumpDirectionSet)
        {
            return;
        }

        startingJumpPoint = unit.transform.position;
        rigidbody.useGravity = true;
        rigidbody.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
        isJumped = true;
        isJumpDirectionSet = false;

        LineDrawer.ClearLines(lineRenderer);
    }

    private void DrawSlider(Vector3 mousePos)
    {
        Vector3 unitPos = unit.transform.position;
        Vector3 slider = mousePos - unitPos;
        if (slider.magnitude > maxSliderLength)
        {
            slider.Normalize();
            slider *= maxSliderLength;
        }
        LineDrawer.DrawLine(lineRenderer, unitPos, unitPos + slider, 0.5f, 0.05f);
    }

    private void SetJumpDirecton(Vector3 mousePos)
    {
        jumpDirection = unit.transform.position - mousePos;
        if(jumpDirection.magnitude < 2)
        {
            jumpDirection = Vector3.zero;
            isJumpDirectionSet = false;
        }
        else
        {
            jumpDirection.Normalize();
            isJumpDirectionSet = true;
        } 
    }

    private Vector3 MousePositionToWorldPoint()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = unit.transform.position.z;
        return mousePos;
    }

    private void CheckJumpDistance()
    {
        Vector3 jumpedDistance = unit.transform.position - startingJumpPoint;

        if(jumpedDistance.magnitude > maxDistance)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.useGravity = false;
            isJumped = false;
        }
    }
}
