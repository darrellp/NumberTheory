using System.Linq;
using System.Numerics;

namespace NumberTheory;

/// <summary>
/// Extended Euclidean algorithm.
/// </summary>
/// <remarks>
/// This is used to return the results from the extended Euclidean algorithm as given in
/// Computational Number Theory by Wagon and Bressoud.
/// </remarks>
public class EuclideanExt<T> where T : IBinaryInteger<T>
{
	/// <summary>   Gets the gcd after a call to Euclidean.EuclideanExt. </summary>
	public T GCD { get; private set; }

	/// <summary>   Gets the coefficient for the first value in a call to Euclidean.EuclideanExt. </summary>
	public T Coeff1 { get; private set; }

	/// <summary>   Gets the coefficient for the second value in a call to Euclidean.EuclideanExt. </summary>
	public T Coeff2 { get; private set; }

	/// <summary>
	/// Constructor based on the extended Euclidean Algorithm given in
	/// Computation Number Theory by Wagon and Bressoud.
	/// </summary>
	/// <param name="val1"> The first value. </param>
	/// <param name="val2"> The second value. </param>
	internal EuclideanExt(T val1, T val2)
	{
		// Set up our matrix
		var rst = new[] { new[] { T.Abs(val1), T.One, T.Zero }, new[] { T.Abs(val2), T.Zero, T.One } };

		// While our remainder is non-zero
		while (!T.IsZero(rst[1][0]))
		{
			// Figure out the current quotient
			var q = rst[0][0] / rst[1][0];
			var rst1Save = rst[1];

			// Do the matrix multiply
			rst[1] = rst[0].Zip(rst[1], (v1, v2) => v1 - q * v2).ToArray();
			rst[0] = rst1Save;
		}

		// Pull the results we want out of the matrix
		GCD = rst[0][0];
		Coeff1 = rst[0][1];
		Coeff2 = rst[0][2];
	}
}
