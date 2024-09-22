# Equity RFQ System

## Overview

This creates a small web API that is designed for a request for quote (RFQ) system 
that allows for the querying of quotes for equity instruments.

The setup using ASP.NET and using the Task parallel library and async await structure 
naturally allows for concurrent requests and provides fast and accurate responses.

## Building

The application requires latest Visual Studio 2022 or Rider to build, or at least a 
version that allows for .NET 8, as well as the tools for ASP.NET installed.

## Running

Once built, the app can be run by running the `EquityRFQSystem.exe` executable located 
in the relevant sub directory of `src\EquityRFQSystem\bin\`

## Workflows

Several workflows are present in the application

- Quote Request:
  Here a user will make a request for a quote for a given instrument via the Ticker, 
  and an optional quantity of shares, and the system will query a third party service 
  for the Bid and ask prices and sizes, and return these to the user. 
  Quantity here is signed, so a positive quantity is considered a buy and a negative
  quantity is considered a sell.
- Quote Request and Trade firm:
  Following on from a Quote request, a user can then use the `Id` of the quote from the
  Quote request to confirm a trade at those prices. The user can specify a quantity at
  this stage (regardless of whether a quantity was specified before) and this later 
  quantity will take precedence when booking the trade. A successful trade, or a failure
  will be reported. 
  If the available quantity in the quote is less than the requested quantity, then only
  the available quantity is booked in the trade.
  If more than 300s has elapsed between the quote time and the firm time, then the 
  firming up will not be successful and a new quote will be required to be retrieved.
- Quote Request and Cancellation:
  Following on from a Quote request, a user can then use the `Id` of the quote from the
  Quote request to cancel the firming up. This marks the original quote in a cancelled
  state and that quote can no longer be used to firm up in the future.

## Simulation data

Currently the data provider returns random data back to the service for the purposes of 
testing only. In reality this would be linked to an actual service that provides the 
correct data.

## More documentation

In the `doc` folder there is some detail on the limitations, assumptions made, 
and future enhancements that could be made to this if needed.
