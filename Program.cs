using System;
using System.Collections.Generic;
using System.Linq;

namespace CnLINQ_Numbers
{
	/// <summary>
	/// Three interesting years: 2010, 2011, 2012.
	/// These are consecutive four-digit positive integers containing 
	/// the same three digits where exactly one digit appears exactly 
	/// twice in each number. Find additional such four-digit number 
	/// triplets where the sum of digits does not exceed 21!
	/// This application demonstrates various LINQ query capabilities.
	/// </summary>
	class FourDigitNumbersApp
	{
		/// <summary>
		/// Generic list for storing four-digit number list.
		/// </summary>
		static List<int[]> NumberList = new List<int[]>();

		/// <summary>
		/// Populates the list with all possible four-digit number sequences.
		/// </summary>
		static void PopulateNumberList()
		{
			for (int i = 1; i <= 9; i++)
				for (int j = 0; j <= 9; j++)
					for (int k = 0; k <= 9; k++)
						for (int l = 0; l <= 9; l++)
						{
							int[] Row = new int[4];
							Row[0] = i;
							Row[1] = j;
							Row[2] = k;
							Row[3] = l;
							NumberList.Add(Row);
						}
		}

		/// <summary>
		/// Main method
		/// </summary>
		static void Main(string[] args)
		{
			// Populate the list with all possible number sequences.
			PopulateNumberList();
			// First operation: Sum of digits <= 21.
			IEnumerable<int[]> E1 = from x in NumberList
															where x[0] + x[1] + x[2] +
																		x[3] <= 21
															select x;
			Console.WriteLine($"Numbers with digit sum <=21: {E1.Count()} sequences satisfy this condition");

			// Second operation: One digit appears exactly twice (three distinct digits total).
			IEnumerable<int[]> E2 = from x in E1
															where
																x.Distinct().Count() == 3
															select x;
			Console.WriteLine($"Exactly one digit appears twice: {E2.Count()} sequences satisfy this condition");

			// Third operation: Create groups with identical digit sets.
			// Grouping key will be the sorted distinct digits of each number.
			var E3 = from x in E2
							 group x by
								new
								{
									a = (x.Distinct().OrderBy(i=>i).ToArray())[0],
									b = (x.Distinct().OrderBy(i=>i).ToArray())[1],
									c = (x.Distinct().OrderBy(i=>i).ToArray())[2]
								}
								into y
									select y;
			Console.WriteLine($"Number of groups with identical digit sets: {E3.Count()}");

			// Analyze each group.
			foreach (var x in E3)
			{
				// Convert four-element arrays to four-digit numbers
				int[] NumberArray = new int[x.Count()];
				int i = 0;
				foreach (var t in x)
				{
					NumberArray[i] = t[0] * 1000 + t[1] * 100 + t[2] * 10 + t[3];
					// Check for three consecutive numbers starting from third element.
					if (i > 1)
						if (NumberArray[i] - NumberArray[i - 1] == 1 && NumberArray[i - 1] - NumberArray[i - 2] == 1)
							Console.WriteLine(NumberArray[i - 2] + ", " + NumberArray[i - 1] + ", " + NumberArray[i]);
					i++;
				}
			}
			Console.ReadLine();
		}
	}
}


