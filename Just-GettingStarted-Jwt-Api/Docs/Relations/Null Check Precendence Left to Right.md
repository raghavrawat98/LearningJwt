# Null Check Precendence Left to Right

[This is the chat for reference with gemini.](https://share.google/aimode/ELq9GaT7EYIrGHOJe)

for `bool valid = ` <br>
The line <br>
`(user.RefreshToken.Equals(details.refreshToken)) || (String.IsNullOrEmpty(user.RefreshToken)) ;` <br> 
will throw error, but <br>
`(String.IsNullOrEmpty(user.RefreshToken)) || (user.RefreshToken.Equals(details.refreshToken)) ;` <br>
will run successfully.

This happens because expressions are evaluated left to right in C#. <br>
The error comes in `user.RefreshToken` being null so the `.Equals` is `not nullable`. <br>
On going Left To Right and checking if it's null or empty, it will not throw error. <br>
[|-Resuming Back](../Journey.md)