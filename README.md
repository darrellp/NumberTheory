Number Theory Library

I've implemented most of the algorithms from "Computational Number Theory" by Bressoud and Wagon
plus a few others.  Still working on quadratic sieve, but this contains the Pollard-Rho factoring
algorithm, Chinese remainder theorem, continued fraction convergents, the extended Euclidean
algorithm and solving linear diophantine equations, the Euler-Phi function, Lucas psuedoprime test,
solution to Pell's equation, an efficient PowerMod function, a test for primes, Quadratic residues,
quadratic sieve (still in production), and a class for rational numbers.

It uses IBinaryInteger for it's arguments, so it should work with any type that implements that interface,
including BigInteger and longs.



No documentation right now to speak of but a quick glance through the Tests project should give you an
idea of how to use the various functions.