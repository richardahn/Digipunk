using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField]
    protected float floatDuration = 1f;
    [SerializeField]
    protected float minScale = 0.5f;
    [SerializeField]
    protected float maxScale = 1f;
    [SerializeField]
    protected float minXForce = -1f;
    [SerializeField]
    protected float maxXForce = 1f;
    [SerializeField]
    protected float minYForce = 1f;
    [SerializeField]
    protected float maxYForce = 2f;
    [SerializeField]
    protected float maxYVelocity = 5f;
    [SerializeField]
    protected float xDeceleration = 10f;
    [SerializeField]
    protected float yDeceleration = 10f;
    [SerializeField]
    protected Vector3 offset = new Vector3(0, 1.7f, 0);

    // State
    protected float elapsedTime;

    // Dependents
    protected TextMesh textMesh;
    protected Rigidbody2D rb;

    private void Awake()
    {
        textMesh = GetComponent<TextMesh>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, floatDuration);
        transform.position += offset;

        Vector2 initialForce = new Vector2(Random.Range(minXForce, maxXForce), Random.Range(minYForce, maxYForce));
        rb.AddForce(initialForce, ForceMode2D.Impulse);
    }

    public void SetText(string text)
    {
        textMesh.text = text;
    }
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        Shrink();
    }
    private void FixedUpdate()
    {
        float deceleratedX = Mathf.MoveTowards(rb.velocity.x, 0f, xDeceleration * Time.deltaTime);
        float deceleratedY = Mathf.Clamp(rb.velocity.y - yDeceleration * Time.deltaTime, -maxYVelocity, maxYVelocity);
        rb.velocity = new Vector2(deceleratedX, deceleratedY);
    }

    private void Shrink()
    {
        float newScale = Mathf.Lerp(maxScale, minScale, elapsedTime / floatDuration);
        transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);
    }


}
