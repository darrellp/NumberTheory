using System.Linq;
#if BIGINTEGER
using nt = System.Numerics.BigInteger;
#elif LONG
using nt = System.Int64;
#endif

// ReSharper disable CheckNamespace
#if BIGINTEGER
namespace NumberTheoryBig
#elif LONG
namespace NumberTheoryLong
#endif
{
	////////////////////////////////////////////////////////////////////////////////////////////////////
	/// <summary>   Extended Euclidean algorithm. </summary>
	///
	/// <remarks>   This is used to return the results from the extended Euclidean algorithm as given in
	/// Computational Number Theory by Wagon and Bressoud.  See Euclidean.EuclideanExt for the function which
	/// returns an object of this class.  I made a small attempt to try to keep the returned coefficients
	/// relatively small.
	/// Darrellp, 2/12/2011. </remarks>
	////////////////////////////////////////////////////////////////////////////////////////////////////

	public class EuclideanExt
	{
		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>   Gets the gcd after a call to Euclidean.EuclideanExt. </summary>
		///
		/// <value> The gcd. </value>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public nt GCD { get; private set; }

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>   Gets the coefficient for the first value in a call to Euclidean.EuclideanExt. </summary>
		///
		/// <value> The coefficient. </value>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public nt Coeff1 { get; private set; }

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>   Gets the coefficient for the second value in a call to Euclidean.EuclideanExt. </summary>
		///
		/// <value> The coefficient. </value>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		public nt Coeff2 { get; private set; }

		////////////////////////////////////////////////////////////////////////////////////////////////////
		/// <summary>   Constructor based on the extended Euclidean Algorithm given in
		/// Computation Number Theory by Wagon and Bressoud. </summary>
		///
		/// <remarks>   Darrellp, 2/13/2011. </remarks>
		///
		/// <param name="val1"> The first value. </param>
		/// <param name="val2"> The second value. </param>
		////////////////////////////////////////////////////////////////////////////////////////////////////

		internal EuclideanExt(nt val1, nt val2)
		{
			// Set up our matrix
			var rst = new[] { new[] { TypeAdaptation.Abs(val1), 1, 0 }, new[] { TypeAdaptation.Abs(val2), 0, 1 } };

			// While our remainder is non-zero
			while (rst[1][0] != 0)
			{
				// Figure out the current quotient
				var q = rst[0][0]/rst[1][0];
				var rst1Save = rst[1];

				// Do the matrix multiply
				rst[1] = rst[0].Zip(rst[1], (v1, v2) => v1 - q*v2).ToArray();
				rst[0] = rst1Save;
			}

			// Pull the results we want out of the matrix
			GCD = rst[0][0];
			Coeff1 = rst[0][1];
			Coeff2 = rst[0][2];
		}

	}
}
