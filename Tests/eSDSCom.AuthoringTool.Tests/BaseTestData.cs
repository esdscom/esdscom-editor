

namespace eSDSCom.Editor.Tests;
public class BaseTestData : IClassFixture<DatabaseFixture>
{
   public static readonly string TestConnectionString;

    static BaseTestData()
    {
        if (string.IsNullOrEmpty(TestConnectionString))
        {
            SecretClientOptions options = new()
            {
                Retry =
                    {
                        Delay= TimeSpan.FromSeconds(2),
                        MaxDelay = TimeSpan.FromSeconds(16),
                        MaxRetries = 5,
                        Mode = RetryMode.Exponential
                     }
            };
            var client = new SecretClient(new Uri("https://esdscomeditorkeyvault.vault.azure.net/"), new DefaultAzureCredential(), options);
            KeyVaultSecret secret = client.GetSecret("ConnectionString");
            TestConnectionString = secret.Value;
        }

    }
    
    public static User GetTestUser(Guid theGuid, Guid theOrgGuid)
    {
        return new ()
        {
            Id = theGuid,
            Email = "tester@test.com",
            Name = "A test user",
            Role = 1,
            OrganizationId = theOrgGuid,
            IsActive = true
        };
    }

    public static Organization GetTestOrganization(Guid theOrgGuid)
    {
        return new()
        {
            Id = theOrgGuid,
            Name = "A Test Organization",
            Address = "123 Smith Street",
            OrganizationType = "Org Type 1",
            InfoExSysString = "<InformationFromExportingSystem/>"
        };
    }


    public static Datasheet GetTestDatasheet(Guid theGuid, Guid theOrgGuid, Guid theUserGuid, string comments = "")
    {   
        return new()
        {
            Id = theGuid,
            Name = "A datasheet",
            OrganizationId = theOrgGuid,
            UserId = theUserGuid,
            Status = 1,
            UserName = "A test user",
            DatasheetString = "<Datasheet/>",
            Comments = comments,
            RegionsString = ""
        };
    }

    public static DatasheetFeed GetTestDatasheetFeed(Guid dsfGuid, Guid orgGuid, Guid userGuid, string comments = "")
    {      
        return new()
        {
            Id = dsfGuid,
            Name = "A datasheetFeed",
            OrganizationId = orgGuid,
            UserId = userGuid,
            Status = 1,
            UserName = "A test user",
            DatasheetItems = new(),
            DatasheetFeedString = "<DatasheetFeed/>",
            Comments = comments
        };
    }

    public static DatasheetFeedItem GetTestDatasheetFeedItem(Guid theDSFGuid, Guid theDSGuid, Guid userGuid)
    {
        return new()
        {
            DatasheetFeedId = theDSFGuid,
            DatasheetId = theDSGuid,
            UserId = userGuid           
        };
    }

    public static Phrase GetTestPhrase()
    {
        return new()
        {
            StrucCode = "01.01.02.02.00.1006",
            XPath = @"//DatasheetFeed/Datasheet/IdentificationSubstPrep/RelevantIdentifiedUse",
            Region = "EU",
            OrigCode = "",
            English = "Aerating and dearating agents",
            German = "Belüftungs- und Entlüftungsmittel",
            RevDate = "01/03/17",
            Source = "Table R.12- 15: Descriptor list for Technical functions (TF)",
            Info = "Substance that influences the amount of air or gases entrained in a material.",
            Origin = "Core"
        };
    }

    public static Substance GetTestSubstance()
    {
        return new()
        {
            Id = Guid.Parse("46e1be11-4eba-4c5f-a967-18db3c97f1e3"),
            SubstanceId = "100.003.133",
            Name = "1-bromopropane",
            ECNumber = "203-445-0",
            CASNumber = "106-94-5",
            RegistrationStatus = "Active",
            RegistrationType = "INTERMEDIATE",
            SubmissionType = "INDIVIDUAL_SUBMISSION",
            TonnageBand = "Intermediate use only",
            LastUpdated = "07-07-2015",
            FactsheetURL = "https://echa.europa.eu/registration-dossier/-/registered-dossier/6200"
        };
    }
}

