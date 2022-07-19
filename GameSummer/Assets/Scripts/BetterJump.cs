using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace TrainingProject.Prototype2
{
    public class BetterJump : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float fallMultiplier = 1.5f;
        [SerializeField] private float lowJumpMultiplier = 1f;
        // Update is called once per frame
        void Update()
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier) * Time.deltaTime;
            }
        }
    }
}
