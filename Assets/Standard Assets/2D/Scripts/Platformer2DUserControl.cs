using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        [SerializeField] private float m_Speed = .5f;
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        private bool m_Run;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!m_Run)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Run = CrossPlatformInputManager.GetButtonDown("Run");
            }
        }


        private void FixedUpdate()
        {
            // Read the inputs.  
            float h = 0;
            if (Input.GetKey(KeyCode.D))
                h = 1;
            else if (Input.GetKey(KeyCode.A))
                h = -1;
            bool crouch = Input.GetKey(KeyCode.C);
            bool run = Input.GetKeyDown(KeyCode.LeftShift);
            Debug.Log(h);
            Debug.Log(crouch);
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump, m_Run);
            m_Jump = false;
            m_Run = false;
        }
    }
}
