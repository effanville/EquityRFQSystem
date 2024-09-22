using EquityRFQSystem.Common;
using EquityRFQSystem.Controllers;
using EquityRFQSystem.Models;
using EquityRFQSystem.Persistence;
using EquityRFQSystem.RealTimeQuote;
using EquityRFQSystem.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Quote = EquityRFQSystem.Models.Quote;

namespace EquityRFQSystem.Unit.Tests;

public sealed class QuoteControllerTests
{
    private int _quoteIdCounter = 1;
    private int _tradeIdCounter = 1;

    [SetUp]
    public void Setup()
    {
        _quoteIdCounter = 1;
        _tradeIdCounter = 1;
    }

    [Test]
    public async Task GIVEN_QuoteController_THEN_GetQuoteWithQtyAndFirmUpCorrectlyReturnsQuoteAndTrade()
    {
        string identifier = "id";
        int qty = 100;
        DateTime firstQueryTime = new DateTime(2024, 1, 1, 12, 0, 0);
        DateTime secondQueryTime = firstQueryTime.AddSeconds(1);

        QuoteController quoteController = SetupController(new[] { firstQueryTime, secondQueryTime }, out IPersistence context);

        ActionResult<Quote> result = await quoteController.Get(identifier, qty);
        result.Should().NotBeNull();

        Quote? quote = result.Value;
        quote.ShouldBeEqual(1, firstQueryTime, identifier, 63, 11, 63, 25);

        var contextQuote = new Persistence.Models.Quote()
        {
            Ticker = quote.Ticker,
            Id = quote.Id,
            TimeStamp = quote.TimeStamp,
            BidPrice = quote.BidPrice,
            BidSize = quote.BidSize,
            AskPrice = quote.AskPrice,
            AskSize = quote.AskSize,
            RequestedQty = qty,
            IsLive = true
        };
        context.FindQuoteAsync(quote.Id).Returns(contextQuote);

        ActionResult<Trade> firmResult = await quoteController.ConfirmQuote(quote.Id);

        firmResult.Should().NotBeNull();

        Trade? trade = firmResult.Value;
        trade.ShouldBeEqual(secondQueryTime, identifier, quote.AskPrice, 25);

        // ensure quote has been set as firmed up.
        contextQuote.IsFirm.Should().BeTrue();
        contextQuote.IsLive.Should().BeFalse();
    }

    [Test]
    public async Task GIVEN_QuoteController_THEN_FirmUpForMissingQuoteThrows()
    {
        string identifier = "id";
        int qty = 100;
        DateTime firstQueryTime = new DateTime(2024, 1, 1, 12, 0, 0);
        DateTime secondQueryTime = firstQueryTime.AddSeconds(1);

        QuoteController quoteController = SetupController(new[] { firstQueryTime, secondQueryTime }, out IPersistence context);

        ActionResult<Quote> result = await quoteController.Get(identifier, qty);
        result.Should().NotBeNull();

        Quote? quote = result.Value;
        quote.ShouldBeEqual(1, firstQueryTime, identifier, 63, 11, 63, 25);

        var contextQuote = new Persistence.Models.Quote()
        {
            Ticker = quote.Ticker,
            Id = quote.Id,
            TimeStamp = quote.TimeStamp,
            BidPrice = quote.BidPrice,
            BidSize = quote.BidSize,
            AskPrice = quote.AskPrice,
            AskSize = quote.AskSize,
            RequestedQty = qty,
            IsLive = true
        };
        context.FindQuoteAsync(quote.Id).Returns(contextQuote);

        ActionResult<Trade> firmResult = await quoteController.ConfirmQuote(quote.Id);

        firmResult.Should().NotBeNull();

        Trade? trade = firmResult.Value;
        trade.ShouldBeEqual(secondQueryTime, identifier, quote.AskPrice, 25);

        // ensure quote has been set as firmed up.
        contextQuote.IsFirm.Should().BeTrue();
        contextQuote.IsLive.Should().BeFalse();
    }

