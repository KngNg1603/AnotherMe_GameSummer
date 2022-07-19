using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Character : MonoBehaviour
    {
        [SerializeField] float moveSpeed;
        [SerializeField] float jumpForce;

        public float MoveSpeed
        {
            get { return moveSpeed; }
            set { moveSpeed = value; }
        }

        public float JumpForce
        {
            get { return jumpForce; }
            set { jumpForce = value; }
        }
        
    }
}