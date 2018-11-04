using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionDebugScript : MonoBehaviour {

	public Color col;
	public int x;
	
	private void OnDrawGizmos() { DrawRays(); }
	private void OnDrawGizmosSelected() { DrawRays(); }

	void DrawRays(){
		float targetX = transform.position.x;
		float targetY = transform.position.z;
		switch (x) {
			case 0:
				targetY -= 1;
				break;
			case 1:
				targetX += 1;
				break;
			case 2:
				targetY += 1;
				break;
			case 3:
				targetX -= 1;
				break;
		}

		//Quaternion lineRotation = Quaternion.Euler(0, (x * 90) + 90, 0);
		Gizmos.color = col;
		Gizmos.DrawLine(transform.position, new Vector3(targetX,gameObject.transform.position.y,targetY));

		//Gizmos.DrawCube(new Vector3(targetX, gameObject.transform.position.y, targetY), new Vector3(0.2f,0.2f,0.2f));

	}
}
