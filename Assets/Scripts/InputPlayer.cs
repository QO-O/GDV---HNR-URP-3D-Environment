using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private InputActionAsset input;
   [SerializeField] private string actionMapName = "Player1";
   [SerializeField] private float walkSpeed = 5f;
  [SerializeField] private float turnSpeed = 150f;
   [SerializeField] private float jumpForce = 5f;
   

   private InputAction moveAction;
   private InputAction jumpAction;
   private InputAction sprintAction;
   
   private Rigidbody rb;
   private bool isGrounded = true;
   private InputActionMap map;

    private void Awake()
    {
        map = input.FindActionMap(actionMapName);
        moveAction   = map.FindAction("Move");
        jumpAction   = map.FindAction("Jump");
        sprintAction = map.FindAction("Sprint");
    }
    void OnEnable()
    {
        map.Enable();
    }
    void OnDisable()
    {
        map.Disable();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        float speed = walkSpeed * moveInput.y;

        if (sprintAction.IsPressed()) speed *= 2f;
        Vector3 movement = transform.forward * speed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        float angle = moveInput.x * turnSpeed * Time.deltaTime;
        transform.Rotate(0f, angle, 0f, Space.World);

        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }
    void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }
}
