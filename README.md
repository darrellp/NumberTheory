Number Theory Library

I've implemented most of the algorithms from "Computational Number Theory" by Bressoud and Wagon
plus a few others.  Still working on quadratic sieve, but this contains the Pollard-Rho factoring
algorithm, Chinese remainder theorem, continued fraction convergents, the extended Euclidean
algorithm and solving linear diophantine equations, the Euler-Phi function, Lucas psuedoprime test,
solution to Pell's equation, an efficient PowerMod function, a test for primes, Quadratic residues,
quadratic sieve (still in production), and a class for rational numbers.

The library builds to handle either Longs of BigIntegers depending on an IFDEF variable.  All of
the algorithms will handle either type.  Of course BigIntegers are a bit slower but handle
arbitrarily large values.
