# Changes at 1st Aug
I changed the code from 
```csharp
claims: new[] { new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) },
```
to 
```csharp
claims: new[] { new Claim(JwtRegisteredClaimNames.Jti, whatItClaims) },
```

**so it doesn't claim any random value** <br>
but <br>
**it claims the email ID of the user i.e. the primary identifier.** <br>

[|-Resuming Back](../Journey.md)