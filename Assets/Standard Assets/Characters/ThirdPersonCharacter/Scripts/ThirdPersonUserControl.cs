using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter Player; // A reference to the ThirdPersonCharacter on the object        
        private Vector3 move;

        
        private void Start()
		{
			Player = GetComponent<ThirdPersonCharacter>();
        }

        private void FixedUpdate()
        {
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
			move = v*Vector3.forward + h*Vector3.right;
			Player.Move(move, false, false);
        }
    }
}
