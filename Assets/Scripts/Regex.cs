using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

	private string pattern_multiple_order = "(?s).*(and|&|then)(?s).*";
	private string pattern_joint_detection = "(and|&|then)";
	private string pattern_angle_detection = "([0-9]|[0,360])";
	private string pattern_cardinality_detection = "(south|north|west|east){1}";
	private string pattern_direction_detection = "(down|up|left|right){1}";

	private string pattern_angle_cardinality = "(?s).*([0-9]|[0,360])(?s).*(south|north|west|east){1}(?s).*";
	private string pattern_cardinality_angle = "(?s).*(south|north|west|east){1}(?s).*([0-9]|[0,360])(?s).*";
	private string pattern_angle_direction = "(?s).*([0-9]|[0,360])(?s).*(down|up|left|right){1}(?s).*";
	private string pattern_direction_angle = "(?s).*(down|up|left|right){1}(?s).*([0-9]|[0,360])(?s).*";

	private string pattern_cancel = "(?s).*(cancel|abort|escape|return|previous|scratch)(?s).*";

	// -----------------------------------------------------------------------------
	// Hashtables.
	// -----------------------------------------------------------------------------
	
	private Hashtable cardinalityHT = new Hashtable();
	private Hashtable directionHT = new Hashtable();

	private void initcardinalityHT()
	{
		cardinalityHT.Add("north", 0);
		cardinalityHT.Add("south", 1);
		cardinalityHT.Add("west",  2);
		cardinalityHT.Add("east",  3);
	}

	private void initdirectionHT()
	{
		directionHT.Add("up",    0);
		directionHT.Add("down",  1);
		directionHT.Add("left",  2);
		directionHT.Add("rigth", 3);
	}

	// -----------------------------------------------------------------------------
	// Unity functions.
	// -----------------------------------------------------------------------------

	void Start () {
		// Prepare both hashtables.
		initcardinalityHT();
		initdirectionHT();
	}

	// -----------------------------------------------------------------------------
	// Utilities.
	// -----------------------------------------------------------------------------

	// Function to split a double order.
	private string[] split(string input)
	{
		string[] orders = new string[2];
		Match m = System.Text.RegularExpressions.Regex.Match(input, pattern_joint_detection);

		orders[0] = input.Substring(0, m.Index);
		orders[1] = input.Substring(m.Index, input.Length);
		
		return orders;
	}

	// -----------------------------------------------------------------------------
	// Regex.
	// -----------------------------------------------------------------------------

	public string interpret(string input)
	{
		// Pre-treatment.
		string orderStr = input.ToLower();

		// Test if we have one or many orders in the 
		if(System.Text.RegularExpressions.Regex.IsMatch(orderStr, pattern_multiple_order))
		{
			string[] orders = split (orderStr);
			string answer = "default multiple orders";

			for(int i = 0 ; i < orders.Length ; i++)
			{
				answer += " Order " + i + ": " + orders[i] + " ||";
			}

			return answer;
		}
		else
		{
			return detectSingleOrder(orderStr);
		}
	}
	
	// -----------------------------------------------------------------------------
	// Conversion from string to order object.
	// -----------------------------------------------------------------------------

	// Function to return an order object from a string.
	private string detectSingleOrder(string order)
	{
		Order output = new Order();

		// We might have a single order.
		if(System.Text.RegularExpressions.Regex.IsMatch(order, pattern_angle_cardinality))
		{
			return "1x (angle - cardinality)";
		}
		
		else if(System.Text.RegularExpressions.Regex.IsMatch(order, pattern_angle_direction))
		{
			return "1x (angle - direction)";
		}
		
		else if(System.Text.RegularExpressions.Regex.IsMatch(order, pattern_cardinality_angle))
		{
			return "1x (cardinality - angle)";
		}
		
		else if(System.Text.RegularExpressions.Regex.IsMatch(order, pattern_direction_angle))
		{
			return "1x (direction - angle)";
		}
		
		else if(System.Text.RegularExpressions.Regex.IsMatch(order, pattern_cancel))
		{
			output.setIsCancel(true);
			return "cancel last order";
		}

		return "order not recognized";
	}
}