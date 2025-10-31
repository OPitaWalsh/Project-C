using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 15f;
    public float maxVelocityChange = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;
    public float extraGravity = 10f;

    private Vector2 input;
    private bool isSprinting;
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else
        {
            rb.AddForce(Vector3.down * extraGravity, ForceMode.Acceleration);
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(CalculateMovement(), ForceMode.VelocityChange);

        isGrounded = false;
    }

    Vector3 CalculateMovement()
    {
        float speedToUse = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 targetVelocity = transform.TransformDirection(new Vector3(input.x, 0f, input.y)) * speedToUse;
        Vector3 velocityChange = targetVelocity - rb.linearVelocity;

        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0f;

        if (input.magnitude < 0.5f)
        {
            return new Vector3(-rb.linearVelocity.x, 0, -rb.linearVelocity.z);
        }
        else
        {
            return velocityChange;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;

    }
}