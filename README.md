# BloomFilter

A novel, space-efficient, and probabilistic hashing scheme for a large number of arbitrary-length elements. This data structure is more space efficient than a HashTable, the trade off is the hasing is probabilistic, meaning there is a chance of false positives, but features a 100% recall rate.

A bloom filter with 160,000 values hashed and a 1% collision probability results in a filter that is 54KB (when using compression)! 

![alt text](https://github.com/AdamRakaska/BloomFilter/blob/master/bloom_filter1.JPG "BloomFilter Screenshot")




## Introduction

A [bloom filter](https://en.wikipedia.org/wiki/Bloom_filter "Wikipedia's Article on Bloom Filter") is a truly novel data structure. Similar to a hash table, it can tell you if you've hashed a particular value previously. You can add many, many more values to a bloom filter than you can to a hash table, does not degrade performance as the number of values in the set grows large, and requires only a fraction of the space of a hash table to store it! 

This is not just an academic exercise, or something that only works in theory or in special cases. Indeed, companies like google use bloom filters to quickly determine if it has never seen that value before, thus avoiding a more costly lookup against a database every time the bloomfilter returns false.




## Probabilistic

First off, its important to understand that a bloom filter is NOT a hash table, it operates in an entirely different way. A bloom-filter is what is known as a probabilistic data structure. What this means is, that it can tell you to within a certain probability, if an element exists in a set. In other words, false positive matches ARE possible, but false negative matches ARE NOT possible. For example, if you check a bloom filter for the existence of a value, and it returns false, you can know with 100% certainty that the bloom filter does not contain that value value before. However, if you test a value against the filter and it returns true, there is a small probability that it has in fact not seen that value before, but is returning a false positive. How big of a probability? Here's the beauty: It can be as small as you want it to be. It depends on a few factors, including the size of the filter, how full it is, and how many bits you use to store each value in the filter. 

In a HashTable class, each item is stored as a key value pair, so the size of your object plus a 32 bit integer. Contrast that to a bloom filter, which stores only about 3-7 bits per value hashed. Also, my implementation applies compression when saving the filter to disk, providing even more space savings. A bloom filter with 160,000 values hashed and a 1% collision probability results in a filter that is 235KB uncompressed, and a whopping 54KB when compressed! Remember the filter is an array of bits. The entropy of the array is going to be at its greatest, and thus the compression ratio lowest, when exactly 1/2 of the bits are flipped, or the filter is half-way 'full'. This has the unusual property of getting smaller as you add more hashes to the filter. Actually, this is misleading--the actual filter itself never changes size, its only the compressed version that varies in size.

To handle the compression I just used the [System.IO.Compression.DeflateStream class](https://msdn.microsoft.com/en-us/library/system.io.compression.deflatestream(v=vs.100).aspx). An important note about working with this class: build an array of bytes and send your entire file in one go. In this way it will compress the whole file as one chunk. If you sent data to this stream piecemeal, it will compress each piece separately and you will get a poor compression ratio.




## How it works

So how does this all work? The filter part of a bloom filter is just a large array of bits. You also require several different hash functions that each return a unique result for the same input value. When you add a value to the filter, the value is sent to about 3-7 different hash functions. Each hash function will return a value that is between 0 and the number of bits in the filter. Each value is used as an index to access and element on the array of bits that is your filter. When hashing a value, you just set the bit at each index location in the array to 1. Then testing for the presence of a value in the filter, you pass the value to the hash functions the same way as above, then visit each index, checking to see if any of them are 0. If even one bit at one of those index positions are zero, it means the filter has never seen that value before, because it would have set all those bits to 1. If all the bits at the index locations are 1, then it is likely that the filter has seen that value before. However,there is a chance that it is a false positive, because it could be that that value's different hashes all mapped to bits from other values. As the filter becomes more full, more bits are set to 1, and so the odds of a false positive go up. To build your filter by supplying the estimated number of values you think you are likely to store in the filter, and don't go above a certain ratio of 1 bits to 0 bits. If you were to let your filter hash so many values that every bit got set to 1, then the probability of receiving a false positive for a random value becomes 100%.




## Solving the many hash problem
As I mentioned before, this requires several different hash functions that each return a unique result for the same input value. Although I said 3-7 hash functions, you might require 14 or more, when working with filters that can handle large number of hashes or a low false positive likelihood or both. 

Instead of writing a bunch of separate hash algorithms, I implemented a stream cipher where in I just scramble the cipher table by a number of rounds that is unique to that input. Then, I can return as many indices as the filter is configured for. This sets up the table once per value. It needs to reset the table or else the indices that we mark will depend on every value that came before it, and in that particular order. Currently the bottle-neck is how many times it has the scramble the table for each value. If you need to hash really long values, you'll want to lower the number of rounds it scrambles the table.




## Variations

In this implementation, the bloom-filter size is set once you create it, meaning that it cannot grow bigger if it gets too full, nor can you resize this bloom filter to become smaller if you sized it too big. Because multiple values could rely on the same bit, this implementation does not support removal of items, because to do so would cause several values to begin reporting false negatives.

In order to make a bloom filter that supports deletion, use a number like a byte instead of bits in your filter, and each time you visit an index in the filter while adding values, increment the number you find there. Then, to delete a value, visit each index as you did before, but decrement the number there. This way, if two values map to the same index, that information is tracked by incrementing the value. This is what is known as a Counting Bloom Filter.

There are other variants of bloom filters out there, including bloom filters that can grow in size if it gets too full, but such a thing is beyond the scope of my needs. In essence, when the filter gets too full, you create another separate filter, and add new values by first checking the first filter to see if it exists, and if not, adding the value to the second filter. Checking for the presence of a value requires checking both (and other) filters. For information on scalable bloom filters, [please see this whitepaper](http://gsd.di.uminho.pt/members/cbm/ps/dbloom.pdf).
