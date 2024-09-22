# Usage

Several workflows are present in the application

- Quote Request:
  This is a http GET request with a string parameter of the ticker, and an optional double 
  for the quantity:
  Route is to `api/Quote` with query parameters of `ticker` and optional `quantity`
  - GET (5.HK) to `api/Quote?ticker=5.HK`
  - GET (5.HK, 100) to `api/Quote?ticker=5.HK&quantity=100`
  Response is a JSON object with data 
  - Id
  - Timestamp
  - Ticker
  - BidPrice
  - BidSize
  - AskPrice
  - AskSize
  
- Quote Request and Trade firm:
  This is a http POST request with a long consisting of the quote request `Id` response field, 
  and an optional double for the quantity:
  - POST (1)
  - POST(1, 100)
  
  The trade data returned consists of 
  - TimeStamp
  - Ticker
  - Price
  - Quantity
  
- Quote Request and Cancellation:
  This is a http DELETE request with a long consisting of the quote request `Id` response field, 
  and an optional double for the quantity:
  - DELETE (1)

Repeated queries to the Firm and Cancellation endpoints are permitted, they will be idempotent.
In the case of Firming, subsequent requests will return a 404 error.