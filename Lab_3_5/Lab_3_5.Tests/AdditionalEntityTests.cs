using Lab_3_5.BLL;

namespace Lab_3_5.Tests;

public sealed class AdditionalEntityTests
{
    [Fact]
    public void FootballPlayer_PracticesOnlyWhenBallIsAvailable()
    {
        var player = new FootballPlayer(
            "Player",
            "Alex",
            3,
            "ST-F",
            MilitaryServiceStatus.NotServed,
            null,
            hasBall: false);

        Assert.False(player.Practice());
        Assert.Equal(0, player.PracticeSessions);

        player.HasBall = true;

        Assert.True(player.Practice());
        Assert.Equal(1, player.PracticeSessions);
    }

    [Fact]
    public void Lawyer_PracticesOnlyDuringInternship()
    {
        var lawyer = new Lawyer(
            "Lawyer",
            "Alex",
            3,
            "ST-L",
            MilitaryServiceStatus.NotServed,
            null,
            isOnInternship: false);

        Assert.False(lawyer.Practice());
        Assert.Equal(0, lawyer.PracticeSessions);

        lawyer.IsOnInternship = true;

        Assert.True(lawyer.Practice());
        Assert.Equal(1, lawyer.PracticeSessions);
    }
}
