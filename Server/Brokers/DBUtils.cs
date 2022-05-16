namespace eSDSCom.Editor.Server.Brokers;

public static class DBUtils
{        
    internal static Substance GetSubstanceFromReader(NpgsqlDataReader reader)
    {
        return new()
        {
            Id                          = reader.GetGuid("Id"),
            SubstanceId                 = reader["SubstanceId"] as string,               
            Name                        = reader["Name"] as string,  
            ECNumber                    = reader["ECNumber"] as string,
            CASNumber                   = reader["CASNumber"] as string,
            RegistrationStatus          = reader["RegistrationStatus"] as string,
            RegistrationType            = reader["RegistrationType"] as string,
            SubmissionType              = reader["SubmissionType"] as string,
            TonnageBand                 = reader["TonnageBand"] as string,
            TonnageBandMin              = reader["TonnageBandMin"] as string,
            TonnageBandMax              = reader["TonnageBandMax"] as string,
            LastUpdated                 = reader["LastUpdated"] as string,
            FactsheetURL                = reader["FactsheetURL"] as string,
            SubstanceInformationPage    = reader["SubstanceInformationPage"] as string
        };
    }

    internal static Phrase GetPhraseFromReader(NpgsqlDataReader reader)
    {
        return new()
        {
            Id          = reader.GetGuid("Id"),
            StrucCode   = reader["StrucCode"] as string,
            XPath       = reader["Xpath"] as string,
            Region      = reader["Region"] as string,
            OrigCode    = reader["OrigCode"] as string,
            English     = reader["English"] as string,
            German      = reader["German"] as string,  
            RevDate     = reader["RevDate"] as string,
            Source      = reader["Source"] as string,
            Info        = reader["Info"] as string,
            Origin      = reader["Origin"] as string
        };
    }

    internal static User GetUserFromReader(NpgsqlDataReader reader)
    {
        return new()
        {
            Id              = reader.GetGuid("Id"),
            OrganizationId  = reader.GetGuid("OrganizationId"),
            Email           = reader["Email"] as string,
            Name            = reader["Name"] as string,  
            Role            = reader.GetInt32("Role") ,
            IsActive        = reader.GetBoolean("IsActive"),
            CreatedDate     = reader.GetDateTime("CreatedDate"),
            UpdatedDate     = reader.GetDateTime("UpdatedDate")
        };
    }

    internal static Organization GetOrgFromReader(NpgsqlDataReader reader)
    {
        return new()
        {
            Id                  = reader.GetGuid("Id"),
            OrganizationType    = reader["OrganizationType"] as string,
            Name                = reader["Name"] as string,
            Address             = reader["Address"] as string,
            CreatedDate         = reader.GetDateTime("CreatedDate"),
            UpdatedDate         = reader.GetDateTime("UpdatedDate"),
            InfoExSysString           = reader["InformationFromExportingSystem"] as string            
        };
    }

    internal static DatasheetFeed GetDatasheetFeedFromReader(NpgsqlDataReader reader)
    {
        return new()
        {
            Id                  = reader.GetGuid("Id"),
            OrganizationId      = reader.GetGuid("OrganizationId"),
            UserId              = reader.GetGuid("UserId"),
            Name                = reader["Name"] as string,                
            CreatedDate         = reader.GetDateTime("CreatedDate"),
            UpdatedDate         = reader.GetDateTime("UpdatedDate"),
            DatasheetFeedString = reader["DatasheetFeedDoc"] as string,
            UserName            = reader["UserName"] as string,
            Comments            = reader["Comments"] as string,
            Status              = reader.GetInt32("Status")
        };
    }

    internal static Datasheet GetDatasheetFromReader(NpgsqlDataReader reader)
    {
        return new()
        {
            Id                  = reader.GetGuid("Id"),
            OrganizationId      = reader.GetGuid("OrganizationId"),
            UserId              = reader.GetGuid("UserId"),
            Name                = reader["Name"] as string,
            CreatedDate         = reader.GetDateTime("CreatedDate"),
            UpdatedDate         = reader.GetDateTime("UpdatedDate"),
            Status              = reader.GetInt32("Status"),
            DatasheetString     = reader["DatasheetDoc"] as string,
            UserName            = reader["UserName"] as string,
            Comments            = reader["Comments"] as string,
            RegionsString       = reader["RegionsString"] as string,
            MaterialType        = reader["MaterialType"] as string
        };
    }

    internal static DatasheetFeedItem GetDatasheetFeedItemFromReader(NpgsqlDataReader reader)
    {
        return new()
        {
            DatasheetFeedId     = reader.GetGuid("DatasheetFeedId"),
            DatasheetId         = reader.GetGuid("DatasheetId"),                
            UserId              = reader.GetGuid("UserId"),
            CreatedDate         = reader.GetDateTime("CreatedDate"),
            UpdatedDate         = reader.GetDateTime("UpdatedDate")               
        };
    }
}   
