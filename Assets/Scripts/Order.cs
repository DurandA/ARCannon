using UnityEngine;
using System.Collections;

public class Order : MonoBehaviour {

	// -----------------------------------------------------------------------------
	// Main components of an order.
	// -----------------------------------------------------------------------------
	
	private int angle;
	private int orientation;
	private bool isCancel = false;

	// -----------------------------------------------------------------------------
	// Getters-setters.
	// -----------------------------------------------------------------------------
	
	public void setAngle(int angle)
	{
		this.angle = angle;
	}

	public void setOrientation(int orientation)
	{
		this.orientation = orientation;
	}

	public void setIsCancel(bool isCancel)
	{
		this.isCancel = isCancel;
	}

	public int getAngle()
	{
		return this.angle;
	}

	public int getOrientation()
	{
		return this.orientation;
	}

	public bool getIsCancel()
	{
		return this.isCancel;
	}
}