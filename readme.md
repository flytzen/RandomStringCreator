This package generates random strings that can be used for things like IDs or unique strings in URLs or even password generation.

It uses crypto APIs, not Random, to avoid any risk of the same string being returned from concurrent invocations, thus making it more suitable for use where you need a higher probability of uniqueness. 

# Usage
In the simplest case, just instantiate it and call the Create method with a length.
By default, the generated string will be limited to upper and lower characters and numbers, though there is no guarantee that all the different types will be present.  
If you want to accept a different set of characters, pass in a string with all valid characters in the constructor. For example
```
new Generator("abcdefghijklmnopqrstuvwxyz")
``` 
will only return strings with lowercase letters. Similarly, you could add punctuation etc. The only restriction is that characters must all be ASCII characters.

# Performance


## Initialisation
The generator uses lazy instantion of its dependency (a crypto API), meaning you can safely instantiate or inject the class without worrying about any performance implications when you end up not actually generating a string. 

# uniqueness
By default, the generator will return strings consisting of upper and lower characters and numbers, meaning there are 50 possible values for each character. This means that for a six-character string there are 15,625,000,000 different possible combinations. The use of proper crypto APIs *should* also mean that results are randomly spread across the spectrum.  
However, be aware that this doesn't make collisions nearly as rare as you might think - please read <https://blogs.msdn.microsoft.com/ericlippert/2010/03/22/socks-birthdays-and-hash-collisions/> for some good background information on this.

# Implementation
The system uses ```System.Security.Cryptography.RandomNumberGenerator.Create()``` to get a cryptographic generator of random bytes. It is essentially equivalent to ```new RNGCryptoServiceProvider()```, in that the latter is a subclass of the former and in either case maps to underlying APIs, BCryptGenRandom on Windows, OpenSSL on other platforms. ```RNGCryptoServiceProvider``` doesn't exist in ```netstandard``` hence using the other method, which is also cross-platform.

The code will keep reading bytes and discard any that does not map to a char in the "valid" list. This is arguably wasteful as on average only one in every five bytes is "valid", if you use the default list of valid characters. It is possible to implement mathematical solutions that manipulate the bytes instead, but apparantly this is likely to skew the distribution of the generated strings so they are less random. 

# References
