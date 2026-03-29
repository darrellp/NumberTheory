using System;
using System.Collections.Generic;
using System.Numerics;

namespace NumberTheory;

/// <summary>
/// Implements Gaussian algorithms over GF(2) to find vectors in the null space
/// of a binary matrix. Used by the Quadratic Sieve to find dependencies among
/// exponent vectors modulo 2.
/// 
/// I'd like to implement Block-Lanczos but even AI backs off from it claiming it's
/// "overkill" for "these size problems" and "notoriously difficult".  So for the moment
/// we're using Gaussian elimination.
/// </summary>
public static class GaussianGf2
{
	/// <summary>
	/// Finds null-space vectors of the matrix formed by the exponent vectors mod 2.
	/// Each returned vector indicates which rows (relations) to combine so that
	/// their exponent vectors sum to zero mod 2.
	/// </summary>
	/// <param name="exponentVectors">
	/// Each element is an exponent vector of length equal to the factor base size.
	/// </param>
	/// <returns>
	/// Null-space vectors over GF(2). Each returned ulong[] is bit-packed:
	/// bit i of word (i/64) being set means relation i is included in the dependency.
	/// </returns>
	public static List<ulong[]> FindNullSpace(int[][] exponentVectors)
	{
		var rows = exponentVectors.Length;
		if (rows == 0) return [];

		var cols = exponentVectors[0].Length;

		// Build the binary matrix (rows x cols) with an augmented identity to track row combinations.
		// Each row is: [exponent bits | identity bit for this row]
		// We do elimination on the exponent columns and read off dependencies from the identity part.
		var totalBits = cols + rows;
		var totalWords = (totalBits + 63) / 64;
		var colWords = (cols + 63) / 64;
		var rowWords = (rows + 63) / 64;

		var matrix = new ulong[rows][];
		for (var i = 0; i < rows; i++)
		{
			matrix[i] = new ulong[totalWords];

			// Set the exponent bits (mod 2) in the left portion
			for (var j = 0; j < cols; j++)
			{
				if ((exponentVectors[i][j] & 1) != 0)
				{
					matrix[i][j / 64] |= 1UL << (j % 64);
				}
			}

			// Set the identity bit in the right portion
			var idBit = cols + i;
			matrix[i][idBit / 64] |= 1UL << (idBit % 64);
		}

		// Gaussian elimination over GF(2) on the exponent columns
		var pivotRow = 0;
		var pivotCol = new int[rows];
		Array.Fill(pivotCol, -1);

		for (var col = 0; col < cols && pivotRow < rows; col++)
		{
			// Find a row with a 1 in this column
			var pivot = -1;
			for (var row = pivotRow; row < rows; row++)
			{
				if (GetBit(matrix[row], col))
				{
					pivot = row;
					break;
				}
			}

			if (pivot < 0) continue;

			// Swap pivot row into position
			(matrix[pivotRow], matrix[pivot]) = (matrix[pivot], matrix[pivotRow]);
			pivotCol[pivotRow] = col;

			// Eliminate this column from all other rows
			for (var row = 0; row < rows; row++)
			{
				if (row != pivotRow && GetBit(matrix[row], col))
				{
					for (var w = 0; w < totalWords; w++)
					{
						matrix[row][w] ^= matrix[pivotRow][w];
					}
				}
			}

			pivotRow++;
		}

		// Rows beyond pivotRow have all-zero exponent columns.
		// Their identity portion tells us which original rows combine to zero mod 2.
		var result = new List<ulong[]>();
		for (var row = pivotRow; row < rows; row++)
		{
			// Extract the identity portion as the null-space vector
			var nullVec = new ulong[rowWords];
			for (var i = 0; i < rows; i++)
			{
				var idBit = cols + i;
				if (GetBit(matrix[row], idBit))
				{
					nullVec[i / 64] |= 1UL << (i % 64);
				}
			}

			// Only include non-zero vectors
			var isZero = true;
			for (var w = 0; w < rowWords; w++)
			{
				if (nullVec[w] != 0) { isZero = false; break; }
			}

			if (!isZero)
			{
				result.Add(nullVec);
			}
		}

		return result;
	}

	/// <summary>
	/// Gets bit at position 'bit' from a bit-packed ulong array.
	/// </summary>
	private static bool GetBit(ulong[] data, int bit)
	{
		return (data[bit / 64] & (1UL << (bit % 64))) != 0;
	}
}