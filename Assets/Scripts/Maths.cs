using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maths {
	public static float map(float s, float a1, float a2, float b1, float b2) {
		float t = b1 + (s - a1) * (b2 - b1) / (a2 - a1);
		return t;
	}
}
