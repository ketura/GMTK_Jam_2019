using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMouseHandler
{
	void LeftMove(Vector2 target);
	void LeftDown(Vector2 target, int info);
	void LeftDrag(Vector2 target);
	void LeftUp(Vector2 target);
	void RightClick(Vector2 target);
}
