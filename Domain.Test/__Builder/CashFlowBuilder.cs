using Bogus;
using Domain.Models;

internal class CashFlowBuilder
{
    private readonly Faker _faker;
    private string _description;
    private decimal _amount;
    private int _month;
    private int _year;
    private User _user;
    private CashFlowStatus _status;
    private CashFlowType _type;

    public CashFlowBuilder()
    {
        _faker = new();
        _description = _faker.Lorem.Sentence();
        _amount = _faker.Finance.Amount();
        _month = _faker.Random.Int(1, 12);
        _year = DateTime.Now.Year;
        _user = UserBuilder.New().Build();
        _status = CashFlowStatus.PENDING;
        _type = CashFlowType.INCOME;
    }

    public static CashFlowBuilder New()
    {
        return new CashFlowBuilder();
    }

    public CashFlowBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public CashFlowBuilder WithAmount(decimal amount)
    {
        _amount = amount;
        return this;
    }

    public CashFlowBuilder WithMonth(int month)
    {
        _month = month;
        return this;
    }

    public CashFlowBuilder WithYear(int year)
    {
        _year = year;
        return this;
    }

    public CashFlowBuilder WithUser(User user)
    {
        _user = user;
        return this;
    }

    public CashFlowBuilder WithStatus(CashFlowStatus status)
    {
        _status = status;
        return this;
    }

    public CashFlowBuilder WithType(CashFlowType type)
    {
        _type = type;
        return this;
    }

    public CashFlow Build()
    {
        return new CashFlow(_description, _amount, _month, _year, _user, _status, _type);
    }
}
