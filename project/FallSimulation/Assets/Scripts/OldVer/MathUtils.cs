using System;
using System.Collections.Generic;
using System.Linq;

public static class MathUtils {
	public static double CalculateStdDev(IEnumerable<double> values) {
		double ret = 0;
		if (values.Count() > 0) {
			//Compute the Average      
			double avg = values.Average();
			//Perform the Sum of (value-avg)_2_2      
			double sum = values.Sum(d => Math.Pow(d - avg, 2));
			//Put it all together      
			ret = Math.Sqrt((sum) / (values.Count() - 1));
		}

		return ret;
	}

	public static float CalculateMean(IEnumerable<float> values) {
		float ret = 0;
		if (values.Count() > 0) {
			for (int i = 0; i < values.Count(); i++) {
				ret += values.Average();
			}
		}

		return ret;
	}

	public static float MyMean(float[] values) {
		float ret = 0;
		for (int i = 0; i < values.Length; i++) {
			ret += values[i];
		}

		if (values.Length > 0) {
			return ret / values.Length;
		}

		return 0;
	}

	public static float MyStdDev(float[] values) {
		int denominator = values.Length - 1;
		float avg = MyMean(values);

		float sum = 0;

		for (int i = 0; i < values.Count(); i++) {
			sum += (values[i] - avg) * (values[i] - avg);
		}

		sum = (float) Math.Sqrt(sum / denominator);

		return sum;
	}

	public static float MyStdDev(float[] values, float avg) {
		int denominator = values.Length - 1;
		float sum = 0;

		for (int i = 0; i < values.Count(); i++) {
			sum += (values[i] - avg) * (values[i] - avg);
		}

		sum = (float) Math.Sqrt(sum / denominator);

		return sum;
	}
}