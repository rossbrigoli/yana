# Yana
.NET Core API Client for Kraken built for .NET Standard 2.0

See [Kraken API Documentation](https://www.kraken.com/features/api) for details and usage of the APIs.


<br />

## Tests

To make the tests work with Kraken authentication, you need to:

1. Create a text file with a file name: **apikey.secret**
2. Place the file in your Home directory. $HOME for Mac and Linux, User directory for Windows.
3. The file must be formated in a key value pair with a white space separating the key and the value.

    >key ABCDEFGHIJKLMNOP1234567989APIKEY0123\
    >secret ABCDEFGHIJKLMNOP1234567989APISECRET0123
    
4. In the project root directory, run **dotnet test**

\
\
Good luck!
