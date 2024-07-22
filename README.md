# FnvBrute

## Introduction

FnvBrute is a simple tool for finding collisions in 32-bit FNV-1 hashes.
This is a modification of the original repo, made to find hashes in the game THUMPER. It uses FNV-1a, with some additional bit shifting.

The implementation of FnvBrute is very barebone. It offers no GPU acceleration, dictionary support, or pause/resume/checkpoint features. 
FnvBrute uses very basic multithreading;

## Usage

`FnvBrute.exe 0x[hash] [minLength] [maxLength]`

`hash`: The target hash to look for. Accepts a decimal or hexadecimal `uint32_t` value.

`minLength` and `maxLength`: The minimum and maximum length of the plain text. Must be greater or equal to 2.

## Example

```
>FnvBrute.exe 0x50c63a23 2 8
Hash: 1355168291, max plaintext length: 8
Creating hasher for length 2
Creating hasher for length 3
Creating hasher for length 4
Creating hasher for length 5
Creating hasher for length 6
Creating hasher for length 7
Creating hasher for length 8
Hasher for length 2 finished in 0s
Hasher for length 3 finished in 0s
Length 4 match: >> init << in 0s
Hasher for length 4 finished in 0s
Hasher for length 5 finished in 0s
Hasher for length 6 finished in 19s
Length 8 match: >> a1std32t << in 48s
Length 8 match: >> a2erup9z << in 65s
Length 8 match: >> a4d3b7yd << in 120s
Length 7 match: >> eu7cnee << in 133s
Length 8 match: >> a5tcfrd7 << in 160s
Length 8 match: >> a5_1eyxj << in 165s
Length 8 match: >> a7b70fg4 << in 202s
Length 7 match: >> hez2vro << in 204s
...
```

## Development

FnvBrute is written with .NET Core 3, though it is compatible with earlier .NET Core or Framework versions. Pull requests are very welcome.

FNV hash documentation: http://www.isthe.com/chongo/tech/comp/fnv/
