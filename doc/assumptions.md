# Assumptions

The implementation makes the following assumptions

- The application will always be running and never close
  
- Quote prices and sizes are valid from the point of request for 5 minutes 
  for the purpose of firming up

- A user will want to take all the quote quantity that they can
  
- If two users are booking trades for the same ticker simultaneously, then the
  quotes provided are disjoint, so their trades can be booked without issue.

- Any user of the api has permission and ability to perform any action they 
  desire

- The market data provider has market data for any ticker desired