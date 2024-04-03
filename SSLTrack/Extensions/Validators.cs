namespace SSLTrack.Extensions;

public static class Validators
{
    public static bool DomainExists(IEnumerable<Domain> domainDb, string domain, int port )
    {
        if (!domainDb.Any())
        {
            return false;
        }
        if(domainDb.First().DomainName == domain && domainDb.First().Port == port)
        {
            return true;
        }
        return false;
    }
}
