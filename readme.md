This package generates random strings that can be used for things like IDs or unique strings in URLs or even password generation.

It uses crypto APIs, not Random, to avoid any risk of the same string being returned from concurrent invocations, thus making it more suitable for use where you need a higher probability of uniqueness. 

# Build status
[![Build Status](https://dev.azure.com/flytzen/RandomStringCreator/_apis/build/status/flytzen.RandomStringCreator?branchName=master)](https://dev.azure.com/flytzen/RandomStringCreator/_build/latest?definitionId=1)


# Installation
```
Install-Package Install-Package RandomStringCreator
```

# Usage
In the simplest case, just instantiate it and call the Create method with a length like this:
```
new StringCreator().Get(6);
```
By default, the generated string will be limited to upper and lower characters and numbers, though there is no guarantee that all the different types will be present.  

## Control which characters are valid
If you want to accept a different set of characters, pass in a string with all valid characters in the constructor. For example
```
new StringCreator("abcdefghijklmnopqrstuvwxyz")
``` 
will only return strings with lowercase letters. Similarly, you could add punctuation etc. The only restriction is that characters must all be ASCII characters.

# Performance
The implementation uses Windows or OpenSSL Crypto APIs (see Implementation). Ideally you should cache one instance of RandomStringCreator and use it throughout, though the actual performance impact on this is very minimal so you probably don't want to bother unless you do this a lot.
On my i7 laptop, using BenchmarkDotNet (included in the repo), you are looking at ~14 micro seconds to get a string if you just create a StringCreator and call Get. If you cache the one instance, you save about one micro second per invocation. Other .NET frameworks and environments may vary.
One good reason for caching the single instance is that it *does* call into underlying Windows or OpenSSL APIs so minimising the number of external invokes is probably a good idea.

## Buffer Size
The constructor allows you to specify a buffer size; the implementation uses a Crypto API method to get an array of random bytes. By default, it will only get 128 bytes at a time. If you are caching a shared instance and generating a lot of strings, it is probably a good idea to increase the buffer size to 1024, 2048 or more, depending on your needs. In the benchmarks it makes a very small difference, on the order of 0.1 micro seconds per invocation, but a larger buffer does reduce the number of calls to the underlying APIs.

## Initialisation
The generator uses lazy instantion of its dependency (a crypto API), meaning you can safely instantiate or inject the class without worrying about any performance implications when you end up not actually generating a string. 

## Disposable
If you do *not* cache one instance but instead create many instances, you should wrap it in a *using* statement, to ensure the reference to the external Crypto APIs is released as soon as possible. Note that if you use a DI container to create instances, this is usually handled for you automatically.

# Uniqueness
By default, the generator will return strings consisting of upper and lower characters and numbers, meaning there are 50 possible values for each character. This means that for a six-character string there are 15,625,000,000 different possible combinations. The use of proper crypto APIs *should* also mean that results are randomly spread across the spectrum.  
However, be aware that this doesn't make collisions nearly as rare as you might think - please read <https://blogs.msdn.microsoft.com/ericlippert/2010/03/22/socks-birthdays-and-hash-collisions/> for some good background information on this.

# Implementation
The system uses ```System.Security.Cryptography.RandomNumberGenerator.Create()``` to get a cryptographic generator of random bytes. It is essentially equivalent to ```new RNGCryptoServiceProvider()```, in that the latter is a subclass of the former and in either case maps to underlying APIs, BCryptGenRandom on Windows, OpenSSL on other platforms. ```RNGCryptoServiceProvider``` doesn't exist in ```netstandard``` hence using the other method, which is also cross-platform.

The code will keep reading bytes and discard any that does not map to a char in the "valid" list. This is arguably wasteful as on average only one in every five bytes is "valid", if you use the default list of valid characters. It is possible to implement mathematical solutions that manipulate the bytes instead, but apparantly this is likely to skew the distribution of the generated strings so they are less random. 

# References
- <http://securitydriven.net/>  (an excellent book about security and cryptography in .Net)
- <http://stackoverflow.com/questions/32932679/using-rngcryptoserviceprovider-to-generate-random-string>  
- <http://stackoverflow.com/questions/38632735/rngcryptoserviceprovider-in-net-core>

