using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float m_moveTime = 0.1f;

    private BoxCollider2D m_boxCollider;
    private Rigidbody2D m_rb2D;
    private float m_inverseMoveTime = 0;
    private LayerMask m_layer;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_layer = 1 << gameObject.layer;
        m_boxCollider = GetComponent<BoxCollider2D>();
        m_rb2D = GetComponent<Rigidbody2D>();
        m_inverseMoveTime = 1f / m_moveTime;
    }

    protected IEnumerator SmoothMovement(Vector3 endDestination_)
    {
        float sqrRemainingDistance = (transform.position - endDestination_).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(m_rb2D.position, endDestination_, m_inverseMoveTime * Time.deltaTime);
            m_rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - endDestination_).sqrMagnitude;
            yield return null;
        }
    }

    protected bool Move(int xDir_, int yDir_, out RaycastHit2D hit_)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir_, yDir_);

        m_boxCollider.enabled = false;
        hit_ = Physics2D.Linecast(start, end, m_layer);
        m_boxCollider.enabled = true;

        if (hit_.transform != null)
        {
            return false;
        }

        StartCoroutine(SmoothMovement(end));
        return true;
    }

    //T is the component we want to verify if there is a collision with.
    protected virtual void AttemptMove<T>(int xDir_, int yDir_)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDir_, yDir_, out hit);

        if (hit.transform == null)
        {
            return;
        }

        T hitComponent = hit.transform.GetComponent<T>();
        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }

    protected abstract void OnCantMove<T>(T component_) where T : Component;
}
