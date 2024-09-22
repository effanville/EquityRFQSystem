using EquityRFQSystem.Models;
using FluentAssertions;

namespace EquityRFQSystem.Unit.Tests;

public static class Assertions
{
    public static void ShouldBeEqual(this Quote? quote, 
        long id, 
        DateTime timestamp, string identifier, double bidPrice, double bidSize, double askPrice, double askSize)
    {
        quote.Should().NotBeNull();
        quote.Id.Should().Be(id);
        quote.TimeStamp.Should().Be(timestamp);
        quote.Ticker.Should().Be(identifier);
        quote.BidSize.Should().Be(bidSize);
        quote.BidPrice.Should().Be(bidPrice);
        quote.AskSize.Should().Be(askSize);
        quote.AskPrice.Should().Be(askPrice);
    }

    public static void ShouldBeEqual(this Trade? trade, DateTime timestamp, string identifier, double price, double quantity)
    {
        trade.Should().NotBeNull();
        trade.TimeStamp.Should().Be(timestamp);
        trade.Ticker.Should().Be(identifier);
        trade.Quantity.Should().Be(quantity);
        trade.Price.Should().Be(price);
    }
}