    [TestCase(null, 100, 100, false)]
    [TestCase("", 100, 100, false)]
    [TestCase("id", 15, 15, true)]
    [TestCase("id", -10, -10, true)]
    [TestCase("id", 100, 25, true)]
    [TestCase("id", -100, -11, true)]
    [TestCase("id", 200, 25, true)]
    [TestCase("5.HK", 200, 25, true)]
    public async Task GIVEN_QuoteController_THEN_GetQuoteAndFirmUpWithQtyCorrectlyReturnsQuoteAndTrade(string? identifier, int requestedQty, int bookedQty, bool quotePresent)
    {
        DateTime firstQueryTime = new DateTime(2024, 1, 1, 12, 0, 0);
        DateTime secondQueryTime = firstQueryTime.AddSeconds(1);

        QuoteController quoteController = SetupController(new[] { firstQueryTime, secondQueryTime }, out IPersistence context);

        ActionResult<Quote> result = await quoteController.Get(identifier);
        result.Should().NotBeNull();

        Quote? quote = result.Value;
        if (!quotePresent)
        {
            quote.Should().BeNull();
            return;
        }

        quote.ShouldBeEqual(1, firstQueryTime, identifier, 63, 11, 63, 25);
        var contextQuote = new Persistence.Models.Quote()
        {
            Ticker = quote.Ticker,
            Id = quote.Id,
            TimeStamp = quote.TimeStamp,
            BidPrice = quote.BidPrice,
            BidSize = quote.BidSize,
            AskPrice = quote.AskPrice,
            AskSize = quote.AskSize,
            RequestedQty = requestedQty,
            IsLive = true
        };
        context.FindQuoteAsync(quote.Id).Returns(contextQuote);

        ActionResult<Trade> firmResult = await quoteController.ConfirmQuote(quote.Id, requestedQty);

        firmResult.Should().NotBeNull();

        Trade? trade = firmResult.Value;
        trade.ShouldBeEqual(secondQueryTime, identifier, quote.AskPrice, bookedQty);

        // ensure quote has been set as firmed up.
        contextQuote.IsFirm.Should().BeTrue();
        contextQuote.IsLive.Should().BeFalse();
    }

    [Test]
    public async Task GIVEN_QuoteController_THEN_GetQuoteAndCancelSetsCancelledState()
    {
        string identifier = "id";
        int qty = 100;
        DateTime firstQueryTime = new DateTime(2024, 1, 1, 12, 0, 0);
        DateTime secondQueryTime = firstQueryTime.AddSeconds(1);

        var quoteController = SetupController(new[] {firstQueryTime, secondQueryTime}, out IPersistence context);

        ActionResult<Quote> result = await quoteController.Get(identifier, qty);
        result.Should().NotBeNull();

        Quote? quote = result.Value;
        quote.ShouldBeEqual(1, firstQueryTime, identifier, 63, 11, 63, 25);

        var contextQuote = new Persistence.Models.Quote()
        {
            Ticker = quote.Ticker,
            Id = quote.Id,
            TimeStamp = quote.TimeStamp,
            BidPrice = quote.BidPrice,
            BidSize = quote.BidSize,
            AskPrice = quote.AskPrice,
            AskSize = quote.AskSize,
            RequestedQty = qty
        };
        context.FindQuoteAsync(quote.Id).Returns(contextQuote);

        await quoteController.CancelPendingQuote(quote.Id);

        // ensure quote has been set as finished.
        contextQuote.IsFirm.Should().BeFalse();
        contextQuote.IsLive.Should().BeFalse();

        await quoteController.CancelPendingQuote(quote.Id);

        // ensure quote is still set as finished.
        contextQuote.IsFirm.Should().BeFalse();
        contextQuote.IsLive.Should().BeFalse();
    }

    private QuoteController SetupController(DateTime[] clockQueryTimes, out IPersistence dbContext)
    {
        ILogger<QuoteController> logger = Substitute.For<ILogger<QuoteController>>();
        Random random = new Random(1);
        IClock clock = Substitute.For<IClock>();
        if (clockQueryTimes.Length == 1)
        {
            clock.UtcNow.Returns(clockQueryTimes[0]);
        }
        else if (clockQueryTimes.Length > 1)
        {
            clock.UtcNow.Returns(clockQueryTimes[0], clockQueryTimes.Skip(1).ToArray());
        }
        RealTimeQuoteProvider realTimeQuotes = new RealTimeQuoteProvider(clock, random);
        dbContext = Substitute.For<IPersistence>();
        dbContext.When(x => x.AddQuote(Arg.Any<Persistence.Models.Quote>())).Do(cont => cont.ArgAt<Persistence.Models.Quote>(0).Id = _quoteIdCounter++);
        dbContext.When(x => x.BookTrade(Arg.Any<Persistence.Models.Trade>())).Do(cont => cont.ArgAt<Persistence.Models.Trade>(0).Id = _tradeIdCounter++);
        QuoteRetrieverService quoteService = new QuoteRetrieverService(clock, dbContext, realTimeQuotes);
        QuoteController quoteController = new QuoteController(logger, quoteService);

        return quoteController;
    }
}
