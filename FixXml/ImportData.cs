using Npgsql;
using System.Xml;

using Azure.Core;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

namespace FixXml;

public class ImportData
{
    static string connString;

    public ImportData()
    {
        if (string.IsNullOrEmpty(connString))
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
            connString = secret.Value;
        }

        //  this is a 'one-time' utility to push source content for populating the db
        //  with content from the phrases and substances xml files that are obtained 
        //  from the ECHA and EUPHRAC sites
    }

    public static void Run()
    {
        ImportSubstances();

        ImportPhrases();
    }

    static void ImportSubstances()
    {
        try
        {
            var conn = new NpgsqlConnection(connString);

            var delCmd = new NpgsqlCommand(@"DELETE FROM SUBSTANCES", conn);
            conn.Open();
            delCmd.ExecuteNonQuery();
            conn.Close();

            XmlDocument xDoc = new();
            xDoc.Load(@"c:\temp\substances.xml");

            var nodes = xDoc.DocumentElement.SelectNodes("results/result");

            conn.Open();
            foreach (XmlNode node in nodes)
            {
                var insCmd = new NpgsqlCommand(@"INSERT INTO SUBSTANCES 
                                    (name,ecnumber,casnumber, substanceid,registrationstatus,registrationtype, 
                                     submissiontype,tonnageband,tonnagebandmin, tonnagebandmax,
                                     lastupdated, factsheetURL,substanceInformationPage)
                                    VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13)", conn)
                {
                    Parameters =
                    {
                        new() { Value = GetValueFromNode(node,"name") },
                        new() { Value = GetValueFromNode(node,"ecNumber") },
                        new() { Value = GetValueFromNode(node,"casNumber") },
                        new() { Value = GetValueFromNode(node,"ID") },
                        new() { Value = GetValueFromNode(node,"registrationStatus") },
                        new() { Value = GetValueFromNode(node,"registrationType") },
                        new() { Value = GetValueFromNode(node,"submissionType") },
                        new() { Value = GetValueFromNode(node,"tonnageBand") },
                        new() { Value = GetValueFromNode(node,"tonnageBandMin") },
                        new() { Value = GetValueFromNode(node,"tonnageBandMin") },
                        new() { Value = GetValueFromNode(node,"tonnageBandMax") },
                        new() { Value = GetValueFromNode(node,"lastUpdated") },
                        new() { Value = GetValueFromNode(node,"factsheetURL") },
                        new() { Value = GetValueFromNode(node,"substanceInformationPage") }
                    }
                };

                insCmd.ExecuteNonQuery();
            }
            conn.Close();
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            throw;
        }

    }

    static void ImportPhrases()
    {
        try
        {
            var conn = new NpgsqlConnection(connString);

            var delCmd = new NpgsqlCommand(@"DELETE FROM PHRASES", conn);
            conn.Open();
            delCmd.ExecuteNonQuery();
            conn.Close();

            XmlDocument xDoc = new();
            xDoc.Load(@"c:\temp\phrases.xml");

            var nodes = xDoc.DocumentElement.SelectNodes("PhraseItems/Phrase");

            conn.Open();
            foreach (XmlNode node in nodes)
            {
                var insCmd = new NpgsqlCommand(@"INSERT INTO PHRASES 
                                    (StrucCode,XPath,Region,OrigCode,English,German,RevDate,Source,Info,Origin)
                                    VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10)", conn)
                {
                    Parameters =
                    {
                        new() { Value = GetValueFromNode(node,"StrucCode") },   //39
                        new() { Value = GetValueFromNode(node,"XPath") },       //218
                        new() { Value = GetValueFromNode(node,"Region") },      //2
                        new() { Value = GetValueFromNode(node,"OrigCode") },    //22
                        new() { Value = GetValueFromNode(node,"English") },     //405
                        new() { Value = GetValueFromNode(node,"German") },      //285
                        new() { Value = GetValueFromNode(node,"RevDate") },     //300
                        new() { Value = GetValueFromNode(node,"Source") },      //309
                        new() { Value = GetValueFromNode(node,"Info") },        //1167
                        new() { Value = GetValueFromNode(node,"Origin") }       //742
                    }
                };

                insCmd.ExecuteNonQuery();
            }
            conn.Close();
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            throw;
        }
    }

    static string GetValueFromNode(XmlNode node, string elemName)
    {
        if (node.SelectSingleNode(elemName) is null)
        {
            return string.Empty;
        }
        else
        {
            return node.SelectSingleNode(elemName).InnerText;
        }
    }
}
