# OrderProcessing
```diff
- WARNING!
- IN ORDER TO DEBUG THIS APPLICATION YOU WILL HAVE TO MODIFY launchSettings.JSON FILE
- YOU HAVE TO UPDATE workingDirectory OF THE APPLICATION TO THE DIRECTORY THAT REPOSITORY WAS CLONED!
! Example: If repository was cloned to desktop, workingDirectory should be: 
! "workingDirectory":"C:\Users\UserName\Desktop"
```
### This is my implementation for the recruitment task, and I have ensured that all the specified criteria are met. The application is designed to handle orders and store data efficiently using an SQLite database file. The database file is created and managed seamlessly, providing persistent storage for order information, products, and statuses. The application ensures the correct manipulation and retrieval of data.

### In terms of functionality, the application allows users to place new orders, update the status of existing orders, and view all current orders and list of available products. It also provides flexibility for users to interact with the system, with clear prompts and validation mechanisms in place to guarantee proper user input.

### I have utilized SQLite as the database solution, making sure the data is persistently stored, and the application maintains a consistent state between sessions. The database file is properly configured to reside in the correct directory, ensuring that users can access and interact with it as expected. The database is seeded with example products, in order for user to be able to place new order.

### I have implemented unit tests for the methods where applicable, ensuring that the application logic is working correctly. I am using a separate database file dedicated only for testing, which ensures that tests run in an isolated environment without affecting the production data.
