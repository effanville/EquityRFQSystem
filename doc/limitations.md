# Limitations

The current implementation has a few limitations

- No persistence across application lifetime. The implementation uses an in memory database,
  and this does not persist any data after the application is closed. This can easily be
  resolved by changing the database type, possibly through configuration to a SQL server.

  The configuration here can be provided through registering the `IOptions<T>` classes in the DI
  container, and then retrieving suitable configuration for the database type and connection. 
  In this manner it can be changed easily, without recourse to altering other code.

- All endpoints are freely available to all users.
  This motivates a future enhancement where users first need to authenticate and authorise
  themselves prior to being able to use the api

- In reality quote data will change between retrieving and firming. In the firming step, one
  ideally should again query for the quote to ensure that there is sufficient liquidity to 
  book the trade, and that the price hasn't deviated too far from the booking price.

- A further limitation is that the error reporting back to a user of the api is limited. It
  reports success or failure clearly, but in the case of failure the error message is not 
  demonstrative of the error in some cases. This could be enhanced by the service returning
  a `Result<T>` object that combines a failure message with a valid result.

- Inevitably there are limitations to the concurrency of the system, to a large part limited 
  to the infrastructure where this code is being run. However, the code here has deliberately
  been made as lightweight and simple as possible to ensure that the overheads are minimal.