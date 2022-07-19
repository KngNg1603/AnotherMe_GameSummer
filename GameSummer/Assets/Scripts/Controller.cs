using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class Controller : MonoBehaviour
    {
        [Header("Move")]
        private float moveDir;       
        private Character character;
        private bool canMove;
        [SerializeField] Rigidbody2D rb;

        [Header("Jump")]
        public Transform groundCheck;
        public float groundCheckRadius;
        public LayerMask whatIsGround;
        private bool isGrounded;
        private int extraJump = 1;
        private float jumpTimeCounter;
        public float jumpTime;
        private bool canJump;
        private float jumpForceOriginal;

        public Animator anim;
        private bool isRunning;

        public LayerMask whatIsWall;
        
        private bool isTouchingWall;
        private bool isWallSliding;
        [SerializeField] Transform wallCheck;
        [SerializeField] float wallCheckRange;
        [SerializeField] float wallSlideSpeed;
        [SerializeField] float wallSlideSpeedOriginal;

        [Header("Wall Jump")]
        public Vector2 wallJumpDir;
        public float wallJumpForce;
        private float wallJumpForceOriginal;

        private float facingDir;

        [Header("Dash")]
        private bool canDash = true;
        private float dashSpeed = 35f;
        private float dashingTime = 0.1f;
        private float dashingCooldown = 0.5f;

        [SerializeField] GameObject dashEffect;
        
        [Header("Death")]
        bool isDeath = false;

        [Header("SoundEffect")]
        [SerializeField] private AudioSource jumpSoundEffect;
        [SerializeField] private AudioSource dashSoundEffect;
        [SerializeField] private AudioSource dieSoundEffect;

        private void Start()
        {
            character = gameObject.GetComponent<Character>();

            //wallHopDir.Normalize();
            wallJumpDir.Normalize();

            dashEffect.SetActive(false);

            //Ice sliding
            jumpForceOriginal = character.JumpForce;
            wallSlideSpeedOriginal = wallSlideSpeed;
            wallJumpForceOriginal = wallJumpForce;
        }

        private void Update()
        {
            Move();
            Jump();        
            CheckIfWallSliding();
            CheckIfCanMove();
            CheckIfCanJump();
            Dash();
            UpdateAnimation();
        }

        private void FixedUpdate()
        {
            CheckSurrounding();
            WallSlide();
        }

        void Move()
        {
            moveDir = Input.GetAxisRaw("Horizontal");
            if (canMove)
            {
                this.transform.Translate((Vector2)transform.right * moveDir * character.MoveSpeed * Time.deltaTime);
                Flip();
                if (moveDir != 0) isRunning = true;
                else isRunning = false;
            }
        }
        
        void Flip()
        {
            if(moveDir != 0)
            {
                facingDir = moveDir;
                this.transform.localScale = new Vector3(moveDir, 1, 1);
                wallCheckRange = moveDir * Mathf.Abs(wallCheckRange);
            }
        }

        void CheckSurrounding()
        {
            //Ground Check
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

            //Wall check
            isTouchingWall = Physics2D.Raycast(wallCheck.position, this.transform.right, wallCheckRange , whatIsWall);
        }

        void Jump()
        {
            if(Input.GetKeyDown(KeyCode.Space) && !isDeath)
            {
                if (canJump && !isWallSliding)
                {
                    jumpSoundEffect.Play();
                    rb.velocity = Vector2.up * character.JumpForce;
                    extraJump--;
                }
                else if (isWallSliding && (moveDir == 0 || moveDir == facingDir) && canJump)
                {
                    jumpSoundEffect.Play();
                    rb.velocity = Vector2.up * character.JumpForce;
                    extraJump--;
                }
                else if((isWallSliding || isTouchingWall) && moveDir != facingDir && canJump)
                {
                    jumpSoundEffect.Play();
                    isWallSliding = false;
                    Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDir.x * moveDir, wallJumpForce * wallJumpDir.y);
                    rb.AddForce(forceToAdd, ForceMode2D.Impulse);
                }
            }
        }

        void CheckIfWallSliding()
        {
            if (isTouchingWall && !isGrounded  && rb.velocity.y < 0)
            {
                isWallSliding = true;
            }
            else isWallSliding = false;
        }

        void CheckIfCanMove()
        {
            if (!isWallSliding && !isDeath) canMove = true;
            else canMove = false;
        }

        void CheckIfCanJump()
        {
            if(isGrounded || isWallSliding)
            {
                extraJump = 1;
            }
            else if(isWallSliding)
            {
                extraJump = 1;
            }

            if (extraJump <= 0)
            {
                canJump = false;
            }
            else canJump = true;
        }

        void WallSlide()
        {
            if (isWallSliding)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
            }

            anim.SetBool("isWallSliding", isWallSliding);
        }

        private IEnumerator Dashing()
        {
            dashSoundEffect.Play();
            canDash = false;
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(this.transform.localScale.x * dashSpeed, 0f);
            dashEffect.SetActive(true);
            
            yield return new WaitForSeconds(dashingTime);
            dashEffect.SetActive(false);
            Vector2 originalSpeed = rb.velocity;
            rb.velocity = Vector2.zero;
            rb.gravityScale = originalGravity;
            yield return new WaitForSeconds(0.1f);
            
            yield return new WaitForSeconds(dashingCooldown);
            print("Ok");
            canDash = true;
        }

        void Dash()
        {
            if(Input.GetButtonDown("Dash") && canDash && !isWallSliding)
            {
                StartCoroutine(Dashing());
            }
        }

        private void UpdateAnimation()
        {
            if(!isDeath)
            {
                //Idle
                anim.SetBool("isGround", isGrounded);
                if (isGrounded)
                {
                    anim.SetBool("isDoubleJump", false);
                }

                // Moving
                if (isRunning) anim.SetBool("isMoving", true);
                else anim.SetBool("isMoving", false);

                //Jumping
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (isGrounded)
                    {
                        anim.SetBool("isJumping", true);
                    }
                    else if (!isGrounded && !isWallSliding)
                    {
                        anim.SetBool("isJumping", false);
                        anim.SetBool("isDoubleJump", true);
                    }
                }
                // Falling
                if (rb.velocity.y < 0 && !isGrounded)
                {
                    anim.SetBool("isJumping", false);
                    anim.SetBool("isDoubleJump", false);
                    anim.SetBool("isFalling", true);
                }
                else
                {
                    anim.SetBool("isFalling", false);
                }
            }         
        }

        private int count = 0;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "Trap" && count == 0)
            {
                count++;
                isDeath = true;
                canMove = false;
                StartCoroutine(Die());
            }
            
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Moving platform
            if (collision.gameObject.CompareTag("Platform"))
            {
                this.gameObject.transform.parent = collision.gameObject.transform;
            }

            // Trap
            if ((collision.gameObject.CompareTag("Trap") || (collision.gameObject.CompareTag("Off_Object")) && count == 0))
            {
                count++;
                isDeath = true;
                canMove = false;
                StartCoroutine(Die());
            }

            if(collision.gameObject.CompareTag("Ice"))
            {
                character.JumpForce = 0;
                wallJumpForce = 0f;
                wallSlideSpeed = 3f;
            }
        }
        
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Platform"))
            {
                print("Exit");
                this.gameObject.transform.parent = null;
            }

            if (collision.gameObject.CompareTag("Ice"))
            {
                character.JumpForce = jumpForceOriginal;
                wallJumpForce = wallJumpForceOriginal;
                wallSlideSpeed = wallSlideSpeedOriginal;
            }
        }

        IEnumerator Die()
        {
            dieSoundEffect.Play();
            isDeath = true;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            extraJump = 0;
            anim.SetBool("isMoving", false);
            anim.SetBool("isJumping", false);
            anim.SetBool("isDoubleJump", false);
            anim.SetBool("isFalling", false);
            anim.SetBool("isWallSliding", false);
            anim.SetTrigger("isDeath");
            yield return new WaitForSeconds(1f);
            Destroy(this.gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
            Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckRange, wallCheck.position.y, wallCheck.position.z));
        }
    }
}

