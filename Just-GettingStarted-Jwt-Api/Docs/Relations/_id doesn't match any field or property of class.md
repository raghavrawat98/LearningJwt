# _id doesn't match any field or property of class

![alt text](../Assets/Pasted%20image%2020260409045544.png)

To fix this <br>
add <br>
tag of `[BsonId]` since it is stored as underscore id in mongo <br>
![alt text](../Assets/Pasted%20image%2020260409045708.png)

Need to replace userID with email
[Some Article which says it maps to null](https://www.mongodb.com/community/forums/t/id-field-mapped-to-null-pojocodecregistry/5774/7)

[|-Resuming Back](../Journey.md)

