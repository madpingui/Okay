using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public event Action OnMouseClick;

    public InputData inputData;
    public LayerMask layerToCollideWith;

    public float moveSpeed = 5f;

    Vector3 m_clickedPos;
    Vector3 m_releasePos;
    Vector3 m_dir;

    Rigidbody2D m_rigid2D;

    Camera m_cam;

    PlayerVFX m_playerVFX;

    bool m_hitBlock;

    private void Start()
    {
        GetComponents();
    }

    void GetComponents()
    {
        m_rigid2D = GetComponent<Rigidbody2D>();
        m_playerVFX = GetComponent<PlayerVFX>();

        m_cam = FindObjectOfType<Camera>();
    }

    private void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if(inputData.isPressed == true)
        {
            m_hitBlock = CheckIfHitBlock();
            if (m_hitBlock)
                return;

            m_clickedPos = m_cam.ScreenToWorldPoint(Input.mousePosition);
            m_clickedPos = new Vector3(m_clickedPos.x, m_clickedPos.y, 0f);

            ResetPlayerPos();

            m_playerVFX.SetDotStartPos(m_clickedPos);
            m_playerVFX.ChangeDotActiveState(true);
            m_playerVFX.ChangeTrailState(false, 0f);

            OnMouseClick?.Invoke();
        }

        if(inputData.isHeld == true)
        {
            if (m_hitBlock)
                return;

            m_playerVFX.SetDotPos(m_clickedPos, m_cam.ScreenToWorldPoint(Input.mousePosition));
            m_playerVFX.MakeBallPulse();
        }

        if (inputData.isReleased == true)
        {
            if (m_hitBlock)
                return;

            m_releasePos = m_cam.ScreenToWorldPoint(Input.mousePosition);
            m_releasePos = new Vector3(m_releasePos.x, m_releasePos.y, 0f);

            m_playerVFX.ChangeDotActiveState(false);
            m_playerVFX.ResetBallSize();
            m_playerVFX.ChangeTrailState(true, 0.75f);

            CalculateDirection();
            MovePlayerInDirection();
        }
    }

    void CalculateDirection()
    {
        m_dir = (m_releasePos - m_clickedPos).normalized;
    }

    void MovePlayerInDirection()
    {
        m_rigid2D.velocity = m_dir * moveSpeed;
    }

    void ResetPlayerPos()
    {
        transform.position = m_clickedPos;
        m_rigid2D.velocity = Vector3.zero;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            Vector2 _wallNormal = collision.contacts[0].normal;
            m_dir = Vector2.Reflect(m_rigid2D.velocity, _wallNormal).normalized;

            m_rigid2D.velocity = m_dir * moveSpeed;
        }
    }

    bool CheckIfHitBlock()
    {
        Ray _ray = m_cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D _hitBlock = Physics2D.Raycast(_ray.origin, _ray.direction, 100f, layerToCollideWith);

        return _hitBlock;
    }
}
