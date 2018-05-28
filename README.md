# PIWorks Assignment

I solved the problem with two ways

First one;
- Program reads file line by line
- Create an object from each line
- Check Object's time stamp for spesific time
- Calculate played song count for each client with each coming object and add it to a dictionary
- Calculate distinct played count from dictionary which is filled in above step and add it to a new dictionary
- Bind dictionary to datagrid so user can see the result
- Show operation time for upload and analyze the data

Operation Time : 7.7 sec


Second one;
- Program reads all file and create a object list
- Use lambda expressions to;
  - Check Object's time stamp for spesific time
  - Calculate played song count for each client
  - Calculate distinct played count and return a dictionary
 - Bind dictionary to datagrid so user can see the result
 - Show operation time for upload File
 - Show operation time for analyze the data
  
  Operation Time for Upload : 7.44 sec
  
  Operation Time for Analyze: 0.7 sec

  
  
  I also override the equal and gethashcode methods in PlayedSongsModel class so user can use "Contain()" and other methods on Hashset, list etc.
  Overriding these methods increased the performance of first solution. 