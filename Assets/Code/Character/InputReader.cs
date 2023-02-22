using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer2D
{
	public class InputReader : MonoBehaviour
	{
		private const float targetOffset = 0.1f;

		public enum ControlMethod
		{
			VirtualJoystick,
			Tap
		}

		private ControlMethod currentControlMethod = ControlMethod.Tap;

		// T�h�n muuttujaan tallennetaan k�ytt�j�n Move-actionia vastaava arvo.
		private Vector2 moveInput;

		// Piste pelimaailmassa, johon k�ytt�j� haluaa hahmon liikkuvan
		private Vector3 worldTouchPosition;

		public void OnMove(InputAction.CallbackContext context)
		{
			// Luetaan k�ytt�j�n hahmoa liikuttava sy�te muuttujaan.
			moveInput = context.ReadValue<Vector2>();
			Debug.Log($"Sy�te: {moveInput}");
		}

		public void OnTapMove(InputAction.CallbackContext context)
		{
			if (context.phase == InputActionPhase.Performed)
			{
				currentControlMethod = ControlMethod.Tap;

				// Lue t�pp�yksen koodrdinaatti n�yt�ll�
				Vector2 touchPosition = context.ReadValue<Vector2>();

				// Implisiittinen tyyppimuunnos, Vector2 muuttuu siis Vector3:ksi automaattisesti
				// eli sama kuin
				// Vector3 screenCoordinate = new Vector3(touchPosition.x, touchPosition.y, 0);
				Vector3 screenCoordinate = touchPosition;

				// Koordinaattimuunnos n�yt�n koordinaatista pelimaailman koordinaatistoon
				this.worldTouchPosition = Camera.main.ScreenToWorldPoint(screenCoordinate);
			}
		}

		public Vector2 GetMoveInput()
		{
			if (currentControlMethod == ControlMethod.VirtualJoystick)
			{
				return moveInput;
			}

			Vector2 toTarget = (Vector3)(worldTouchPosition - transform.position);

			if (toTarget.magnitude < targetOffset)
			{
				// Kohde saavutettu
				return Vector2.zero;
			}

			return toTarget.normalized;
		}
	}
}
