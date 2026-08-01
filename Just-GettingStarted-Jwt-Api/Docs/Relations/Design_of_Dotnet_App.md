# Design_of_Dotnet_App
As on 8 April <br>
For now the design is as follows <br>
Folder structure I can't add Nests so basics are models and helpers. <br>

![alt text](../Assets/Pasted%20image%2020260409023915.png)

Purposefully using **General -> Specific** naming <br>
for **abstraction -> implementation** <br>
and better *readability and understanding*. <br>

I am doing many changes in **Controller and Repos** <br>
I am doing this to keep the `Functions` as in required to *what they are meant to do*. <br>
Eg: <br>
I have `UserExists` and `ValidateUser` <br>
Both of them are Implemented through `Get.User.MongoDB` way <br>
but I don't want to add complexities to Controller <br>

*They are more readable and it's fine if I write some extra code now* <br>
*to come after long time for revision and refigure the logic.* <br>

[|-Resuming Back](../Journey.md)