using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

/*
 * Author : Thomas Rouvinez
 * Creation date : 03.11.2014
 * Last modified : 03.11.2014
 * 
 * Description : class to handle orders recognition from strings.
 */
public class Regex : MonoBehaviour {

	// -----------------------------------------------------------------------------
	// Variables.
	// -----------------------------------------------------------------------------
	
	private bool cardinality = false;

	// -----------------------------------------------------------------------------
	// Regex patterns.
	// -----------------------------------------------------------------------------

	private string pattern_multiple_order = "(?s).*(and|&|then){1}(?s).*";
	private string pattern_joint_detection = "(and|&|then){1}";
	private string pattern_angle_detection = "[0-9]+(?:[0-9]*)?";
	private string pattern_cardinality_detection = "(south|north|west|east){1}";
	private string pattern_direction_detection = "(down|up|left|right){1}";

	private string pattern_angle_cardinality = "(?s).*[0-9]+(?:[0-9]*)?(?s).*(south|north|west|east){1}(?s).*";
	private string pattern_cardinality_angle = "(?s).*(south|north|west|east){1}(?s).*[0-9]+(?:[0-9]*)?(?s).*";
	private string pattern_angle_direction = "(?s).*[0-9]+(?:[0-9]*)?(?s).*(down|up|left|right){1}(?s).*";
	private string pattern_direction_angle = "(?s).*(down|up|left|right){1}(?s).*[0-9]+(?:[0-9]*)?(?s).*";

	private string pattern_cancel = "(?s).*(cancel|abort|escape|return|previous|scratch)(?s).*";

	// -----------------------------------------------------------------------------
	// Hashtables.
	// -----------------------------------------------------------------------------

	private int directionToInt(string direction)
	{
		if(direction == ("north") || direction == ("up"))
		{
			return 1;
		}
		else if(direction == ("south") || direction == ("down"))
		{
			return -1;
		}
		else if(direction == ("east") || direction == ("left"))
		{
			return 2;
		}
		else if(direction == ("west") || direction == ("right"))
		{
			return -2;
		}

		return -6;
	}

	// -----------------------------------------------------------------------------
	// Regex.
	// -----------------------------------------------------------------------------

	public Order[] interpret(string input)
	{
		// Pre-treatment.
		string orderStr = input.ToLower();

		// Test if we have one or many orders in the 
		if(System.Text.RegularExpressions.Regex.IsMatch(orderStr, pattern_multiple_order))
		{
			Order[] converted = new Order[2];
			int link = System.Text.RegularExpressions.Regex.Match(orderStr, pattern_joint_detection).Index;

			converted[0] = detectSingleOrder(orderStr.Substring(0, link));
			converted[1] = detectSingleOrder(orderStr.Substring(link+1, orderStr.Length-1-link));

			return converted;
		}
		else
		{
			Order[] converted = new Order[1];
			converted[0] = detectSingleOrder(orderStr);

			return converted;
		}

		return null;
	}
	
	// -----------------------------------------------------------------------------
	// Conversion from string to order object.
	// -----------------------------------------------------------------------------

	// Function to return an order object from a string.
	private Order detectSingleOrder(string order)
	{
		Order output = new Order();
		output.setAngle(0);
		output.setOrientation(-6);

		// We might have a single order.
		if(System.Text.RegularExpressions.Regex.IsMatch(order, pattern_angle_cardinality))
		{
			int angle = int.Parse(System.Text.RegularExpressions.Regex.Match(order, pattern_angle_detection).ToString());
			string cardinality = System.Text.RegularExpressions.Regex.Match(order, pattern_cardinality_detection).ToString();

			output.setAngle(angle);
			output.setOrientation(directionToInt(cardinality));
		}
		
		else if(System.Text.RegularExpressions.Regex.IsMatch(order, pattern_angle_direction))
		{
			int angle = int.Parse(System.Text.RegularExpressions.Regex.Match(order, pattern_angle_detection).ToString());
			string direction = System.Text.RegularExpressions.Regex.Match(order, pattern_direction_detection).ToString();

			output.setAngle(angle);
			output.setOrientation(directionToInt(direction));
		}
		
		else if(System.Text.RegularExpressions.Regex.IsMatch(order, pattern_cardinality_angle))
		{
			int angle = int.Parse(System.Text.RegularExpressions.Regex.Match(order, pattern_angle_detection).ToString());
			string cardinality = System.Text.RegularExpressions.Regex.Match(order, pattern_cardinality_detection).ToString();

			output.setAngle(angle);
			output.setOrientation(directionToInt(cardinality));
		}
		
		else if(System.Text.RegularExpressions.Regex.IsMatch(order, pattern_direction_angle))
		{
			int angle = int.Parse(System.Text.RegularExpressions.Regex.Match(order, pattern_angle_detection).ToString());
			string direction = System.Text.RegularExpressions.Regex.Match(order, pattern_direction_detection).ToString();

			output.setAngle(angle);
			output.setOrientation(directionToInt(direction));
		}
		
		else if(System.Text.RegularExpressions.Regex.IsMatch(order, pattern_cancel))
		{
			output.setIsCancel(true);
		}

		return output;
	}
}